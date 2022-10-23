using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace WetPet.Infrastructure.Common.Utils;

public class AnonymousAuthorizationHandler : IAuthorizationHandler
{
    private readonly IConfiguration _config;

    public AnonymousAuthorizationHandler(IConfiguration config)
    {
        _config = config;
    }

    public Task HandleAsync(AuthorizationHandlerContext context)
    {
        foreach (IAuthorizationRequirement requirement in context.PendingRequirements.ToList())
            context.Succeed(requirement); //Simply pass all requirements


        var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, "Test-Subject")
            };

        context.User.AddIdentity(new(claims));
        return Task.CompletedTask;
    }
}