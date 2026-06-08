using MercadoApp.Application.Common;
using MercadoApp.Application.Groups.DTOs;
using MercadoApp.Domain.Entities;
using MercadoApp.Domain.Enums;

namespace MercadoApp.Application.Groups;

public class GroupService(IGroupRepository groupRepository)
{
    public async Task<Result<GroupResponse>> CreateAsync(CreateGroupRequest request, Guid userId)
    {
        var group = new Group
        {
            Id = Guid.NewGuid(),
            Name = request.Name
        };

        group.Members.Add(new GroupMember
        {
            UserId = userId,
            GroupId = group.Id,
            Role = GroupRole.Owner
        });

        await groupRepository.AddAsync(group);
        await groupRepository.SaveChangesAsync();

        return Result<GroupResponse>.Success(new GroupResponse(group.Id, group.Name, group.CreatedAt));
    }

    public async Task<Result<List<GroupResponse>>> GetMyGroupsAsync(Guid userId)
    {
        var groups = await groupRepository.GetByUserIdAsync(userId);
        var response = groups
            .Select(g => new GroupResponse(g.Id, g.Name, g.CreatedAt))
            .ToList();

        return Result<List<GroupResponse>>.Success(response);
    }
}