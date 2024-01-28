using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VaccineApi.Models;

namespace VaccineApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly VaccineManagementDbContext _adminService;

        public AdminController(VaccineManagementDbContext adminService)
        {
            _adminService = adminService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Admin>>> GetAdmin()
        {
            if (_adminService.Admins == null)
            {
                return NotFound();
            }
            return await _adminService.Admins.ToListAsync();

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Admin>> GetAdmin(int id)
        {
            if (_adminService.Admins == null)
            {
                return NotFound();
            }
            var admin = await _adminService.Admins.FindAsync(id);
            if (admin == null)
            {
                return NotFound();
            }
            return admin;

        }

        [HttpPost]
        public async Task<ActionResult<Admin>> PostAdmin(Admin admin)
        {
            _adminService.Admins.Add(admin);
            await _adminService.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAdmin), new { id = admin.AdminId }, admin);

        }
        [HttpPut]
        public async Task<IActionResult> PutAdmin(int id, Admin admin)
        {
            if (id != admin.AdminId)
            {
                return BadRequest();
            }
            _adminService.Entry(admin).State = EntityState.Modified;
            try
            {
                await _adminService.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (!AdminAvailable(id))
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
        private bool AdminAvailable(int id)
        {
            return (_adminService.Admins?.Any(x => x.AdminId == id)).GetValueOrDefault();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            if (_adminService.Admins == null)
            {
                return NotFound();
            }

            var admin = await _adminService.Admins.FindAsync(id);
            if (admin == null)
            {
                return NotFound();
            }
            _adminService.Admins.Remove(admin);
            await _adminService.SaveChangesAsync();
            return Ok();
        }
    }
}
