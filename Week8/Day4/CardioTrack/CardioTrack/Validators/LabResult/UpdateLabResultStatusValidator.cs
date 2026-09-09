using CardioTrack.DTOs.LabResult;
using FluentValidation;

namespace CardioTrack.Validators.LabResult
{
    public class UpdateLabResultStatusValidator : AbstractValidator<UpdateLabResultStatusRequestDto>
    {
        public UpdateLabResultStatusValidator()
        {
            RuleFor(x => x.LabResultId).GreaterThan(0);
            RuleFor(x => x.NewStatus).IsInEnum();
        }
    }
}