using CardioTrack.DTOs.Admin;

namespace CardioTrack.Interfaces.IAdmin
{
    public interface IAddDoctor
    {
        Task<string> AddDoctorAsync(AddDoctorRequestDto request);
    }
}
