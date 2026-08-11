using CardioTrack.DTOs;
using CardioTrack.DTOs.LogIn;

namespace CardioTrack.Interfaces.IAdmin
{
    public interface IAuth
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    }
}
