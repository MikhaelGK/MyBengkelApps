using BENGKEL_API.Helper;
using BENGKEL_API.Models;
using BENGKEL_API.Viewmodel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BENGKEL_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrxesController : ControllerBase
    {
        private readonly SidorejoBengkelContext _context;

        public TrxesController(SidorejoBengkelContext context)
        {
            this._context = context;
        }

        [HttpPost]
        public async Task<ActionResult<HeaderTrx>> PostTrxes(TrxesDto trx)
        {
            var strgs = new List<string>()
            {
                trx.CustomerId,
                trx.EmployeeId,
                trx.VehicleId,
            };
            var validEmpty = Validation.ValidationString(strgs);
            if (!validEmpty) return BadRequest("All input must be filled");

            trx.ID = Generator.GenerateID(Master.Transaction.ToString(), 5);

            var hData = new HeaderTrx()
            {
                TrxId = trx.ID,
                Date = DateTime.Now,
                CustomerId = trx.CustomerId,
                EmployeeId = trx.EmployeeId
            };
            _context.HeaderTrxes.Add(hData);
            await _context.SaveChangesAsync();

            var customerVehicleId = await _context.CustomerVehicles
                .Where(x => x.CustomerId == trx.CustomerId && x.VehicleId == trx.VehicleId)
                .Select(x => x.CustomerVehicleId).FirstOrDefaultAsync();

            var dData = new DetailTrx()
            {
                 TrxId = trx.ID,
                 CustomerVehicleId = customerVehicleId,
                 Description = trx.Description,
                 Cost = trx.Cost
            };
            _context.DetailTrxes.Add(dData);
            await _context.SaveChangesAsync();

            return Ok("Success");
        }

        [HttpPut]
        [Authorize(Policy = "Admin")] 
        public async Task<ActionResult<DetailTrx>> UpdateTrxes(string id, int cost)
        {
            var data = await _context.DetailTrxes
                .Where(x => x.TrxId == id).FirstOrDefaultAsync();
            if (data == null)
            {
                return NotFound("Not Found");
            }
            data.Cost = cost;
            await _context.SaveChangesAsync();
            return Ok("Success");
        }
    }
}
