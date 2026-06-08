using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MercadoApp.Application.Auth.DTOs;
using MercadoApp.Application.Common;
using MercadoApp.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace MercadoApp.Application.Auth;

public class AuthService(IUserRepository userRepository, IConfiguration configuration)
{
    public async Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request)
    {
        if (await userRepository.ExistsByEmailAsync(request.Email))
            return Result<AuthResponse>.Failure("E-mail já cadastrado.");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        await userRepository.AddAsync(user);
        await userRepository.SaveChangesAsync();

        var token = GenerateToken(user);
        return Result<AuthResponse>.Success(new AuthResponse(token, user.Name, user.Email));
    }

    public async Task<Result<AuthResponse>> LoginAsync(LoginRequest request)
    {
        var user = await userRepository.GetByEmailAsync(request.Email);

        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return Result<AuthResponse>.Failure("E-mail ou senha inválidos.");

        var token = GenerateToken(user);
        return Result<AuthResponse>.Success(new AuthResponse(token, user.Name, user.Email));
    }

    private string GenerateToken(User user)
    {
        var secret = configuration["Jwt__Secret"] ?? configuration["Jwt:Secret"]!;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name)
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}