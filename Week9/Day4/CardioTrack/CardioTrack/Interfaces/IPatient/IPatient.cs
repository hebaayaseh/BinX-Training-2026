using CardioTrack.DTOs.Patient;

namespace CardioTrack.Interfaces.IPetient
{
    public interface IPatient
    {
        Task<ViewAppointmentResponseDto> ViewAppointmentAsync(int userId , ViewAppointmentRequestDto request);
        Task<ViewMedicalHistoryResponseDto> ViewMedicalHistoryAsync(int userId);
        Task<PatientViewVitalSignReponseDto> PatientViewVitalSignAsync(int userId);
        Task<ViewMedicationResponseDto> ViewMedicationAsync(int userId); 
    }
}
