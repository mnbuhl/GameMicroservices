using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Play.Common.Identity
{
    public static class IdentityExtensions
    {
        public static AuthenticationBuilder AddJwtBearerAuthentication(this IServiceCollection services)
        {
            return services
                .ConfigureOptions<ConfigureJwtBearerOptions>()
                .AddAuthentication()
                .AddJwtBearer();
        }
    }
}