using CardioTrack.DTOs.EditProfile;

namespace CardioTrack.Interfaces.IProfile
{
    public interface IProfile
    {
        Task<EditProfileResponseDto> EditProfileAsync(int userId, EditProfileRequestDto request);
        Task<string> EditEmailRequest(int userId, EditEmailRequestDto request);
        Task<string> ConfirmCode(int userId, CodeVerify codeVerify);
    }
}
