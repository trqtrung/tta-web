using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TTA.Api.Data;
using TTA.Api.Models;

namespace TTA.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/optionlists")]
    [ApiController]
    public class OptionListsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OptionListsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/OptionLists/key
        [HttpGet]
        public async Task<IActionResult> GetOptionLists([FromQuery] string filter, [FromQuery] int pageSize, [FromQuery] int pageNumber)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var query = from o in _context.OptionLists select o;

            if (!string.IsNullOrEmpty(filter))
                query = query.Where(x => x.Key.Contains(filter) || x.Name.Contains(filter));

            var optionLists = await query.Skip(pageSize * pageNumber).Take(pageSize).OrderBy(x => x.Name).ToListAsync();

            if (optionLists == null)
            {
                return NotFound();
            }

            return Ok(optionLists);
        }

        // GET: api/OptionLists/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOptionList([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int n;
            bool isId = int.TryParse(id, out n);

            if (isId)
            {
                var optionList = await _context.OptionLists.FindAsync(id);

                if (optionList == null)
                {
                    return NotFound();
                }

                return Ok(optionList);
            }
            else
            {
                string key = id;

                var optionLists = await _context.OptionLists.Where(x => x.Key == key).ToListAsync();

                if (optionLists == null)
                {
                    return NotFound();
                }

                return Ok(optionLists);
            }
        }

        // PUT: api/OptionLists/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOptionList([FromRoute] int id, [FromBody] OptionList optionList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != optionList.Id)
            {
                return BadRequest();
            }

            _context.Entry(optionList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OptionListExists(id))
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

        // POST: api/OptionLists
        [HttpPost]
        public async Task<IActionResult> PostOptionList([FromBody] OptionList optionList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.OptionLists.Add(optionList);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOptionList", new { id = optionList.Id }, optionList);
        }

        // DELETE: api/OptionLists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOptionList([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var optionList = await _context.OptionLists.FindAsync(id);
            if (optionList == null)
            {
                return NotFound();
            }

            _context.OptionLists.Remove(optionList);
            await _context.SaveChangesAsync();

            return Ok(optionList);
        }

        private bool OptionListExists(int id)
        {
            return _context.OptionLists.Any(e => e.Id == id);
        }
    }
}