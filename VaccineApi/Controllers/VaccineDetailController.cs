using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VaccineApi.Models;

namespace VaccineApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VaccineDetailController : ControllerBase
    {
        private readonly VaccineManagementDbContext _vaccine;

        public VaccineDetailController(VaccineManagementDbContext vaccine)
        {
            _vaccine = vaccine;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VaccineDetail>>> GetVaccineDetail()
        {
            if (_vaccine.VaccineDetails == null)
            {
                return NotFound();
            }
            return await _vaccine.VaccineDetails.ToListAsync();

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VaccineDetail>> GetVaccineDetail(int id)
        {
            if (_vaccine.VaccineDetails == null)
            {
                return NotFound();
            }
            var vaccine = await _vaccine.VaccineDetails.FindAsync(id);
            if (vaccine == null)
            {
                return NotFound();
            }
            return vaccine;

        }

        [HttpPost]
        public async Task<ActionResult<VaccineDetail>> PostVaccineDetail(VaccineDetail vaccine)
        {
            _vaccine.VaccineDetails.Add(vaccine);
            await _vaccine.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVaccineDetail), new { id = vaccine.VaccineId }, vaccine);

        }
        [HttpPut]
        public async Task<IActionResult> PutVaccineDetail(int id, VaccineDetail vaccine)
        {
            if (id != vaccine.VaccineId)
            {
                return BadRequest();
            }
            _vaccine.Entry(vaccine).State = EntityState.Modified;
            try
            {
                await _vaccine.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (!VaccineDetailAvailable(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok();
        }
        private bool VaccineDetailAvailable(int id)
        {
            return (_vaccine.VaccineDetails?.Any(x => x.VaccineId == id)).GetValueOrDefault();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVaccineDetail(int id)
        {
            if (_vaccine.VaccineDetails == null)
            {
                return NotFound();
            }

            var vaccine = await _vaccine.VaccineDetails.FindAsync(id);
            if (vaccine == null)
            {
                return NotFound();
            }
            _vaccine.VaccineDetails.Remove(vaccine);
            await _vaccine.SaveChangesAsync();
            return Ok();
        }
    }
}
