using Microsoft.AspNetCore.Identity;

namespace makeupStore.Services.AuthAPI.Models;

public class ApplicationUser : IdentityUser
{
    public string Name { get; set; }
}