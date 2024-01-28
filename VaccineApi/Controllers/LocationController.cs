using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VaccineApi.Models;

namespace VaccineApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly VaccineManagementDbContext _location;

        public LocationController(VaccineManagementDbContext location)
        {
            _location = location;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Location>>> GetLocation()
        {
            if (_location.Locations == null)
            {
                return NotFound();
            }
            return await _location.Locations.ToListAsync();

        }

        [HttpGet("{cityName}")]
        public async Task<ActionResult<Location>> GetLocation(string cityName)
        {
            if (_location.Locations == null)
            {
                return NotFound();
            }
            var location = await _location.Locations.FindAsync(cityName);
            if (location == null)
            {
                return NotFound();
            }
            return location;

        }

        [HttpPost]
        public async Task<ActionResult<Location>> PostLocation(Location location)
        {
            _location.Locations.Add(location);
            await _location.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLocation), new { id = location.CityName }, location);

        }
        [HttpPut]
        public async Task<IActionResult> PutLocation(String cityName, Location location)
        {
            if (cityName != location.CityName)
            {
                return BadRequest();
            }
            _location.Entry(location).State = EntityState.Modified;
            try
            {
                await _location.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (!LocationAvailable(cityName))
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

        private bool LocationAvailable(String cityName)
        {
            return (_location.Locations?.Any(x => x.CityName == cityName)).GetValueOrDefault();
        }
        [HttpDelete("{cityName}")]
        public async Task<IActionResult> DeleteLocation(String cityName)
        {
            if (_location.Locations == null)
            {
                return NotFound();
            }

            var location = await _location.Locations.FindAsync(cityName);
            if (location == null)
            {
                return NotFound();
            }
            _location.Locations.Remove(location);
            await _location.SaveChangesAsync();
            return Ok();
        }
    }
}
