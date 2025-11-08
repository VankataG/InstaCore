using InstaCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InstaCore.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = 
                builder.Configuration.GetConnectionString("InstaCore") ?? 
                throw new InvalidOperationException("Connection string InstaCore not founf.");

            builder.Services.AddDbContext<InstaCoreDbContext>(options =>
                options.UseSqlServer(
                    connectionString,
                    sql => sql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(3), null))
            );

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
