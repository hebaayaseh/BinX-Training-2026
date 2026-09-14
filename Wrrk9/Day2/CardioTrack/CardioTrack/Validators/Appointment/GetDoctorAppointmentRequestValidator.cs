using CardioTrack.DTOs.Doctor;
using FluentValidation;

namespace CardioTrack.Validators.Appointment
{
    public class GetDoctorAppointmentRequestValidator : AbstractValidator<GetDoctorAppointmentRequestDto>
    {
        public GetDoctorAppointmentRequestValidator()
        {
            RuleFor(x => x.AppointmentStatus).IsInEnum();
        }
    }
}
