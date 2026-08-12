using CardioTrack.DTOs.Admin;

namespace CardioTrack.Interfaces.IAdmin
{
    public interface IGetStaff
    {
        Task<GetStaffResponseDto> GetStaffAsync(int userId); 
    }
}
