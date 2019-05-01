using log4net.Appender;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MyWinService
{
    public static class Helper
    {
        public static StringBuilder GetAllLogs(MemoryAppender mappender)
        {
            StringBuilder message = new StringBuilder();
            foreach (var logEvent in mappender.GetEvents().ToList())
            {
                message.AppendLine(logEvent.RenderedMessage);
            }

            return message;
        }

        public static bool ServiceEndSuccessfully(MemoryAppender mappender)
        {
            var failedLevels = new List<Level> { Level.Fatal, Level.Error };
            var hasFailLevels = mappender.GetEvents().Any(x => failedLevels.Contains(x.Level));

            return !hasFailLevels;
        }

        public static void SendEventSummuryEmail(MemoryAppender mappender)
        {
            string to = "configTo", from = "configFrom", server = "configServer", subject;
            StringBuilder body = new StringBuilder();

            if (ServiceEndSuccessfully(mappender))
            {
                subject = "Serivce works successfully";
            }
            else
            {
                subject = "Serivce has errors";
            }

            body.AppendLine(subject);
            body.AppendLine("\n\n+Details:\n");
            string allLogs = GetAllLogs(mappender).ToString();
            body.AppendLine(allLogs);

            SendSMTPMail(to, from, server, subject, body.ToString());
        }

        public static bool SendSMTPMail(string to, string from, string server, string subject, string body)
        {

            MailMessage message = new MailMessage(from, to);
            message.Subject = subject;
            message.Body = body;
            SmtpClient client = new SmtpClient(server);
            // Credentials are necessary if the server requires the client 
            // to authenticate before it will send email on the client's behalf.
            client.UseDefaultCredentials = true;

            try
            {
                client.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}