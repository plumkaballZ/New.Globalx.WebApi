using Microsoft.AspNetCore.Mvc;
using New.Globalx.WebApi.Repos;

namespace New.Globalx.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductRepo _productRepo = new ProductRepo();

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            var allProducts = _productRepo.GetAllProduct();

            return Ok(allProducts);
        }
    }
}
