using System;
using System.IO;
using Xamarin.Forms;
using Envanter.Helpers;
using Envanter.Models;

namespace Envanter.Views
{
    public partial class MainPage : ContentPage
    {
        private readonly Database _database;

        public MainPage()
        {
            InitializeComponent();
            _database = new Database(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "products.db3"));
            LoadProducts();
        }

        private async void LoadProducts()
        {
            var products = await _database.GetProductsAsync();
            productStackLayout.Children.Clear();

            if (products != null)
            {
                foreach (var product in products)
                {
                    var productView = new Grid
                    {
                        Padding = new Thickness(10),
                        ColumnDefinitions =
                        {
                            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                            new ColumnDefinition { Width = GridLength.Auto },
                            new ColumnDefinition { Width = GridLength.Auto }
                        }
                    };

                    productView.Children.Add(new Label { Text = product.Name, HorizontalOptions = LayoutOptions.StartAndExpand }, 0, 0);
                    productView.Children.Add(new Label { Text = product.Category, HorizontalOptions = LayoutOptions.StartAndExpand }, 1, 0);
                    productView.Children.Add(new Label { Text = product.StockQuantity.ToString(), HorizontalOptions = LayoutOptions.CenterAndExpand }, 2, 0);

                    var decreaseButton = new Button
                    {
                        Text = "Kullan",
                        BackgroundColor = Color.Orange,
                        FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Button)),
                        WidthRequest = 80,
                        HeightRequest = 40,
                        CornerRadius = 10
                    };
                    decreaseButton.Clicked += async (sender, args) =>
                    {
                        if (product.StockQuantity > 0)
                        {
                            int decreaseAmount = 1; // Örnek değer, kullanıcıdan alınabilir
                            await _database.UpdateProductStockAsync(product.Id, product.StockQuantity - decreaseAmount);
                            LoadProducts();
                        }
                        else
                        {
                            await DisplayAlert("Uyarı", "Stok miktarı 0'dan az olamaz.", "Tamam");
                        }
                    };

                    var deleteButton = new Button
                    {
                        Text = "Sil",
                        BackgroundColor = Color.Red,
                        FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Button)),
                        WidthRequest = 60,
                        HeightRequest = 40,
                        CornerRadius = 10
                    };
                    deleteButton.Clicked += async (sender, args) =>
                    {
                        await _database.DeleteProductAsync(product);
                        LoadProducts();
                    };

                    productView.Children.Add(decreaseButton, 3, 0);
                    productView.Children.Add(deleteButton, 4, 0);

                    var tapGestureRecognizer = new TapGestureRecognizer();
                    tapGestureRecognizer.Tapped += async (sender, args) =>
                    {
                        await DisplayAlert("Ürün Detayları", $"Name: {product.Name}\nStock: {product.StockQuantity}\nBarcode: {product.Barcode}\nDetails: {product.Details}\nCategory: {product.Category}\nAddedDate: {product.AddedDate}", "OK");
                    };
                    productView.GestureRecognizers.Add(tapGestureRecognizer);

                    productStackLayout.Children.Add(productView);

                    // Satır altına çizgi ekle
                    var separator = new BoxView
                    {
                        HeightRequest = 1,
                        BackgroundColor = Color.OrangeRed,
                        Margin = new Thickness(0, 5)
                    };
                    productStackLayout.Children.Add(separator);
                }
            }
        }

        private async void OnAddProductClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddProduct());
        }
    }
}
