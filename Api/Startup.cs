using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Api
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication()
                .AddScheme<AuthenticationSchemeOptions, CustomAuthenticationHandler>("DefaultAuth", null);

            services.AddAuthorization(config => 
            {
                var defaultAuthBuilder = new AuthorizationPolicyBuilder();
                var defaultAuthPolicy = defaultAuthBuilder
                    .AddRequirements(new OAuthRequirement.JwtRequirement())
                    .Build();
            });

            services.AddScoped<IAuthorizationHandler, OAuthRequirement.JwtRequirementHandler>();

            services.AddHttpClient()
                .AddHttpContextAccessor();

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => 
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
