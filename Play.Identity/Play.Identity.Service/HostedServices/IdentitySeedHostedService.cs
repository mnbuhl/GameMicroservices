using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Play.Identity.Service.Entities;
using Play.Identity.Service.Settings;

namespace Play.Identity.Service.HostedServices
{
    public class IdentitySeedHostedService : IHostedService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IdentitySettings _identitySettings;

        public IdentitySeedHostedService(IServiceScopeFactory serviceScopeFactory,
            IOptions<IdentitySettings> identityOptions)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _identitySettings = identityOptions.Value;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<IdentitySeedHostedService>>();

            await CreateRoleIfNotExisting(Roles.Admin, roleManager, logger);
            await CreateRoleIfNotExisting(Roles.Player, roleManager, logger);

            await CreateAdminIfNotExisting(_identitySettings, userManager, logger);
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        private static async Task CreateRoleIfNotExisting(string role, RoleManager<AppRole> roleManager,
            ILogger<IdentitySeedHostedService> logger)
        {
            bool exists = await roleManager.RoleExistsAsync(role);

            if (exists)
                return;

            var result = await roleManager.CreateAsync(new AppRole { Name = role });

            if (result.Errors.Any())
            {
                logger.LogError(string.Join(", ", result.Errors));
            }
        }

        private static async Task CreateAdminIfNotExisting(IdentitySettings settings, UserManager<AppUser> userManager,
            ILogger<IdentitySeedHostedService> logger)
        {
            var adminUser = await userManager.FindByEmailAsync(settings.AdminUserEmail);

            if (adminUser != null)
                return;

            adminUser = new AppUser
            {
                UserName = settings.AdminUserEmail,
                Email = settings.AdminUserEmail,
                Balance = 100
            };

            var result = await userManager.CreateAsync(adminUser, settings.AdminUserPassword);

            if (result.Errors.Any())
            {
                logger.LogError(string.Join(", ", result.Errors));
                return;
            }

            await userManager.AddToRoleAsync(adminUser, Roles.Admin);
        }
    }
}