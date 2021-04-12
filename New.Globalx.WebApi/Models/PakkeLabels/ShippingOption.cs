using System.Collections.Generic;

namespace New.Globalx.WebApi.Models.PakkeLabels
{
    public class ShippingOption
    {
        public string dispaly_string { get; set; }
        public string carrier_code { get; set; }
        public string description { get; set; }
        public string product_code { get; set; }
        public string price { get; set; }
        public string price_before_vat { get; set; }
        public List<PickupPoint> pickup_points { get; set; }
        public bool has_pickup_points { get; set; }
    }
}
