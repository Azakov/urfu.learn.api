using Contracts.Services;
using Contracts.Types.Course;
using Contracts.Types.Group;
using Contracts.Types.Solution;
using Contracts.Types.User;
using Core.Services;
using Dapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Core
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy(DefaultCorsPolicy, builder =>
            {
                builder.WithOrigins("http://localhost:4200")
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials();
            }));

            services.AddMvc();
            services.AddControllers();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie();
            services.AddAuthorization();

            services.AddSingleton<IAuthService, AuthService>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IGroupService, GroupService>();
            services.AddSingleton<ICourseService, CourseService>();
            services.AddSingleton<ISolutionService, SolutionService>();

            DefaultTypeMap.MatchNamesWithUnderscores = true;
            SqlMapper.AddTypeHandler(typeof(Profile), new DapperTypeHandler());
            SqlMapper.AddTypeHandler(typeof(Solution), new DapperTypeHandler());
            SqlMapper.AddTypeHandler(typeof(Group), new DapperTypeHandler());
            SqlMapper.AddTypeHandler(typeof(Course), new DapperTypeHandler());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseCors(DefaultCorsPolicy);
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        private const string DefaultCorsPolicy = nameof(DefaultCorsPolicy);
    }
}