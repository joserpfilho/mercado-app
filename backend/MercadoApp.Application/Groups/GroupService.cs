using MercadoApp.Application.Common;
using MercadoApp.Application.Groups.DTOs;
using MercadoApp.Domain.Entities;
using MercadoApp.Domain.Enums;

namespace MercadoApp.Application.Groups;

public class GroupService(IGroupRepository groupRepository, IUserRepository userRepository)
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

    public async Task<Result<GroupMemberResponse>> AddMemberAsync(Guid groupId, AddMemberRequest request)
    {
        var group = await groupRepository.GetByIdAsync(groupId);
        if (group is null)
            return Result<GroupMemberResponse>.Failure("Grupo não encontrado.");

        var user = await userRepository.GetByEmailAsync(request.Email);
        if (user is null)
            return Result<GroupMemberResponse>.Failure("Usuário não encontrado com esse e-mail.");

        if (group.Members.Any(m => m.UserId == user.Id))
            return Result<GroupMemberResponse>.Failure("Usuário já é membro deste grupo.");

        var member = new GroupMember
        {
            UserId = user.Id,
            GroupId = groupId,
            Role = GroupRole.Member
        };

        await groupRepository.AddMemberAsync(member);
        await groupRepository.SaveChangesAsync();

        return Result<GroupMemberResponse>.Success(
            new GroupMemberResponse(user.Id, user.Name, user.Email, member.Role.ToString()));
    }

    public async Task<Result<List<GroupMemberResponse>>> GetMembersAsync(Guid groupId)
    {
        var group = await groupRepository.GetByIdWithMembersAsync(groupId);
        if (group is null)
            return Result<List<GroupMemberResponse>>.Failure("Grupo não encontrado.");

        var response = group.Members
            .Select(m => new GroupMemberResponse(m.UserId, m.User.Name, m.User.Email, m.Role.ToString()))
            .ToList();

        return Result<List<GroupMemberResponse>>.Success(response);
    }
}