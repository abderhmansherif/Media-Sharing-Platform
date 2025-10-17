using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace BeatBox.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;
        private readonly string _apiKey;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            _apiKey = _configuration["SendGridConfig:ApiKey"];
        }

        public async Task<bool> SendEmail(string to, string subject, string emailBodyHtml, string emailBodyPlainText)
        {
            var client = new SendGridClient(_apiKey);

            var fromAddress = new EmailAddress("bebosherif202@gmail.com", "BeatBox"); // bebosherif202@gmail.com // bebosherif202@gmail.com

            var toAddress = new EmailAddress(to, "Username");

            var msg = MailHelper.CreateSingleEmail(
                fromAddress,
                toAddress,
                subject,
                emailBodyPlainText,  // plain text first
                emailBodyHtml        // then HTML
            );

            var response = await client.SendEmailAsync(msg);

            return response.IsSuccessStatusCode;
        }
    }
}
