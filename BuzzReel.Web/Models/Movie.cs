namespace BuzzReel.Web.Models;

public class Movie {
  public int Id { get; set; }
  public int UserId { get; set; }
  public string Title { get; set; } = null!;
  public string Description { get; set; } = null!;
  public DateTime CreatedAt { get; set; }
}
