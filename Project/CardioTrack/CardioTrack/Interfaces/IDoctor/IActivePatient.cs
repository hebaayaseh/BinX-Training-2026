using CardioTrack.DTOs.Doctor;

namespace CardioTrack.Interfaces.IDoctor
{
    public interface IActivePatient
    {
        Task<string> ActivePatientProfile(int userId, ActivePatientProfileRequestDto request);
    }
}
