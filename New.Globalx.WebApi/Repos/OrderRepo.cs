using System;
using System.Collections.Generic;
using System.Linq;
using New.Globalx.WebApi.Models;

namespace New.Globalx.WebApi.Repos
{
    public class OrderRepo : BaseRepo
    {
        public Order CreateNewOrder(string userId, string ip)
        {
            var orderUid = Guid.NewGuid().ToString();

            var paramDic = new Dictionary<string, object>
            {
                {"@userUid", userId}, {"@orderUid", orderUid}, {"@ip", ip}
            };

            ExecuteSp("xOrder_Create", paramDic);

            return new Order()
            {
                Id = orderUid,
                Number = "R335381310"
            };
        }


        public bool UpdateOrder(Order orderToUpdate)
        {
            var cmd = orderToUpdate.Special_Instructions;

            if (string.IsNullOrEmpty(cmd))
            {
                return false;
            }

            return cmd switch
            {
                "updatePayment" => SetPaymentDoneAndCreateShipment(orderToUpdate),
                "updateShipment" => SetShipmentSent(orderToUpdate.Id, orderToUpdate.AddressUid),
                "addLineItem" => CreateOrderLineOnOrder(orderToUpdate),
                "deleteLineItem" => DeleteOrderLineFromOrder(orderToUpdate),
                "updateLineItem" => UpdateOrderLine(orderToUpdate),
                _ => true
            };
        }
        private bool UpdateOrderLine(Order order)
        {
            if (order.line_items == null)
                return false;

            foreach (var orderLine in order.line_items
                .Where(orderLine => orderLine.Updated))
            {
                UpdateOrderLine(order.Id, orderLine);
            }


            return true;
        }
        private bool DeleteOrderLineFromOrder(Order order)
        {
            if (order.line_items == null)
                return false;

            foreach (var orderLine in order.line_items
                .Where(orderLine => orderLine.Deleted))
            {
                DeleteOrderLine(order.Id, orderLine);
            }

            return true;
        }
        private bool CreateOrderLineOnOrder(Order order)
        {
            if (order.line_items == null)
                return false;

            foreach (var orderLine in order.line_items
                .Where(orderLine => orderLine.NewLine))
            {
                CreateOrderLine(order.Id, orderLine);
            }

            return true;
        }
        public bool DeleteOrderLine(string orderId, OrderLine line)
        {
            var paramDic = new Dictionary<string, object>
            {
                { "@orderId", orderId }, { "@prodUid", line.Id }
            };

            ExecuteSp("xOrderLine_Delete", paramDic);

            return true;
        }
        public Guid CreateOrderLine(string orderId, OrderLine line)
        {
            var orderLineUid = Guid.NewGuid();

            var paramDic = new Dictionary<string, object>
            {
                {"@orderId", orderId},
                {"@orderLineUid", orderLineUid.ToString()},
                {"@prodUid", line.Variant_Id},
                {"@price", line.Price},
                {"@quant", line.Quantity},
                {"@size", line.Size},
                {"@color", line.Color}
            };

            ExecuteSp("xOrderLine_Create", paramDic);

            return orderLineUid;
        }
        public Guid UpdateOrderLine(string orderId, OrderLine line)
        {
            var orderLineUid = Guid.NewGuid();

            var paramDic = new Dictionary<string, object>
            {
                {"@orderId", orderId},
                {"@variantId", line.Variant_Id},
                {"@quant", line.Quantity}
            };

            ExecuteSp("orderline_update", paramDic);

            return orderLineUid;
        }

