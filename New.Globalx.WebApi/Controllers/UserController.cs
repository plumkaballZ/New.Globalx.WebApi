using Microsoft.AspNetCore.Mvc;
using New.Globalx.WebApi.Models;
using New.Globalx.WebApi.Repos;

namespace New.Globalx.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserRepo _userRepo = new UserRepo();

        [HttpGet("{email}/{password}")]
        public IActionResult Get([FromRoute] string email, [FromRoute] string password)
        {
            var resString = "not found";

            if (_userRepo.CheckUserLogin(email))
            {
                var refUid = _userRepo.LoginUser(email, password);

                if (refUid != null)
                {
                    var user = _userRepo.GetUser(refUid);
                    resString = "ok";

                    return Ok(new
                    {
                        resString,
                        user
                    });
                }

                resString = "wrong password";
            }

            return Ok(new
            {
                resString
            });

        }


        [HttpPost]
        public IActionResult Post([FromBody] UserLogin userLogin)
        {
            var resString = "user already exists";

            if (!_userRepo.CheckUserLogin(userLogin.Email))
            {
                var loginUid = _userRepo.CreateUserLogin(userLogin);
                _userRepo.CreateUser(loginUid);
                resString = "ok";

                return Ok(new
                {
                    resString,
                    user = userLogin
                });
            }

            return Ok(new { resString });
        }

        [HttpPost]
        [Route("UpdateUser")]
        public IActionResult UpdateUser([FromBody] User user)
        {
            return Ok(_userRepo.Update(user));
        }
    }
}
