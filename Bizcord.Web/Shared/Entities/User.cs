namespace Bizcord.Web.Shared.Entities;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; } = default!;
    public string? DisplayName { get; set; }
    public string? AvatarUrl { get; set; }


    public List<Message> Messages { get; set; } = new();
}