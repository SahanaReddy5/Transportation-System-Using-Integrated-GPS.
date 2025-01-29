using System;
using System.Collections.Generic;
using TransportSystemClient.Models;
using Xamarin.Forms;

namespace TransportSystemClient
{
    public partial class DriverPage : ContentPage
    {
        RestServices rest;
        string DriverName;
        string latlon;
        DriverModel driver = new DriverModel();
        public DriverPage(string Name)
        {
            InitializeComponent();
            rest = new RestServices();
            DriverName = Name;
            
        }


       async void btnUpdatelocation_Clicked(System.Object sender, System.EventArgs e)
        {
            string lattitude, longitude, responsemsg;
            if (!string.IsNullOrEmpty(Lattitude.Text) && !string.IsNullOrEmpty(Longitude.Text))
            {
                lattitude = Lattitude.Text;
                longitude = Longitude.Text;
                latlon=string.Concat(lattitude,"," ,longitude);
            }
          DriverModel[] drivers= await rest.GetDriverdata();
            foreach(DriverModel d in drivers)
            {
                if (d.DriverName == DriverName)
                {
                    driver = d;
                }
            }
            driver.Uid = latlon.ToString();
           responsemsg = await rest.UpdateDriverdata(driver.DriverId,driver);
            if (!string.IsNullOrWhiteSpace(responsemsg))
            {
                await DisplayAlert("GPS Alert", "Updated current location", "ok");
            }
        }
    }
}
