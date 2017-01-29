using System;
using System.Collections.Generic;
using System.Linq;
using APM.Application.Data;
using APM.Application.Data.Migrations;
using APM.Application.Models;
using Microsoft.AspNetCore.Mvc;
using Remotion.Linq.Parsing;

namespace APM.Application.Controllers
{
    [Route("api/Products")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Product> list = new List<Product>();
            foreach (Product product in _context.Products)
                list.Add(product);
            return View(list);
        }

        [HttpGet()]
        public IActionResult GetProducts()
        {
            try
            {
                return Ok(_context.Products.ToList());
            }
            catch (Exception)
            {
            }
            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult GetProducts(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
            return Ok(product);
        }

        public IActionResult Create([FromBody] Product product)
        {
            return Ok();
        }
    }
}