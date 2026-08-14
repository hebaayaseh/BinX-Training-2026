using CardioTrack.DTOs.Admin;
using CardioTrack.Services.Admin;
using FluentValidation;

namespace CardioTrack.Validators.Patient
{
    public class PatientRequestValidtor : AbstractValidator<AddPatientRequestDto>
    {
        public PatientRequestValidtor()
        {
            RuleFor(x => x.FullName).NotEmpty().Length(3, 100);
            RuleFor(x => x.DatrOfBirth).LessThan(DateTime.UtcNow).GreaterThan(DateTime.UtcNow.AddYears(-120))
        }
    }
}
