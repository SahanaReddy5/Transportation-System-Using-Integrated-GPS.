using System;
using System.Collections.Generic;
using TransportSystemClient.Models;
using Xamarin.Forms;

namespace TransportSystemClient
{
    public partial class LoginPage : ContentPage
    {
        RestServices restServices;
        public LoginPage()
        {
            InitializeComponent();
            restServices = new RestServices();
        }



        async void btnLogin_Clicked(System.Object sender, System.EventArgs e)
        {
            UserModel[] Usersarray = await restServices.GetUserdata();
            List<UserModel> UsersList = new List<UserModel>(Usersarray);
            DriverModel[] DriversArray = await restServices.GetDriverdata();
            List<DriverModel> driverlist=new List<DriverModel>(DriversArray);
            //null or empty field validation, check weather email and password is null or empty  
            if (string.IsNullOrEmpty(Username.Text) || string.IsNullOrEmpty(Password.Text))
                await DisplayAlert("Empty Values", "Please enter Email and Password", "OK");
            else
            {
                if (!string.IsNullOrEmpty(Username.Text)&& !string.IsNullOrEmpty(Password.Text))
                {
                    foreach (var user in UsersList)
                    {
                        if (Username.Text == user.Name && Password.Text == user.Password)
                        {
                            if (user.Role == "user" || user.Role == "lecturer")
                            {
                                await DisplayAlert("Message", "Login Successful", "OK");
                                await Navigation.PushModalAsync(new NavigationPage(new StudentPage()));
                            }
                            else if (user.Role == "driver")
                            {
                                await DisplayAlert("Message", "Login Successful", "OK");
                                await Navigation.PushModalAsync(new NavigationPage(new DriverPage(user.Name)));
                            }
                        }
                    }

                }
                else
                    await DisplayAlert("Login Fail", "Please enter correct Email and Password", "OK");
            }
        }

        async void btnRegister_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new RegistrationPage()));

        }
    }
}
