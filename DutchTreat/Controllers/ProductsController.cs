using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DutchTreat.Data.Entities;
using DutchTreatAdvanced.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DutchTreatAdvanced.Controllers
{
    // [Controller] - name of the controller, prefixing with "api"
    [Route("api/[Controller]")]
    // To tell this is an API controller - for API documentation purpose
    [ApiController]
    // To tell this API controller will always return "application/json" - for API documentation purpose
    [Produces("application/json")]
    public class ProductsController : ControllerBase
    {
        private readonly IDutchRepository _repository;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IDutchRepository repository, ILogger<ProductsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        // Method level - To tell the expected response type in normal cases - for API documentation purpose
        [ProducesResponseType(200)]
        // Use ActionResult instead of IEnumerable/Json to be flexible on return type
        // Can be further serialized according to the accept type from the request, eg xml or any newly invented data types
        // Tie the method calls to actual status codes
        // Json is the only built-in type that MVC 6 enables by default
        // IActionResult makes API documentation hard as it hides the actual retur type
        // So Use ActionResult<> to allow API to define what's returned
        public ActionResult<IEnumerable<Product>> Get()
        {
            try
            {
                return Ok(_repository.GetProducts());
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get products: {e}");
                return BadRequest();
            }
        }

    }
}
