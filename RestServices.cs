using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TransportSystemClient.Models;

namespace TransportSystemClient
{
    public class RestServices
    {

        //Gets all users from the  api & deserializes the Json response
        async public Task<UserModel[]> GetUserdata()
        {
            UserSerialize users;
            ServicePointManager.Expect100Continue = true;
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            using (HttpClient client = new HttpClient(httpClientHandler))
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string s = await client.GetStringAsync(RestConstants.UserEndpoint);
                users = JsonConvert.DeserializeObject<UserSerialize>(s);
            }
            return users.Data.ToArray();
        }
        //posts the serialized user object to  the api & deserializes the Json response
        async public Task<string> PostUserdata(UserModel user)
        {
            string responsemessage = string.Empty;
            ServicePointManager.Expect100Continue = true;
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            StringContent content = new StringContent(JsonConvert.SerializeObject(user));
            content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");
            using (HttpClient client = new HttpClient(httpClientHandler))
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var result = await client.PostAsync(RestConstants.UserEndpoint, content).ConfigureAwait(false);
                if (result.IsSuccessStatusCode)
                {
                    responsemessage = await result.Content.ReadAsStringAsync();
                }
            }
            var msg = JObject.Parse(responsemessage);
            return msg["Data"].ToString();
        }
        //gets all drivers list and deserializes the json response 
        async public Task<DriverModel[]> GetDriverdata()
        {
            DriverSerialize Drivers;
            ServicePointManager.Expect100Continue = true;
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            using (HttpClient client = new HttpClient(httpClientHandler))
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string s = await client.GetStringAsync(RestConstants.DriverEndpoint);
                Drivers = JsonConvert.DeserializeObject<DriverSerialize>(s);

            }

            return Drivers.Data.ToArray();
        }
        //posts the serialized driver object to the api 
        async public Task<string> PostDriverdata(DriverModel d)
        {
            string responsemessage = string.Empty;
            ServicePointManager.Expect100Continue = true;
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            var content = new StringContent(JsonConvert.SerializeObject(d));
            content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");
            using (HttpClient client = new HttpClient(httpClientHandler))
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var result = await client.PostAsync(RestConstants.DriverEndpoint, content).ConfigureAwait(false);
                if (result.IsSuccessStatusCode)
                {
                    responsemessage = await result.Content.ReadAsStringAsync();
                }
            }
            var msg = JObject.Parse(responsemessage);
            return msg["Data"].ToString();
        }
        //gets the driver & bus details for a specific bus
        async public Task<DriverModel> GetDetailsByBusno(string busno)
        {
            DriverModel Drivers = new DriverModel(); DriverInfo driverdata;
            ServicePointManager.Expect100Continue = true;
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            using (HttpClient client = new HttpClient(httpClientHandler))
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string s = await client.GetStringAsync(RestConstants.DriverEndpoint + "?Busno=" + busno);
                driverdata = JsonConvert.DeserializeObject<DriverInfo>(s);
            }

            return driverdata.Data;
        }
        //updates the drivers details based on the given driver id & driver object
        async public Task<string> UpdateDriverdata(int driverid,DriverModel d)
        {
            string responsemessage = string.Empty;
            ServicePointManager.Expect100Continue = true;
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            var content = new StringContent(JsonConvert.SerializeObject(d));
            content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");
            using (HttpClient client = new HttpClient(httpClientHandler))
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var result = await client.PutAsync(RestConstants.DriverEndpoint+"?DriverId="+driverid, content).ConfigureAwait(false);
                if (result.IsSuccessStatusCode)
                {
                    responsemessage = await result.Content.ReadAsStringAsync();
                }
            }
            var msg = JObject.Parse(responsemessage);
            return msg["Data"].ToString();
        }
    }
}
