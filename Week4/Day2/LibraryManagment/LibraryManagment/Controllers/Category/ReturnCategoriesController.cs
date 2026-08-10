using LibraryManagment.Interface.Caegory;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagment.Controllers.Category
{
    [ApiController]
    [Route("api-category")]
    public class ReturnCategoriesController :ControllerBase
    {
        private readonly IReturnCategories returnCategories;
        public ReturnCategoriesController(IReturnCategories returnCategories)
        {
            this.returnCategories = returnCategories;
        }
        [HttpGet("get-categories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await returnCategories.ReturnCategories();
            if(categories == null)
            {
                return NotFound("No categories found.");
            }
            return Ok(categories);
        }
        [HttpGet("get-category/{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await returnCategories.ReturnCategory(id);
            if (category == null)
            {
                return NotFound("No category found.");
            }
            return Ok(category);
        }

    }
}
