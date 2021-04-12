using System;
using System.Collections.Generic;

namespace New.Globalx.WebApi.Models
{
    public class Order
    {
        public string AddressUid { get; set; }
        public string Id { get; set; }
        public string Number { get; set; }
        public int Ship_Total { get; set; }
        public string State { get; set; }
        public string Adjustment_Total { get; set; }
        public string User_Id { get; set; }
        public DateTime Created_At { get; set; }
        public string Updated_At { get; set; }
        public string Completed_At { get; set; }
        public string Payment_Total { get; set; }
        public string Shipment_State { get; set; }
        public string Payment_State { get; set; }
        public string Email { get; set; }
        public string Special_Instructions { get; set; }
        public string Channel { get; set; }
        public string Included_Tax_Total { get; set; }
        public string Additional_Tax_Total { get; set; }
        public string Display_Included_Tax_Total { get; set; }
        public string Display_Additional_Tax_Total { get; set; }
        public string Tax_Total { get; set; }
        public string Currency { get; set; }
        public string Considered_Risky { get; set; }
        public string Canceler_Id { get; set; }
        public int Total_Quantity { get; set; }
        public string Token { get; set; }

        public IEnumerable<OrderLine> line_items;

        public Address Bill_Address { get; set; }
        public Address Ship_Address { get; set; }


        public string DeliveryCode;
        public string ShippingId;

        public static Order CreateNewEmptyOrder()
        {
            return new Order()
            {
                line_items = new List<OrderLine>()
            };
        }
    }
}
