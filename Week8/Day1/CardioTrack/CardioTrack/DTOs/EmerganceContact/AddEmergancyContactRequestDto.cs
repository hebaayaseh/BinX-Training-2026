namespace CardioTrack.DTOs.EmerganceContact
{
    public class AddEmergancyContactRequestDto
    {
        public int PatientId { get; set; }
        public List<RequestEmergancyContactDto> EmergenceContact { get; set; }
    }
}
