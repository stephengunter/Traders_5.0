using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Helpers;
using ApplicationCore.Services;

namespace ApplicationCore.Authorization
{
	public class HasPermissionHandler : AuthorizationHandler<HasPermissionRequirement>
	{

		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasPermissionRequirement requirement)
		{
			var permission = requirement.Permission;

			bool valid = false;

			if (permission == Permissions.Admin)
			{
				valid = context.User.Claims.IsBoss() || context.User.Claims.IsDev();
			}
			else if (permission == Permissions.Subscriber)
			{
				valid = context.User.Claims.IsSubscriber();
			}
			else if (permission == Permissions.User)
			{
				valid = context.User.Claims.HasItems();
			}

			if (valid) context.Succeed(requirement);
			else context.Fail();
			
			return Task.CompletedTask;
		}
	}
}
