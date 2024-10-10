namespace WpfPostApp.Models;

public class Post
{
    public required int Id { get; set; }
    public required int UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}
