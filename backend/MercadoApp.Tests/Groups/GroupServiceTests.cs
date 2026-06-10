using FluentAssertions;
using MercadoApp.Application.Common;
using MercadoApp.Application.Groups;
using MercadoApp.Application.Groups.DTOs;
using MercadoApp.Domain.Entities;
using NSubstitute;

namespace MercadoApp.Tests.Groups;

public class GroupServiceTests
{
    private readonly IGroupRepository _groupRepository;
    private readonly GroupService _groupService;

    public GroupServiceTests()
    {
        _groupRepository = Substitute.For<IGroupRepository>();
        _groupService = new GroupService(_groupRepository);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnSuccess_WhenRequestIsValid()
    {
        // Arrange
        var request = new CreateGroupRequest("Família Silva");
        var userId = Guid.NewGuid();

        // Act
        var result = await _groupService.CreateAsync(request, userId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value!.Name.Should().Be(request.Name);
        result.Value.Id.Should().NotBeEmpty();
    }

    [Fact]
    public async Task CreateAsync_ShouldAddOwnerAsMember()
    {
        // Arrange
        var request = new CreateGroupRequest("Família Silva");
        var userId = Guid.NewGuid();
        Group? savedGroup = null;
        await _groupRepository.AddAsync(Arg.Do<Group>(g => savedGroup = g));

        // Act
        await _groupService.CreateAsync(request, userId);

        // Assert
        savedGroup.Should().NotBeNull();
        savedGroup!.Members.Should().HaveCount(1);
        savedGroup.Members.First().UserId.Should().Be(userId);
        savedGroup.Members.First().Role.Should().Be(Domain.Enums.GroupRole.Owner);
    }

    [Fact]
    public async Task GetMyGroupsAsync_ShouldReturnGroups_WhenUserHasGroups()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var groups = new List<Group>
        {
            new() { Id = Guid.NewGuid(), Name = "Família Silva", CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Trabalho", CreatedAt = DateTime.UtcNow }
        };
        _groupRepository.GetByUserIdAsync(userId).Returns(groups);

        // Act
        var result = await _groupService.GetMyGroupsAsync(userId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetMyGroupsAsync_ShouldReturnEmptyList_WhenUserHasNoGroups()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _groupRepository.GetByUserIdAsync(userId).Returns([]);

        // Act
        var result = await _groupService.GetMyGroupsAsync(userId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }
}