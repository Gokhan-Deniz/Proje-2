using Envanter.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;
using Envanter.Models;

namespace Envanter.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListePage : ContentPage
    {
        private readonly Database _database;
        private List<Product> _products;

        public ListePage()
        {
            InitializeComponent();
            _database = new Database(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "products.db3"));
            LoadProducts();
        }

        private async void LoadProducts()
        {
            _products = await _database.GetProductsAsync();
            LoadCategories();
            DisplayProducts(_products);
        }

        private void LoadCategories()
        {
            var categories = _products.Select(p => p.Category).Distinct().ToList();
            categories.Insert(0, "Tümü"); // İlk sıraya "Tümü" ekle
            categoryPicker.ItemsSource = categories;
        }

        private void DisplayProducts(IEnumerable<Product> products)
        {
            productStackLayout.Children.Clear();

            if (products != null)
            {
                foreach (var product in products)
                {
                    var productView = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Padding = new Thickness(10),
                        Children =
                        {
                            new Label { Text = product.Name, HorizontalOptions = LayoutOptions.Start, WidthRequest = 100 },
                            new Label { Text = product.Category, HorizontalOptions = LayoutOptions.Start, WidthRequest = 100 },
                            new Label { Text = product.StockQuantity.ToString(), HorizontalOptions = LayoutOptions.CenterAndExpand, WidthRequest = 100 },
                            new Label { Text = product.Barcode, HorizontalOptions = LayoutOptions.EndAndExpand, WidthRequest = 120 }
                        }
                    };

                    var tapGestureRecognizer = new TapGestureRecognizer();
                    tapGestureRecognizer.Tapped += async (sender, args) =>
                    {
                        await DisplayAlert("Ürün Detayları", $"Ad: {product.Name}\nStok: {product.StockQuantity}\nBarkod: {product.Barcode}\nDetaylar: {product.Details}\nKategori: {product.Category}\nEklenme Tarihi: {product.AddedDate}", "Tamam");
                    };
                    productView.GestureRecognizers.Add(tapGestureRecognizer);

                    productStackLayout.Children.Add(productView);
                }
            }
        }

        private void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
        {
            FilterProducts();
        }

        private void OnCategoryPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            FilterProducts();
        }

        private void FilterProducts()
        {
            var filteredProducts = _products;

            // Arama kutusundaki metne göre filtreleme
            if (!string.IsNullOrWhiteSpace(searchBar.Text))
            {
                filteredProducts = filteredProducts.Where(p => p.Name.ToLower().Contains(searchBar.Text.ToLower())).ToList();
            }

            // Kategoriye göre filtreleme
            if (categoryPicker.SelectedIndex > 0) // "Tümü" hariç
            {
                var selectedCategory = categoryPicker.SelectedItem.ToString();
                filteredProducts = filteredProducts.Where(p => p.Category == selectedCategory).ToList();
            }

            DisplayProducts(filteredProducts);
        }
    }
}

