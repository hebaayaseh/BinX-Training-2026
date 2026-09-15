using CardioTrack.DTOs.Admin;
using FluentValidation;

namespace CardioTrack.Validators.Sttaf
{
    public class AddNurseValidator : AbstractValidator<AddNurseRequestDto>
    {
        public AddNurseValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.PhoneNumber).NotEmpty();
        }
    }
}
