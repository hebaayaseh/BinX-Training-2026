using CardioTrack.Controllers.Doctor;
using CardioTrack.DTOs.Doctor;
using CardioTrack.Interfaces.IDoctor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace CardioTrack.Tests.Controllers
{
    public class MedicalHistoryControllerTests
    {
        [Fact]
        public async Task AddMedicalHistory_ValidRequest_ReturnsOk()
        {
            // Arrange
            var medicalHistoryMock = new Mock<IMedicalHistory>();

            var request = new MedicalHistoryRequestDto
            {
                PatientId = 1,
                Condition = "Hypertension",
                DiagnosisDate = DateTime.UtcNow,
                Note = "Patient requires follow-up"
            };

            var expectedResult = new MedicalHistoryResponseDto
            {
                PatientId = 1,
                PatientName = "Test Patient",
                DiagnosisDate = request.DiagnosisDate,
                Condition = request.Condition,
                Note = request.Note
            };

            medicalHistoryMock
                .Setup(x => x.AddMedicalHistoryAsync(1, request))
                .ReturnsAsync(expectedResult);

            var controller = new MedicalHistoryController(
                medicalHistoryMock.Object);

            var claims = new List<Claim>
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    "1")
            };

            var identity = new ClaimsIdentity(
                claims,
                "TestAuth");

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(identity)
                }
            };

            // Act
            var result = await controller.AddMedicalHistory(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(
                expectedResult,
                okResult.Value);

            medicalHistoryMock.Verify(
                x => x.AddMedicalHistoryAsync(1, request),
                Times.Once);
        }

        [Fact]
        public async Task AddMedicalHistory_ServiceThrowsException_ThrowsException()
        {
            // Arrange
            var medicalHistoryMock = new Mock<IMedicalHistory>();

            var request = new MedicalHistoryRequestDto
            {
                PatientId = 1,
                Condition = "Hypertension",
                DiagnosisDate = DateTime.UtcNow,
                Note = "Test"
            };

            medicalHistoryMock
                .Setup(x => x.AddMedicalHistoryAsync(1, request))
                .ThrowsAsync(
                    new Exception("Patient not found"));

            var controller = new MedicalHistoryController(
                medicalHistoryMock.Object);

            var claims = new List<Claim>
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    "1")
            };

            var identity = new ClaimsIdentity(
                claims,
                "TestAuth");

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(identity)
                }
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => controller.AddMedicalHistory(request));

            Assert.Equal(
                "Patient not found",
                exception.Message);

            medicalHistoryMock.Verify(
                x => x.AddMedicalHistoryAsync(1, request),
                Times.Once);
        }
    }
}