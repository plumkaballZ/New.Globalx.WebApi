using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace New.Globalx.WebApi.Models.PakkeLabels
{
    public class ShippingProduct
    {
        public string code { get; set; }
        public string name { get; set; }
        public string sender_country_code { get; set; }
        public string receiver_country_code { get; set; }
        public string expected_transit_time { get; set; }
    }
}
