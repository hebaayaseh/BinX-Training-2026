using CardioTrack.DTOs.Doctor;

namespace CardioTrack.Interfaces.IDoctor
{
    public interface IAppointment
    {
        Task<AddAppointmentResponseDto> AddAppointmentAsync(int userId, AddApointmentRequestDto request);
    }
}
