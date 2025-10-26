using Bizcord.Web.Shared.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Bizcord.Web.Services.Implementations;

public interface IUserService
{
    Task<User> CreateAsync(string userName, string? displayName, string? avatarUrl);
    Task<List<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);  
}