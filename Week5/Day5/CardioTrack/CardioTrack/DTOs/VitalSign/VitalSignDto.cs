namespace CardioTrack.DTOs.VitalSign
{
    public class VitalSignDto
    {
        public int VitalSignId{ get; set; }
        public string PatientFullName { get; set; }
        public DateTime RecordedAt { get; set; }
        public double HeartRate { get; set; }
        public double BloodPressureSystolic { get; set; }
        public double BloodPressureDiastolic { get; set; }
        public decimal Temperature { get; set; }
        public double OxygenSaturation { get; set; }
        public int RecordedByUserId { get; set; }
        public string RecordedByUseName { get; set; }
    }
}
