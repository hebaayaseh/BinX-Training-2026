using CardioTrack.DTOs.EditProfile;
using FluentValidation;

namespace CardioTrack.Validators.Profile
{
    public class EditEmailValidtor : AbstractValidator<EditEmailRequestDto>
    {
        public EditEmailValidtor()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }
}
