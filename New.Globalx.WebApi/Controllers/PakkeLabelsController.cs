using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using New.Globalx.WebApi.Clients;
using New.Globalx.WebApi.Models;
using New.Globalx.WebApi.Models.PakkeLabels;
using New.Globalx.WebApi.Repos;
using Newtonsoft.Json;

namespace New.Globalx.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class PakkeLabelsController : ControllerBase
    {
        private readonly PakkeLabelsApiClient _pakkeLabelsApiClient = new PakkeLabelsApiClient();
        private readonly AddressRepo _addressRepo = new AddressRepo();

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var whitelistedProdCodes = new List<string>()
            {
                "GLSDK_SD",
                "PDK_MH",
                "PDK_MC",
                "dhl_express"
            };

            var shippingProducts = new List<ShippingProduct>();

            foreach (var prodCode in whitelistedProdCodes)
            {
                var prodsJson = await _pakkeLabelsApiClient.GetProductsByCode(prodCode);
                var prodsToAdd = JsonConvert.DeserializeObject<List<ShippingProduct>>(prodsJson);

                if (prodsToAdd != null)
                {
                    shippingProducts.AddRange(prodsToAdd);
                }
            }

            return Ok(shippingProducts);
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrderOverview orderOverview)
        {
            var address = _addressRepo.Get(orderOverview.AddressUid);

            var res = await _pakkeLabelsApiClient.CreateShipment(orderOverview, address, 1000);

            return Ok(res);
        }
    }
}
