using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using New.Globalx.WebApi.Models;
using New.Globalx.WebApi.Models.PakkeLabels;
using Newtonsoft.Json;

namespace New.Globalx.WebApi.Clients
{
    public class PakkeLabelsToken
    {
        public string token;
        public string expires_at;
    }

    public class PakkeLabelsApiClient
    {
        private PakkeLabelsToken _token;
        private string _urlV2 = "https://app.pakkelabels.dk/api/public/v2/";
        private string _urlV3 = "https://app.pakkelabels.dk/api/public/v3/";

        private string _api_user = "0930aeb1-1412-4260-9f19-63fb1d010720";
        private string _api_key = "1031bb73-6770-46eb-9960-6b1ab31d322e";


        public async Task<PakkeLabelsToken> GetToken()
        {
            using var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Post,
                new Uri(_urlV2 + "users/login"));

            var apiUser = new
            {
                api_user = _api_user,
                api_key = _api_key
            };

            var json = await DoRequestAndReturnJson(httpRequestMessage, apiUser);

            var token = JsonConvert.DeserializeObject<PakkeLabelsToken>(json);

            return token;
        }
        public async Task<List<PickupPoint>> GetPickUpPoints(Address address, string carrier_code)
        {
            using var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Get,
                new Uri(_urlV3 + "pickup_points"));

            var sendData = new
            {
                carrier_code,
                zipcode = address.Zipcode,
                country_code = address.CountryId,
                address = address.Address1,
                quantity = 3
            };

            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", GetBasicAuth());

            var json = await DoRequestAndReturnJson(httpRequestMessage, sendData);
            if (json.Contains("error"))
            {
                return new List<PickupPoint>();
            }
            var pickupPoints = JsonConvert.DeserializeObject<List<PickupPoint>>(json);
            return pickupPoints;
        }


        public async Task<List<ShippingQuote>> GetQuotesByAddress(Address address, int weight)
        {
            using var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Post,
                new Uri(_urlV3 + "quotes/list"));

            var sendData = new
            {
                sender = new
                {
                    address1 = "Scandiagade 14",
                    zipcode = "8930",
                    city = "Randers NØ",
                    country_code = "DK"
                },
                receiver = new
                {
                    address1 = address.Address1,
                    zipcode = address.Zipcode,
                    city = address.City,
                    country_code = address.CountryId
                },
                parcels = new[] { new { weight } }
            };

            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", GetBasicAuth());

            var json = await DoRequestAndReturnJson(httpRequestMessage, sendData);

            var quotes = JsonConvert.DeserializeObject<List<ShippingQuote>>(json);

            return quotes;
        }
        public async Task<string> GetProductsByCode(string product_code)
        {
            using var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Get,
                new Uri(_urlV3 + "products"));

            var sendData = new
            {
                sender_country_code = "DK",
                product_code,
                per_page = 50
            };

            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", GetBasicAuth());

            var json = await DoRequestAndReturnJson(httpRequestMessage, sendData);

            return json;
        }


        private string GetBasicAuth()
        {
            return Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(_api_user + ':' + _api_key));
        }
        private async Task<string> DoRequestAndReturnJson(HttpRequestMessage httpRequest, object data)
        {
            httpRequest.Content =
                new StringContent(JsonConvert.SerializeObject(data, Formatting.Indented), Encoding.UTF8, "application/json");

            var httpClient = CreateHttpClient();

            using var httpResponseMessage = await httpClient.SendAsync(httpRequest);
            var json = httpResponseMessage.Content.ReadAsStringAsync().Result;

            return json;
        }

        private static HttpClient CreateHttpClient()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return httpClient;
        }


    }
}
