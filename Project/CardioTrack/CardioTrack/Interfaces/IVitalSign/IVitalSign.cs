using CardioTrack.DTOs.VitalSign;

namespace CardioTrack.Interfaces.IVitalSign
{
    public interface IVitalSign
    {
        Task<ViewVitalSignResponceDto> ViewVitalSign(int userId , ViewVitalSignRequestDto request);
        Task<VitalSignDto> AddVitalSign(int userId, AddVitalSignRequestDto request);
    }
}
