using System.Net;
using Xunit;

namespace CardioTrack.Tests.Integration
{
    public class ExceptionHandlingIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public ExceptionHandlingIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task TriggerError_UnhandledException_ReturnsProblemDetailsWithoutLeakingDetails()
        {
            // Act
            var response = await _client.GetAsync("/api/diagnostics/trigger-error");

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

            var body = await response.Content.ReadAsStringAsync();
            
            Assert.DoesNotContain("sensitive internal details", body);
            Assert.DoesNotContain("InvalidOperationException", body);

            Assert.Contains("unexpected error", body, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task TriggerAppException_KnownException_ReturnsProperMessageAndStatus()
        {
            // Act
            var response = await _client.GetAsync("/api/diagnostics/trigger-app-exception");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var body = await response.Content.ReadAsStringAsync();
            Assert.Contains("deliberate BadRequestException", body);
        }
    }
}