using CardioTrack.Interfaces.IEmail;
using System.Net;
using System.Net.Mail;

namespace CardioTrack.Services.Email
{
    public class EmailService : IEmail
    {
        private readonly IConfiguration _config;
        private readonly Microsoft.Extensions.Logging.ILogger<EmailService> _logger;

        public EmailService(IConfiguration config, Microsoft.Extensions.Logging.ILogger<EmailService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task SendAsync(string toEmail, string subject, string body)
        {
            using var client = new SmtpClient(
                _config["Email:SmtpHost"],
                int.Parse(_config["Email:SmtpPort"]!))
            {
                Credentials = new NetworkCredential(
                    _config["Email:Username"],
                    _config["Email:Password"]),
                EnableSsl = true
            };
            var message = new MailMessage
            {
                From = new MailAddress(_config["Email:From"]!, _config["Email:DisplayName"]),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            message.To.Add(toEmail);

            await client.SendMailAsync(message);
        
        }

        public async Task SendOtpAsync(string email, string code, string purpose)
        {
            var subject = purpose switch
            {
                "change-email" => "تأكيد تغيير الإيميل - CardioTrack",
                "change-password" => "تأكيد تغيير كلمة السر - CardioTrack",
                _ => "رمز التحقق - CardioTrack"
            };

            var body = $@"
                <div dir='rtl' style='font-family:Arial;padding:20px'>
                    <h2>رمز التحقق الخاص بك</h2>
                    <p style='font-size:32px;font-weight:bold;letter-spacing:8px'>{code}</p>
                </div>";

            await SendAsync(email, subject, body);
        }

        public async Task SendTempPasswordAsync(string email, string name, string password)
        {
            var body = $@"
                <div dir='rtl' style='font-family:Arial;padding:20px'>
                    <h2>مرحباً {name}</h2>
                    <p>تم إنشاء حسابك بمركز <strong>CadioTrack</strong></p>
                    <p>كلمة السر المؤقتة الخاصة بك:</p>
                    <p style='font-size:24px;font-weight:bold;background:#f0f0f0;padding:10px;border-radius:8px'>{password}</p>
                    <p style='color:red'>يرجى تغيير كلمة السر فور تسجيل الدخول.</p>
                </div>";

            await SendAsync(email, "بيانات حسابك - CardioTrack", body);
        }
    }
}
