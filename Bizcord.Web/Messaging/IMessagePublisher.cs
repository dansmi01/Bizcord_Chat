using System;
using System.Threading.Tasks;

namespace Bizcord.Web.Messaging;

public interface IMessagePublisher
{
    public record MessageEvent
    {
        public long Id { get; init; }
        public int ChannelId { get; init; }
        public string Content { get; init; } = string.Empty;
        public string UserName { get; init; } = string.Empty;
        public DateTime CreatedAt { get; init; }
    }


    public interface IMessagePublisher
    {
        Task PublishMessageCreatedAsync(MessageEvent evt);
    }
}