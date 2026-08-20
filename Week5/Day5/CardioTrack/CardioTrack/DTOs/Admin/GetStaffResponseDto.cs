namespace CardioTrack.DTOs.Admin
{
    public class GetStaffResponseDto
    {
        public List<DoctorDto> Doctors { get; set; }
        public List<NurseDto> Nurses { get; set; }
    }
}
