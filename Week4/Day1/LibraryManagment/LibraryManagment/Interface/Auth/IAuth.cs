using LibraryManagment.DTO.AuthDto;

namespace LibraryManagment.Interface.Auth
{
    public interface IAuth
    {
        Task<RegisterResponceDto> Register(RegisterRequestDto request);
    }
}
