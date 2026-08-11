using LibraryManagment.Data;
using LibraryManagment.DTO.CategoryDto;
using LibraryManagment.Interface.Caegory;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagment.Services.Category
{
    public class CreateGategoryService : ICreateCatgory
    {
        private readonly AppDBContext context;

        public CreateGategoryService(AppDBContext context)
        {
            this.context = context;
        }
        public async Task<CreateCategoryResponseDto> CreateCategory(CreateCategoryRequestDto request)
        {
            var category = await context.categories
                .FirstOrDefaultAsync(c=>c.name == request.name);

            if(category !=null)
                throw new Exception("Category already exists");

            await context.categories.AddAsync(new Models.Category
            {
                name = request.name,
                decription = request.description
            });
            await context.SaveChangesAsync();

            return new CreateCategoryResponseDto
            {
                message = "Category created successfully"
            };
        }
    }
}
