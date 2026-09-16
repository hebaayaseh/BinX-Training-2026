using System.ComponentModel.DataAnnotations;

namespace CardioTrack.DTOs.EmerganceContact
{
    public class SummaryEmergenceContactDto
    {
        public int EmergenceContactId { get; set; }
        [MaxLength(50)]
        public string FullName { get; set; }
        [MaxLength(50)]
        public string Relationship { get; set; }
        [MaxLength(15)]
        public string PhoneNumber { get; set; }
    }
}
