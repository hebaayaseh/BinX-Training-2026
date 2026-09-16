namespace CardioTrack.DTOs.Admin
{
    /// <summary>Details of a doctor account to create.</summary>
    public class AddDoctorRequestDto
    {
        /// <summary>Full name as it appears to patients.</summary>
        /// <example>Dr. Sara Khalil</example>
        public string FullName { get; set; }

        /// <summary>Work email. The temporary password is sent here.</summary>
        /// <example>sara.khalil@cardiotrack.com</example>
        public string Email { get; set; }

        /// <summary>Contact number.</summary>
        /// <example>0599123456</example>
        public string PhoneNumber { get; set; }
    }
}