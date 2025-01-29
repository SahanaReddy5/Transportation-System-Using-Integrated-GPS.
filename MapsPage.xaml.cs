using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TransportSystemClient.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Shapes;

namespace TransportSystemClient
{
    public partial class MapsPage : ContentPage
    {
        
        string BusNumber;
        RestServices rest ;
        DriverModel driverdata = new DriverModel();
        UserLocation driverloc = new UserLocation();
        UserLocation userLocation = new UserLocation();
        Position Driverposition, Userposition;
        MapLaunchOptions options;
        Position Hyderabad = new Position();
        public MapsPage(string busno)
        {

            BusNumber = busno;
            rest = new RestServices();
            InitializeComponent();
           
        }

        protected  override void OnAppearing()
        {
            base.OnAppearing();
             allmycode();


        }
        public async Task allmycode()
        {
            List<Position> list=new List<Position>();
            await getdriverdetails();
            await GetCurrentLocation();
            if (driverloc != null && userLocation != null)
            {
                list = await getDirectionsdata(driverloc, userLocation);
            }
            Xamarin.Forms.Maps.Polyline polyline = new Xamarin.Forms.Maps.Polyline() { StrokeColor=Color.Blue};
            foreach(Position p in list)
            {
                polyline.Geopath.Add(p);
            }

                Driverposition = new Position(driverloc.lat, driverloc.lon);
                Userposition = new Position(userLocation.lat, userLocation.lon);
                Distance metres = Distance.BetweenPositions(Driverposition, Userposition);

            Pin Driverpin = new Pin() { Label="Bus is here",Position=Driverposition};
            Pin Userpin = new Pin() { Position = Userposition };
            //if (metres.Kilometers == )
            //{
            //    await DisplayAlert("Reminder", "Bus is near your Pick", "ok");
            //}
            //     options = new MapLaunchOptions { NavigationMode = NavigationMode.Driving };
            //}
            //Moves the Map to the Specified Region and keeps it focused
            Map.MoveToRegion(MapSpan.FromCenterAndRadius(Userposition, Distance.FromMiles(10)));
            Map.MapElements.Add(polyline);
            Map.Pins.Add(Driverpin);
            Map.Pins.Add(Userpin);
        }
        //gets driver location from the API
        async Task getdriverdetails()
        {
            try
            {
                driverdata = await rest.GetDetailsByBusno(BusNumber);
                if (driverdata != null)
                {
                    string[] latlon = driverdata.Uid.Split(',');
                    driverloc.lat = Convert.ToDouble(latlon[0]);
                    driverloc.lon = Convert.ToDouble(latlon[1]);
                }
            }
            catch (Exception ex)
            {

            }


        }
        CancellationTokenSource cts;
        //gets user location from the Emulator
        async Task GetCurrentLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);

                if (location != null)
                {
                    userLocation.lat = location.Latitude;
                    userLocation.lon = location.Longitude;
                    //Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }
        protected override void OnDisappearing()
        {
            if (cts != null && !cts.IsCancellationRequested)
                cts.Cancel();
            base.OnDisappearing();
        }
        //gets the Directions between two given positions from the Google api
        async Task<List<Position>> getDirectionsdata(UserLocation startloc, UserLocation endloc)
        {
            DirectionHelper.Directionsmodel jsondata;
            string mapapiurlendpoint = "https://maps.googleapis.com/maps/api/directions/";
            string apikey = "AIzaSyDbjhnDgJn2svkCpQYpKpCP4eBMSZmswRs";
            string origin = startloc.lat.ToString() + "," + startloc.lon.ToString();
            string destination = endloc.lat.ToString() + "," + endloc.lon.ToString();
            string url = mapapiurlendpoint + "json?origin=" + origin + "&destination=" + destination + "&mode=Driving&key=" + apikey;
            ServicePointManager.Expect100Continue = true;
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            using (HttpClient client = new HttpClient(httpClientHandler))
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string s = await client.GetStringAsync(url);
                jsondata = JsonConvert.DeserializeObject<DirectionHelper.Directionsmodel>(s);
            }
            var points = jsondata.Routes[0].OverviewPolyline.Points;

            List<Position> lines = DecodePolyline(points);

            return lines;
        }
        //converts the Polyline coordinates into the Listof positions to draw the line over the path
        private List<Position> DecodePolyline(string encodedPoints)
        {
            if (string.IsNullOrWhiteSpace(encodedPoints))
            {
                return null;
            }

            int index = 0;
            var polylineChars = encodedPoints.ToCharArray();
            var poly = new List<Position>();
            int currentLat = 0;
            int currentLng = 0;
            int next5Bits;

            while (index < polylineChars.Length)
            {
                // calculate next latitude
                int sum = 0;
                int shifter = 0;

                do
                {
                    next5Bits = polylineChars[index++] - 63;
                    sum |= (next5Bits & 31) << shifter;
                    shifter += 5;
                }
                while (next5Bits >= 32 && index < polylineChars.Length);

                if (index >= polylineChars.Length)
                {
                    break;
                }

                currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                // calculate next longitude
                sum = 0;
                shifter = 0;

                do
                {
                    next5Bits = polylineChars[index++] - 63;
                    sum |= (next5Bits & 31) << shifter;
                    shifter += 5;
                }
                while (next5Bits >= 32 && index < polylineChars.Length);

                if (index >= polylineChars.Length && next5Bits >= 32)
                {
                    break;
                }

                currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                var mLatLng = new Position(Convert.ToDouble(currentLat) / 100000.0, Convert.ToDouble(currentLng) / 100000.0);
                poly.Add(mLatLng);
            }

            return poly;
        }
    }


}
