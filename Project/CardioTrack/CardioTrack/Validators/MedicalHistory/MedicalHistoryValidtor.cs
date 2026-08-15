using CardioTrack.DTOs.Doctor;
using FluentValidation;

namespace CardioTrack.Validators.MedicalHistory
{
    public class MedicalHistoryValidtor :AbstractValidator<AddHistoryRequestDto>
    {
        public MedicalHistoryValidtor()
        {
            RuleFor(x => x.PatientId).GreaterThan(0);
            RuleFor(x=> x.Condition).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Note).MaximumLength(200);
            RuleFor(x => x.DiagnosisDate).LessThanOrEqualTo(DateTime.UtcNow);
        }
    }
}
