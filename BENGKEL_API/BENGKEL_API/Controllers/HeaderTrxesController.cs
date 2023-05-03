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

        [HttpGet("{customerId}")]
        public async Task<ActionResult<IEnumerable<HeaderTrx>>> GetHeaderTrxes(string? query, string customerId)
        {
            if (_context.HeaderTrxes == null)
            {
                return NotFound();
            }

            var header = await _context.HeaderTrxes.Where(x => x.CustomerId == customerId).ToListAsync();

            var histories = new List<DetailTrxDto>();
            foreach (var h in header)
            {
                var detail = await _context.DetailTrxes
                    .Where(x => x.TrxId == h.TrxId)
                    .ToListAsync();

                var detailCost = 0;
                foreach (var d in detail)
                {
                    detailCost += d.Cost == null ? 0 : Convert.ToInt32(d.Cost);
                }

                var history = new DetailTrxDto()
                {
                    TrxId = h.TrxId,
                    Date = h.Date.ToString("yyyy-MM-dd"),
                    Cost = detailCost
                };
                histories.Add(history);
            }

            if (query != null)
            {
                histories = histories
                    .Where(x => x.TrxId.Contains(query) ||
                        x.Date.ToString().Contains(query)).ToList();
            }

            return Ok(histories.OrderByDescending(x => x.Date));
        }
    }
}
