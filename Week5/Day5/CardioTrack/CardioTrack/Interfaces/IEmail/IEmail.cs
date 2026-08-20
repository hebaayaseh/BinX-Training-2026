namespace CardioTrack.Interfaces.IEmail
{
    public interface IEmail
    {
        Task SendTempPasswordAsync(string email, string name ,string password);
        Task SendOtpAsync(string email, string code, string purpose);
    }
}
