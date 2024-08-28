using Envanter.Helpers;
using Envanter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Envanter.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text;
            string password = PasswordEntry.Text;

            if (username == "admin" && password == "123")
            {
                // Giriş başarılı, Dashboard'a yönlendir
                await DisplayAlert("Başarılı", "Giriş başarılı!", "Tamam");
                MessageLabel.IsVisible = false;
                Application.Current.MainPage = new NavigationPage(new Dashboard());
            }
            else
            {
                // Hatalı giriş
                await DisplayAlert("Hata", "Kullanıcı adı veya şifre yanlış.", "Tamam");
                MessageLabel.Text = "Kullanıcı adı veya şifre yanlış.";
                MessageLabel.IsVisible = true;
            }
        }
        private async void OnRegisterButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }
    }
}