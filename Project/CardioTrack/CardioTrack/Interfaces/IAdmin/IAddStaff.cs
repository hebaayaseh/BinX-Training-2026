using CardioTrack.DTOs.Admin;

namespace CardioTrack.Interfaces.IAdmin
{
    public interface IAddStaff
    {
        Task<string> AddDoctorAsync(AddDoctorRequestDto request);
    }
}
