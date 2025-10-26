using System.Collections.Generic;
namespace Bizcord.Web.Shared.Entities;

public class Channel
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public List<Message> Messages { get; set; } = new();
}