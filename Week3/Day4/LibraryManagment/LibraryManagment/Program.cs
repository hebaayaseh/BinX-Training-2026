
using LibraryManagment.Data;
using LibraryManagment.Interface;
using LibraryManagment.Services.Category;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagment
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

            var connectionString = builder.Configuration.GetConnectionString("Connection");
            builder.Services.AddDbContext<AppDBContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));


            builder.Services.AddScoped<ICreateCatgory, CreateGategoryService>();
            builder.Services.AddScoped<IReturnCategories, ReturnCategoriesService>();
            builder.Services.AddScoped<IUpdateCategory, UpdateCategoryService>();
            builder.Services.AddScoped<IDeleteCategory, DeleteCategoryService>();

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
