using LibraryManagment.DTO.CategoryDto;

namespace LibraryManagment.Interface.Caegory
{
    public interface IUpdateCategory
    {
        Task<UpdateCategoryResponseDto> UpdateCategory(int id, UpdateCategoryRequestDto request);
    }
}
