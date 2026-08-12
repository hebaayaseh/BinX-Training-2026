using LibraryManagment.DTO.CategoryDto;
using LibraryManagment.Enum;
using LibraryManagment.Interface.Caegory;
using LibraryManagment.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagment.Controllers.Category
{
    
    [ApiController]
    [Route("api-category")]
    public class CreateCategoryController : ControllerBase
    {
        private readonly ICreateCatgory createCatgory;
        public CreateCategoryController(ICreateCatgory createCatgory)
        {
            this.createCatgory = createCatgory;
        }
        
        [HttpPost("create-category")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequestDto request)
        {
            var valid = new CreateCategoryValidtor();
            var validationResult = await valid.ValidateAsync(request);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e=>e.ErrorMessage));

            try
            {
                var response = await createCatgory.CreateCategory(request);
                return Created("", response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }
    }
}
