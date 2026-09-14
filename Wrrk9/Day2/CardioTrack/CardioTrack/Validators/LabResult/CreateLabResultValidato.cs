using CardioTrack.DTOs.LabResult;
using FluentValidation;

namespace CardioTrack.Validators.LabResult
{
    public class CreateLabResultValidator : AbstractValidator<CreateLabResultDto>
    {
        public CreateLabResultValidator()
        {
            RuleFor(x => x.PatientId).GreaterThan(0);
            RuleFor(x => x.ResultFile).NotEmpty().WithMessage("Result file is required");
            RuleFor(x => x.LabRequestId).GreaterThan(0).When(x => x.LabRequestId.HasValue);
        }
    }
}