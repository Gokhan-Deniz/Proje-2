using Envanter.Helpers;
using Envanter.Models;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Envanter.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoriesPage : ContentPage
    {
        private readonly CategoryHelper _categoryHelper;

        public CategoriesPage()
        {
            InitializeComponent();
            _categoryHelper = new CategoryHelper(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "categories.db3"));
            LoadCategories();
        }

        private async void LoadCategories()
        {
            var categories = await _categoryHelper.GetCategoriesAsync();
            categoryStackLayout.Children.Clear();

            foreach (var category in categories)
            {
                var categoryView = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Padding = new Thickness(10),
                    Children =
                    {
                        new Label { Text = category.Id.ToString(), HorizontalOptions = LayoutOptions.Start, WidthRequest = 20 },
                        new Label { Text = category.Name, HorizontalOptions = LayoutOptions.StartAndExpand, WidthRequest = 150 },
                        CreateStyledButton("Düzenle", category, Color.Orange, 100, 35, 11), // Güncellenmiş özellikler
                        CreateStyledButton("Sil", category, Color.Red, 80, 35, 11)      // Güncellenmiş özellikler
                    }
                };

                ((Button)categoryView.Children[2]).Clicked += OnEditCategoryClicked;
                ((Button)categoryView.Children[3]).Clicked += OnDeleteCategoryClicked;

                categoryStackLayout.Children.Add(categoryView);
            }
        }

        private Button CreateStyledButton(string text, Category category, Color backgroundColor, double width, double height, double fontSize)
        {
            return new Button
            {
                Text = text,
                CommandParameter = category,
                BackgroundColor = backgroundColor,
                WidthRequest = width,
                HeightRequest = height,
                FontSize = fontSize,
                FontAttributes = FontAttributes.Bold,
                CornerRadius = (int)(height / 4), // Köşeleri kavisli yapmak için
                HorizontalOptions = text == "Düzenle" ? LayoutOptions.Center : LayoutOptions.End
            };
        }

        private async void OnAddCategoryClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddCategoryPage());
        }

        private async void OnEditCategoryClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var category = button?.CommandParameter as Category;

            if (category != null)
            {
                await Navigation.PushAsync(new EditCategoryPage(category));
            }
        }

        private async void OnDeleteCategoryClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var category = button?.CommandParameter as Category;

            if (category != null)
            {
                var result = await DisplayAlert("Silme Onayı", "Bu kategoriyi silmek istediğinizden emin misiniz?", "Evet", "Hayır");
                if (result)
                {
                    await _categoryHelper.DeleteCategoryAsync(category);
                    LoadCategories();
                }
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadCategories();
        }
    }
}
