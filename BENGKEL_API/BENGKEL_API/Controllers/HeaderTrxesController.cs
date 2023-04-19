using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BENGKEL_API.Models;
using BENGKEL_API.Helper;
using Microsoft.EntityFrameworkCore.Storage;
using BENGKEL_API.Viewmodel;

namespace BENGKEL_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeaderTrxesController : ControllerBase
    {
        private readonly SidorejoBengkelContext _context;

        public HeaderTrxesController(SidorejoBengkelContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<HeaderTrx>>> GetHeaderTrxes(string? query, string customerId)
        {
            if (_context.HeaderTrxes == null)
            {
                return NotFound();
            }

            var header = await _context.HeaderTrxes.Where(x => x.CustomerId == customerId).ToListAsync();

            if (query != null)
            {
                header = await _context.HeaderTrxes
                    .Where(x => x.Customer.Name.Contains(query) ||
                        x.Date.ToString().Contains(query) ||
                        x.CustomerId.Contains(query)).ToListAsync();
            }

            

            var histories = new List<DetailDto>();
            foreach (var h in header)
            {
                var detail = await _context.DetailTrxes
                    .Where(x => x.TrxId == h.TrxId)
                    .ToListAsync();

                var cost = 0;
                foreach (var d in detail)
                {
                    cost += d.Cost;
                }

                var history = new DetailDto()
                {
                    TrxId = h.TrxId,
                    Date = h.Date.ToString("yyyy-MM-dd"),
                    Cost = cost
                };
                histories.Add(history);
            }
            return Ok(histories);
        }
    }
}
