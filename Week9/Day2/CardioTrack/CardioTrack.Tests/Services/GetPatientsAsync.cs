using CardioTrack.Data;
using CardioTrack.Enums;
using CardioTrack.ExceptionService;
using CardioTrack.Models;
using CardioTrack.Services.Doctor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Moq;

namespace CardioTrack.Tests.Services
{
    public class GetPatientsAsync
    {
        private readonly Mock<IDistributedCache> cacheMock;

        public GetPatientsAsync()
        {
            cacheMock = new Mock<IDistributedCache>();
            cacheMock
                .Setup(c => c.GetAsync(It.IsAny<string>(), default))
                .ReturnsAsync((byte[]?)null);
        }

        private static CardioTrackDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<CardioTrackDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new CardioTrackDbContext(options);
        }

        [Fact]
        public async Task GetPatientsAsync_NonDoctorUser_ThrowsInvalidToken()
        {
            await using var dbContext = CreateDbContext();
            var nurse = new User { FullName = "Nurse", Email = "n@test.com", PasswordHash = "hash", IsActive = true, Role = UserRole.Nurse };
            await dbContext.users.AddAsync(nurse);
            await dbContext.SaveChangesAsync();

            var service = new GetPatientsService(dbContext, cacheMock.Object);
            var query = new GetPatientsQueryDto();

            await Assert.ThrowsAsync<InvalidTokenException>(() => service.GetPatientsAsync(nurse.Id, query));
        }

        [Fact]
        public async Task GetPatientsAsync_DoctorWithNoPatients_ReturnsEmptyList()
        {
            await using var dbContext = CreateDbContext();
            var doctor = new User { FullName = "Dr. Empty", Email = "empty@test.com", PasswordHash = "hash", IsActive = true, Role = UserRole.Doctor };
            await dbContext.users.AddAsync(doctor);
            await dbContext.SaveChangesAsync();

            var service = new GetPatientsService(dbContext, cacheMock.Object);
            var query = new GetPatientsQueryDto();

            var result = await service.GetPatientsAsync(doctor.Id, query);

            Assert.Empty(result.Patients);
            Assert.Equal(0, result.TotalCount);
        }
    }
}