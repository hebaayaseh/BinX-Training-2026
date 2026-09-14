using CardioTrack.DTOs.Admin;
using FluentValidation;

namespace CardioTrack.Validators.Sttaf
{
    public class AddDoctorValidator : AbstractValidator<AddDoctorRequestDto>
    {
        public AddDoctorValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.PhoneNumber).NotEmpty();
        }
    }
}
