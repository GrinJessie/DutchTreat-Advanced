using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DutchTreatAdvanced.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DutchTreatAdvanced.Controllers
{
    [Route("api/[Controller]")]
    public class OrdersController: Controller

    {
        private readonly IDutchRepository _repository;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IDutchRepository repository, ILogger<OrdersController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public ActionResult Get()
        {
            try
            {
                return Ok(_repository.GetOrders());
            }
            catch (Exception e)
            {
                _logger.LogError($"Fail to get orders: {e}");
                return BadRequest();
            }
        }
    }
}
