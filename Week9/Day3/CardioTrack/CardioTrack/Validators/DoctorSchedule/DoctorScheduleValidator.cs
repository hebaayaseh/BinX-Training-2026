using CardioTrack.DTOs.DoctorSchedule;
using FluentValidation;

namespace CardioTrack.Validators.DoctorSchedule
{
    public class DoctorScheduleValidator : AbstractValidator<AddDoctorSheduleRequestDto>
    {
        public DoctorScheduleValidator()
        {

            RuleFor(x => x.DayOfWeek)
                .IsInEnum()
                .WithMessage("Invalid day.");

            RuleFor(x => x.StartTime)
                .NotEmpty()
                .WithMessage("Start time is required.");

            RuleFor(x => x.EndTime)
                .NotEmpty()
                .WithMessage("End time is required.");

            RuleFor(x => x)
                .Must(x => x.EndTime > x.StartTime)
                .WithMessage("End time must be greater than start time.");
        }
    }
}