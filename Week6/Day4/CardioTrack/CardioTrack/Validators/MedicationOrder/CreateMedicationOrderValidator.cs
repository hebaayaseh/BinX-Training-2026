using CardioTrack.DTOs.MedicationOrder;
using FluentValidation;

namespace CardioTrack.Validators.MedicationOrder
{
    public class CreateMedicationOrderValidator : AbstractValidator<CreateMedicationOrderRequestDto>
    {
        public CreateMedicationOrderValidator()
        {
            RuleFor(x => x.PatientId).GreaterThan(0);
            RuleFor(x => x.Items).NotEmpty().WithMessage("يجب إضافة دواء واحد على الأقل للطلب");

            RuleForEach(x => x.Items).ChildRules(item =>
            {
                item.RuleFor(i => i.PharmacyStockId).GreaterThan(0);
                item.RuleFor(i => i.Quantity).GreaterThan(0);
            });
        }
    }
}