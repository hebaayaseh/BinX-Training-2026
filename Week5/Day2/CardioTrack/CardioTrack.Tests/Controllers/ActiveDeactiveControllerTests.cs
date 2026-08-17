using CardioTrack.Controllers.ActiveDeactiveActor;
using CardioTrack.DTOs.Admin;
using CardioTrack.ExceptionService;
using CardioTrack.Interfaces.IAdmin;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CardioTrack.Tests.Controllers
{
    public class ActiveDeactiveControllerTests
    {
        private static ActiveDeactiveController BuildController(IActiveDeactive service, int userId = 1)
        {
            var controller = new ActiveDeactiveController(service);
            var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(identity) }
            };
            return controller;
        }

    
      [Fact]
        public async Task ActiveActor_ValidRequest_ReturnsOkWithMessage()
        {
            var activeDeactiveMock = new Mock<IActiveDeactive>();
            var validatorMock = new Mock<IValidator<ActiveDeactiveDto>>();
            var request = new ActiveDeactiveDto { ActorId = 2 };

            validatorMock
                .Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            activeDeactiveMock
                .Setup(x => x.ActiveActor(1, request))
                .ReturnsAsync("تم تفعيل الحساب بنجاح.");

            var controller = BuildController(activeDeactiveMock.Object, userId: 1);

            var result = await controller.ActiveActor(request, validatorMock.Object);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("تم تفعيل الحساب بنجاح.", okResult.Value);
        }

        [Fact]
        public async Task ActiveActor_InvalidRequest_ReturnsBadRequest()
        {
            var activeDeactiveMock = new Mock<IActiveDeactive>();
            var validatorMock = new Mock<IValidator<ActiveDeactiveDto>>();
            var failures = new List<FluentValidation.Results.ValidationFailure>
            {
                new FluentValidation.Results.ValidationFailure("ActorId", "ActorId is required")
            };

            validatorMock
                .Setup(v => v.ValidateAsync(It.IsAny<ActiveDeactiveDto>(), default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult(failures));

            var controller = BuildController(activeDeactiveMock.Object);

            var result = await controller.ActiveActor(new ActiveDeactiveDto(), validatorMock.Object);

            Assert.IsType<BadRequestObjectResult>(result);
            activeDeactiveMock.Verify(x => x.ActiveActor(It.IsAny<int>(), It.IsAny<ActiveDeactiveDto>()), Times.Never);
        }

        [Fact]
        public async Task ActiveActor_ServiceThrowsBadRequestException_ExceptionPropagates()
        {
            var activeDeactiveMock = new Mock<IActiveDeactive>();
            var validatorMock = new Mock<IValidator<ActiveDeactiveDto>>();
            var request = new ActiveDeactiveDto { ActorId = 999 };

            validatorMock
                .Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            activeDeactiveMock
                .Setup(x => x.ActiveActor(1, request))
                .ThrowsAsync(new BadRequestException("Actor Not Found!"));

            var controller = BuildController(activeDeactiveMock.Object, userId: 1);

            await Assert.ThrowsAsync<BadRequestException>(
                () => controller.ActiveActor(request, validatorMock.Object));

            activeDeactiveMock.Verify(x => x.ActiveActor(1, request), Times.Once);
        }
        [Fact]
        public async Task ActiveActor_OnSuccess_CallsServiceExactlyOnce()
        {
            var activeDeactiveMock = new Mock<IActiveDeactive>();
            var validatorMock = new Mock<IValidator<ActiveDeactiveDto>>();
            var request = new ActiveDeactiveDto { ActorId = 2 };

            validatorMock
                .Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            activeDeactiveMock
                .Setup(x => x.ActiveActor(1, request))
                .ReturnsAsync("تم تفعيل الحساب بنجاح.");

            var controller = BuildController(activeDeactiveMock.Object, userId: 1);

            await controller.ActiveActor(request, validatorMock.Object);

            activeDeactiveMock.Verify(x => x.ActiveActor(1, request), Times.Once);
        }

        [Fact]
        public async Task DeactiveActor_ValidRequest_ReturnsOkWithMessage()
        {
            var activeDeactiveMock = new Mock<IActiveDeactive>();
            var validatorMock = new Mock<IValidator<ActiveDeactiveDto>>();
            var request = new ActiveDeactiveDto { ActorId = 2 };

            validatorMock
                .Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            activeDeactiveMock
                .Setup(x => x.DeactiveActor(1, request))
                .ReturnsAsync("تم تعطيل الحساب بنجاح.");

            var controller = BuildController(activeDeactiveMock.Object, userId: 1);

            var result = await controller.DeactiveActor(request, validatorMock.Object);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("تم تعطيل الحساب بنجاح.", okResult.Value);
        }

        [Fact]
        public async Task DeactiveActor_InvalidRequest_ReturnsBadRequest()
        {
            var activeDeactiveMock = new Mock<IActiveDeactive>();
            var validatorMock = new Mock<IValidator<ActiveDeactiveDto>>();
            var failures = new List<FluentValidation.Results.ValidationFailure>
            {
                new FluentValidation.Results.ValidationFailure("ActorId", "ActorId is required")
            };

            validatorMock
                .Setup(v => v.ValidateAsync(It.IsAny<ActiveDeactiveDto>(), default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult(failures));

            var controller = BuildController(activeDeactiveMock.Object);

            var result = await controller.DeactiveActor(new ActiveDeactiveDto(), validatorMock.Object);

            Assert.IsType<BadRequestObjectResult>(result);
            activeDeactiveMock.Verify(x => x.DeactiveActor(It.IsAny<int>(), It.IsAny<ActiveDeactiveDto>()), Times.Never);
        }

        [Fact]
        public async Task DeactiveActor_ServiceThrowsBadRequestException_ExceptionPropagates()
        {
            var activeDeactiveMock = new Mock<IActiveDeactive>();
            var validatorMock = new Mock<IValidator<ActiveDeactiveDto>>();
            var request = new ActiveDeactiveDto { ActorId = 999 };

            validatorMock
                .Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            activeDeactiveMock
                .Setup(x => x.DeactiveActor(1, request))
                .ThrowsAsync(new BadRequestException("Actor Not Found!"));

            var controller = BuildController(activeDeactiveMock.Object, userId: 1);

            await Assert.ThrowsAsync<BadRequestException>(
                () => controller.DeactiveActor(request, validatorMock.Object));

            activeDeactiveMock.Verify(x => x.DeactiveActor(1, request), Times.Once);
        }

        [Fact]
        public async Task DeactiveActor_OnSuccess_CallsServiceExactlyOnce()
        {
            var activeDeactiveMock = new Mock<IActiveDeactive>();
            var validatorMock = new Mock<IValidator<ActiveDeactiveDto>>();
            var request = new ActiveDeactiveDto { ActorId = 2 };

            validatorMock
                .Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            activeDeactiveMock
                .Setup(x => x.DeactiveActor(1, request))
                .ReturnsAsync("تم تعطيل الحساب بنجاح.");

            var controller = BuildController(activeDeactiveMock.Object, userId: 1);

            await controller.DeactiveActor(request, validatorMock.Object);

            activeDeactiveMock.Verify(x => x.DeactiveActor(1, request), Times.Once);
        }
    }
}
