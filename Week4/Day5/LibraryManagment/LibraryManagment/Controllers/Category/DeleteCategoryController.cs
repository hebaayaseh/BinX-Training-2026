using LibraryManagment.Enum;
using LibraryManagment.Interface.Caegory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace LibraryManagment.Controllers.Category
{
    [Authorize]
    [ApiController]
    [Route("api-category")]
    public class DeleteCategoryController : ControllerBase
    {
        private readonly IDeleteCategory deleteCategory;
        public DeleteCategoryController(IDeleteCategory deleteCategory)
        {
            this.deleteCategory = deleteCategory;
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpDelete("delete-category/{id}")]
        [EnableRateLimiting("General")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var response = await deleteCategory.DeleteCategory(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
