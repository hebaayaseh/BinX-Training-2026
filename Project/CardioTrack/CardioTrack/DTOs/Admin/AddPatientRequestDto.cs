using CardioTrack.Enums;

namespace CardioTrack.DTOs.Admin
{
    public class AddPatientRequestDto
    {
        public string FullName { get; set; }
        public string phoneNumber {  get; set; }
        public DateTime DatrOfBirth {  get; set; }
        public Gender Gender { get; set; }
        public BloodType BloodType { get; set; }
        public int DoctorId { get; set; }
        public string Address { get; set; }
    }
}
