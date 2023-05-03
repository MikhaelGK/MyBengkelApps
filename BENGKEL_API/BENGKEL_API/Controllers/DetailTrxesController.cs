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

            var detailTrx = await _context.DetailTrxes
                .Where(x => x.TrxId == id)
                .Include(x => x.Trx)
                .Include(x => x.CustomerVehicle)
                .FirstOrDefaultAsync();

            if (detailTrx is null) return NotFound("Error");

            var vehicle = await _context.Vehicles
                .FirstOrDefaultAsync(x => x.VehicleId == detailTrx.CustomerVehicle.VehicleId);

            if (vehicle is null) return NotFound();

            var detail = new DetailTrxDto()
            {
                TrxId = detailTrx.TrxId,
                Date = detailTrx.Trx.Date.ToString("yyyy-MM-dd"),
                VehicleName = vehicle.Name,
                VehicleNumber = detailTrx.CustomerVehicle.Number,
                Description = detailTrx.Description,
                Cost = detailTrx.Cost == null ? 0 : Convert.ToInt32(detailTrx.Cost)
            };

            return Ok(detail);
        }
    }
}
