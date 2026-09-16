namespace CardioTrack.DTOs.LogIn
{
    /// <summary>Credentials used to sign in.</summary>
    public class LoginRequestDto
    {
        /// <summary>Registered email address.</summary>
        /// <example>doctor@cardiotrack.com</example>
        public string Email { get; set; }

        /// <summary>Account password.</summary>
        /// <example>Doctor@2026</example>
        public string Password { get; set; }
    }
}