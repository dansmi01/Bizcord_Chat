using System;
namespace Bizcord.Web.Shared.Entities;

public class Message
{
    public long Id { get; set; }
    public int UserId { get; set; }
    public int ChannelId { get; set; }
    public string Content { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    public User User { get; set; } = default!;
    public Channel Channel { get; set; } = default!; 
}