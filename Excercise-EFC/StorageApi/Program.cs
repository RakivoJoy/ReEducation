using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace StorageApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("StorageApiContext") ?? throw new InvalidOperationException("Connection string 'StorageApiContext' not found.");

            builder.Services.AddDbContext<StorageApiContext>(options => options.UseSqlServer(connectionString));

            // Add AutoMapper - only scan this assembly to avoid loading incompatible types
            builder.Services.AddAutoMapper(typeof(Program).Assembly);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/openapi/v1.json", "Storage API v1");
                    options.RoutePrefix = "swagger";
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
