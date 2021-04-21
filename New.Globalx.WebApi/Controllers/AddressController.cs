using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using New.Globalx.WebApi.Clients;
using New.Globalx.WebApi.Models;
using New.Globalx.WebApi.Models.PakkeLabels;
using New.Globalx.WebApi.Repos;

namespace New.Globalx.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly AddressRepo _addressRepo = new AddressRepo();
        private readonly PakkeLabelsApiClient _pakkeLabelsApiClient = new PakkeLabelsApiClient();

        [HttpGet]
        public IActionResult Get(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest($"{nameof(id)} must be specified");
            }

            return Ok(_addressRepo.Get(id));
        }

        [HttpGet]
        [HttpGet("GetAll/{userId}")]
        public async Task<IActionResult> GetAll(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest($"{nameof(userId)} must be specified");
            }

            var allAddresses = _addressRepo.GetAll(userId);

            if (allAddresses == null || !allAddresses.Any()) return Ok(new
            {
                allAddresses
            });

            var firstAddress = allAddresses.FirstOrDefault();

            var shippingOptions = await GetShippingOptions(firstAddress, 1000);

            return Ok(new
            {
                allAddresses,
                shippingOptions
            });
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Address address)
        {
            var res = string.IsNullOrEmpty(address.Uid) ? _addressRepo.Create(address) : null;

            List<ShippingOption> shippingOptions;

            if (res != null)
            {
                shippingOptions = await GetShippingOptions(res, 1000);
            }
            else
            {
                shippingOptions = null;
            }

            return Ok(new
            {
                address = res,
                shippingOptions
            });
        }

        [HttpPost("getshippingoptions")]
        public async Task<IActionResult> GetAllShippingOptions([FromBody] Address address)
        {
            if (address == null)
            {
                return Ok();
            }

            var shippingOptions = await GetShippingOptions(address, 1000);

            return Ok(shippingOptions);
        }
        [HttpGet]
        [HttpGet("delete/{addressUid}")]
        public IActionResult Delete(string addressUid)
        {
            var res = _addressRepo.Delete(addressUid);

            return Ok(res);
        }

        private async Task<List<ShippingOption>> GetShippingOptions(Address address, int weight)
        {
            //pakke til udleveringssted
            var pickupProds = new List<string>()
            {
                "DAO_STS",
                "GLSDK_SD",
                "PDK_MC"
            };

            //pakke til privat
            var privateDeliveryProds = new List<string>()
            {
                "DAO_STH",
                "PDK_MH",
                "PDK_EMS"
            };

            var allProds = pickupProds.Concat(privateDeliveryProds);

            var quotes = await _pakkeLabelsApiClient.GetQuotesByAddress(address, 1000);

            var shippingOpts = new List<ShippingOption>();


            foreach (var pickupQuote in quotes.Where(x => pickupProds.Contains(x.product_code)))
            {
                var servicePoints = await _pakkeLabelsApiClient.GetServicePoints(address, pickupQuote.carrier_code);

                var hasServicePoints = servicePoints.Any();

                var opt = new ShippingOption()
                {
                    dispaly_string = GetDisplayStringForQuote(pickupQuote.product_code),
                    carrier_code = pickupQuote.carrier_code,
                    description = pickupQuote.description,
                    product_code = pickupQuote.product_code,
                    price = pickupQuote.price,
                    price_before_vat = pickupQuote.price_before_vat,
                    service_points = hasServicePoints ? servicePoints : null,
                    has_service_points = hasServicePoints
                };

                shippingOpts.Add(opt);

            }

            foreach (var pickupQuote in quotes.Where(x => privateDeliveryProds.Contains(x.product_code)))
            {

                var opt = new ShippingOption()
                {
                    dispaly_string = GetDisplayStringForQuote(pickupQuote.product_code),
                    carrier_code = pickupQuote.carrier_code,
                    description = pickupQuote.description,
                    product_code = pickupQuote.product_code,
                    price = pickupQuote.price,
                    price_before_vat = pickupQuote.price_before_vat,
                    service_points = null,
                    has_service_points = false
                };

                shippingOpts.Add(opt);

            }

            return shippingOpts;
        }

        private string GetDisplayStringForQuote(string productCode)
        {
            return productCode switch
            {
                "DAO_STS" => "dao - Pakke til udleveringssted",
                "DAO_STH" => "dao - Pakke til privat",
                "GLSDK_SD" => "GLS Denmark - Pakke til pakkeshop",
                "PDK_MC" => "PostNord - Pakke til udleveringssted",
                "PDK_MH" => "PostNord - Pakke til privat",
                "PDK_EMS" => "PostNord - Pakke til privat og erhverv",
                _ => "notfound"
            };
        }
    }
}
