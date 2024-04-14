using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using BuzzReel.Web.Hubs;
using BuzzReel.Web.Models;

namespace BuzzReel.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MoviesController : ControllerBase
{
    private readonly DatabaseContext _context;
    private readonly IHubContext<MovieHub> _hubContext;

    public MoviesController(DatabaseContext context, IHubContext<MovieHub> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
    }

    [HttpGet]
    public IEnumerable<MovieWithUser> Get()
    {
        var movies = _context.Movies.Join(
            _context.Users,
            movie => movie.UserId,
            user => user.Id,
            (movie, user) => new MovieWithUser
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                User = user
            }
        );
        return movies;
    }

    [HttpGet("{id}")]
    public MovieWithUser Get(int id)
    {
        var movie = _context.Movies.Find(id);
        if (movie == null) return null;
        var user = _context.Users.Find(movie.UserId);
        return new MovieWithUser
        {
            Id = movie.Id,
            Title = movie.Title,
            Description = movie.Description,
            User = user
        };
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Movie movie)
    {
        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();
        var user = _context.Users.Find(movie.UserId);
        await _hubContext.Clients.All.SendAsync("MovieCreated", new MovieWithUser
        {
            Id = movie.Id,
            Title = movie.Title,
            Description = movie.Description,
            User = user
        });
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var movie = _context.Movies.Find(id);
        if (movie != null)
        {
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("MovieDeleted", movie.Id);
            return Ok();
        }
        return NotFound("Movie not found");
    }
}

public class MovieWithUser
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public User User { get; set; }
}
