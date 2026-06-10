using FluentAssertions;
using MercadoApp.Application.Common;
using MercadoApp.Application.Items;
using MercadoApp.Application.Items.DTOs;
using MercadoApp.Domain.Entities;
using MercadoApp.Domain.Enums;
using NSubstitute;

namespace MercadoApp.Tests.Items;

public class ItemServiceTests
{
    private readonly IItemRepository _itemRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly ItemService _itemService;

    public ItemServiceTests()
    {
        _itemRepository = Substitute.For<IItemRepository>();
        _groupRepository = Substitute.For<IGroupRepository>();
        _itemService = new ItemService(_itemRepository, _groupRepository);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnSuccess_WhenGroupExists()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var request = new CreateItemRequest("Arroz", ItemUnit.Kg);
        _groupRepository.GetByIdAsync(groupId).Returns(new Group { Id = groupId, Name = "Família" });

        // Act
        var result = await _itemService.CreateAsync(request, groupId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value!.Name.Should().Be(request.Name);
        result.Value.Unit.Should().Be(request.Unit);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnFailure_WhenGroupNotFound()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var request = new CreateItemRequest("Arroz", ItemUnit.Kg);
        _groupRepository.GetByIdAsync(groupId).Returns((Group?)null);

        // Act
        var result = await _itemService.CreateAsync(request, groupId);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Grupo não encontrado.");
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnSuccess_WhenItemExists()
    {
        // Arrange
        var itemId = Guid.NewGuid();
        var item = new Item { Id = itemId, Name = "Arroz", Unit = ItemUnit.Kg };
        _itemRepository.GetByIdAsync(itemId).Returns(item);

        // Act
        var result = await _itemService.DeleteAsync(itemId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        item.IsDeleted.Should().BeTrue();
        item.DeletedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFailure_WhenItemNotFound()
    {
        // Arrange
        var itemId = Guid.NewGuid();
        _itemRepository.GetByIdAsync(itemId).Returns((Item?)null);

        // Act
        var result = await _itemService.DeleteAsync(itemId);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Item não encontrado.");
    }
}