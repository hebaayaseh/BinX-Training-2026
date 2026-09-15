namespace CardioTrack.Models
{
    public class Doctorschedule
    {
        public int Id { get; set; }

        public int DoctorId { get; set; }

        public DayOfWeek DayOfWeek { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }
        public bool IsActive { get; set; } = true;

        //  Navigation Properties :
        public User? Doctor { get; set; } = null!;
    }
}
