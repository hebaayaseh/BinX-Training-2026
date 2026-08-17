using CardioTrack.DTOs.EditProfile;
using FluentValidation;

namespace CardioTrack.Validators.Profile
{
    public class EditProfileValidator : AbstractValidator<EditProfileRequestDto>
    {
        public EditProfileValidator()
        {
            RuleFor(x => x.FullName).Length(3, 100).When(x => x.FullName != null);
            RuleFor(x => x.PhoneNumber).Matches(@"^05\d{8}$").When(x => x.PhoneNumber != null);

        }
    }
}
