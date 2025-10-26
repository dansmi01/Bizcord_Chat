using System;

namespace Bizcord.Web.Shared.DTOs;

public class MessageDto
{
    public long Id { get; set; }
    public int ChannelId { get; set; }
    public string Content { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
}