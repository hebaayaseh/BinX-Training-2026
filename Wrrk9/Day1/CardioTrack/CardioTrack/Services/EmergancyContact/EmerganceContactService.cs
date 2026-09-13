using CardioTrack.Data;
using CardioTrack.DTOs.EmerganceContact;
using CardioTrack.ExceptionService;
using CardioTrack.Interfaces.IEmergancyContact;
using CardioTrack.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace CardioTrack.Services.EmergancyContact
{
    public class EmerganceContactService : IEmergancyContact
    {
        private readonly CardioTrackDbContext dbContext;
        public EmerganceContactService(CardioTrackDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<AddEmergancyContactResponseDto> AddEmergancyContactAsync(AddEmergancyContactRequestDto request)
        {
            var contacts = request.EmergenceContact
                .Select(c => new EmergencyContact
                {
                    PatientId = request.PatientId,
                    FullName = c.FullName,
                    PhoneNumber = c.PhoneNumber,
                    Relationship = c.Relationship
                })
                .ToList();

            await dbContext.emergencyContact.AddRangeAsync(contacts);

            await dbContext.SaveChangesAsync();

            return new AddEmergancyContactResponseDto
            {
                EmergenceContact = contacts.Select(c => new SummaryEmergenceContactDto
                {
                    EmergenceContactId = c.Id,
                    FullName = c.FullName,
                    PhoneNumber = c.PhoneNumber,
                    Relationship = c.Relationship
                }).ToList()
            };
        }

        public async Task<GetEmergancyContactsResponseDto> GetEmergancyContactsAsync(int patientId)
        {
            var patient = await dbContext.patients
                .FirstOrDefaultAsync(p => p.Id == patientId);

            if (patient == null)
                throw new BadRequestException("Patient not found");

            var contacts = await dbContext.emergencyContact
                .Where(p => p.PatientId == patientId)
                .Select(c => new SummaryEmergenceContactDto 
                { 
                    EmergenceContactId = c.Id,
                    FullName=c.FullName,
                    PhoneNumber=c.PhoneNumber,
                    Relationship=c.Relationship
                }).ToListAsync();

            return new GetEmergancyContactsResponseDto
            {
                EmergenceContact = contacts
            };
        }

        public async Task<string> RemoveEmergancyContactAsync(RemoveEmergencyContactRequestDto request)
        {
            var contact = await dbContext.emergencyContact
                .FirstOrDefaultAsync(c => c.Id == request.EmergancyContactId
                                     && c.PatientId == request.patientId);

            if (contact == null)
                throw new BadHttpRequestException("Contact not found");

            dbContext.emergencyContact.Remove(contact);
            await dbContext.SaveChangesAsync();
            return "تم الحذف بنجاح.";
        }

        public async Task<string> UpdateEmerganceContactAsync(UpdateEmerganceContactRequestDto request)
        {
            var contact = await dbContext.emergencyContact
                .FirstOrDefaultAsync(c => c.Id == request.EmergancyContactId
                                     && c.PatientId == request.PatientId);

            if (contact == null)
                throw new BadHttpRequestException("Contact not found");

            if (request.PhoneNumber != null)
                contact.PhoneNumber = request.PhoneNumber;

            if (request.FullName != null)
                contact.FullName = request.FullName;

            if (request.Relationship != null)
                contact.Relationship = request.Relationship;

            await dbContext.SaveChangesAsync();
            return "تم التعديل بنجاح.";

        }
    }
}
