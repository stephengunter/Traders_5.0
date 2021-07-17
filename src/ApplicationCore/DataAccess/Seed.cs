using ApplicationCore.Helpers;
using ApplicationCore.Models;
using ApplicationCore.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Views;

namespace ApplicationCore.DataAccess
{
	public class AppDBSeed
	{
		static string AdminEmail = "traders.com.tw@gmail.com";
		static string SubscriberRoleName = AppRoles.Subscriber.ToString();
		static string DevRoleName = AppRoles.Dev.ToString();
		static string BossRoleName = AppRoles.Boss.ToString();

		public static async Task EnsureSeedData(IServiceProvider serviceProvider)
		{
			Console.WriteLine("Seeding database...");

			using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
			{
				var defaultContext = scope.ServiceProvider.GetRequiredService<DefaultContext>();
				defaultContext.Database.Migrate();
				
				var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
				var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

				await SeedRoles(roleManager);
				await SeedUsers(userManager);

				await SeedSymbols(defaultContext);

			}

			Console.WriteLine("Done seeding database.");
			Console.WriteLine();
		}

		static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
		{
			var roles = new List<string> { DevRoleName, BossRoleName, SubscriberRoleName };
			foreach (var item in roles)
			{
				await AddRoleIfNotExist(roleManager, item);
			}


		}

		static async Task AddRoleIfNotExist(RoleManager<IdentityRole> roleManager, string roleName)
		{
			var role = await roleManager.FindByNameAsync(roleName);
			if (role == null)
			{
				await roleManager.CreateAsync(new IdentityRole { Name = roleName });

			}


		}

		static async Task SeedUsers(UserManager<User> userManager)
		{
			string email = AdminEmail;
			var roles = new List<string>() { DevRoleName };

			await CreateUserIfNotExist(userManager, email, roles);

		}


		static async Task CreateUserIfNotExist(UserManager<User> userManager, string email, IList<string> roles = null)
		{
			var user = await userManager.FindByEmailAsync(email);
			if (user == null)
			{
				bool isAdmin = false;
				if (!roles.IsNullOrEmpty())
				{
					isAdmin = roles.Select(r => r.EqualTo(DevRoleName) || r.EqualTo(BossRoleName)).FirstOrDefault();
				}

				var newUser = new User
				{
					Email = email,
					UserName = email,


					EmailConfirmed = isAdmin,
					SecurityStamp = Guid.NewGuid().ToString()

				};


				var result = await userManager.CreateAsync(newUser);

				if (!roles.IsNullOrEmpty())
				{
					await userManager.AddToRolesAsync(newUser, roles);
				}


			}
			else
			{
				if (!roles.IsNullOrEmpty())
				{
					foreach (var role in roles)
					{
						bool hasRole = await userManager.IsInRoleAsync(user, role);
						if (!hasRole) await userManager.AddToRoleAsync(user, role);
					}
				}

			}
		}

		static async Task SeedSymbols(DefaultContext context)
		{
			var symbols = new List<Symbol> 
			{ 
				new Symbol { Code = SymbolCodes.TXI, Title = "台股指數", Type = SymbolType.Index, TimeZone = Models.TimeZone.TW },
				new Symbol { Code = SymbolCodes.TX, Title = "台指期", Type = SymbolType.Futures, TimeZone = Models.TimeZone.TW },
				new Symbol { Code = SymbolCodes.BTCUSD, Title = "比特幣/美元", Type = SymbolType.Crypto, TimeZone = Models.TimeZone.UTC }

			};
			foreach (var symbol in symbols)
			{
				await AddSymbolIfNotExist(context, symbol);
			}


		}

		static async Task AddSymbolIfNotExist(DefaultContext context, Symbol symbol)
		{
			var existingEntity = await context.Symbols.FirstOrDefaultAsync(x => x.Code == symbol.Code);
			if (existingEntity == null)
			{
				await context.Symbols.AddAsync(symbol);
				await context.SaveChangesAsync();
			}
		}

	}
}
