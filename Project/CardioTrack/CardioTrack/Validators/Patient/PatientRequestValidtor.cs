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
            RuleFor(x => x.DatrOfBirth).LessThan(DateTime.UtcNow).GreaterThan(DateTime.UtcNow.AddYears(-120));
            RuleFor(x => x.Gender).IsInEnum();
            RuleFor(x => x.phoneNumber).NotEmpty().Matches(@"^05\d{8}$");
            RuleFor(x => x.Address).NotEmpty().MaximumLength(100);
            RuleFor(x => x.BloodType).IsInEnum();
        }
    }
}
