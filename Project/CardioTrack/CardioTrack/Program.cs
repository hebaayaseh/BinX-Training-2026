
using CardioTrack.Data;
using CardioTrack.Helper;
using CardioTrack.Infrastructure.Services.TokenService;
using CardioTrack.Interfaces.IAdmin;
using CardioTrack.Interfaces.IDoctor;
using CardioTrack.Interfaces.IEmail;
using CardioTrack.Interfaces.IProfile;
using CardioTrack.Interfaces.RefreshToken;
using CardioTrack.Middleware;
using CardioTrack.Services.Admin;
using CardioTrack.Services.Doctor;
using CardioTrack.Services.Email;
using CardioTrack.Services.Profile;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System.Text;

namespace CardioTrack
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // 1. CONTROLLERS
            builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
            });

            // 2. SWAGGER
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
            // 3. Connection Database
            builder.Services.AddDbContext<CardioTrackDbContext>(options =>
               options.UseMySql(
                   builder.Configuration.GetConnectionString("Connection"),
                   ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("Connection"))
               )
           );

            // 4. CORS
            var allowedOrigins = builder.Configuration
                .GetSection("Cors:AllowedOrigins")
                .Get<string[]>() ?? [];

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CardioTrackPolicy", policy =>
                {
                    policy.WithOrigins(allowedOrigins)
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });
            // 5. JWT AUTHENTICATION
            var jwtKey = builder.Configuration["Jwt:Key"]!;
            var jwtIssuer = builder.Configuration["Jwt:Issuer"]!;
            var jwtAudience = builder.Configuration["Jwt:Audience"]!;

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                    ValidateIssuer = true,
                    ValidIssuer = jwtIssuer,
                    ValidateAudience = true,
                    ValidAudience = jwtAudience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    NameClaimType = System.Security.Claims.ClaimTypes.NameIdentifier
                };

                options.Events = new JwtBearerEvents
                {
                    
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsJsonAsync(new
                        {
                            status = 401,
                            message = "Unauthorized. Please login first."
                        });
                    },
                    OnForbidden = async context =>
                    {
                        context.Response.StatusCode = 403;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsJsonAsync(new
                        {
                            status = 403,
                            message = "Forbidden. You do not have permission to access this resource."
                        });
                    }
                };
            });

            // 6. AUTHORIZATION — ROLES
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
                options.AddPolicy("MedicalStaff", policy => policy.RequireRole("Admin", "Doctor", "Nurse"));
                options.AddPolicy("DoctorOnly", policy => policy.RequireRole("Doctor"));
                options.AddPolicy("NurseOnly", policy => policy.RequireRole("Nurse"));
                options.AddPolicy("PatientOnly", policy => policy.RequireRole("Patient"));
                options.AddPolicy("AllActors", policy => policy.RequireRole("Admin", "Doctor", "Nurse", "Patient"));
            });

            //7. Services  
            builder.Services.AddSingleton<JwtTokenGenerator>();
            builder.Services.AddScoped<SeedData>();
            builder.Services.AddScoped<IAuth, AuthService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IEmail, EmailService>();
            builder.Services.AddScoped<IAddStaff, AddStaffService>();
            builder.Services.AddScoped<IProfile, EditProfileService>();
            builder.Services.AddScoped<IGetStaff, GetStaffService>();
            builder.Services.AddScoped<IGetPatient, GetPatientService>();
            builder.Services.AddScoped<IActiveDeactive, ActiveDeactiveActorService>();
            builder.Services.AddScoped<IGetPatients, GetPatientsService>();
            builder.Services.AddScoped<IManageMedication, ManageMedicationService>();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<CardioTrackDbContext>();
                await dbContext.Database.MigrateAsync();

                var seeder = scope.ServiceProvider.GetRequiredService<SeedData>();
                await seeder.SeedAllAsync();
            }

            // Configure the HTTP request pipeline.


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRequestLocalization();
            app.UseCors("CardioTrackPolicy");
            app.UseRouting();
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
