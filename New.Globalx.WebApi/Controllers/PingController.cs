using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using New.Globalx.WebApi.Clients;

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
            //var token = await _pakkeLabelsApiClient.GetQuotesList();

            //var carriersTest = await _pakkeLabelsApiClient.GetCarriers();
            //var prodsTest = await _pakkeLabelsApiClient.GetProducts();

            //var pickupPoints = await _pakkeLabelsApiClient.GetPickupPointsTest(token);

            //var freightRates = _pakkeLabelsApiClient.GetFreightRatesByCountry("DK", token);

            var pingString = $@"
[web api is running:: True]
[db state:: {GetDbSate()}]";

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
    }
}
