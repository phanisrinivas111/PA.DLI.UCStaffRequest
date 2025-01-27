using PA.DLI.UCStaffRequest.DataAccess.DataAccess;
using PA.DLI.UCStaffRequest.DataAccess.DataObjects;
using PA.DLI.UCStaffRequest.Helper;
using PA.DLI.UCStaffRequest.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PA.DLI.UCStaffRequest.Common.Util;
using Newtonsoft.Json;

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
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                ErrorLogging.LogWritter("Request", "Index", ex.Message, "Error in the request view");
                return View("Error"); // Redirect to a generic error view.
            }
        }
        [HttpGet]
        public ActionResult Confirmation()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                ErrorLogging.LogWritter("Request", "Confirmation", ex.Message, "Error in the confirmation view");
                return View("Error"); // Redirect to a generic error view.
            }
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
                        if (!model.FromEmail.EndsWith("@pa.gov", StringComparison.CurrentCultureIgnoreCase))
                        {
                            errors.Add("From Email must be in the format of @pa.gov");
                        }

                    }
                    if (!string.IsNullOrEmpty(model.CC))
                    {

                        var invalidEmails = model.CC
                         .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries) // Split by semicolon
                         .Select(email => email.Trim()) // Trim whitespace from each email
                         .Where(email =>
                             !email.EndsWith("@pa.Gov", StringComparison.OrdinalIgnoreCase) || // Check domain
                             !IsValidEmailFormat(email)) // Check email format
                         .ToList();

                        if (invalidEmails.Any())
                        {
                            errors.Add("CC Email must be in the format of @pa.gov and cannot contain special characters or duplicates.");
                        }
                        var duplicateEmails = model.CC
                         .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries) // Split by semicolon
                         .Select(email => email.Trim()) // Trim whitespace from each email
                         .GroupBy(email => email, StringComparer.OrdinalIgnoreCase) // Group by email (case-insensitive)
                         .Where(group => group.Count() > 1) // Filter groups with more than one email
                         .ToDictionary(group => group.Key, group => group.Count()); // Create a dictionary of email and count

                        if (duplicateEmails.Any())
                        {
                            foreach (var duplicate in duplicateEmails)
                            {
                                errors.Add($"CC Email: {duplicate.Key}, Count: {duplicate.Value}");
                            }
                        }

                    }

                    if (errors.Any())
                    {
                        return Json(new { success = false, message = errors.Distinct() });
                    }
                    else
                    {

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
        [HttpPost]
        bool IsValidEmailFormat(string email)
        {
            string str = "Email:" + email;
            // Regular expression to check for special characters in the email
            try
            {
                var specialCharPattern = @"[^a-zA-Z0-9@._-]";
                return !System.Text.RegularExpressions.Regex.IsMatch(email, specialCharPattern);
            }
            catch (Exception ex)
            {
                ErrorLogging.LogWritter("Request", "IsValidEmailFormat", ex.Message, "Details: " + str);
                throw ex;
            }
        }
        [HttpPost]
        private Inquiry MapToInquiry(RequestViewModel viewModel)
        {
            try
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
            catch (Exception ex)
            {
                ErrorLogging.LogWritter("Request", "MapToInquiry", ex.Message, "Details: " + JsonConvert.SerializeObject(viewModel));
                throw ex;
            }
        }
    }

}