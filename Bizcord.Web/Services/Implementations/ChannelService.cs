using Bizcord.Web.Data;
using Bizcord.Web.Services;
using Bizcord.Web.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bizcord.Web.Services.Implementations;

public class ChannelService : IChannelService
{
    private readonly BizcordDbContext _db;
    public ChannelService(BizcordDbContext db) => _db = db;


    public async Task<Channel> CreateAsync(string name)
    {
        var channel = new Channel { Name = name };
        _db.Channels.Add(channel);
        await _db.SaveChangesAsync();
        return channel;
    }


    public Task<List<Channel>> GetAllAsync() => _db.Channels.AsNoTracking().OrderBy(c => c.Name).ToListAsync();


    public Task<Channel?> GetByIdAsync(int id) => _db.Channels.FirstOrDefaultAsync(x => x.Id == id);  
}