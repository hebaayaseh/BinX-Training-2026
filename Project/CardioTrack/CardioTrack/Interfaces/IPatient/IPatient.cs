using CardioTrack.DTOs.Patient;

namespace CardioTrack.Interfaces.IPetient
{
    public interface IPatient
    {
        Task<ViewAppointmentResponseDto> ViewAppointment(int userId , ViewAppointmentRequestDto request);
        Task<ViewMedicalHistoryResponseDto> ViewMedicalHistory(int userId);
    }
}
