using LibraryManagment.DTO;

namespace LibraryManagment.Interface
{
    public interface IDeleteCategory
    {
        Task<DeleteCategoryResponseDto> DeleteCategory(int id); 
    }
}
