﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DutchTreat.Data.Entities;
using DutchTreatAdvanced.Data;
using DutchTreatAdvanced.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DutchTreatAdvanced.Controllers
{
    [Route("api/[Controller]")]
    public class OrdersController: Controller

    {
        private readonly IDutchRepository _repository;
        private readonly ILogger<OrdersController> _logger;
        private readonly IMapper _mapper;

        public OrdersController(IDutchRepository repository, ILogger<OrdersController> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        // always set a default value for query strings so don't need to include it in every case
        // By using optional parameter, we can combine two endpoints together :
        // Get(bool includeItems) & Get()
        public IActionResult Get(bool includeItems = true)
        {
            try
            {
                var results = _repository.GetOrders(includeItems);
                // only need to call map at the top level, autoMapper will walk down to children and map the best it can
                // however, children mapping definition are required
                return Ok(_mapper.Map<IEnumerable<OrderViewModel>>(results));
            }
            catch (Exception e)
            {
                _logger.LogError($"Fail to get orders: {e}");
                return BadRequest();
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            try
            {
                var order = _repository.GetOrderById(id);
                if (order != null) return Ok(_mapper.Map<Order,OrderViewModel>(order));
                // Not found makes more sense than a bad request
                else return NotFound();

            }
            catch (Exception e)
            {
                _logger.LogError($"Fail to get orders: {e}");
                return BadRequest();
            }
        }

        [HttpPost]
        // without [FromBody], it assumes that the input is coming from the query string
        public IActionResult Post([FromBody] OrderViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var newOrder = _mapper.Map<OrderViewModel, Order>(model);

                if(newOrder.OrderDate == DateTime.MinValue)
                    newOrder.OrderDate = DateTime.Now;

                // push any data attached to the context so they will be saved to DB
                _repository.AddEntity(newOrder);
                if (_repository.SaveAll())
                {
                    var newModel = _mapper.Map<Order, OrderViewModel>(newOrder);

                    // In HTTP, in a POST, if an object is created, it must return a "created"
                    // 201 instead of 200 for OK
                    // Pass ID back since there could be more fields generated by the server so users will know this is the actual latest version of the object created
                    // The second parameter is the return value
                    // The first parameter become the value for "location" attribute in response header  
                    return Created($"/api/orders/{newModel.OrderId}", newModel);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to save a new order: {e}");
            }

            return BadRequest("Failed to save new order.");

        }
    }
}
