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
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private async void OnRegisterButtonClicked(object sender, EventArgs e)
        {
            try
            {
                string username = UsernameEntry.Text;
                string password = PasswordEntry.Text;
                string confirmPassword = ConfirmPasswordEntry.Text;

                if (password != confirmPassword)
                {
                    MessageLabel.Text = "Passwords do not match";
                    MessageLabel.IsVisible = true;
                    return;
                }

                DatabaseHelper db = new DatabaseHelper();
                User existingUser = await db.GetUserAsync(username, password);

                if (existingUser != null)
                {
                    MessageLabel.Text = "Username already exists";
                    MessageLabel.IsVisible = true;
                    return;
                }

                User newUser = new User
                {
                    Username = username,
                    Password = password
                };

                bool isRegistered = await db.RegisterUserAsync(newUser);

                if (isRegistered)
                {
                    MessageLabel.TextColor = Color.Green;
                    MessageLabel.Text = "Registration successful!";
                    MessageLabel.IsVisible = true;

                    // Optionally, navigate to the login page
                    await Navigation.PopAsync();
                }
                else
                {
                    MessageLabel.Text = "Registration failed";
                    MessageLabel.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                // Hata mesajını göster
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());
        }
    }
}