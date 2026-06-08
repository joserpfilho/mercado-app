using MercadoApp.Application.Common;
using MercadoApp.Application.Departments.DTOs;
using MercadoApp.Domain.Entities;

namespace MercadoApp.Application.Departments;

public class DepartmentService(IDepartmentRepository departmentRepository, IGroupRepository groupRepository)
{
    public async Task<Result<DepartmentResponse>> CreateAsync(CreateDepartmentRequest request, Guid groupId)
    {
        var group = await groupRepository.GetByIdAsync(groupId);
        if (group is null)
            return Result<DepartmentResponse>.Failure("Grupo não encontrado.");

        var department = new Department
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Icon = request.Icon,
            GroupId = groupId
        };

        await departmentRepository.AddAsync(department);
        await departmentRepository.SaveChangesAsync();

        return Result<DepartmentResponse>.Success(
            new DepartmentResponse(department.Id, department.Name, department.Icon));
    }

    public async Task<Result<bool>> DeleteAsync(Guid departmentId)
    {
        var department = await departmentRepository.GetByIdAsync(departmentId);
        if (department is null)
            return Result<bool>.Failure("Departamento não encontrado.");

        department.IsDeleted = true;
        department.DeletedAt = DateTime.UtcNow;
        await departmentRepository.SaveChangesAsync();

        return Result<bool>.Success(true);
    }

    public async Task<Result<List<DepartmentResponse>>> GetByGroupAsync(Guid groupId)
    {
        var departments = await departmentRepository.GetByGroupIdAsync(groupId);
        var response = departments
            .Select(d => new DepartmentResponse(d.Id, d.Name, d.Icon))
            .ToList();

        return Result<List<DepartmentResponse>>.Success(response);
    }
}