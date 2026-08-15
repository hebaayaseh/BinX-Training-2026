using CardioTrack.DTOs.Doctor;

namespace CardioTrack.Interfaces.IDoctor
{
    public interface IActiveDeactivePatient
    {
        Task<string> ActivePatientProfile(int userId, ActivePatientProfileRequestDto request);
        Task<string> DeactivePatientProofile(int userId, GetPatientRequestDto request);
    }
}
