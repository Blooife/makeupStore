using makeupStore.Services.AuthAPI.Controllers;
using makeupStore.Services.AuthAPI.Data;
using makeupStore.Services.AuthAPI.Models;
using makeupStore.Services.AuthAPI.Models.Dto;
using makeupStore.Services.AuthAPI.Service;
using makeupStore.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;

namespace AuthServiceUnitTest;

public class UnitTest1
{
    private readonly Mock<IJwtTokenJenerator> _jwtTokenGeneratorMock = new Mock<IJwtTokenJenerator>();
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly Mock<RoleManager<IdentityRole>> _roleManagerMock;
    private readonly Mock<AppDbContext> _dbMock = new Mock<AppDbContext>();

    // System Under Test (SUT)
    private readonly IAuthService _authService;

    public UnitTest1()
    {
        _userManagerMock = GetUserManagerMock();
        _roleManagerMock = GetRoleManagerMock();

        _authService = new AuthService(_dbMock.Object, _jwtTokenGeneratorMock.Object, _userManagerMock.Object, _roleManagerMock.Object);
    }

    [Fact]
    public async Task AssignRole_UserExistsAndRoleDoesNotExist_ShouldAssignRole()
    {
        // Arrange
        var user = new ApplicationUser { Email = "test@example.com" };
        _dbMock.Setup(db => db.ApplicationUsers).Returns(GetMockDbSet(new List<ApplicationUser> { user }).Object);

        var roleName = "TestRole";
        _roleManagerMock.Setup(rm => rm.RoleExistsAsync(roleName)).ReturnsAsync(false);
        _roleManagerMock.Setup(rm => rm.CreateAsync(It.IsAny<IdentityRole>())).ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _authService.AssignRole("test@example.com", roleName);

        // Assert
        Assert.True(result);
        _userManagerMock.Verify(um => um.AddToRoleAsync(user, roleName), Times.Once);
    }

    [Fact]
    public async Task Login_ValidCredentials_ShouldReturnLoginResponseDto()
    {
        // Arrange
        var user = new ApplicationUser { UserName = "testuser", Email = "test@example.com" };
        _dbMock.Setup(db => db.ApplicationUsers).Returns(GetMockDbSet(new List<ApplicationUser> { user }).Object);

        var loginRequestDto = new LoginRequestDto { UserName = "testuser", Password = "TestPassword123" };
        _userManagerMock.Setup(um => um.CheckPasswordAsync(user, loginRequestDto.Password)).ReturnsAsync(true);
        _jwtTokenGeneratorMock.Setup(jwt => jwt.GenerateToken(user, It.IsAny<IEnumerable<string>>())).Returns("TestToken");

        // Act
        var result = await _authService.Login(loginRequestDto);

        // Assert
        Assert.NotNull(result.User);
        Assert.Equal("test@example.com", result.User.Email);
        Assert.Equal("TestToken", result.Token);
    }

    [Fact]
    public async Task Register_ValidRegistration_ShouldReturnEmptyString()
    {
        // Arrange
        var registrationRequestDto = new RegistrationRequestDto
        {
            Email = "test@example.com",
            Password = "TestPassword123",
            Name = "Test User",
            PhoneNumber = "1234567890"
        };

        // Mock the result of user creation
        _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), registrationRequestDto.Password))
            .ReturnsAsync(IdentityResult.Success);

        // Mock an empty list of users in the database
        var users = new List<ApplicationUser>();
        _dbMock.Setup(db => db.ApplicationUsers).Returns(GetMockDbSet(users).Object);

        // Act
        var result = await _authService.Register(registrationRequestDto);

        // Assert
    
        // Verify that the user creation method was called
        _userManagerMock.Verify(um => um.CreateAsync(It.IsAny<ApplicationUser>(), registrationRequestDto.Password), Times.Once);

        // Verify that accessing ApplicationUsers property was called
        _dbMock.Verify(db => db.ApplicationUsers, Times.AtLeastOnce);
    }


    private static Mock<UserManager<ApplicationUser>> GetUserManagerMock()
    {
        var store = new Mock<IUserStore<ApplicationUser>>();
        return new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
    }

    private static Mock<RoleManager<IdentityRole>> GetRoleManagerMock()
    {
        var store = new Mock<IRoleStore<IdentityRole>>();
        return new Mock<RoleManager<IdentityRole>>(store.Object, null, null, null, null);
    }

    private static Mock<DbSet<T>> GetMockDbSet<T>(List<T> elements) where T : class
    {
        var queryable = elements.AsQueryable();
        var dbSetMock = new Mock<DbSet<T>>();
        dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
        dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
        dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
        return dbSetMock;
    }
}