﻿using Microsoft.AspNetCore.Mvc;
using ProductMicroservices.Models;
using ProductMicroservices.Repositories;
using System.Transactions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProductMicroservices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _repository;

        public ProductController(IProductRepository repository)
        {
            _repository = repository;
        }


        // GET: api/<ProductController>
        [HttpGet("GetAllProducts")]
        public IActionResult Get()
        {
            var products = _repository.GetAllProducts();
            return new OkObjectResult(products);

        }

        // GET api/<ProductController>/5
        [HttpGet("getProduct/{id}")]
        public IActionResult Get(int id)
        {
            var product = _repository.GetProductById(id);
            return new OkObjectResult(product);
        }

        // POST api/<ProductController>
        [HttpPost("addProduct")]
        public IActionResult Post([FromBody] Product product)
        {
            using(var scope = new TransactionScope())
            {
                _repository.InsertProduct(product);
                scope.Complete();
                return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
            }
        }

        // PUT api/<ProductController>/5
        [HttpPut("updateProduct/{id}")]
        public IActionResult Put(int id, [FromBody] Product product)
        {
            if(product != null)
            {
                using(var scope = new TransactionScope())
                {
                    _repository.UpdateProduct(product);
                    scope.Complete();
                    return new OkResult();
                }
            }
            return new NoContentResult();
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            _repository.DeleteProduct(id);
            return new OkResult();
        }
    }
}
