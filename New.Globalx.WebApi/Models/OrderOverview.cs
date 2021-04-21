namespace New.Globalx.WebApi.Models
{
    public class OrderOverview
    {
        public string OrderId { get; set; }

        public string AddressUid { get; set; }
        //public int SubTotal { get; set; }
        //public int ShippingPrice { get; set; }
        //public int TotalQuantity { get; set; }

        public string TotalPrice { get; set; }

        public string ServicePointName { get; set; }
        public bool HasServicePoint { get; set; }
        public int ServicePointId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public string Address { get; set; }
        public string Zipcode { get; set; }
        public string City { get; set; }
        public string CountryId { get; set; }
        public string Product_code { get; set; }
    }
}

