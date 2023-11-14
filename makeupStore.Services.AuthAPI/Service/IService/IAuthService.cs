using makeupStore.Services.AuthAPI.Models.Dto;

namespace makeupStore.Services.AuthAPI.Service.IService;

public interface IAuthService
{
    Task<string> Register(RegistrationRequestDto registrationRequestDto);
    Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
    Task<bool> AssignRole(string email, string roleName);
}