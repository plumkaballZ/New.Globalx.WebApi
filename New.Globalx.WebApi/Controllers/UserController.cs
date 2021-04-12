using Microsoft.AspNetCore.Mvc;
using New.Globalx.WebApi.Repos;

namespace New.Globalx.WebApi.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly UserRepo _userRepo = new UserRepo();

        [HttpGet("{email}/{password}")]
        public IActionResult Get([FromRoute] string email, [FromRoute] string password)
        {
            if (_userRepo.CheckUserLogin(email))
            {
                var refUid = _userRepo.LoginUser(email, password);

                if (refUid != null)
                {
                    return Ok(_userRepo.GetUser(refUid));
                }


                //return refUid == null ? Ok(new ReqRes() { error = true, msg = "wrong password" }) : Json(_xUserRepo.GetSignle(refUid));
            }

            return BadRequest();

            //return Json(new ReqRes() { error = true, msg = "cannot find you :(" });
        }
    }
}
