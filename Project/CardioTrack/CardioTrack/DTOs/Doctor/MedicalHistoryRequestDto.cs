using System.ComponentModel.DataAnnotations;

namespace CardioTrack.DTOs.Doctor
{
    public class MedicalHistoryRequestDto
    {
        public string Condition { get; set; }
        public string Note { get; set; }
        public DateTime DiagnosisDate { get; set; }
        public int PatientId { get; set; }
    }
}
