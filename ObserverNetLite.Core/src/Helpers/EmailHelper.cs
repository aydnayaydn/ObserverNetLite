using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

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
            Console.WriteLine($"[EMAIL DEBUG] Attempting to send email to: {toEmail}");
            Console.WriteLine($"[EMAIL DEBUG] SMTP Server: {_smtpHost}:{_smtpPort}");
            Console.WriteLine($"[EMAIL DEBUG] SSL Enabled: {_enableSsl}");
            Console.WriteLine($"[EMAIL DEBUG] Username: {_smtpUsername}");
            Console.WriteLine($"[EMAIL DEBUG] From: {_fromEmail}");

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_fromName, _fromEmail));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder();
            if (isHtml)
            {
                bodyBuilder.HtmlBody = body;
            }
            else
            {
                bodyBuilder.TextBody = body;
            }
            message.Body = bodyBuilder.ToMessageBody();

            using var smtpClient = new SmtpClient();
            
            Console.WriteLine($"[EMAIL DEBUG] Connecting to SMTP server...");
            
            // Gmail için doğru SSL ayarları
            SecureSocketOptions secureOptions;
            if (_smtpPort == 465)
            {
                // Port 465: SSL/TLS (implicit)
                secureOptions = SecureSocketOptions.SslOnConnect;
            }
            else if (_smtpPort == 587)
            {
                // Port 587: STARTTLS (explicit)
                secureOptions = SecureSocketOptions.StartTls;
            }
            else
            {
                // Diğer portlar için SSL ayarına göre
                secureOptions = _enableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None;
            }
            
            Console.WriteLine($"[EMAIL DEBUG] Using secure option: {secureOptions}");
            await smtpClient.ConnectAsync(_smtpHost, _smtpPort, secureOptions);
            
            Console.WriteLine($"[EMAIL DEBUG] Authenticating...");
            await smtpClient.AuthenticateAsync(_smtpUsername, _smtpPassword);
            
            Console.WriteLine($"[EMAIL DEBUG] Sending email...");
            await smtpClient.SendAsync(message);
            
            Console.WriteLine($"[EMAIL DEBUG] Disconnecting...");
            await smtpClient.DisconnectAsync(true);
            
            Console.WriteLine($"[EMAIL DEBUG] Email sent successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[EMAIL ERROR] Exception:");
            Console.WriteLine($"[EMAIL ERROR] Type: {ex.GetType().Name}");
            Console.WriteLine($"[EMAIL ERROR] Message: {ex.Message}");
            Console.WriteLine($"[EMAIL ERROR] InnerException: {ex.InnerException?.Message}");
            Console.WriteLine($"[EMAIL ERROR] StackTrace: {ex.StackTrace}");
            
            throw new InvalidOperationException(
                $"Email gönderme hatası: {ex.Message} | Server: {_smtpHost}:{_smtpPort} | SSL: {_enableSsl}", 
                ex);
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
