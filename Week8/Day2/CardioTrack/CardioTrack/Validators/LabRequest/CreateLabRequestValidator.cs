using CardioTrack.DTOs.LabRequest;
using FluentValidation;

namespace CardioTrack.Validators.LabRequest
{
    public class CreateLabRequestValidator : AbstractValidator<CreateLabRequestDto>
    {
        public CreateLabRequestValidator()
        {
            RuleFor(x => x.PatientId).GreaterThan(0);
            RuleFor(x => x.TestNames).NotEmpty().WithMessage("Must add at least one test");
            RuleForEach(x => x.TestNames).NotEmpty().MaximumLength(100);
        }
    }
}
