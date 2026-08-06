using LibraryManagment.DTO;

namespace LibraryManagment.Interface
{
    public interface IUpdateCategory
    {
        Task<UpdateCategoryResponseDto> UpdateCategory(int id, UpdateCategoryRequestDto request);
    }
}
