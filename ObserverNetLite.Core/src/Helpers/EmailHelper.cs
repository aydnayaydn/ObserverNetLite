using System.Net;
using System.Net.Mail;

namespace ObserverNetLite.Core.Helpers;

public class EmailHelper
{
    private readonly string _smtpHost;
    private readonly int _smtpPort;
    private readonly string _smtpUsername;
    private readonly string _smtpPassword;
    private readonly string _fromEmail;
    private readonly string _fromName;
    private readonly bool _enableSsl;

    public EmailHelper(
        string smtpHost,
        int smtpPort,
        string smtpUsername,
        string smtpPassword,
        string fromEmail,
        string fromName,
        bool enableSsl = true)
    {
        _smtpHost = smtpHost;
        _smtpPort = smtpPort;
        _smtpUsername = smtpUsername;
        _smtpPassword = smtpPassword;
        _fromEmail = fromEmail;
        _fromName = fromName;
        _enableSsl = enableSsl;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body, bool isHtml = true)
    {
        try
        {
            using var smtpClient = new SmtpClient(_smtpHost, _smtpPort)
            {
                EnableSsl = _enableSsl,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword)
            };

            using var mailMessage = new MailMessage
            {
                From = new MailAddress(_fromEmail, _fromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = isHtml
            };

            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }
        catch (Exception ex)
        {
            // Log the exception (you can inject ILogger if needed)
            throw new InvalidOperationException($"Email gönderme hatası: {ex.Message}", ex);
        }
    }

    public async Task SendPasswordResetEmailAsync(string toEmail, string userName, string resetToken, string resetUrl)
    {
        var subject = "Şifre Sıfırlama Talebi - ObserverNetLite";
        
        var body = $@"
            <html>
            <body style='font-family: Arial, sans-serif;'>
                <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
                    <h2 style='color: #333;'>Şifre Sıfırlama Talebi</h2>
                    <p>Merhaba <strong>{userName}</strong>,</p>
                    <p>Hesabınız için şifre sıfırlama talebinde bulundunuz. Şifrenizi sıfırlamak için aşağıdaki linke tıklayın:</p>
                    <p style='margin: 30px 0;'>
                        <a href='{resetUrl}?token={resetToken}' 
                           style='background-color: #4CAF50; color: white; padding: 12px 24px; 
                                  text-decoration: none; border-radius: 4px; display: inline-block;'>
                            Şifremi Sıfırla
                        </a>
                    </p>
                    <p style='color: #666; font-size: 14px;'>
                        Bu link 1 saat geçerlidir. Eğer şifre sıfırlama talebinde bulunmadıysanız, bu e-postayı görmezden gelebilirsiniz.
                    </p>
                    <p style='color: #666; font-size: 12px; margin-top: 40px; border-top: 1px solid #ddd; padding-top: 20px;'>
                        ObserverNetLite - Observer Network Monitoring
                    </p>
                </div>
            </body>
            </html>
        ";

        await SendEmailAsync(toEmail, subject, body, true);
    }
}
