    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BENGKEL_API.Models;
using BENGKEL_API.Helper;
using NuGet.Versioning;

namespace BENGKEL_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly SidorejoBengkelContext _context;

        public CustomersController(SidorejoBengkelContext context)
        {
            _context = context;
        }

        // GET: api/Customers/5
        [HttpGet("{email}")]
        public async Task<ActionResult<Customer>> GetCustomer(string email)
        {
            if (_context.Customers == null)
            {
                return NotFound("Not Found");
            }
            var customer = await _context.Customers.FirstOrDefaultAsync(x => x.Email == email);
            var employee = await _context.Employees.FirstOrDefaultAsync(x => x.Email == email);
            if (customer != null)
            {
                return Ok(customer);
            }
            if (employee != null)
            {
                return Ok(employee);
            }
            return NotFound("Data Not Found");
        }

        // PUT: api/Customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(string id, [FromBody]Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return BadRequest("Error");
            }

            var strgs = new List<string>()
            {
                customer.Name,
                customer.Email,
                customer.Password,
                customer.Address,
                customer.Phone,
            };
            var validEmpty = Validation.ValidationString(strgs);
            if (!validEmpty) return BadRequest("All input must be filled");

            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok("Success");
        }

        private bool CustomerExists(string id)
        {
            return (_context.Customers?.Any(e => e.CustomerId == id)).GetValueOrDefault();
        }
    }
}
