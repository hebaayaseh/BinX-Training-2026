using LibraryManagment.Interface;
using Microsoft.AspNetCore.Mvc;

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
        [HttpPut("update-category/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] DTO.UpdateCategoryRequestDto request)
        {
            try
            {
                var response = await updateCategory.UpdateCategory(id, request);
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
