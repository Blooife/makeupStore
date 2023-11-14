using makeupStore.Services.AuthAPI.Models;

namespace makeupStore.Services.AuthAPI.Service.IService;

public interface IJwtTokenJenerator
{
    string GenerateToken(ApplicationUser applicationUser, IEnumerable<string> roles);
}