using CardioTrack.DTOs.Admin;

namespace CardioTrack.Interfaces.IAdmin
{
    public interface IActiveDeactive 
    {
        Task<string> ActiveActor(int userId ,ActiveDeactiveDto Actor);
        Task<string> DeactiveActor(int userId ,ActiveDeactiveDto Actor);
    }
}
