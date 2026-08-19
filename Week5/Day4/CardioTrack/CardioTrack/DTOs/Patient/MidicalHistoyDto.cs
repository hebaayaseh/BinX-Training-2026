namespace CardioTrack.DTOs.Patient
{
    public class MidicalHistoyDto
    {
        public int Id { get; set; }
        public string Condition { get; set; }
        public string Note { get; set; }
        public DateTime DiagnosisDate { get; set; }
        public int RecordedByDoctorId { get; set; }
    }
}
