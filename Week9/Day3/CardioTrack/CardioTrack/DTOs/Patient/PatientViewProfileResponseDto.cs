using CardioTrack.Enums;

namespace CardioTrack.DTOs.Patient
{
    public class PatientViewProfileResponseDto
    {
        public int PatientId { get; set; }
        public string PatientFullName { get; set; }
        public BloodType BloodType { get; set; }
        public Gender Gender { get; set; }
        public string Email {  get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
