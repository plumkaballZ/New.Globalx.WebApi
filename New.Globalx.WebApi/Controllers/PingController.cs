using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Mime;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using New.Globalx.WebApi.Clients;
using New.Globalx.WebApi.Models;
using New.Globalx.WebApi.Models.PakkeLabels;

namespace New.Globalx.WebApi.Controllers
{
    [ApiController]
    [Route("api/ping")]
    public class PingController : ControllerBase
    {
        private readonly PakkeLabelsApiClient _pakkeLabelsApiClient = new PakkeLabelsApiClient();

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var pingString = $@"
[web api is running:: true]
[database state:: {GetDbSate()}]
[pakkelabelsapi found number of quotes:: {GetTestQuotes().Result.Count}]";

            return Ok(pingString);
        }

        private ConnectionState GetDbSate()
        {
            var connectionStringBuilder = new MySqlConnectionStringBuilder
            {
                Server = "62.75.168.220",
                Database = "Globase",
                UserID = "superErbz",
                Password = "Jqi5fqfb",
                ConnectionTimeout = 60,
                Port = 3306,
                ConvertZeroDateTime = true
            };

            try
            {
                var connection = new MySqlConnection(connectionStringBuilder.ConnectionString);
                connection.Open();

                return connection.State;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        private async Task<List<ShippingQuote>> GetTestQuotes()
        {
            var senderAddress = new Address()
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
            var quotes = await _pakkeLabelsApiClient.GetQuotesByAddress(senderAddress, 1000);

            return quotes;
        }
    }
}
