using LibraryManagment.DTO.CategoryDto;

namespace LibraryManagment.Interface.Caegory
{
    public interface IReturnCategories
    {
        Task<CategoriesResponseDto> ReturnCategories();
        Task<CategoryResponseDto> ReturnCategory(int id);
    }
}
