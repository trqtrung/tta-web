using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TTA.Api.Data;
using TTA.Api.Models;
using TTA.Api.ViewModels;
using TTA.Api.Helpers;
using Microsoft.Extensions.Logging;

namespace TTA.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger _logger;

        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context, ILogger<ProductsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        //public IEnumerable<Product> Get()
        //{
        //    return _context.Products.ToList();
        //}

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _context.Products.FindAsync(id));
        }

        //[EnableCors("AllowSpecificOrigin")]
        //[DisableCors]
        public async Task<IEnumerable<ProductsViewModel>> Get([FromQuery] string keyword)
        {
            var products = (from p in _context.Products
                            join b in _context.Brands on p.BrandID equals b.Id into p_b
                            from t1 in p_b.DefaultIfEmpty()
                            join s in _context.SellingPrices on p.Id equals s.ProductId into p_s
                            from t2 in p_s.DefaultIfEmpty()
                            join i in _context.BuyingPrices on p.Id equals i.ProductId into p_i
                            from t3 in p_i.DefaultIfEmpty()
                            join o in _context.OptionLists on p.Type equals o.Id into p_t
                            from t4 in p_t.DefaultIfEmpty()
                            orderby p.Name
                            select new ProductsViewModel { ID = p.Id,
                                Brand = (t1.Name == string.Empty ? "No-Brand" : t1.Name ),
                                Name = string.IsNullOrEmpty(p.Name) ? "" : p.Name,
                                SKU = string.IsNullOrEmpty(p.SKU) ? "" : p.SKU,
                                Price = (t2.Price > 0 ? t2.Price : 0),
                                BuyingPrice = (t3.Price > 0  ? t3.Price : 0),
                                Description = p.Description,
                                Type = t4.Name}
                            );

            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.ToLower();
                products = products.Where(x => x.Brand.ToLower().Contains(keyword) || x.Description.ToLower().Contains(keyword) || x.Name.ToLower().Contains(keyword) || x.Type.ToLower().Contains(keyword) || x.SKU.ToLower().Contains(keyword));
            }
            string sql = products.ToSql();

            //Console.WriteLine(sql);
            _logger.LogError(sql);
            List<ProductsViewModel> list = new List<ProductsViewModel>();

            //try
            //{               
            //    list = await products.ToListAsync();
            //}
            //catch(Exception ex)
            //{
            //    _logger.LogError(ex.Message);
            //}
            return products;
        }

        [HttpPost]
        //[ProducesResponseType(201)]
        //[ProducesResponseType(400)]
        public IActionResult CreateAsync([FromBody]Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(_context.Products.Any(p => p.Name.Contains(product.Name)))
            {
                return BadRequest("Product already exists");
            }

            product.Created = DateTime.Now;
            _context.Products.Add(product);
            _context.SaveChanges();

            //update new price
            double currentPrice = _context.SellingPrices.Where(x => x.ProductId == product.Id).OrderByDescending(x => x.PriceDate).Select(x => x.Price).FirstOrDefault();

            if(currentPrice != product.Price)
            {
                SellingPrice sp = new SellingPrice();
                sp.Price = product.Price;
                sp.ProductId = product.Id;
                sp.PriceDate = DateTime.Now;
                sp.QuantityFrom = 1;

                _context.SellingPrices.Add(sp);
                _context.SaveChanges();
            }

            //update import price
            double importPrice = _context.BuyingPrices.Where(x => x.ProductId == product.Id).OrderByDescending(x => x.PriceDate).Select(x=>x.Price).FirstOrDefault();

            if(importPrice != product.BuyingPrice)
            {
                BuyingPrice bp = new BuyingPrice();
                bp.Price = product.BuyingPrice;
                bp.PriceDate = DateTime.Now;
                bp.ProductId = product.Id;                

                if (product.SupplierID > 0)
                    bp.SupplierId = product.SupplierID;

                _context.BuyingPrices.Add(bp);
                _context.SaveChanges();
            }

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        //[HttpPost]
        //public Task<IActionResult> Create([FromBody]Product product)
        //{
        //    return Json(true);
        //}

        //[HttpPut]
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]Product value)
        //{
        //    _context.Products.Update(value);
        //    _context.SaveChangesAsync();
        //}

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                //update new price
                double currentPrice = _context.SellingPrices.Where(x => x.ProductId == product.Id).OrderByDescending(x => x.PriceDate).Select(x => x.Price).FirstOrDefault();

                if (product.Price > 0 && currentPrice != product.Price)
                {
                    SellingPrice sp = new SellingPrice();
                    sp.Price = product.Price;
                    sp.ProductId = product.Id;
                    sp.PriceDate = DateTime.Now;
                    sp.QuantityFrom = 1;

                    _context.SellingPrices.Add(sp);
                    _context.SaveChanges();
                }

                //update import price
                double importPrice = _context.BuyingPrices.Where(x => x.ProductId == product.Id).OrderByDescending(x => x.PriceDate).Select(x => x.Price).FirstOrDefault();

                if (product.BuyingPrice > 0 && importPrice != product.BuyingPrice)
                {
                    BuyingPrice bp = new BuyingPrice();
                    bp.Price = product.BuyingPrice;
                    bp.PriceDate = DateTime.Now;
                    bp.ProductId = product.Id;

                    if (product.SupplierID > 0)
                        bp.SupplierId = product.SupplierID;

                    _context.BuyingPrices.Add(bp);
                    _context.SaveChanges();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}