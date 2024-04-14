using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BuzzReel.Web.Models;

namespace BuzzReel.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly DatabaseContext _context;

    public UsersController(DatabaseContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        return await _context.Users.ToListAsync();
    }

  
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        return user;
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(int id, User user)
    {
        if (id != user.Id)
        {
            return BadRequest();
        }

        _context.Entry(user).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }


    [HttpPost]
    public async Task<ActionResult<User>> PostUser([FromBody] User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetUser", new { id = user.Id }, user);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<User>> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return user;
    }

    private bool UserExists(int id)
    {
        return _context.Users.Any(e => e.Id == id);
    }

    [HttpPost("login")]
    public async Task<ActionResult<object>> Login([FromBody] User user)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email && u.Password == user.Password);
        if (existingUser == null)
        {
            return NotFound("User not found");
        }
      var userWithoutPassword = new {
          Id = existingUser.Id,
          Email = existingUser.Email,
          Name = existingUser.Name
      };
        return userWithoutPassword;
    }
}