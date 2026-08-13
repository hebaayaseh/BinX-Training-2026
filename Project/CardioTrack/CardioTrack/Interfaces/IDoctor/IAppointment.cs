using CardioTrack.DTOs.Doctor;

namespace CardioTrack.Interfaces.IDoctor
{
    public interface IAppointment
    {
        Task<AddAppointmentResponseDto> AddAppointmentAsync(int userId, AddAppointmentRequestDto request);
        Task<string> CompleteAppointmentAsync(int userId , CompleteAppointmentRequestDto request);
        Task<string> CancelAppointmentAsync(int userId , CancelAppointmentRequestDto request);
        Task<GetAppointmentResponseDto> GetAppointmentAsync(int userId , GetAppointmentsRequestDto request);
    }
}
