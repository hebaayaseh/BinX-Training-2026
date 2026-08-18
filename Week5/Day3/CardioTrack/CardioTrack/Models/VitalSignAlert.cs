using CardioTrack.Enums;
using System.ComponentModel.DataAnnotations;

namespace CardioTrack.Models
{
    public class VitalSignAlert
    {
        [Key]
        public int Id { get; set; }
        public int VitalSignId { get; set; }
        public int PatientId { get; set; }
        public Severity Severity { get; set; }
        public AlterType AlterType { get; set; }
        [MaxLength(200)]
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsResolved { get; set; }
        // Navigation property
        public VitalSign? VitalSign { get; set; }
        public Patient? Patient { get; set; }
    }
}
