using CardioTrack.DTOs.EmerganceContact;

namespace CardioTrack.Interfaces.IEmergancyContact
{
    public interface IEmergancyContact
    {
        Task<AddEmergancyContactResponseDto> AddEmergancyContactAsync(AddEmergancyContactRequestDto request);
        Task<GetEmergancyContactsResponseDto> GetEmergancyContactsAsync(int patientId);
        Task<string> RemoveEmergancyContactAsync(RemoveEmergencyContactRequestDto request);
        Task<string> UpdateEmerganceContactAsync(UpdateEmerganceContactRequestDto request);
    }
}
