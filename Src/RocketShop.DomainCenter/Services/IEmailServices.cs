using LanguageExt;
using Microsoft.Extensions.Hosting;
using MimeKit;
using RocketShop.DomainCenter.Model;
using RocketShop.Framework.Extension;
using RocketShop.Framework.Services;
using RocketShop.Shared.Model;

namespace RocketShop.DomainCenter.Services
{
    public interface IEmailServices
    {
        Task<Either<Exception, string>> SendAsync(MailRequest request);
    }
    public class EmailServices(ILogger<EmailServices> logger,IConfiguration configuration) : BaseServices<EmailServices>("Email Service",logger), IEmailServices
    {
        readonly string username = configuration.GetSection("Email:Name").Value ??"",
            password = configuration.GetSection("Email:Password").Value ?? "", 
            host = configuration.GetSection("Email:Host").Value ?? "", 
            signature = configuration.GetSection("Email:Signature").Value ?? "";
        readonly int port = configuration.GetSection("Email:Signature").Value.ToInt(587);

        public async Task<Either< Exception,string>> SendAsync(MailRequest request) =>
            await InvokeServiceAsync(async () =>
            {
                    MimeMessage email = new MimeMessage();
                    email.Sender = MailboxAddress.Parse(username);
                    foreach (var to in request.Receiver.Split(";", StringSplitOptions.RemoveEmptyEntries))
                        email.To.Add(MailboxAddress.Parse(to));

                    if (!string.IsNullOrEmpty(request.CC))
                    {
                        foreach (var cc in request.CC.Split(";", StringSplitOptions.RemoveEmptyEntries))
                            email.Cc.Add(MailboxAddress.Parse(cc));

                    }
                    if (!string.IsNullOrEmpty(request.BCC))
                    {
                        foreach (var bcc in request.BCC.Split(";", StringSplitOptions.RemoveEmptyEntries))
                            email.Bcc.Add(MailboxAddress.Parse(bcc));
                    }

                    var builder = new BodyBuilder();
                    if (request.Attachments != null)
                    {
                        foreach (var attachment in request.Attachments)
                        {
                            builder.Attachments.Add(attachment.FileName, attachment.Content, MimeKit.ContentType.Parse(attachment.ContentType));
                        }
                    }
                    builder.HtmlBody = request.Body; 
                    email.Subject = request.Subject;
                    email.Body = builder.ToMessageBody();
                    var smtp = new MailKit.Net.Smtp.SmtpClient();
                    await smtp.ConnectAsync(host, port);
                    await smtp.AuthenticateAsync(username, password);
                    await smtp.SendAsync(email);
                    await smtp.DisconnectAsync(true);
                    return "Mail has been sent successfully";
               
            },
                retries:3,
                intervalSecond:5);
    }
}
