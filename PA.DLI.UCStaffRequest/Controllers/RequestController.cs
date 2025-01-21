using Newtonsoft.Json;
using PA.DLI.UCStaffRequest.Common.Util;
using PA.DLI.UCStaffRequest.DataAccess.DataAccess;
using PA.DLI.UCStaffRequest.DataAccess.DataObjects;
using PA.DLI.UCStaffRequest.Helper;
using PA.DLI.UCStaffRequest.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;

namespace PA.DLI.UCStaffRequest.Controllers
{
    public class RequestController : Controller
    {
        private readonly InquiryDataAccess _dataAccess;

        public RequestController()
        {
            _dataAccess = new InquiryDataAccess();
        }
        // GET: Request
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Confirmation()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Submit(RequestViewModel model)
        {
            try
            {
                var errors = new List<string>();
                if (model.Category != 0 && !string.IsNullOrEmpty(model.FromEmail) && !string.IsNullOrEmpty(model.Subject) && !string.IsNullOrEmpty(model.Message))
                {
                    if (!string.IsNullOrEmpty(model.FromEmail))
                    {
                        if (model.FromEmail.EndsWith("@pa.gov", StringComparison.CurrentCultureIgnoreCase))
                        {
                            errors.Add("From Email must be in the format of @pa.gov");
                        }

                    }
                    if (!string.IsNullOrEmpty(model.CC))
                    {
                        var invalidEmails = model.CC
                      .Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries) // Split by semicolon
                      .Select(email => email.Trim()) // Trim whitespace from each email
                      .Where(email => !email.EndsWith("@pa.Gov", StringComparison.CurrentCultureIgnoreCase)) // Filter invalid emails
                      .ToList();
                        if (invalidEmails.Any())
                        {
                            errors.Add("CC Email must be in the format of @pa.gov");
                        }

                    }

                    if (errors.Any())
                    {
                        return Json(new { success = false, message = errors.Distinct() });
                    }
                    else
                    {
                        EmailHelper.SendEmail(
                  fromEmail: model.FromEmail,
                  toEmail: model.CC,
                  cc: model.CC,
                  subject: model.Subject,
                  body: model.Message,
                  attachments: model.files
              );
                        model.LastChangeUser = "Chetana";
                        _dataAccess.AddInquiry(MapToInquiry(model));
                        return Json(new { success = true, message = "SuccessFully Created" });
                    }
                }
                else
                {

                    if (model.Category == 0)
                    {
                        errors.Add("Category is required.");
                    }
                    if (string.IsNullOrEmpty(model.Subject))
                    {
                        errors.Add("Subject is required.");
                    }
                    if (string.IsNullOrEmpty(model.Message))
                    {
                        errors.Add("Message is required.");
                    }

                    return Json(new { success = false, message = errors });
                }
            }
            catch (Exception ex)
            {
                ErrorLogging.LogWritter("Request", "Submit", ex.Message, "Details: " + JsonConvert.SerializeObject(model));
                return Json(new { success = false, message = ex.Message });
            }
        }

        private Inquiry MapToInquiry(RequestViewModel viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException(nameof(viewModel), "The RequestViewModel cannot be null.");
            }

            return new Inquiry
            {
                Category = viewModel.Category,
                FromEmail = viewModel.FromEmail,
                CC = viewModel.CC,
                Subject = viewModel.Subject,
                Message = viewModel.Message,
                LastChangeDate = DateTime.Now, // Set to the current time or customize as needed
                LastChangeUser = viewModel.LastChangeUser,
                files = viewModel.files
            };
        }
    }

}