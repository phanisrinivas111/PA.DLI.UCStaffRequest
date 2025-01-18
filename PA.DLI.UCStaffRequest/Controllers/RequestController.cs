using PA.DLI.UCStaffRequest.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PA.DLI.UCStaffRequest.Controllers
{
    public class RequestController : Controller
    {
        // GET: Request
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Submit(RequestFormViewModel model)
        {
            try
            {
                var errors = new List<string>();
                if (model.Category!=0 && !string.IsNullOrEmpty(model.FromEmail) && !string.IsNullOrEmpty(model.Subject) && !string.IsNullOrEmpty(model.Message))
                {
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
                }
                else
                {

                    if (model.Category==0)
                    {
                        errors.Add("Please Select Category");
                    }
                    if (string.IsNullOrEmpty(model.FromEmail))
                    {
                        errors.Add("Please enter From Email");
                    }
                    if (string.IsNullOrEmpty(model.Subject))
                    {
                        errors.Add("Please enter Subject");
                    }
                    if (string.IsNullOrEmpty(model.Message))
                    {
                        errors.Add("Please enter Message");
                    }

                    return Json(new { success = false, message = errors });
                }
                return Json(new { success = false, message = "Validation failed." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

    }
}