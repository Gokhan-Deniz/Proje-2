using Envanter.Helpers;
using Envanter.Models;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Envanter.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddCategoryPage : ContentPage
    {
        private readonly CategoryHelper _categoryHelper;

        public AddCategoryPage()
        {
            InitializeComponent();
            _categoryHelper = new CategoryHelper(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "categories.db3"));
        }

        private async void OnAddCategoryButtonClicked(object sender, EventArgs e)
        {
            var categoryName = categoryNameEntry.Text;

            if (string.IsNullOrWhiteSpace(categoryName))
            {
                await DisplayAlert("Hata", "Kategori adı boş olamaz!", "Tamam");
                return;
            }

            var newCategory = new Category
            {
                Name = categoryName
            };

            await _categoryHelper.SaveCategoryAsync(newCategory);
            await DisplayAlert("Başarılı", "Kategori eklendi!", "Tamam");
            categoryNameEntry.Text = string.Empty;

            await Navigation.PopAsync(); // Kategori eklendikten sonra geri dön
        }
    }
}
