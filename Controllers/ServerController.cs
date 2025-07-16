using bi_dashboard_api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace bi_dashboard_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServerController : ControllerBase
    {
        private readonly ApiContext _context;

        public ServerController(ApiContext context)
        {
            _context = context;
        }
        // GET: api/Server
        [HttpGet("{id}", Name = "GetServerById")]
        public IActionResult Get(int id)
        {
            var server = _context.Servers.Find(id);
            if (server == null)
            {
                return NotFound();
            }
            return Ok(server);
        }

        [HttpPut("{id}")]
        public IActionResult Message(int id, [FromBody] ServerMessage msg)
        {
            if (msg == null) return BadRequest();

            var server = _context.Servers.Find(id);
            if (server == null) return NotFound();

            if (msg.Payload == "activate")
            {
                server.isOnline = true;
            }
            else if (msg.Payload == "deactivate")
            {
                server.isOnline = false;
            }
            else
            {
                return BadRequest("Invalid payload");
            }

            _context.SaveChanges();
            return Ok(server); // Changed from NoContent() to Ok(server)
        }
    }
}
