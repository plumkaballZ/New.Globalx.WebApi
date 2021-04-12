using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using New.Globalx.WebApi.Clients;
using New.Globalx.WebApi.Models.PakkeLabels;
using Newtonsoft.Json;

namespace New.Globalx.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class PakkeLabelsController : ControllerBase
    {
        private readonly PakkeLabelsApiClient _pakkeLabelsApiClient = new PakkeLabelsApiClient();

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
    }
}
