namespace CardioTrack.Interfaces.ICache
{
    public interface IPatientCacheInvalidator
    {
        Task InvalidateAsync(int doctorId);
    }
}
