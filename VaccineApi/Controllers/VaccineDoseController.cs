using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VaccineApi.Models;

namespace VaccineApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VaccineDoseController : ControllerBase
    {
        private readonly VaccineManagementDbContext _vaccine;

        public VaccineDoseController(VaccineManagementDbContext vaccine)
        {
            _vaccine = vaccine;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VaccineDose>>> GetVaccineDose()
        {
            if (_vaccine.VaccineDoses == null)
            {
                return NotFound();
            }
            return await _vaccine.VaccineDoses.ToListAsync();

        }

        [HttpGet("{numberofdose}")]
        public async Task<ActionResult<VaccineDose>> GetVaccineDose(int numberofdose)
        {
            if (_vaccine.VaccineDoses == null)
            {
                return NotFound();
            }
            var vaccine = await _vaccine.VaccineDoses.FindAsync(numberofdose);
            if (vaccine == null)
            {
                return NotFound();
            }
            return vaccine;

        }

        [HttpPost]
        public async Task<ActionResult<VaccineDose>> PostVaccineDose(VaccineDose vaccine)
        {
            _vaccine.VaccineDoses.Add(vaccine);
            await _vaccine.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVaccineDose), new { numberofdose = vaccine.NumberOfDose }, vaccine);

        }
        [HttpPut]
        public async Task<IActionResult> PutVaccineDose(int numberofdose, VaccineDose vaccine)
        {
            if (numberofdose != vaccine.NumberOfDose)
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
                if (!VaccineDoseAvailable(numberofdose))
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
        private bool VaccineDoseAvailable(int numberofdose)
        {
            return (_vaccine.VaccineDoses?.Any(x => x.NumberOfDose == numberofdose)).GetValueOrDefault();
        }
        [HttpDelete("{numberofdose}")]
        public async Task<IActionResult> DeleteVaccineDose(int numberofdose)
        {
            if (_vaccine.VaccineDoses == null)
            {
                return NotFound();
            }

            var vaccine = await _vaccine.VaccineDoses.FindAsync(numberofdose);
            if (vaccine == null)
            {
                return NotFound();
            }
            _vaccine.VaccineDoses.Remove(vaccine);
            await _vaccine.SaveChangesAsync();
            return Ok();
        }
    }
}
