using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTA.Api.Data;
using TTA.Api.Helpers;
using TTA.Api.Models;

namespace TTA.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger _logger;

        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context, ILogger<OrdersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<IEnumerable<Order>> GetOrdersAsync([FromQuery] UrlQuery urlQuery, [FromQuery] DateTime from, [FromQuery] DateTime to, [FromQuery] string client, [FromQuery] string stage, [FromQuery] string customer, [FromQuery] string phone, [FromQuery] string orderNo)
        {
            int? totalRecords = null;

            DateTime _from = DateTime.Today.AddDays(-60);
            DateTime _to = DateTime.Today.AddDays(1).AddSeconds(-1);

            if (from > DateTime.MinValue)
                _from = from;
            if (to > DateTime.MinValue && to > _from)
                _to = to;

            List<Order> orders = new List<Order>();

            var query = from o in _context.Orders where o.OrderDate >= _from && o.OrderDate <= _to select o;

            if (!string.IsNullOrEmpty(orderNo))
                query = query.Where(x => x.OrderNo.Contains(orderNo));

            if(!string.IsNullOrEmpty(customer))
                query = query.Where(x => x.CustomerName.Contains(customer));

            if (!string.IsNullOrEmpty(phone))
                query = query.Where(x => x.CustomerPhone.Contains(phone));

            if (!string.IsNullOrEmpty(client))
                query  = query.Where(x => x.Client == client);

            if (!string.IsNullOrEmpty(stage))
                query = query.Where(x => x.Stage == stage);

            query = query.OrderByDescending(x => x.OrderDate);

            if (urlQuery.PageNumber.HasValue && urlQuery.IncludeCount)
                totalRecords = query.Count();

            if (urlQuery.PageNumber.HasValue)
            {
                query = query.Skip((urlQuery.PageNumber.Value -1) * urlQuery.PageSize).Take(urlQuery.PageSize);

                Pagination p = new Pagination()
                {
                    PageNumber = urlQuery.PageNumber.Value,
                    PageSize = urlQuery.PageSize
                };
                if(urlQuery.IncludeCount)
                {
                    p.TotalRecords = totalRecords.Value;
                }
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(p));
            }

           
            string sql = query.ToSql();

            _logger.LogError(sql);

            orders = await query.OrderByDescending(x => x.OrderDate).ToListAsync();

            return orders;
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // PUT: api/Orders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder([FromRoute] Guid id, [FromBody] Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != order.Id)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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

        // POST: api/Orders
        [HttpPost]
        public async Task<IActionResult> PostOrder([FromBody] Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            order.Created = DateTime.Now;
            order.Stage = "new";
            
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return Ok(order);
        }

        private bool OrderExists(Guid id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}