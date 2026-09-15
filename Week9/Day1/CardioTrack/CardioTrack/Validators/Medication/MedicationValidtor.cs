using CardioTrack.DTOs.Doctor;
using FluentValidation;

namespace CardioTrack.Validators.Medication
{
    public class MedicationValidtor : AbstractValidator<AddMedicationRequestDto>
    {
        public MedicationValidtor()
        {
            RuleFor(x => x.PatientId).GreaterThan(0);
            RuleFor(x => x.DrugName).NotEmpty().Length(2, 100);
            RuleFor(x => x.Dosage).NotEmpty().MaximumLength(50);
            RuleFor(x=> x.Frequency).NotEmpty().MaximumLength(50);
            RuleFor(x => x.StartDate).NotEmpty();
            RuleFor(x => x.EndDate).GreaterThan(x => x.StartDate).When(x => x.EndDate.HasValue);


        }
    }
}
