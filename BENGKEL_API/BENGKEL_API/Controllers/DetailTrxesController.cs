using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BENGKEL_API.Models;
using BENGKEL_API.Viewmodel;

namespace BENGKEL_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetailTrxesController : ControllerBase
    {
        private readonly SidorejoBengkelContext _context;

        public DetailTrxesController(SidorejoBengkelContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DetailTrx>> GetDetailTrx(string id)
        {
            if (_context.HeaderTrxes == null)
            {
                return NotFound("Data Not Found");
            }
            var headerTrx = await _context.HeaderTrxes
                .FirstOrDefaultAsync(x => x.TrxId == id);

            if (headerTrx == null)
            {
                return NotFound("Data Not Found");
            }

            var details = await _context.DetailTrxes
                .Where(x => x.TrxId == headerTrx.TrxId)
                .ToListAsync();

            var data = new List<DetailDto>();
            foreach (var d in details)
            {
                var customerVehicle = await _context.CustomerVehicles
                    .FirstOrDefaultAsync(x => x.CustomerVehicleId == d.CustomerVehicleId);

                var vehicle = await _context.Vehicles
                    .FirstOrDefaultAsync(x => x.VehicleId == customerVehicle.VehicleId);

                var detail = new DetailDto()
                {
                    TrxId = headerTrx.TrxId,
                    Date = headerTrx.Date.ToString("yyyy-MM-dd"),
                    VehicleName = vehicle.Name,
                    VehicleNumber = customerVehicle.Number,
                    Cost = d.Cost
                };
                data.Add(detail);
            }

            return Ok(data);
        }
    }
}
