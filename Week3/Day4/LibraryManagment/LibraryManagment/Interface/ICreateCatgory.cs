using LibraryManagment.DTO;

namespace LibraryManagment.Interface
{
    public interface ICreateCatgory
    {
        Task<CreateCategoryResponseDto> CreateCategory(CreateCategoryRequestDto request);
    }
}
