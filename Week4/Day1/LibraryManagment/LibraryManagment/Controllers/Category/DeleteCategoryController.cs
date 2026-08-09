using LibraryManagment.Interface.Caegory;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagment.Controllers.Category
{
    [ApiController]
    [Route("api-category")]
    public class DeleteCategoryController : ControllerBase
    {
        private readonly IDeleteCategory deleteCategory;
        public DeleteCategoryController(IDeleteCategory deleteCategory)
        {
            this.deleteCategory = deleteCategory;
        }
        [HttpDelete("delete-category/{id}")]
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
