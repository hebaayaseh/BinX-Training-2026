using LibraryManagment.DTO.CategoryDto;

namespace LibraryManagment.Interface.Caegory
{
    public interface IDeleteCategory
    {
        Task<DeleteCategoryResponseDto> DeleteCategory(int id); 
    }
}
