using CardioTrack.DTOs.Doctor;
using FluentValidation;

namespace CardioTrack.Validators.MedicalHistory
{
    public class UpdateMedicalHistoryValidator : AbstractValidator<UpdateMedicalHistoryRequestDto>
    {
        public UpdateMedicalHistoryValidator()
        {
            RuleFor(x => x.MedicalHistoryId).GreaterThan(0);
            RuleFor(x => x.Condition).MaximumLength(200).When(x => x.Condition != null);
            RuleFor(x => x.Note).MaximumLength(200).When(x => x.Note != null);
        }
    }
}
