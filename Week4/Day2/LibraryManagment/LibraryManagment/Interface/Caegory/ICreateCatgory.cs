using LibraryManagment.DTO.CategoryDto;

namespace LibraryManagment.Interface.Caegory
{
    public interface ICreateCatgory
    {
        Task<CreateCategoryResponseDto> CreateCategory(CreateCategoryRequestDto request);
    }
}
