using CardioTrack.DTOs.LogIn;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Xunit;

namespace CardioTrack.Tests.Integration
{
    /// <summary>
    /// GAP #1 (P0) — nothing in the suite proved that the [Authorize(Policy = ...)]
    /// attributes actually block the wrong role. If someone deleted an attribute by
    /// accident, every existing test would still pass.
    ///
    /// These tests run against the real pipeline (authentication -> authorization ->
    /// controller), so they fail the moment a policy is removed or weakened.
    /// They stop at the authorization layer, so no service code runs and no email
    /// is sent, which keeps them fast and side-effect free.
    /// </summary>
    public class RoleProtectionIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory factory;

        public RoleProtectionIntegrationTests(CustomWebApplicationFactory factory)
        {
            this.factory = factory;
        }

        private async Task<HttpClient> CreateDoctorClientAsync()
        {
            var client = factory.CreateClient();

            var loginResponse = await client.PostAsJsonAsync("/api/login", new LoginRequestDto
            {
                Email = "integration.doctor@test.com",
                Password = "Test1234@"
            });
            loginResponse.EnsureSuccessStatusCode();

            var login = await loginResponse.Content.ReadFromJsonAsync<LoginResponseDto>();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", login!.AccessToken);

            return client;
        }

        // ---------- AdminOnly endpoints must reject a Doctor token ----------

        [Fact]
        public async Task AddDoctor_WithDoctorToken_ReturnsForbidden()
        {
            // Arrange
            var client = await CreateDoctorClientAsync();
            var request = new
            {
                FullName = "Should Never Be Created",
                Email = "privilege.escalation@test.com",
                PhoneNumber = "0599000000"
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/admin/add-doctor", request);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task AddNurse_WithDoctorToken_ReturnsForbidden()
        {
            var client = await CreateDoctorClientAsync();
            var request = new
            {
                FullName = "Should Never Be Created",
                Email = "nurse.escalation@test.com",
                PhoneNumber = "0599000001"
            };

            var response = await client.PostAsJsonAsync("/api/admin/add-nurse", request);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task DeactivateActor_WithDoctorToken_ReturnsForbidden()
        {
            var client = await CreateDoctorClientAsync();

            var response = await client.PutAsJsonAsync("/api/admin/deactive", new { ActorId = 1 });

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task GetAuditLog_WithDoctorToken_ReturnsForbidden()
        {
            var client = await CreateDoctorClientAsync();

            var response = await client.GetAsync("/api/Admin");

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        // ---------- TechnicianOnly endpoint must reject a Doctor token ----------

        [Fact]
        public async Task UpdateLabRequestStatus_WithDoctorToken_ReturnsForbidden()
        {
            var client = await CreateDoctorClientAsync();
            var request = new { LabRequestId = 1, NewStatus = "Collected" };

            var response = await client.PutAsJsonAsync("/api/Doctor/status", request);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        // ---------- PatientOnly endpoint must reject a Doctor token ----------

        [Fact]
        public async Task PatientViewProfile_WithDoctorToken_ReturnsForbidden()
        {
            var client = await CreateDoctorClientAsync();

            var response = await client.GetAsync("/api/editprofile/patient-view-profile");

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        // ---------- No token at all must be 401, not 403 and not 200 ----------

        [Theory]
        [InlineData("/api/admin/get-staff")]
        [InlineData("/api/admin/get-patient-by-doctor")]
        [InlineData("/api/Admin")]
        [InlineData("/api/editprofile/staff-view-profile")]
        [InlineData("/api/DoctorOrNurse/doctor-view-vitalsignalert")]
        public async Task ProtectedGetEndpoints_WithoutToken_ReturnUnauthorized(string url)
        {
            var client = factory.CreateClient();

            var response = await client.GetAsync(url);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task AddDoctor_WithoutToken_ReturnsUnauthorized()
        {
            var client = factory.CreateClient();
            var request = new
            {
                FullName = "Anonymous Attacker",
                Email = "anonymous@test.com",
                PhoneNumber = "0599000002"
            };

            var response = await client.PostAsJsonAsync("/api/admin/add-doctor", request);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}