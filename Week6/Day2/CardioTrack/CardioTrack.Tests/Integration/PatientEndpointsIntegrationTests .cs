using CardioTrack.DTOs.LogIn;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Xunit;

namespace CardioTrack.Tests.Integration
{
    public class PatientEndpointsIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient client;

        public PatientEndpointsIntegrationTests(CustomWebApplicationFactory factory)
        {
            client = factory.CreateClient();
        }

        private async Task<string> GetDoctorAccessTokenAsync()
        {
            var loginRequest = new LoginRequestDto
            {
                Email = "integration.doctor@test.com",
                Password = "Test1234@"
            };

            var response = await client.PostAsJsonAsync("/api/login", loginRequest);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
            return result!.AccessToken;
        }

        [Fact]
        public async Task GetPatients_ValidDoctorToken_ReturnsOkWithPatientList()
        {
            // Arrange
            var token = await GetDoctorAccessTokenAsync();
            client.DefaultRequestHeaders.Authorization =new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await client.GetAsync("/api/Doctor/get-patients");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var body = await response.Content.ReadAsStringAsync();
            Assert.Contains("Integration Test Patient", body);   
        }

        [Fact]
        public async Task ManageMedication_NonExistentPatient_ReturnsBadRequest()
        {
            // Arrange
            var token = await GetDoctorAccessTokenAsync();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = new
            {
                PatientId = 600,   
                DrugName = "TestDrug",
                Dosage = "10mg",
                Frequency = "Once daily",
                StartDate = DateTime.UtcNow
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/Doctor/add-Medication", request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var body = await response.Content.ReadAsStringAsync();
            Assert.Contains("Patient not found", body);
        }

        [Fact]
        public async Task GetPatients_WithoutToken_ReturnsUnauthorized()
        {
            // Act
            var response = await client.GetAsync("/api/Doctor/get-patients");

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
        
        [Fact]
        public async Task GetPatients_WithValidJwt_ReturnsOk()
        {
            // Arrange
            var token = await GetDoctorAccessTokenAsync();
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/Doctor/get-patients");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}