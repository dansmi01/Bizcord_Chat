using Bizcord.Web.Shared.DTOs;
using Bizcord.Web.Shared.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bizcord.Web.Services.Implementations;

public interface IMessageService
{
    Task<Message> CreateAsync(int userId, int channelId, string content);
    Task<List<MessageDto>> GetByChannelAsync(int channelId, int take = 100); 
}