using LibraryManagment.DTO.CategoryDto;
using LibraryManagment.Interface.Caegory;
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
