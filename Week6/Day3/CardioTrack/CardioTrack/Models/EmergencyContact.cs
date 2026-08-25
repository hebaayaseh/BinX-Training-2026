using System.ComponentModel.DataAnnotations;

namespace CardioTrack.Models
{
    public class EmergencyContact
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        [MaxLength(50)]
        public string FullName {  get; set; }
        [MaxLength(50)]
        public string Relationship { get; set; }
        [MaxLength(15)]
        public string PhoneNumber { get; set; }

        // Navication Proparity :
        public Patient? Patient { get; set; }
    }
}
