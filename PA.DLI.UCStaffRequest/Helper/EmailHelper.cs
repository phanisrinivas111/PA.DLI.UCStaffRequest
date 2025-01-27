using PA.DLI.UCStaffRequest.Common.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Web;
namespace PA.DLI.UCStaffRequest.Helper
{


    public static class EmailHelper
    {
        //public static bool SendEmail(string fromEmail, string toEmail, string cc, string subject, string body, List<HttpPostedFileBase> attachments)
        //{
        //    try
        //    {
        //        // Configure the SMTP client
        //        using (SmtpClient smtpClient = new SmtpClient("smtp.your-email-provider.com"))
        //        {
        //            smtpClient.Port = 587; // Use appropriate port for your provider
        //            smtpClient.Credentials = new NetworkCredential("your-email@example.com", "your-email-password");
        //            smtpClient.EnableSsl = true;

        //            // Create the email message
        //            using (MailMessage mailMessage = new MailMessage())
        //            {
        //                mailMessage.From = new MailAddress(fromEmail);
        //                mailMessage.To.Add(toEmail);

        //                if (!string.IsNullOrWhiteSpace(cc))
        //                    mailMessage.CC.Add(cc);

        //                mailMessage.Subject = subject;
        //                mailMessage.Body = body;
        //                mailMessage.IsBodyHtml = true;

        //                // Attach files
        //                if (attachments != null && attachments.Count > 0)
        //                {
        //                    foreach (var file in attachments)
        //                    {
        //                        if (file != null)
        //                        {
        //                            string fileName = System.IO.Path.GetFileName(file.FileName);
        //                            mailMessage.Attachments.Add(new Attachment(file.InputStream, fileName));
        //                        }
        //                    }
        //                }

        //                // Send the email
        //                smtpClient.Send(mailMessage);
        //            }
        //        }

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log or handle the exception as needed
        //        Console.WriteLine($"Error sending email: {ex.Message}");
        //        return false;
        //    }
        //}

        public static bool SendEmail(string fromEmail, string toEmail, string cc, string subject, string body, List<HttpPostedFileBase> attachments)
        {
            try
            {
                bool EnableEmailGeneration = false;
                string enableEmailGenerationConfig = ConfigurationManager.AppSettings["EnableEmailGeneration"];
                if (!string.IsNullOrEmpty(enableEmailGenerationConfig))
                {
                    EnableEmailGeneration = Convert.ToBoolean(enableEmailGenerationConfig);
                }
                if (EnableEmailGeneration)
                {
                    string smtpServer = ConfigurationManager.AppSettings["UCBSREmailServer"];
                    string configuredFromEmail = ConfigurationManager.AppSettings["UCBSREmailFrom"];

                    // Use the configured FromEmail if none is provided
                    fromEmail = string.IsNullOrWhiteSpace(fromEmail) ? configuredFromEmail : fromEmail;

                    // Configure the SMTP client
                    using (SmtpClient smtpClient = new SmtpClient(smtpServer))
                    {
                        smtpClient.Port = 25; // Default port for SMTP (adjust if necessary)
                        smtpClient.UseDefaultCredentials = true; // Use system credentials if required
                        smtpClient.EnableSsl = false; // Set to true if your server supports SSL (adjust as needed)

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
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }

}