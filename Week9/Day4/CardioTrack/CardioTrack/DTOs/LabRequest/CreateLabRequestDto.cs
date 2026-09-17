namespace CardioTrack.DTOs.LabRequest
{
    /// <summary>A doctor's order for one or more lab tests.</summary>
    public class CreateLabRequestDto
    {
        /// <summary>Id of the patient the tests are for.</summary>
        /// <example>12</example>
        public int PatientId { get; set; }

        /// <summary>Optional appointment the order is attached to.</summary>
        /// <example>45</example>
        public int? AppointmentId { get; set; }

        /// <summary>One lab request is created per test name.</summary>
        public List<string> TestNames { get; set; } = new List<string>();
    }
}