using MercadoApp.Application.Common;
using MercadoApp.Domain.Entities;
using MercadoApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MercadoApp.Infrastructure.Repositories;

public class DepartmentRepository(AppDbContext context) : IDepartmentRepository
{
    public async Task<Department?> GetByIdAsync(Guid id) =>
        await context.Departments.FirstOrDefaultAsync(d => d.Id == id);

    public async Task<List<Department>> GetByGroupIdAsync(Guid groupId) =>
        await context.Departments
            .Where(d => d.GroupId == groupId)
            .ToListAsync();

    public async Task AddAsync(Department department) =>
        await context.Departments.AddAsync(department);

    public async Task SaveChangesAsync() =>
        await context.SaveChangesAsync();
}