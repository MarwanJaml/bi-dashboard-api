using bi_dashboard_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly ApiContext _context;

    public CustomerController(ApiContext context)
    {
        _context = context;
    }

    // GET: api/Customer
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
    {
        return await _context.Customers.ToListAsync();
    }

    // GET: api/Customer/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Customer>> GetCustomer(int id)
    {
        var customer = await _context.Customers.FindAsync(id);

        if (customer == null)
        {
            return NotFound();
        }

        return customer;
    }

    // POST: api/Customer
    [HttpPost]
    public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        
        customer.Id = 0;
        if (customer.Orders != null)
        {
            foreach (var order in customer.Orders)
            {
                order.Id = 0;
                order.CustomerId = 0; 
            }
        }

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetCustomer", new { id = customer.Id }, customer);
    }



}