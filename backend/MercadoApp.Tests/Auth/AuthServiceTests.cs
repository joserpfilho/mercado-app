using FluentAssertions;
using MercadoApp.Application.Auth;
using MercadoApp.Application.Auth.DTOs;
using MercadoApp.Application.Common;
using MercadoApp.Domain.Entities;
using Microsoft.Extensions.Configuration;
using NSubstitute;

namespace MercadoApp.Tests.Auth;

public class AuthServiceTests
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _userRepository = Substitute.For<IUserRepository>();

        var inMemorySettings = new Dictionary<string, string?>
        {
            { "Jwt:Secret", "mercado_super_secret_key_2025_abcdef!" }
        };

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        _authService = new AuthService(_userRepository, _configuration);
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnSuccess_WhenEmailIsNew()
    {
        // Arrange
        var request = new RegisterRequest("José", "jose@email.com", "123456");
        _userRepository.ExistsByEmailAsync(request.Email).Returns(false);

        // Act
        var result = await _authService.RegisterAsync(request);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value!.Email.Should().Be(request.Email);
        result.Value.Name.Should().Be(request.Name);
        result.Value.Token.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnFailure_WhenEmailAlreadyExists()
    {
        // Arrange
        var request = new RegisterRequest("José", "jose@email.com", "123456");
        _userRepository.ExistsByEmailAsync(request.Email).Returns(true);

        // Act
        var result = await _authService.RegisterAsync(request);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("E-mail já cadastrado.");
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnSuccess_WhenCredentialsAreValid()
    {
        // Arrange
        var request = new LoginRequest("jose@email.com", "123456");
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "José",
            Email = "jose@email.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456")
        };
        _userRepository.GetByEmailAsync(request.Email).Returns(user);

        // Act
        var result = await _authService.LoginAsync(request);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value!.Email.Should().Be(request.Email);
        result.Value.Token.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnFailure_WhenUserNotFound()
    {
        // Arrange
        var request = new LoginRequest("notfound@email.com", "123456");
        _userRepository.GetByEmailAsync(request.Email).Returns((User?)null);

        // Act
        var result = await _authService.LoginAsync(request);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("E-mail ou senha inválidos.");
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnFailure_WhenPasswordIsWrong()
    {
        // Arrange
        var request = new LoginRequest("jose@email.com", "wrongpassword");
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "José",
            Email = "jose@email.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456")
        };
        _userRepository.GetByEmailAsync(request.Email).Returns(user);

        // Act
        var result = await _authService.LoginAsync(request);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("E-mail ou senha inválidos.");
    }
}