using CardioTrack.DTOs.Doctor;
using FluentValidation;

namespace CardioTrack.Validators.Appointment
{
    public class AppointmentValidtor : AbstractValidator<AddAppointmentRequestDto>
    {
        public AppointmentValidtor()
        {
            RuleFor(x => x.PatientId).GreaterThan(0);
            RuleFor(x => x.DoctorId).GreaterThan(0);
            RuleFor(x=> x.AppointmentDate).GreaterThan(DateTime.UtcNow).WithMessage("Appointment must be in the future");
            RuleFor(x => x.Reason).NotEmpty().MaximumLength(200);
        }
    }
}
