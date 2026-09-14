using CardioTrack.DTOs.Doctor;
using FluentValidation;

namespace CardioTrack.Validators.Appointment
{
    public class GetAppointmentsRequestValidator : AbstractValidator<GetAppointmentsRequestDto>
    {
        public GetAppointmentsRequestValidator()
        {
            RuleFor(x => x.DoctorId).GreaterThan(0);
            RuleFor(x => x.AppointmentStatus).IsInEnum();
        }
    }
}
