using FluentAssertions;
using MercadoApp.Application.Common;
using MercadoApp.Application.Departments;
using MercadoApp.Application.Departments.DTOs;
using MercadoApp.Domain.Entities;
using NSubstitute;

namespace MercadoApp.Tests.Departments;

public class DepartmentServiceTests
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly DepartmentService _departmentService;

    public DepartmentServiceTests()
    {
        _departmentRepository = Substitute.For<IDepartmentRepository>();
        _groupRepository = Substitute.For<IGroupRepository>();
        _departmentService = new DepartmentService(_departmentRepository, _groupRepository);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnSuccess_WhenGroupExists()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var request = new CreateDepartmentRequest("Cozinha", "🍳");
        _groupRepository.GetByIdAsync(groupId).Returns(new Group { Id = groupId, Name = "Família" });

        // Act
        var result = await _departmentService.CreateAsync(request, groupId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value!.Name.Should().Be(request.Name);
        result.Value.Icon.Should().Be(request.Icon);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnFailure_WhenGroupNotFound()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var request = new CreateDepartmentRequest("Cozinha", "🍳");
        _groupRepository.GetByIdAsync(groupId).Returns((Group?)null);

        // Act
        var result = await _departmentService.CreateAsync(request, groupId);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Grupo não encontrado.");
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnSuccess_WhenDepartmentExists()
    {
        // Arrange
        var departmentId = Guid.NewGuid();
        var department = new Department { Id = departmentId, Name = "Cozinha", Icon = "🍳" };
        _departmentRepository.GetByIdAsync(departmentId).Returns(department);

        // Act
        var result = await _departmentService.DeleteAsync(departmentId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        department.IsDeleted.Should().BeTrue();
        department.DeletedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFailure_WhenDepartmentNotFound()
    {
        // Arrange
        var departmentId = Guid.NewGuid();
        _departmentRepository.GetByIdAsync(departmentId).Returns((Department?)null);

        // Act
        var result = await _departmentService.DeleteAsync(departmentId);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Departamento não encontrado.");
    }
}