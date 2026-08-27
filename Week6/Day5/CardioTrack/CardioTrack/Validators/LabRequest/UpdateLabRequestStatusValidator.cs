using CardioTrack.DTOs.LabRequest;
using FluentValidation;

namespace CardioTrack.Validators.LabRequest
{
    public class UpdateLabRequestStatusValidator : AbstractValidator<UpdateLabRequestStatusRequestDto>
    {
        public UpdateLabRequestStatusValidator()
        {
            RuleFor(x => x.LabRequestId).GreaterThan(0);
            RuleFor(x => x.NewStatus).IsInEnum();
        }
    }
}
