
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyFirstApi.Models;

namespace MyFirstApi
{
//    Hands-On Lab: Scaffold the API & Build First Endpoints
//1. Run "dotnet new webapi -o MyFirstApi" and confirm it runs with "dotnet run", checking the Swagger UI in a browser. ?
//2. Add a Controller with a GET endpoint returning a hardcoded list of items from your domain model.?
//3. Add a GET endpoint with a route parameter that returns a single item by ID.?
//4. Add the same two endpoints again as Minimal APIs directly in Program.cs, and compare the two approaches.
//5. Test all 4 endpoints in Postman and save them as a collection
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


            app.MapControllers();

            app.Run();
        }
    }
}
