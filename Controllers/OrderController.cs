using bi_dashboard_api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bi_dashboard_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ApiContext _context;

        public OrderController(ApiContext context)
        {
            _context = context;
        }


        // GET: api/Order/pageNumber/pageSize
        [HttpGet("{pageNumber}/{pageSize}")]
        public IActionResult Get(int pageNumber, int pageSize)
        {
            // Validate input
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            // Get total count for pagination metadata
            var totalCount = _context.Orders.Count();

            var data = _context.Orders
                .Include(o => o.Customer)  // Include related Customer data
                .OrderBy(o => o.Id)    // Order by placement date
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Return data with pagination metadata
            return Ok(new
            {
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                Data = data
            });
        }

        [HttpGet("ByState")]
        public IActionResult ByState()
        {
            var result = _context.Orders
                .Include(o => o.Customer)  // Explicitly include Customer
                .Where(o => o.Customer != null)  // Safety check
                .GroupBy(o => o.Customer.Status)  // Group by Status
                .Select(g => new
                {
                    Status = g.Key,
                    Count = g.Count(),
                    TotalAmount = g.Sum(o => o.Total)
                })
                .OrderByDescending(x => x.TotalAmount)
                .ToList();

            return Ok(result);
        }

        [HttpGet("ByCustomer/{n}")]
        public IActionResult ByCustomer(int n)
        {
            var result = _context.Orders
                .Include(o => o.Customer)
                .Where(o => o.Customer != null)
                .GroupBy(o => new { o.Customer.Id, o.Customer.Name })
                .Select(g => new
                {
                    g.Key.Name,
                    Count = g.Count(),
                    TotalAmount = g.Sum(o => o.Total)
                })
                .OrderByDescending(x => x.TotalAmount)
                .Take(n)
                .ToList();

            return Ok(result);
        }

        [HttpGet("GetOrder/{id}", Name = "GetOrder")] 
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            return order;
        }


    }
}
