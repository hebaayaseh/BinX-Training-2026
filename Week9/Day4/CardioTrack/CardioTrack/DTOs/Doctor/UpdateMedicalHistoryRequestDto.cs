namespace CardioTrack.DTOs.Doctor
{
    public class UpdateMedicalHistoryRequestDto
    {
        public int MedicalHistoryId { get; set; }
        public string? Condition { get; set; }
        public string? Note { get; set; }
    }
}
