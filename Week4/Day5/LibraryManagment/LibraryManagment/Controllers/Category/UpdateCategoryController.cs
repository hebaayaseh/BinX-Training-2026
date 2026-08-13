using LibraryManagment.DTO.CategoryDto;
using LibraryManagment.Interface.Caegory;
using LibraryManagment.Validations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace LibraryManagment.Controllers.Category
{
    [ApiController]
    [Route("api-category")]
    public class UpdateCategoryController :ControllerBase
    {
        private readonly IUpdateCategory updateCategory;
        public UpdateCategoryController(IUpdateCategory updateCategory)
        {
            this.updateCategory = updateCategory;
        }
        [HttpPut("update-category")]
        [EnableRateLimiting("General")]
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryRequestDto request)
        {
            var valid = new UpdateCategoryValidtor();
            var validationResult = await valid.ValidateAsync(request);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            try
            {
                var response = await updateCategory.UpdateCategory( request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                // 404 ==> Not Found
                // 400 ==> Bad Request
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
