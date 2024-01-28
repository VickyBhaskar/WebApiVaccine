using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VaccineApi.Models;

namespace VaccineApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDetailController : ControllerBase
    {
        private readonly VaccineManagementDbContext _user;

        public UserDetailController(VaccineManagementDbContext user)
        {
            _user = user;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDetail>>> GetUserDetail()
        {
            if (_user.UserDetails == null)
            {
                return NotFound();
            }
            return await _user.UserDetails.ToListAsync();

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDetail>> GetUserDetail(int id)
        {
            if (_user.UserDetails == null)
            {
                return NotFound();
            }
            var user = await _user.UserDetails.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return user;

        }

        [HttpPost]
        public async Task<ActionResult<UserDetail>> PostUserDetail(UserDetail user)
        {
            _user.UserDetails.Add(user);
            await _user.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserDetail), new { id = user.UserId }, user);

        }
        [HttpPut]
        public async Task<IActionResult> PutUserDetail(int id, UserDetail user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }
            _user.Entry(user).State = EntityState.Modified;
            try
            {
                await _user.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserDetailAvailable(id))
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
        private bool UserDetailAvailable(int id)
        {
            return (_user.UserDetails?.Any(x => x.UserId == id)).GetValueOrDefault();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserDetail(int id)
        {
            if (_user.UserDetails == null)
            {
                return NotFound();
            }

            var user = await _user.UserDetails.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            _user.UserDetails.Remove(user);
            await _user.SaveChangesAsync();
            return Ok();
        }
    }
}
