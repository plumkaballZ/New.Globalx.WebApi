using System;
using Microsoft.AspNetCore.Mvc;
using New.Globalx.WebApi.Models;
using New.Globalx.WebApi.Repos;

namespace New.Globalx.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly OrderRepo _orderRepo = new OrderRepo();
        private readonly AddressRepo _addressRepo = new AddressRepo();
        private readonly UserRepo _userRepo = new UserRepo();

        [HttpGet("{ip}/{email}/{pw}")]
        public IActionResult Get([FromRoute] string ip, string email, string pw)
        {
            //userId is either email (if signed up)
            //or random guid, which becomes userUid when signed up... I think
            var loginUid = _userRepo.LoginUser(email, pw);

            if (loginUid == null)
            {
                if (Guid.TryParse(ip, out _))
                {
                    if (!_orderRepo.CheckIfOrderExistsOnIp(ip))
                    {
                        _orderRepo.CreateOrderFromIp(ip);
                    }

                    var currentOrderWitNoUser = _orderRepo.GetCurrentOrderByIp((ip));

                    return Ok(currentOrderWitNoUser);
                }
            }

            var userUid = _userRepo.GetUser(loginUid).Uid;


            if (!_orderRepo.CheckIfOrderExistsOnEmail(email))
            {
                if (_orderRepo.CheckIfOrderExistsOnIp(ip))
                {
                    _orderRepo.UpdateOrderIp(userUid, ip);
                }
                else
                {
                    _orderRepo.CreateOrderFromEmail(userUid, ip);
                }
            }

            var currentOrder = _orderRepo.GetCurrentOrderByEmail(email);

            return Ok(currentOrder);
        }

        [HttpPost]
        [Route("update")]
        public IActionResult UpdateOrder([FromBody] GlobalRequest globalRequest)
        {
            var order = globalRequest.Order;

            if (order == null)
            {
                return BadRequest("order cannot be null");
            }

            var res = _orderRepo.UpdateOrder(order);

            return Ok(order);
        }

        [HttpGet("setordersent/{orderId}/{addressUid}")]
        public IActionResult SetOrderSent(string orderId, string addressUid)
        {

            if (string.IsNullOrEmpty(orderId))
            {
                return BadRequest("orderId cannot be null");
            }

            var res = _orderRepo.SetShipmentSent(orderId, addressUid);

            return Ok(res);
        }


        [HttpGet("getall/{email}/{ip}")]
        public IActionResult GetAll(string email, string ip)
        {
            var allOrders = _orderRepo.GetAll(email, ip);

            foreach (var order in allOrders)
            {
                order.Ship_Address = _addressRepo.Get(order.AddressUid);
                var pickedServicePoint = _addressRepo.GetPickedServicePoint(order.Uid);

                if (pickedServicePoint != null)
                {
                    pickedServicePoint.carrierCode = GetDisplayStringForQuote(pickedServicePoint.carrierCode);
                    order.Picked_ServicePoint = pickedServicePoint;
                }
            }

            return Ok(allOrders);
        }

        [HttpGet("getalllvl99")]
        public IActionResult GetAllLvl99()
        {
            var allOrders = _orderRepo.GetAll_lvl99();

            foreach (var order in allOrders)
            {
                order.Ship_Address = _addressRepo.Get(order.AddressUid);

                var pickedServicePoint = _addressRepo.GetPickedServicePoint(order.Uid);

                if (pickedServicePoint != null)
                {
                    pickedServicePoint.carrierCode = GetDisplayStringForQuote(pickedServicePoint.carrierCode);
                    order.Picked_ServicePoint = pickedServicePoint;
                }
            }

            return Ok(allOrders);
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
