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
                        _orderRepo.CreateOrderFromIp(ip);

                    var currentOrderWitNoUser = _orderRepo.GetCurrentOrderByIp((ip));

                    return Ok(currentOrderWitNoUser);
                }
            }

            var userUid = _userRepo.GetUser(loginUid).Uid;


            if (!_orderRepo.CheckIfOrderExistsOnEmail(email))
            {
                _orderRepo.CreateOrderFromEmail(userUid, ip);
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

            Console.WriteLine(res);

            return Ok(order);
        }
    }

}
