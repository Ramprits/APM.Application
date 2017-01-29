using System.Collections.Generic;
using System.Linq;
using APM.Application.Data;
using APM.Application.Models;
using Microsoft.AspNetCore.Mvc;

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
            foreach (var product in _context.Products)
                list.Add(product);
            return View(list);
        }

        [HttpGet()]
        public IActionResult GetProducts()
        {
            return Ok(_context.Products.ToList());
        }
    }
}