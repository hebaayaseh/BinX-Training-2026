using CardioTrack.DTOs.EditProfile;

namespace CardioTrack.Interfaces.IProfile
{
    public interface IProfile
    {
        Task<EditProfileResponseDto> EditProfileAsync(int userId, EditProfileRequestDto request);
        Task<string> EditEmailRequest(int userId, EditEmailRequestDto request);
        Task<string> ConfirmEmailCode(int userId, CodeVerify codeVerify);
        Task<string> EditPasswordRequest(int userId, EditPasswordRequestDto request);
        Task<string> ConfirmPasswordCode(int userId, CodeVerify codeVerify);
    }
}
