using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using New.Globalx.WebApi.Models;
using New.Globalx.WebApi.Models.PakkeLabels;
using Newtonsoft.Json;

namespace New.Globalx.WebApi.Clients
{

    public class PakkeLabelsApiClient
    {
        private readonly string _urlV3 = "https://app.pakkelabels.dk/api/public/v3/";

        private readonly string _api_user = "0930aeb1-1412-4260-9f19-63fb1d010720";
        private readonly string _api_key = "1031bb73-6770-46eb-9960-6b1ab31d322e";

        private readonly Address _senderAddress = new Address()
        {
            Firstname = "Anthony",
            Lastname = "Shevlin",
            Company = "Shevlin.co",
            Address1 = "Scandiagade 14",
            Zipcode = "8930",
            City = "Randers NØ",
            Email = "shevlinco@gmail.com",
            CountryId = "DK"
        };

        public async Task<List<ServicePoint>> GetServicePoints(Address address, string carrier_code)
        {
            using var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Get,
                new Uri(_urlV3 + "pickup_points"));

            var sendData = new
            {
                carrier_code,
                zipcode = address.Zipcode,
                country_code = address.CountryId,
                address = address.Address1
            };

            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", GetBasicAuth());

            var json = await DoRequestAndReturnJson(httpRequestMessage, sendData);
            if (json.Contains("error"))
            {
                return new List<ServicePoint>();
            }
            var pickupPoints = JsonConvert.DeserializeObject<List<ServicePoint>>(json);

            var pickupPointsFormatted =
                (pickupPoints ?? new List<ServicePoint>()).Select(c => { c.company_name = c.company_name.Replace("- KRÆVER POSTNORD APP", ""); return c; }).ToList();

            return pickupPointsFormatted;

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
                    address1 = _senderAddress.Address1,
                    zipcode = _senderAddress.Zipcode,
                    city = _senderAddress.City,
                    country_code = _senderAddress.CountryId
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
        public async Task<string> CreateShipment(OrderOverview orderOverview, Address reciverAddress, int weight)
        {
            using var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Post,
                new Uri(_urlV3 + "shipments"));


            var sendData = new
            {
                test_mode = true,
                own_agreement = false,
                product_code = orderOverview.Product_code,
                automatic_select_service_point = false,
                service_codes = "EMAIL_NT,SMS_NT",
                sender = new
                {
                    name = _senderAddress.Company,
                    address1 = _senderAddress.Address1,
                    zipcode = _senderAddress.Zipcode,
                    city = _senderAddress.City,
                    country_code = _senderAddress.CountryId,
                    email = _senderAddress.Email
                },
                receiver = new
                {
                    name = reciverAddress.Firstname + " " + reciverAddress.Lastname,
                    address1 = reciverAddress.Address1,
                    zipcode = reciverAddress.Zipcode,
                    city = reciverAddress.City,
                    country_code = reciverAddress.CountryId,
                    email = reciverAddress.Email,
                    mobile = reciverAddress.Phone
                },
                parcels = new[] { new { weight } },
                service_point = orderOverview.HasServicePoint ? new
                {
                    id = orderOverview.ServicePointId,
                    name = orderOverview.ServicePointName,
                    address1 = orderOverview.Address,
                    zipcode = orderOverview.Zipcode,
                    city = orderOverview.City,
                    country_code = orderOverview.CountryId
                } : null
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
