using LibraryManagment.Data;
using LibraryManagment.DTO;
using LibraryManagment.Interface;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagment.Services.Category
{
    public class UpdateCategoryService : IUpdateCategory
    {
        private readonly AppDBContext context;
        public UpdateCategoryService(AppDBContext context)
        {
            this.context = context;
        }
        public async Task<UpdateCategoryResponseDto> UpdateCategory(int id, UpdateCategoryRequestDto request)
        {
            var category = await context.categories
                .FirstOrDefaultAsync(c => c.id == id);
            if(category == null)
                return new UpdateCategoryResponseDto { message = "Category not found" };

            if (string.IsNullOrWhiteSpace(request.name))
            {
                return new UpdateCategoryResponseDto { message = "Name cannot be empty." };
            }
            category.name = request.name;
            category.decription = request.description;
            await context.SaveChangesAsync();
            return new UpdateCategoryResponseDto { message = "Category updated successfully" };

        }
    }
}
