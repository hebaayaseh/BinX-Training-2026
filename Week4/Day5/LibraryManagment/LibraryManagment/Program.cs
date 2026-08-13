using FluentValidation;
using LibraryManagment.Data;
using LibraryManagment.Enum;
using LibraryManagment.Helper;
using LibraryManagment.Interface.Auth;
using LibraryManagment.Interface.Caegory;
using LibraryManagment.Services.Auth;
using LibraryManagment.Services.Category;
using LibraryManagment.Validations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Threading.RateLimiting;
using System.Threading.Tasks;

namespace LibraryManagment
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Enter your JWT token. Example: Bearer eyJhbGci..."
                });

                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id   = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

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

            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
            var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings!.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSettings.Key))
                    };
                });

            builder.Services.AddValidatorsFromAssemblyContaining<CreateCategoryValidtor>();
            builder.Services.AddValidatorsFromAssemblyContaining<UpdateCategoryValidtor>();
            builder.Services.AddScoped<ICreateCatgory, CreateGategoryService>();
            builder.Services.AddScoped<IReturnCategories, ReturnCategoriesService>();
            builder.Services.AddScoped<IUpdateCategory, UpdateCategoryService>();
            builder.Services.AddScoped<IDeleteCategory, DeleteCategoryService>();
            builder.Services.AddScoped<IAuth, AuthService>();

            builder.Services.AddScoped(sp => sp.GetRequiredService<IOptions<JwtSettings>>().Value);
            builder.Services.AddScoped<SignInManager<IdentityUser>>();

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy =>
                    policy.RequireRole(UserRole.Admin.ToString()));
            });


            var allowedOrigin = builder.Configuration["Cors:AllowedOrigin"]
                ?? throw new InvalidOperationException(
                    "Cors:AllowedOrigin is missing from configuration. Check appsettings.json.");

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                    policy.WithOrigins(allowedOrigin)
                          .AllowAnyHeader()
                          .AllowAnyMethod());
            });

            var rateLimitSection = builder.Configuration.GetSection("RateLimiting");

            builder.Services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                options.AddFixedWindowLimiter("General", opt =>
                {
                    opt.PermitLimit = rateLimitSection.GetValue<int>("GeneralPermitLimit");
                    opt.Window = TimeSpan.FromSeconds(rateLimitSection.GetValue<int>("GeneralWindowSeconds"));
                    opt.QueueLimit = 0;
                });

                options.AddFixedWindowLimiter("Login", opt =>
                {
                    opt.PermitLimit = rateLimitSection.GetValue<int>("LoginPermitLimit");
                    opt.Window = TimeSpan.FromSeconds(rateLimitSection.GetValue<int>("LoginWindowSeconds"));
                    opt.QueueLimit = 0;
                });
            });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                await DbSeeder.SeedRolesAndUser(scope.ServiceProvider);
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowFrontend");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseRateLimiter();

            app.MapControllers();

            app.Run();
        }
    }
}