using CardioTrack.DTOs.Admin;

namespace CardioTrack.Interfaces.IAdmin
{
    public interface IGetPatient
    {
        Task<GettPatientResponseDto> GettPatientAsync(int userId);
    }
}
