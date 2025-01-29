using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TransportSystemClient.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TransportSystemClient
{
    public partial class StudentPage : ContentPage
    {
        UserLocation userLocation = new UserLocation();
        public ObservableCollection<DriverModel> driverslist;
        public  StudentPage()
        {
            InitializeComponent();
            Getbuslist();

        }
        //Gets the list of busses available and populates them in the list 
        public async void Getbuslist()
        {
          
             ServicePointManager.Expect100Continue = true;
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            using (HttpClient client = new HttpClient(httpClientHandler))
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string s = await client.GetStringAsync(RestConstants.DriverEndpoint);
                var Drivers = JsonConvert.DeserializeObject<DriverSerialize>(s);
                driverslist = new ObservableCollection<DriverModel>(Drivers.Data);
            }
            Buslist.ItemsSource = driverslist;
        }

        async void Buslist_ItemSelected(System.Object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            var obj = (DriverModel)e.SelectedItem;
            var busid = Convert.ToInt32(obj.BusNumber);
            string busno = busid.ToString();
            await Navigation.PushModalAsync(new NavigationPage(new MapsPage(busno)));
        }
    }
}
