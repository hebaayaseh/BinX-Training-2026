namespace CardioTrack.DTOs.DoctorSchedule
{
    public class UpdateDoctorScheduleRequestDto
    {
        public int  ScheduleId { get; set; }
        public int DoctorId { get; set; }

        public DayOfWeek DayOfWeek { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }
    }
}
