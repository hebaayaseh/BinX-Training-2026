using CardioTrack.Enums;

namespace CardioTrack.DTOs.Doctor
{
    public class GetPatientsQueryRequestDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public Gender? Gender { get; set; }
        public BloodType? BloodType { get; set; }
        public string SortBy { get; set; } = "FullName";   
        public bool SortDescending { get; set; } = false;
    }
}