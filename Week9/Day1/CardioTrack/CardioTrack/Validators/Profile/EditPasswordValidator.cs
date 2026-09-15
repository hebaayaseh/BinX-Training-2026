using CardioTrack.DTOs.EditProfile;
using FluentValidation;

namespace CardioTrack.Validators.Profile
{
    public class EditPasswordValidator : AbstractValidator<EditPasswordRequestDto>
    {
        public EditPasswordValidator()
        {
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8).Matches("[A-Z]").Matches("[0-9]");
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Passwords do not match");
        }
    }
}
