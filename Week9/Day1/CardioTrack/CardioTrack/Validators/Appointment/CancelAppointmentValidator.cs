using CardioTrack.DTOs.Doctor;
using FluentValidation;

namespace CardioTrack.Validators.Appointment
{
    public class CancelAppointmentValidator : AbstractValidator<CancelAppointmentRequestDto>
    {
        public CancelAppointmentValidator()
        {
            RuleFor(x => x.DoctorId).GreaterThan(0);
            RuleFor(x => x.AppointmentId).GreaterThan(0);
        }
    }
}
