using CardioTrack.DTOs.Doctor;
using FluentValidation;

namespace CardioTrack.Validators.Appointment
{
    public class CompleteAppointmentValidator : AbstractValidator<CompleteAppointmentRequestDto>
    {
        public CompleteAppointmentValidator()
        {
            RuleFor(x => x.AppointmentId).GreaterThan(0);
            RuleFor(x => x.DoctorId).GreaterThan(0);
        }
    }
}
