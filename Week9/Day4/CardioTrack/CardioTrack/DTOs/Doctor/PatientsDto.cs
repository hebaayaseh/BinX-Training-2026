using CardioTrack.Enums;

namespace CardioTrack.DTOs.Doctor
{
    public class PatientsDto
    {
        public int PatientId { get; set; }
        public string FullName { get; set; }
        public Gender Gender {  get; set; }
        public BloodType BloodType {  get; set; }
        public string PhoneNumber {  get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
    }
}
