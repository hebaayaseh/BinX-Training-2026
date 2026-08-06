using LibraryManagment.DTO;

namespace LibraryManagment.Interface
{
    public interface IReturnCategories
    {
        Task<CategoriesResponseDto> ReturnCategories();
        Task<CategoryResponseDto> ReturnCategory(int id);
    }
}
