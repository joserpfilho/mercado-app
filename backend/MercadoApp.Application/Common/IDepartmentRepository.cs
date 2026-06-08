using MercadoApp.Domain.Entities;

namespace MercadoApp.Application.Common;

public interface IDepartmentRepository
{
    Task<Department?> GetByIdAsync(Guid id);
    Task<List<Department>> GetByGroupIdAsync(Guid groupId);
    Task AddAsync(Department department);
    Task SaveChangesAsync();
}