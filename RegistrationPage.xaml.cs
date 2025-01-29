using System;
using System.Collections.Generic;
using TransportSystemClient.Models;
using Xamarin.Forms;

namespace TransportSystemClient
{
    public partial class RegistrationPage : ContentPage
    {
        RestServices restServices;
        public RegistrationPage()
        {
            InitializeComponent();
            restServices = new RestServices();
        }

        async void btnRegister_Clicked(System.Object sender, System.EventArgs e)
        {
            bool ispresent=false;
            UserModel[] Usersarray = await restServices.GetUserdata();
            List<UserModel> UsersList = new List<UserModel>(Usersarray);
            if (string.IsNullOrEmpty(Username.Text) || string.IsNullOrEmpty(Password.Text)|| RolePicker.SelectedItem == null)
                await DisplayAlert("Empty Values", "Please enter Email and Password", "OK");
            else
            {
                if (!string.IsNullOrEmpty(Username.Text) && !string.IsNullOrEmpty(Password.Text)&& RolePicker.SelectedItem!=null)
                {
                    foreach (var user in UsersList)
                    {
                        if (Username.Text == user.Name)
                        {
                            ispresent = true;
                           
                        }
                        else
                        {
                            ispresent = false;
                        }
                        
                    }
                    if (ispresent)
                    {
                        await DisplayAlert("Message", "User already exists.Try logging", "OK");
                        await Navigation.PushModalAsync(new NavigationPage(new LoginPage()));

                    }
                    else
                    {
                        UserModel NewUser = new UserModel();
                        NewUser.Name = Username.Text.ToLower();
                        NewUser.Password = Password.Text;
                        NewUser.Role = RolePicker.SelectedItem.ToString().ToLower();
                        NewUser.PhoneNumber = phonenumber.Text;
                        string responsemsg = await restServices.PostUserdata(NewUser);
                        await DisplayAlert("Message", responsemsg.Trim(), "OK");
                        await Navigation.PushModalAsync(new NavigationPage(new LoginPage()));

                    }
                   
                }
                else
                    await DisplayAlert("Registration failed", "Please enter valid User Details", "OK");
            }
        }

    }
}
