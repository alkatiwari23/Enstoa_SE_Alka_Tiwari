using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task.Data;
using Task.Model;

namespace Task.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class ProductController : Controller
    {
        public TaskAPIContext _context;
        public ProductController(TaskAPIContext context)
        {
            _context = context;
        }

        // GET: api/Product
        [HttpGet]
        [Route("/list")]
        public async Task<ActionResult<IEnumerable<Record>>> GetProduct()
        {
            var allItems = await _context.records.ToListAsync();
            return allItems;
        }

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("/list")]
        public async Task<ActionResult<Record>> PostProduct([FromBody] Record reco)
        {
            _context.records.Add(reco);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = reco.ItemId }, reco);
        }

        // GET: api/Product/2
        [HttpGet("/list/{ItemId}")]
        public async Task<ActionResult<Record>> GetProduct(string ItemId)
        {
            var thisRecord = await _context.records.FindAsync(ItemId);

            if (thisRecord == null)
            {
                return NotFound();
            }

            return thisRecord;
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("/list/{ItemId}")]
        public async Task<IActionResult> PutEmployee(string ItemId, [FromBody] Record reco)
        {
            if (ItemId != reco.ItemId)
            {
                return BadRequest();
            }

            _context.Entry(reco).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                //return NotFound();
                if (!ProductExists(ItemId))
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

        // DELETE: api/Employees/5
        [HttpDelete("/list/{ItemId}")]
        public async Task<IActionResult> DeleteProduct(string ItemId)
        {
            var product = await _context.records.FindAsync(ItemId);
            if (product == null)
            {
                return NotFound();
            }

            _context.records.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(string id)
        {
            return _context.records.Any(x => x.ItemId == id);
        }
    }
}
