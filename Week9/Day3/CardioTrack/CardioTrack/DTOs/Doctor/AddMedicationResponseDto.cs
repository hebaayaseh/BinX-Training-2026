namespace CardioTrack.DTOs.Doctor
{
    public class AddMedicationResponseDto
    {
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string DrugName { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
