namespace CardioTrack.DTOs.Doctor
{
    public class MedicalHistoryResponseDto
    {
        public string Condition { get; set; }
        public string Note { get; set; }
        public DateTime DiagnosisDate { get; set; }
        public int PatientId { get; set; }
        public string PatientName {  get; set; }
    }
}
