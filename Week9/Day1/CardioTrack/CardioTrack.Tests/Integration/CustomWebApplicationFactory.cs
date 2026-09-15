using CardioTrack.Data;
using CardioTrack.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CardioTrack.Tests.Integration
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<CardioTrackDbContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                services.AddDbContext<CardioTrackDbContext>(options =>
                {
                    options.UseInMemoryDatabase("IntegrationTestDb");
                });

                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<CardioTrackDbContext>();
                db.Database.EnsureCreated();
                SeedTestData(db);
            });
        }

        private void SeedTestData(CardioTrackDbContext db)
        {
            if (db.users.Any()) return;   

            var doctor = new Models.User
            {
                FullName = "Dr. Integration Test",
                Email = "integration.doctor@test.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Test1234@"),
                IsActive = true,
                Role = UserRole.Doctor,
                PhoneNumber = "05963322886"
            };
            db.users.Add(doctor);
            db.SaveChanges();

            var patient = new Models.Patient
            {
                FullName = "Integration Test Patient",
                DateOfBirth = new DateTime(2004, 4, 5),
                Gender = Gender.Male,
                PhoneNumber = "059542274542",
                Address = "Test Address",
                BloodType = BloodType.A_Positive,
                DoctorId = doctor.Id
            };
            db.patients.Add(patient);
            db.SaveChanges();
        }
    }
}