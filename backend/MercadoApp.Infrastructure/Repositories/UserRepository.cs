using MercadoApp.Application.Common;
using MercadoApp.Domain.Entities;
using MercadoApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MercadoApp.Infrastructure.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email) =>
        await context.Users.FirstOrDefaultAsync(u => u.Email == email);

    public async Task<bool> ExistsByEmailAsync(string email) =>
        await context.Users.AnyAsync(u => u.Email == email);

    public async Task AddAsync(User user) =>
        await context.Users.AddAsync(user);

    public async Task SaveChangesAsync() =>
        await context.SaveChangesAsync();
}