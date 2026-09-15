using System.ComponentModel.DataAnnotations;

namespace CardioTrack.DTOs.EmerganceContact
{
    public class UpdateEmerganceContactRequestDto
    {
        public int EmergancyContactId { get; set; }
        public int PatientId {  get; set; }
        public string? FullName { get; set; }
        [MaxLength(50)]
        public string? Relationship { get; set; }
        [MaxLength(15)]
        public string? PhoneNumber { get; set; }
    }

}
