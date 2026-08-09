
using LibraryManagment.Data;
using LibraryManagment.Interface.Auth;
using LibraryManagment.Interface.Caegory;
using LibraryManagment.Services.Auth;
using LibraryManagment.Services.Category;
using Microsoft.AspNetCore.Identity;
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

            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<AppDBContext>()
            .AddDefaultTokenProviders();


            builder.Services.AddScoped<ICreateCatgory, CreateGategoryService>();
            builder.Services.AddScoped<IReturnCategories, ReturnCategoriesService>();
            builder.Services.AddScoped<IUpdateCategory, UpdateCategoryService>();
            builder.Services.AddScoped<IDeleteCategory, DeleteCategoryService>();
            builder.Services.AddScoped<IAuth, AuthService>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
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
