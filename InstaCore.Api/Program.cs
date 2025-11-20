using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using InstaCore.Core;
using InstaCore.Core.Contracts;
using InstaCore.Core.Services;
using InstaCore.Data;
using InstaCore.Infrastructure.Security;
using InstaCore.Core.Services.Contracts;

using InstaCore.Infrastructure.Repositories;
using InstaCore.Core.Contracts.Repos;
using InstaCore.Api.Extensions;



namespace InstaCore.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

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

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IFollowRepository, FollowRepository>();
            builder.Services.AddScoped<IPostRepository, PostRepository>();

            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IPostService, PostService>();


            //Add and configure Jwt
            builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
            builder.Services.AddSingleton<IJwtTokenService,JwtTokenService>();

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

            app.UseErrorHandling();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
