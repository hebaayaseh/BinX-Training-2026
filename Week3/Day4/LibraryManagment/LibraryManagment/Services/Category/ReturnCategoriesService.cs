using LibraryManagment.Data;
using LibraryManagment.DTO;
using LibraryManagment.Interface;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagment.Services.Category
{
    public class ReturnCategoriesService : IReturnCategories
    {
        private readonly AppDBContext context;
        public ReturnCategoriesService(AppDBContext context)
        {
            this.context = context;
        }   

        public async Task<CategoriesResponseDto> ReturnCategories()
        {
            var categories = await context.categories
                .Select(c => new CategoryDto
                {
                    id = c.id,
                    name = c.name,
                    description = c.decription
                }).ToListAsync();

            return new CategoriesResponseDto 
            { 
                categories = categories 
            };

        }

        public Task<CategoryResponseDto> ReturnCategory(int id)
        {
            var category = context.categories
                .Where(c => c.id == id)
                .Select(c => new CategoryResponseDto
                {
                    id = c.id,
                    name = c.name,
                    description = c.decription
                }).FirstOrDefaultAsync();

            return category;
        }
    }
}
