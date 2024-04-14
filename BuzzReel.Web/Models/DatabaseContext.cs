using Microsoft.EntityFrameworkCore;

namespace BuzzReel.Web.Models;
public class DatabaseContext : DbContext
{
  public DatabaseContext(DbContextOptions<DatabaseContext> options)
      : base(options) { }

  public DbSet<User> Users => Set<User>();
  public DbSet<Movie> Movies => Set<Movie>();
}