using CardioTrack.DTOs.VitalSign;
using FluentValidation;

namespace CardioTrack.Validators.VitalSign
{
    public class ResolveVitalSignValidtor : AbstractValidator<ResoleVitalSignAlertRequestDto>
    {
        public ResolveVitalSignValidtor()
        {
            RuleFor(x => x.VilateSignAlertId).GreaterThan(0);
        }
    }
}
