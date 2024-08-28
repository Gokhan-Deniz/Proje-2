using Envanter.Helpers;
using Envanter.Models;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms; // ZXing kütüphanesi

namespace Envanter.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddProduct : ContentPage
    {
        private readonly Database _database;
        private readonly CategoryHelper _categoryHelper;

        public AddProduct()
        {
            InitializeComponent();
            _database = new Database(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "products.db3"));
            _categoryHelper = new CategoryHelper(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "categories.db3"));
            LoadCategories();
        }

        private async void LoadCategories()
        {
            var categories = await _categoryHelper.GetCategoriesAsync();
            categoryPicker.ItemsSource = categories;
            categoryPicker.ItemDisplayBinding = new Binding("Name");
        }

        private async void OnSaveProductClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nameEntry.Text) || string.IsNullOrWhiteSpace(stockEntry.Text) ||
                !int.TryParse(stockEntry.Text, out int stockQuantity) || categoryPicker.SelectedItem == null)
            {
                await DisplayAlert("Hata", "Lütfen tüm alanları doldurun ve stok adedini doğru formatta girin.", "Tamam");
                return;
            }

            var selectedCategory = categoryPicker.SelectedItem as Category;

            var product = new Product
            {
                Name = nameEntry.Text,
                StockQuantity = stockQuantity,
                Barcode = barcodeEntry.Text,
                Details = detailsEditor.Text,
                Category = selectedCategory.Name,
                AddedDate = DateTime.Now
            };

            await _database.SaveProductAsync(product);
            await Navigation.PopAsync();
        }

        private async void OnScanBarcodeClicked(object sender, EventArgs e)
        {
            var scanPage = new ZXingScannerPage();
            scanPage.OnScanResult += (result) =>
            {
                scanPage.IsScanning = false;

                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopAsync();
                    barcodeEntry.Text = result.Text;
                    await DisplayAlert("Barkod Tarandı", $"Barkod: {result.Text}", "Tamam");
                });
            };

            await Navigation.PushAsync(scanPage);
        }/*

### `var scanPage = new ZXingScannerPage();`

- Bu satır, yeni bir barkod tarama sayfası oluşturur. `ZXingScannerPage` sınıfı, ZXing kütüphanesi tarafından sağlanan bir sayfadır ve kamerayı kullanarak barkod tarama işlevini gerçekleştirir.

### `scanPage.OnScanResult += (result) => { ... };`

- Bu satır, tarama sonucu elde edildiğinde çalışacak bir olay işleyici ekler. `OnScanResult` olayına bir lambda ifadesi (`(result) => { ... }`) atanır. Bu lambda ifadesi, tarama sonucunu (`result`) alır ve içindeki işlemleri gerçekleştirir.

### `scanPage.IsScanning = false;`

- Bu satır, tarama işlemini durdurur. Tarama başarılı olduğunda veya tamamlandığında kameranın taramayı durdurmasını sağlar.

### `Device.BeginInvokeOnMainThread(async () => { ... });`

- Bu satır, kullanıcının arayüzüyle ilgili işlemlerin ana iş parçacığında yapılmasını sağlar. Xamarin.Forms'da, kullanıcı arayüzü güncellemelerinin ana iş parçacığında yapılması gerekmektedir.
- `async () => { ... }` ifadesi, asenkron bir lambda ifadesidir ve içindeki kodun asenkron olarak çalıştırılmasını sağlar.

### `await Navigation.PopAsync();`

- Bu satır, navigasyon yığınının üstündeki sayfayı çıkarır ve kullanıcıyı bir önceki sayfaya geri götürür. `await` anahtar kelimesi, bu işlemin tamamlanmasını bekler.

### `barcodeEntry.Text = result.Text;`

- Bu satır, taranan barkod sonucunu (`result.Text`) `barcodeEntry` adlı girdi alanına atar. Böylece kullanıcı, taranan barkod numarasını bu alanda görebilir.

### `await DisplayAlert("Barkod Tarandı", $"Barkod: {result.Text}", "Tamam");`

- Bu satır, bir uyarı mesajı gösterir. Uyarı mesajında, taranan barkod numarası kullanıcıya gösterilir. `await` anahtar kelimesi, uyarı mesajının kapatılmasını bekler.

### `await Navigation.PushAsync(scanPage);`

- Bu satır, tarama sayfasını kullanıcıya gösterir ve sayfa navigasyon yığınına eklenir. Bu işlem asenkron olarak gerçekleştirilir.*/
    }
}
