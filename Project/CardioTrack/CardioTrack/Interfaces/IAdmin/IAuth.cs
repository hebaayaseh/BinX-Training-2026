using CardioTrack.DTOs.Admin;

namespace CardioTrack.Interfaces.IAdmin
{
    public interface IAuth
    {
        Task<AdminLoginResponseDto> LoginAsync(AdminLoginRequestDto request);
    }
}
