using CardioTrack.DTOs.VitalSign;

namespace CardioTrack.Interfaces.IDoctor
{
    public interface IVitalSign
    {
        Task<ViewVitalSignResponceDto> ViewVitalSign(int userId , ViewVitalSignRequestDto request);
    }
}
