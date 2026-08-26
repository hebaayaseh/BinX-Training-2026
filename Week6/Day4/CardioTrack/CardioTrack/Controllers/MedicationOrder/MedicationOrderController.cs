using CardioTrack.DTOs.MedicationOrder;
using CardioTrack.Interfaces.IMedicationOrder;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CardioTrack.Controllers.MedicationOrder
{
    [ApiController]
    [Route("api/medication-orders")]
    public class MedicationOrderController : ControllerBase
    {
        private readonly IMedicationOrder orderService;
        public MedicationOrderController(IMedicationOrder orderService)
        {
            this.orderService = orderService;
        }

        [Authorize(Policy = "DoctorOrNurse")]
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateMedicationOrderRequestDto request, IValidator<CreateMedicationOrderRequestDto> validator)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await orderService.CreateOrderAsync(userId, request);
            return Ok(result);
        }
    }
}