using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.Configuration;
using AirlineService.Data;
using System.Text.Json.Serialization;

namespace AirlineService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<AirlineServiceDbContext>(options => {
                options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]);
            });
            builder.Services.AddControllers()
                 .AddJsonOptions(o =>
                 {
                     o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                     //o.JsonSerializerOptions.MaxDepth = 0;
                 });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}