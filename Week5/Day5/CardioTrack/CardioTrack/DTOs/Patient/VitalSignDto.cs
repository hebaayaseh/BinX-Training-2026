namespace CardioTrack.DTOs.Patient
{
    public class VitalSignDto
    {
        public DateTime RecordedAt { get; set; }
        public double HeartRate { get; set; }
        public double BloodPressureSystolic { get; set; }
        public double BloodPressureDiastolic { get; set; }
        public decimal Temperature { get; set; }
        public double OxygenSaturation { get; set; }
        public int RecordedByUserId { get; set; }
    }
}
