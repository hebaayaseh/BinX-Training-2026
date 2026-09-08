using CardioTrack.DTOs.LogIn;
using FluentValidation;

namespace CardioTrack.Validators.Auth
{
    public class LoginValidator : AbstractValidator<LoginRequestDto>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
