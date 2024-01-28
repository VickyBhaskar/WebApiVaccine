using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VaccineApi.Models;

namespace VaccineApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DateTimeSlotController : ControllerBase
    {
        private readonly VaccineManagementDbContext _datetime;

        public DateTimeSlotController(VaccineManagementDbContext datetime)
        {
            _datetime = datetime;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DateTimeSlot>>> GetDateTimeSlot()
        {
            if (_datetime.DateTimeSlots == null)
            {
                return NotFound();
            }
            return await _datetime.DateTimeSlots.ToListAsync();

        }

        [HttpGet("{datetiming}")]
        public async Task<ActionResult<DateTimeSlot>> GetDateTimeSlot(DateTime datetiming)
        {
            if (_datetime.DateTimeSlots == null)
            {
                return NotFound();
            }
            var datetime = await _datetime.DateTimeSlots.FindAsync(datetiming);
            if (datetime == null)
            {
                return NotFound();
            }
            return datetime;

        }

        [HttpPost]
        public async Task<ActionResult<DateTimeSlot>> Postdatetime(DateTimeSlot datetime)
        {
            _datetime.DateTimeSlots.Add(datetime);
            await _datetime.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDateTimeSlot), new { datetiming = datetime.DateTimings }, datetime);

        }
        [HttpPut]
        public async Task<IActionResult> PutDateTimeSlot(DateTime datetiming, DateTimeSlot datetime)
        {
            if (datetiming != datetime.DateTimings)
            {
                return BadRequest();
            }
            _datetime.Entry(datetime).State = EntityState.Modified;
            try
            {
                await _datetime.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (!DateTimeSlotAvailable(datetiming))
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
        private bool DateTimeSlotAvailable(DateTime datetiming)
        {
            return (_datetime.DateTimeSlots?.Any(x => x.DateTimings == datetiming)).GetValueOrDefault();
        }
        [HttpDelete("{datetiming}")]
        public async Task<IActionResult> DeleteDateTimeSlot(DateTime datetiming)
        {
            if (_datetime.DateTimeSlots == null)
            {
                return NotFound();
            }

            var datetime = await _datetime.DateTimeSlots.FindAsync(datetiming);
            if (datetime == null)
            {
                return NotFound();
            }
            _datetime.DateTimeSlots.Remove(datetime);
            await _datetime.SaveChangesAsync();
            return Ok();
        }
    }
}
