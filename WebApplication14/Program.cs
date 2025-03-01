using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApplication14.Data;
using WebApplication14;
using WebApplication14.Models;

namespace WebApplication14
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<WebApplication14Context>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("WebApplication14Context") ?? throw new InvalidOperationException("Connection string 'WebApplication14Context' not found.")));

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

                        if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
};

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

                        app.MapUserEndpoints();

                        app.MapRegionEndpoints();

                        app.MapIssueTypeEndpoints();

                        app.MapReportEndpoints();

            app.Run();
        }
    }
}
