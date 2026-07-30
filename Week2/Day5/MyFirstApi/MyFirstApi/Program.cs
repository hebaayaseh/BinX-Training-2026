
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyFirstApi.Interface;
using MyFirstApi.Middleware; // ✔
using MyFirstApi.Models;
using MyFirstApi.Service;

namespace MyFirstApi
{

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IItem, GetItems>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            List<OrderItem> items = new()
            {
            new OrderItem{Id=1,name = "Laptop",price = 2000 , quantity = 5},
            new OrderItem{Id=2,name = "Caple",price = 20 , quantity = 10},
            new OrderItem{Id=3,name = "Mouse",price = 50 , quantity = 8}
            };

            app.MapGet("/minimal/items", () =>
            {
                return items;
            });

            app.MapGet("/minimal/items/Id", (int Id) =>
            {
                var item = items.FirstOrDefault(i => i.Id == Id);
                return item is null
                ? Results.NotFound()
                : Results.Ok(item);
            });

            app.UseAuthorization();

            // right register it in Program.cs
            app.UseMiddleware<RequestLoggingMiddleware>();// => each request execute it and print requst mwthod and path

            app.MapControllers();

            //  wrong register it in Program.cs.
            // app.UseMiddleware<RequestLoggingMiddleware>(); => Don’t print request method and path for all controller


            app.Run();
        }
    }
}
