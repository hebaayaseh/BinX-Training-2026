using CardioTrack.Controllers.Doctor;
using CardioTrack.DTOs.Doctor;
using CardioTrack.Interfaces.IDoctor;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.ComponentModel.DataAnnotations;
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
            var validatorMock = new Mock<IValidator<MedicalHistoryRequestDto>>();

            var request = new MedicalHistoryRequestDto
            {
                PatientId = 1,
                Condition = "Hypertension",
                DiagnosisDate = DateTime.UtcNow,
                Note = "Patient requires follow-up"
            };

            validatorMock
                .Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());   

            var expectedResult = new MedicalHistoryResponseDto { };
            medicalHistoryMock
                .Setup(x => x.AddMedicalHistoryAsync(1, request))
                .ReturnsAsync(expectedResult);

            var controller = new MedicalHistoryController(medicalHistoryMock.Object);

            // Act
            var result = await controller.AddMedicalHistory(request, validatorMock.Object);   

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedResult, okResult.Value);
        }

        [Fact]
        public async Task AddMedicalHistory_InvalidRequest_ReturnsBadRequest()
        {
            var validatorMock = new Mock<IValidator<MedicalHistoryRequestDto>>();
            var failures = new List<FluentValidation.Results.ValidationFailure>
    {
        new FluentValidation.Results.ValidationFailure("Condition", "Condition is required")
    };
            validatorMock
                .Setup(v => v.ValidateAsync(It.IsAny<MedicalHistoryRequestDto>(), default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult(failures));

            var controller = new MedicalHistoryController(new Mock<IMedicalHistory>().Object);
            var result = await controller.AddMedicalHistory(new MedicalHistoryRequestDto(), validatorMock.Object);

            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}