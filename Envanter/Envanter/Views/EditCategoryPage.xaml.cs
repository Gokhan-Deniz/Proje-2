using Envanter.Helpers;
using Envanter.Models;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Envanter.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditCategoryPage : ContentPage
    {
        private readonly CategoryHelper _categoryHelper;
        private readonly Category _category;

        public EditCategoryPage(Category category)
        {
            InitializeComponent();
            _categoryHelper = new CategoryHelper(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "categories.db3"));
            _category = category;
            categoryNameEntry.Text = category.Name;
        }

        private async void OnSaveCategoryButtonClicked(object sender, EventArgs e)
        {
            var categoryName = categoryNameEntry.Text;

            if (string.IsNullOrWhiteSpace(categoryName))
            {
                await DisplayAlert("Hata", "Kategori adı boş olamaz!", "Tamam");
                return;
            }

            _category.Name = categoryName;
            await _categoryHelper.SaveCategoryAsync(_category);
            await DisplayAlert("Başarılı", "Kategori güncellendi!", "Tamam");
            await Navigation.PopAsync();
        }
    }
}
