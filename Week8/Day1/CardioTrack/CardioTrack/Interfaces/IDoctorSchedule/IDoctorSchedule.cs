using CardioTrack.DTOs.DoctorSchedule;

namespace CardioTrack.Interfaces.IDoctorSchedule
{
    public interface IDoctorSchedule
    {
        Task<AddDoctorScheduleResponseDto> AddDoctorSheduleAsync(int userId, AddDoctorSheduleRequestDto request);
        Task<AddDoctorScheduleResponseDto> UpdaeDoctorScheduleAsync(UpdateDoctorScheduleRequestDto request);
    }
}
