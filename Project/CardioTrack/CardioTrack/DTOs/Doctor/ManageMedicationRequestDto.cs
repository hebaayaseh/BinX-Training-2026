using System.ComponentModel.DataAnnotations;

namespace CardioTrack.DTOs.Doctor
{
    public class ManageMedicationRequestDto
    {
        public int PatientId { get; set; }
        public string DrugName { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
