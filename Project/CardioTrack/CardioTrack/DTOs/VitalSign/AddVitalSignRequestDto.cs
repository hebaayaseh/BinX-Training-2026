namespace CardioTrack.DTOs.VitalSign
{
    public class AddVitalSignRequestDto
    {
        public double HeartRate { get; set; }
        public double BloodPressureSystolic { get; set; }
        public double BloodPressureDiastolic { get; set; }
        public decimal Temperature { get; set; }
        public double OxygenSaturation { get; set; }
    }
}
