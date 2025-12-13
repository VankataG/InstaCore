using Microsoft.EntityFrameworkCore;

using InstaCore.Core;
using InstaCore.Core.Contracts;
using InstaCore.Data;
using InstaCore.Infrastructure.Security;
using InstaCore.Core.Services.Contracts;
using InstaCore.Api.Extensions;



namespace InstaCore.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var corsPolicy = "React_Frontend";

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(corsPolicy, policy =>
                {
                    policy.WithOrigins("http://localhost:5173")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            var connectionString = 
                builder.Configuration.GetConnectionString("InstaCore") ?? 
                throw new InvalidOperationException("Connection string InstaCore not found.");

            builder.Services.AddDbContext<InstaCoreDbContext>(options =>
                options.UseSqlServer(
                    connectionString,
                    sql => sql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(3), null))
            );


            // Add services to the container.
            builder.Services.AddSingleton<IPasswordHasher, BCryptPasswordHasher>();
            builder.Services.AddSingleton<IJwtTokenService, JwtTokenService>();

            builder.Services.AddUserDefinedServices(typeof(IAuthService).Assembly);
            builder.Services.AddRepositories(typeof(UserRepository).Assembly);
            
            builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
            JwtOptions jwt = builder.Configuration.GetSection("Jwt").Get<JwtOptions>()!;
            builder.Services.AddJwtConfiguration(jwt);


            builder.Services.AddAuthorization();
            builder.Services.AddControllers();

            builder.Services.AddCustomSwagger();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors(corsPolicy);
            app.UseStaticFiles();

            app.UseErrorHandling();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
