namespace CardioTrack.ExceptionService
{
    public class Exceptions : Exception
    {
        public Exceptions(string key) : base(key) { }
    }
}
