using CardioTrack.DTOs.VitalSign;
using FluentValidation;

namespace CardioTrack.Validators.VitalSign
{
    public class VitalSignValidtor : AbstractValidator<AddVitalSignRequestDto>
    {
        public VitalSignValidtor()
        {
            RuleFor(x => x.PatientId).GreaterThan(0);

            RuleFor(x => x.HeartRate).InclusiveBetween(20, 250);

            RuleFor(x => x.BloodPressureSystolic).InclusiveBetween(50, 250);
            RuleFor(x => x.BloodPressureDiastolic).InclusiveBetween(30, 150);

            RuleFor(x => x)
                .Must(x => x.BloodPressureSystolic > x.BloodPressureDiastolic)
                .WithMessage("Systolic pressure must be greater than diastolic pressure")
                .WithName("BloodPressure");

            RuleFor(x => x.Temperature).InclusiveBetween(30m, 45m);
            RuleFor(x => x.OxygenSaturation).InclusiveBetween(0, 100);
        }
    }
}