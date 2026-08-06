using LibraryManagment.Data;
using LibraryManagment.DTO;
using LibraryManagment.Interface;

namespace LibraryManagment.Services.Category
{
    public class DeleteCategoryService : IDeleteCategory
    {
        private readonly AppDBContext context;
        public DeleteCategoryService(AppDBContext context)
        {
            this.context = context;
        }   
        public async Task<DeleteCategoryResponseDto> DeleteCategory(int id)
        {
            var category = await context.categories
                .FindAsync(id);
            if (category == null)
            {
                throw new Exception("Category not found");
            }
            context.categories.Remove(category);
            await context.SaveChangesAsync();
            return new DeleteCategoryResponseDto { message = "Category deleted successfully" };
        }
    }
}
