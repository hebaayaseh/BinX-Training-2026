using CardioTrack.DTOs.Doctor;
using FluentValidation;

namespace CardioTrack.Validators.Patient
{
    public class GetPatienValidtor : AbstractValidator<GetPatientRequestDto>
    {
        public GetPatienValidtor()
        {
            RuleFor(x => x.PatientId).GreaterThan(0);
        }
    }
}
