using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BENGKEL_API.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using BENGKEL_API.Helper;
using BENGKEL_API.Viewmodel;

namespace BENGKEL_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerVehiclesController : ControllerBase
    {
        private readonly SidorejoBengkelContext _context;

        public CustomerVehiclesController(SidorejoBengkelContext context)
        {
            _context = context;
        }

        // GET: api/CustomerVehicles
        [HttpGet("{customerId}")]
        public async Task<ActionResult<IEnumerable<CustomerVehicle>>> GetCustomerVehicles(string customerId)
        {
            var customerVehicles = await _context.CustomerVehicles
                .Include(x => x.Vehicle)
                .Where(x => x.CustomerId == customerId)
                .ToListAsync();

            if (customerVehicles == null) return NotFound("You don't have vehicle");

            var cVehicles = new List<CustomerVehiclesDto>();
            foreach (var item in customerVehicles)
            {
                var cVehicle = new CustomerVehiclesDto()
                {
                    CustomerVehicleId = item.CustomerVehicleId,
                    VehicleId = item.Vehicle.VehicleId,
                    VehicleName = item.Vehicle.Name,
                    VehicleNumber = item.Number
                };
                cVehicles.Add(cVehicle);
            }

            return Ok(cVehicles);
        }

        // PUT: api/CustomerVehicles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomerVehicle(int id, [FromBody]CustomerVehicle customerVehicle)
        {
            if (id != customerVehicle.CustomerVehicleId)
            {
                return BadRequest("Error");
            }

            _context.Entry(customerVehicle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerVehicleExists(id))
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

        // POST: api/CustomerVehicles
        [HttpPost]
        public async Task<ActionResult<CustomerVehicle>> PostCustomerVehicle([FromBody]CustomerVehicle customerVehicle)
        {
            if (_context.CustomerVehicles == null)
            {
                return Problem("Entity set 'SidorejoBengkelContext.CustomerVehicles'  is null.");
            }

            var strgs = new List<String>()
            {
                customerVehicle.CustomerId,
                customerVehicle.VehicleId,
                customerVehicle.Number
            };
            var valid = Validation.ValidationString(strgs);
            if (!valid) return BadRequest("All input must be filled");

            _context.CustomerVehicles.Add(customerVehicle);
            await _context.SaveChangesAsync();

            return Ok("Success");
        }

        // DELETE: api/CustomerVehicles/5
        [HttpDelete("{customerVehicleId}")]
        public async Task<IActionResult> DeleteCustomerVehicle(int customerVehicleId)
        {
            if (_context.CustomerVehicles == null)
            {
                return NotFound();
            }
            var customerVehicle = await _context.CustomerVehicles.FindAsync(customerVehicleId);
            if (customerVehicle == null)
            {
                return NotFound();
            }

            var details = await _context.DetailTrxes
                .Where(x => x.CustomerVehicleId == customerVehicleId)
                .ToListAsync();
            foreach (var detail in details)
            {
                _context.DetailTrxes.Remove(detail);
                await _context.SaveChangesAsync();
            }

            _context.CustomerVehicles.Remove(customerVehicle);
            await _context.SaveChangesAsync();

            return Ok("Success");
        }

        private bool CustomerVehicleExists(int id)
        {
            return (_context.CustomerVehicles?.Any(e => e.CustomerVehicleId == id)).GetValueOrDefault();
        }
    }
}
