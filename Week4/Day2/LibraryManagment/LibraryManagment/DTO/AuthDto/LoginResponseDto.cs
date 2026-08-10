namespace LibraryManagment.DTO.AuthDto
{
    public class LoginResponseDto
    {
        public bool IsSuccess { get; set; }
        public List<string>? Errors { get; set; }
        public string? Token { get; set; }
    }
}
