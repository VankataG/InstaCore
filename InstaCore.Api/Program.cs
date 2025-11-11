using System.Text;
using InstaCore.Core;
using InstaCore.Core.Contracts;
using InstaCore.Data;
using InstaCore.Infrastructure.Repositories;
using InstaCore.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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


            //Add and configure Jwt
            builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
            builder.Services.AddSingleton<IJwtTokenService,JwtTokenService>();

            JwtOptions jwt = builder.Configuration.GetSection("Jwt").Get<JwtOptions>()!;

            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = jwt.Issuer,
                        ValidAudience = jwt.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromSeconds(30)
                    };
                });

            builder.Services.AddAuthorization();


            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();

                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
