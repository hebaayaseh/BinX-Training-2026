using CardioTrack.DTOs.VitalSign;

namespace CardioTrack.Interfaces.IVitalSign
{
    public interface IVitalSign
    {
        Task<ViewVitalSignResponseDto> ViewVitalSign(int userId , ViewVitalSignRequestDto request);
        Task<VitalSignDto> AddVitalSign(int userId, AddVitalSignRequestDto request);
        Task<DoctorViewVitalSignAlertResponceDto> DoctorViewVitalSignAlert(int userId);
        Task<NurseViewVitalSignAlertResponceDto> NurseViewVitalSignAlert(int userId);
        Task<string> DoctorResoleVitalSign(int userId, ResoleVitalSignAlertRequestDto request);
        Task<string> NurseResoleVitalSign(int userId, ResoleVitalSignAlertRequestDto request);
    }
}
