using System.ComponentModel.DataAnnotations;

namespace CardioTrack.DTOs.Patient
{
    public class MedicationResponseDto
    {
        public string DrugName { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int PrescribedByDoctorId { get; set; }
    }
}
