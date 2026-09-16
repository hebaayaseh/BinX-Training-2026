using CardioTrack.DTOs.LogIn;
using CardioTrack.DTOs.Token;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace CardioTrack.Tests.Integration
{
    /// <summary>
    /// GAP #2 (P0) — login / refresh were only covered at the service level, and
    /// logout had no test of its own at all (it was only used as a helper inside a
    /// refresh test). Nothing verified the HTTP status codes, the FluentValidation
    /// layer, or that logout actually kills a refresh token end to end.
    /// </summary>
    public class AuthEndpointsIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient client;

        public AuthEndpointsIntegrationTests(CustomWebApplicationFactory factory)
        {
            client = factory.CreateClient();
        }

        // ---------------- Login: happy path ----------------

        [Fact]
        public async Task Login_ValidCredentials_ReturnsOkWithBothTokens()
        {
            // Arrange
            var request = new LoginRequestDto
            {
                Email = "integration.doctor@test.com",
                Password = "Test1234@"
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/login", request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var body = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
            Assert.NotNull(body);
            Assert.False(string.IsNullOrWhiteSpace(body!.AccessToken));
            Assert.False(string.IsNullOrWhiteSpace(body.RefreshToken));
        }

        [Fact]
        public async Task Login_ValidCredentials_DoesNotLeakPasswordHash()
        {
            var request = new LoginRequestDto
            {
                Email = "integration.doctor@test.com",
                Password = "Test1234@"
            };

            var response = await client.PostAsJsonAsync("/api/login", request);
            var raw = await response.Content.ReadAsStringAsync();

            Assert.DoesNotContain("PasswordHash", raw, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("$2a$", raw);   // BCrypt prefix
        }

        // ---------------- Login: error paths ----------------

        [Fact]
        public async Task Login_WrongPassword_ReturnsForbidden()
        {
            var request = new LoginRequestDto
            {
                Email = "integration.doctor@test.com",
                Password = "DefinitelyWrong123@"
            };

            var response = await client.PostAsJsonAsync("/api/login", request);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Login_UnknownEmail_ReturnsForbidden()
        {
            var request = new LoginRequestDto
            {
                Email = "nobody.here@test.com",
                Password = "Test1234@"
            };

            var response = await client.PostAsJsonAsync("/api/login", request);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Login_MalformedEmail_ReturnsBadRequestFromValidator()
        {
            var request = new LoginRequestDto
            {
                Email = "not-an-email",
                Password = "Test1234@"
            };

            var response = await client.PostAsJsonAsync("/api/login", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Login_EmptyPassword_ReturnsBadRequestFromValidator()
        {
            var request = new LoginRequestDto
            {
                Email = "integration.doctor@test.com",
                Password = ""
            };

            var response = await client.PostAsJsonAsync("/api/login", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        // ---------------- Refresh ----------------

        [Fact]
        public async Task Refresh_ValidToken_ReturnsNewTokenPair()
        {
            // Arrange
            var login = await LoginAsync();

            // Act
            var response = await client.PostAsJsonAsync("/api/token/refresh-token",
                new RefreshRequestDto { RefreshToken = login.RefreshToken });

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var refreshed = await response.Content.ReadFromJsonAsync<TokenResponseDto>();
            Assert.NotNull(refreshed);
            Assert.NotEqual(login.RefreshToken, refreshed!.RefreshToken);   // rotation
        }

        [Fact]
        public async Task Refresh_UnknownToken_ReturnsUnauthorized()
        {
            var response = await client.PostAsJsonAsync("/api/token/refresh-token",
                new RefreshRequestDto { RefreshToken = "this-token-was-never-issued" });

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Refresh_ReusedToken_ReturnsUnauthorized()
        {
            // Arrange — use the refresh token once, which rotates and revokes it
            var login = await LoginAsync();
            var first = await client.PostAsJsonAsync("/api/token/refresh-token",
                new RefreshRequestDto { RefreshToken = login.RefreshToken });
            first.EnsureSuccessStatusCode();

            // Act — replay the same (now revoked) token
            var replay = await client.PostAsJsonAsync("/api/token/refresh-token",
                new RefreshRequestDto { RefreshToken = login.RefreshToken });

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, replay.StatusCode);
        }

        // ---------------- Logout ----------------

        [Fact]
        public async Task Logout_ValidToken_ReturnsOk()
        {
            var login = await LoginAsync();

            var response = await client.PostAsJsonAsync("/api/token/logout",
                new LogoutRequestDto { RefreshToken = login.RefreshToken });

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Logout_ThenRefresh_ReturnsUnauthorized()
        {
            // Arrange
            var login = await LoginAsync();

            // Act
            var logout = await client.PostAsJsonAsync("/api/token/logout",
                new LogoutRequestDto { RefreshToken = login.RefreshToken });
            logout.EnsureSuccessStatusCode();

            var refresh = await client.PostAsJsonAsync("/api/token/refresh-token",
                new RefreshRequestDto { RefreshToken = login.RefreshToken });

            // Assert — the session is really dead, not just "logged out" in the UI
            Assert.Equal(HttpStatusCode.Unauthorized, refresh.StatusCode);
        }

        [Fact]
        public async Task Logout_UnknownToken_StillReturnsOk()
        {
            // Logout is intentionally idempotent: it must not tell an attacker
            // whether a given refresh token exists.
            var response = await client.PostAsJsonAsync("/api/token/logout",
                new LogoutRequestDto { RefreshToken = "never-issued-token" });

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        // ---------------- helper ----------------

        private async Task<LoginResponseDto> LoginAsync()
        {
            var response = await client.PostAsJsonAsync("/api/login", new LoginRequestDto
            {
                Email = "integration.doctor@test.com",
                Password = "Test1234@"
            });
            response.EnsureSuccessStatusCode();

            return (await response.Content.ReadFromJsonAsync<LoginResponseDto>())!;
        }
    }
}