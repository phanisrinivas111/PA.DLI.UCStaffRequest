using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Web;
namespace PA.DLI.UCStaffRequest.Helper
{


    public static class EmailHelper
    {
        public static bool SendEmail(string fromEmail, string toEmail, string cc, string subject, string body, List<HttpPostedFileBase> attachments)
        {
            try
            {
                // Configure the SMTP client
                using (SmtpClient smtpClient = new SmtpClient("smtp.your-email-provider.com"))
                {
                    smtpClient.Port = 587; // Use appropriate port for your provider
                    smtpClient.Credentials = new NetworkCredential("your-email@example.com", "your-email-password");
                    smtpClient.EnableSsl = true;

                    // Create the email message
                    using (MailMessage mailMessage = new MailMessage())
                    {
                        mailMessage.From = new MailAddress(fromEmail);
                        mailMessage.To.Add(toEmail);

                        if (!string.IsNullOrWhiteSpace(cc))
                            mailMessage.CC.Add(cc);

                        mailMessage.Subject = subject;
                        mailMessage.Body = body;
                        mailMessage.IsBodyHtml = true;

                        // Attach files
                        if (attachments != null && attachments.Count > 0)
                        {
                            foreach (var file in attachments)
                            {
                                if (file != null)
                                {
                                    string fileName = System.IO.Path.GetFileName(file.FileName);
                                    mailMessage.Attachments.Add(new Attachment(file.InputStream, fileName));
                                }
                            }
                        }

                        // Send the email
                        smtpClient.Send(mailMessage);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false;
            }
        }
    }

}