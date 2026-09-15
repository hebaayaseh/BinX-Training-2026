using CardioTrack.DTOs.Admin;

namespace CardioTrack.Interfaces.IAdmin
{
    public interface IAddPatient
    {
        Task<string> AddPatientAsync(int userId, AddPatientRequestDto request);
    }
}
