using CardioTrack.DTOs.Doctor;

namespace CardioTrack.Interfaces.IDoctor
{
    public interface IGetPatients
    {
        Task<GetPatientsDto> GetPatientsAsync(int userId);
    }
}
