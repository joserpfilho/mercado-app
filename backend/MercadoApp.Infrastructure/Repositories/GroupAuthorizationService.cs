using MercadoApp.Application.Common;
using MercadoApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MercadoApp.Infrastructure.Repositories;

public class GroupAuthorizationService(AppDbContext context) : IGroupAuthorizationService
{
    public async Task<bool> IsMemberAsync(Guid groupId, Guid userId) =>
        await context.GroupMembers
            .AnyAsync(gm => gm.GroupId == groupId && gm.UserId == userId);
}