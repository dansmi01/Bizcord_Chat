using Bizcord.Web.Shared.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bizcord.Web.Services.Implementations;

public interface IChannelService
{
    Task<Channel> CreateAsync(string name);
    Task<List<Channel>> GetAllAsync();
    Task<Channel?> GetByIdAsync(int id);
}