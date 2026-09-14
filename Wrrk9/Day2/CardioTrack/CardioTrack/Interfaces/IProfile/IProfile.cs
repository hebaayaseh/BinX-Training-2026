using CardioTrack.DTOs.EditProfile;
using CardioTrack.DTOs.Patient;

namespace CardioTrack.Interfaces.IProfile
{
    public interface IProfile
    {
        Task<EditProfileResponseDto> EditProfileAsync(int userId, EditProfileRequestDto request);
        Task<string> EditEmailRequest(int userId, EditEmailRequestDto request);
        Task<string> ConfirmEmailCode( CodeVerify codeVerify);
        Task<string> EditPasswordRequest(int userId, EditPasswordRequestDto request);
        Task<string> ConfirmPasswordCode(CodeVerify codeVerify);
        Task<ViewProfileResponseDto> viewProfile(int userId);
        Task<PatientViewProfileResponseDto> PatientViewProfile(int userId);
    }
}
