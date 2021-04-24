using Deliveggie.Backend.Services;
using Deliveggie.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Deliveggie.Backend.Controllers
{
    [Route("product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            var result = new RabbitMqService().GetProductList(new ProductRequest { });
            return Ok(result.Products);
        }

        [Route("details")]
        [HttpGet]
        public IActionResult GetProductById(string id)
        {
            var result = new RabbitMqService().GetDetails(new ProductDetailsRequest() { Id = id });
            return Ok(result);
        }
    }
}
