using System;
using System.Collections.Generic;
using System.Linq;
using New.Globalx.WebApi.Models;

namespace New.Globalx.WebApi.Repos
{
    public class OrderRepo : BaseRepo
    {
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

            if (order.Picked_ServicePoint != null)
            {
                CreatePickedServicePoint(order.Picked_ServicePoint, order.Uid);
            }


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

        public List<Order> GetAll(string email, string ip)
        {

            var paramDic = new Dictionary<string, object> { { "@email", email }, { "@ip", ip } };

            var orders = GetList<Order>("xOrder_GetAll", paramDic).ToList();

            foreach (var order in orders)
            {
                var paramDic2 = new Dictionary<string, object>
                {
                    {"@email", order.Email}, {"@orderId", order.Id}
                };

                order.line_items = GetList<OrderLine>("xOrder_GetLine", paramDic2).ToList();
            }

            return orders;
        }

        public Order GetByOrderId(string email, string orderId)
        {
            var paramDic = new Dictionary<string, object> { { "@email", email }, { "@orderId", orderId } };

            var order = GetSingle<Order>("xOrder_Get", paramDic);

            if (order != null)
                order.line_items = GetList<OrderLine>("xOrder_GetLine", paramDic);

            return order;
        }

        public List<Order> GetAll_lvl99()
        {
            var orders = GetList<Order>("xOrder_GetAll_lvl99", new Dictionary<string, object>()).ToList();

            foreach (var order in orders)
            {
                var paramDic = new Dictionary<string, object>
                {
                    {"@email", order.Email}, {"@orderId", order.Id}
                };

                order.line_items = GetList<OrderLine>("xOrder_GetLine", paramDic).ToList();
            }

            return orders;

        }

        public bool UpdateOrderIp(string userUid, string ip)
        {

            var paramDic = new Dictionary<string, object> { { "@userUid", userUid }, { "@ip", ip } };


            ExecuteSp("xOrder_Update_Ip", paramDic);

            return true;
        }

        public string CreatePickedServicePoint(PickedServicePoint pickedService, string orderUid)
        {
            var paramDic = new Dictionary<string, object>();

            var uid = Guid.NewGuid().ToString();

            paramDic.Add("@uid", uid);
            paramDic.Add("@orderUid", orderUid);
            paramDic.Add("@carrierCode", pickedService.carrierCode);
            paramDic.Add("@companyName", pickedService.companyName);
            paramDic.Add("@address", pickedService.address);
            paramDic.Add("@zipcode", pickedService.zipcode);
            paramDic.Add("@city", pickedService.city);

            ExecuteSp("PickedServicePoint_Create", paramDic);

            return uid;
        }

    }
}