        public bool SetPaymentDoneAndCreateShipment(Order order)
        {
            var paramDic = new Dictionary<string, object>
            {
                {"@orderId", order.Id}, {"@addressUid", order.AddressUid}, {"@shipTotal", order.Ship_Total}
            };

            ExecuteSp("xOrder_SetPaymentDone", paramDic);

            //pakke label api
            //var str = PakkeLabelsApiClient.CreateImportedShipment(new xAddress()
            //    {
            //        firstname = order.ship_address.firstname,
            //        address1 = order.ship_address.address1,
            //        countryId = order.ship_address.countryId,
            //        phone = order.ship_address.phone,
            //        zipcode = order.ship_address.zipcode,
            //        city = order.ship_address.city
            //    },
            //    order.ship_address.email,
            //    order.id,
            //    order.deliveryCode,
            //    order.shippingId);


            return true;
        }
        public bool SetShipmentSent(string orderId, string addressUid)
        {
            var paramDic = new Dictionary<string, object> { { "@orderId", orderId }, { "@addressUid", addressUid } };

            ExecuteSp("xOrder_SetShipmentSent", paramDic);

            return true;
        }
        public bool CheckIfOrderExistsOnEmail(string email)
        {
            var paramDic = new Dictionary<string, object> { { "@email", email } };
            return GetSingle<int>("xOrder_Check", paramDic) == 1;
        }
        public bool CheckIfOrderExistsOnIp(string ip)
        {
            var paramDic = new Dictionary<string, object> { { "@ip", ip } };

            return GetSingle<int>("xOrder_CheckNoUser", paramDic) == 1;
        }
        public Order GetCurrentOrderByIp(string ip)
        {
            var paramDic = new Dictionary<string, object> { { "@ip", ip } };

            var currentOrder = GetSingle<Order>("xOrder_GetCurrentNoUser", paramDic);

            if (currentOrder.Id == null) return currentOrder;

            var paramDic2 = new Dictionary<string, object>
            {
                {"@email", currentOrder.Email}, {"@orderId", currentOrder.Id}
            };

            currentOrder.line_items = GetList<OrderLine>("xOrder_GetLine", paramDic2).ToList();

            foreach (var lineItem in currentOrder.line_items)
            {
                lineItem.Variant_Id = lineItem.Id;
            }

            return currentOrder;
        }

        public Order GetCurrentOrderByEmail(string email)
        {
            var paramDic = new Dictionary<string, object> { { "@email", email } };

            var currentOrder = GetSingle<Order>("xOrder_GetCurrent", paramDic);
            if (currentOrder.Id == null) return currentOrder;

            var paramDic2 = new Dictionary<string, object>
            {
                { "@email", email }, { "@orderId", currentOrder.Id }
            };

            var linesItem = GetList<OrderLine>("xOrder_GetLine", paramDic2);

            var orderLines = linesItem as OrderLine[] ?? linesItem.ToArray();

            foreach (var lineItem in orderLines)
            {
                lineItem.Variant_Id = lineItem.Id;
            }

            currentOrder.line_items = orderLines;

            return currentOrder;
        }
        public Guid CreateOrderFromIp(string userId)
        {
            var orderUid = Guid.NewGuid();

            var paramDic = new Dictionary<string, object>
            {
                {"@ip", userId}, {"@orderUid", orderUid.ToString()}
            };

            ExecuteSp("xOrder_CreateNoUser", paramDic);

            return orderUid;
        }
        public Guid CreateOrderFromEmail(string userUid, string ip)
        {
            var orderUid = Guid.NewGuid();

            var paramDic = new Dictionary<string, object>
            {
                {"@userUid", userUid}, {"@orderUid", orderUid.ToString()}, {"@ip", ip}
            };


            ExecuteSp("xOrder_Create", paramDic);

            return orderUid;
        }

        //public Order GetCurrentOrderNoUser(string userId)
        //{
        //    var paramDic = new Dictionary<string, object> { { "@ip", userId } };

        //    var currOrder = GetSingle<Order>("xOrder_GetCurrentNoUser", paramDic);

        //    if (currOrder.Id == null) return currOrder;

        //    var paramDic2 = new Dictionary<string, object>
        //    {
        //        { "@email", currOrder.Email }, { "@orderId", currOrder.Id }
        //    };

        //    currOrder.line_items = GetList<OrderLine>("xOrder_GetLine", paramDic2).ToList();

        //    return currOrder;
        //}
    }
}
