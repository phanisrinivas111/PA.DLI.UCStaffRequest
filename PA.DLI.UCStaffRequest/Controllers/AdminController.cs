using PA.DLI.UCStaffRequest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PA.DLI.UCStaffRequest.DataAccess.DataAccess;
using PA.DLI.UCStaffRequest.Common.Util;

namespace PA.DLI.UCStaffRequest.Controllers
{
    public class AdminController : Controller
    {
        private readonly AdminDataAccess _dataAccess;
        
        private IEnumerable<PA.DLI.UCStaffRequest.Models.User> MapModelUsers(IEnumerable<PA.DLI.UCStaffRequest.DataAccess.DataObjects.User> dataUserObject)
        {
            return dataUserObject.Select(u => new Models.User
            {
                UserId = u.UserId,
                RoleCode = u.RoleCode,
                LastChangeDate = u.LastChangeDate,
                LastChangeUser = u.LastChangeUser,
                IsDeleted = u.IsDeleted
            }).ToList();
        }
        private IEnumerable<PA.DLI.UCStaffRequest.Models.Category> MapModeCategory(IEnumerable<PA.DLI.UCStaffRequest.DataAccess.DataObjects.Category> dataCategoryObject)
        {
            return dataCategoryObject.Select(u => new Models.Category
            {
                CategoryName = u.CategoryName,
                Email = u.Email,
                LastChangedDate = u.LastChangedDate,
                LastChangedUser = u.LastChangedUser,
                IsDeleted = u.IsDeleted
            }).ToList();
        }
        private IEnumerable<PA.DLI.UCStaffRequest.Models.Role> MapModeRoles(IEnumerable<PA.DLI.UCStaffRequest.DataAccess.DataObjects.Role> dataRoleObject)
        {
            return dataRoleObject.Select(u => new Models.Role
            {
                CDE_ROLE = u.CDE_ROLE,
                TXT_ROLE = u.TXT_ROLE,
            }).ToList();
        }
        public AdminController()
        {
            _dataAccess = new AdminDataAccess();
        }
        [HttpGet]
        public ActionResult Categories()
        {
            var categories = _dataAccess.GetCategories();
            var modelCategory = MapModeCategory(categories);
            return View(modelCategory);
        }

        // GET: Admin
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Title = "Home";
            return View();
        }
        [HttpPost]
        public JsonResult AddCategory(string CategoryName, string Email)
        {
            string str = "CategoryName: " + CategoryName+";"+ "Email:"+ Email;
            try
            {
                var errors = new List<string>();
                var existingCategory = _dataAccess.GetCategories().FirstOrDefault(c => c.CategoryName.Equals(CategoryName, StringComparison.CurrentCultureIgnoreCase));

                

                if (!string.IsNullOrEmpty(CategoryName) && !string.IsNullOrEmpty(Email))
                {
                    if (existingCategory != null)
                    {
                        errors.Add("Category already exist");
                    }
                    // Validate each email and collect errors in one go
                    var invalidEmails = Email
                        .Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries) // Split by semicolon
                        .Select(email => email.Trim()) // Trim whitespace from each email
                        .Where(email => !email.EndsWith("@pa.Gov", StringComparison.CurrentCultureIgnoreCase)) // Filter invalid emails
                        .ToList();
                    if (invalidEmails.Any())
                    {
                        errors.Add("Email must be in the format of @pa.gov");
                    }

                    if (errors.Any())
                    {
                        return Json(new { success = false, message = errors.Distinct() });
                    }
                    else
                    {
                        var newCategory = new PA.DLI.UCStaffRequest.DataAccess.DataObjects.Category
                        {
                            CategoryName = CategoryName,
                            Email = Email,
                            LastChangedDate = DateTime.Now,
                            LastChangedUser = "Chetana",
                            IsDeleted = "N"
                        };
                        _dataAccess.AddCategory(newCategory);
                        return Json(new { success = true, CategoryName = newCategory.CategoryName, categoryEmail = newCategory.Email });
                    }
                }
                else
                {

                    if (string.IsNullOrEmpty(CategoryName))
                    {
                        errors.Add("Please enter Category Name");
                    }
                    if (string.IsNullOrEmpty(Email))
                    {
                        errors.Add("Please enter Email");
                    }

                    return Json(new { success = false, message = errors });

                }

            }
            catch (Exception EX)
            {
                //
                ErrorLogging.LogWritter("Admin", "AddCategory", EX.Message, "Details: " + str);
                return Json(new { success = false, message = EX.Message });
            }
        }

        [HttpGet]
        public ActionResult UserManagement(string search = "", int page = 1, int pageSize = 10)
        {
            string str = "page " + page;
            try
            {

                var user = _dataAccess.GetUsers();

            if (!string.IsNullOrEmpty(search))
            {
                user = user.Where(u => !string.IsNullOrEmpty(u.UserId) && u.UserId.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            }
               
               var modelUser = MapModelUsers(user);
               


            int totalUser = modelUser.Count();
            var pagedUser = modelUser
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalUser / pageSize);
            ViewBag.SearchQuery = search;

                return View(pagedUser);
            }
            catch (Exception EX)
            {
                //
                ErrorLogging.LogWritter("Admin", "UserManagement", EX.Message, "Details: " + str);
                return Json(new { success = false, message = EX.Message });
            }
        }
        [HttpGet]

        public ActionResult GetUsers(string search = "",int page = 1, int pageSize = 10)
        {
            string str = "page " + page;
            try
            {

                        var users = _dataAccess.GetUsers();

                    var modelUser = MapModelUsers(users).OrderBy(u=> u.UserId).ToList();
                    int totalUser = modelUser.Count();
                    var pagedUser = modelUser
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();


                    ViewBag.CurrentPage = page;
                    ViewBag.TotalPages = (int)Math.Ceiling((double)totalUser / pageSize);
                    ViewBag.SearchQuery = search;
                    return PartialView("_UserListPartial", pagedUser);
            }
            catch (Exception EX)
            {
                //
                ErrorLogging.LogWritter("Admin", "GetUsers", EX.Message, "Details: " + str);
                return Json(new { success = false, message = EX.Message });
            }
        }
        [HttpGet]
        public ActionResult GetCategories()
        {
            try
            {
                var categories = _dataAccess.GetCategories();
                var modelCategory = MapModeCategory(categories);
                return PartialView("_CategoriesListPartial", modelCategory);
            }
            catch (Exception EX)
            {
                //
                ErrorLogging.LogWritter("Admin", "GetCategories", EX.Message, "Error while getting the categories");
                return Json(new { success = false, message = EX.Message });
            }
        }
        [HttpGet]
        public JsonResult GetCategoriesList()
        {
            try
            {
                var categories = _dataAccess.GetCategories();
                var list = categories.Select(r => new { value = r.CategoryId, text = r.CategoryName }).ToList();

                Console.WriteLine($"categories fetched: {categories.Count}");

                return Json(new { success = true, data = list }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception EX)
            {
                ErrorLogging.LogWritter("Admin", "GetCategoriesList", EX.Message, "Error while getting the Categories");
                return Json(new { success = false, message = EX.Message });
            }
        }
        [HttpGet]
        public JsonResult GetRoles()
        {
            try
            {
                var role = _dataAccess.GetAllRoles();
                var roles = role.Select(r => new { value = r.CDE_ROLE, text = r.TXT_ROLE }).ToList();

                Console.WriteLine($"roles fetched: {roles.Count}");

                return Json(new { success = true, data = roles }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception EX)
            {
                ErrorLogging.LogWritter("Admin", "GetRoles", EX.Message, "Error while getting the roles");
                return Json(new { success = false, message = EX.Message });
            }
        }
        [HttpPost]

        public JsonResult AddUser(string CWOPAID, string Role)
        {
            string str = "CWOPAID: " + CWOPAID + ";" + "Role:" + Role;
            try
            {
                var errors = new List<string>();
                if (!string.IsNullOrEmpty(CWOPAID) && !string.IsNullOrEmpty(Role))
                {
                    var existingUser = _dataAccess.GetUsers().FirstOrDefault(u => u.UserId == CWOPAID );
                    var deleteUser = _dataAccess.GetAllUsers().FirstOrDefault(u => u.UserId == CWOPAID && u.IsDeleted == "Y");
                    if (existingUser != null)
                    {
                        errors.Add($"User {CWOPAID} already exists.");
                    }

                    if (errors.Any())
                    {
                        return Json(new { success = false, message = errors });
                    }
                    else if(deleteUser != null){
                        var user = new PA.DLI.UCStaffRequest.DataAccess.DataObjects.User
                        {
                            UserId = CWOPAID,
                            RoleCode = Role,
                            LastChangeDate = DateTime.Now,
                            LastChangeUser = "c-cnayudu",
                            IsDeleted = "N"
                        };
                        _dataAccess.EditUser(user);
                        var totalUsers = _dataAccess.GetUsers().Count();
                        int totalPages = (int)Math.Ceiling((double)totalUsers / 10);

                        return Json(new { success = true, pages = totalPages });
                    }
                    else
                    {
                        var user = new PA.DLI.UCStaffRequest.DataAccess.DataObjects.User
                        {
                            UserId = CWOPAID,
                            RoleCode = Role,
                            LastChangeDate = DateTime.Now,
                            LastChangeUser = "c-cnayudu",
                            IsDeleted = "N"
                        };
                        _dataAccess.AddUser(user);
                        var totalUsers = _dataAccess.GetUsers().Count();
                        int totalPages = (int)Math.Ceiling((double)totalUsers / 10);

                        return Json(new { success = true, pages = totalPages });
                    }
                }
                else
                {

                    if (string.IsNullOrEmpty(CWOPAID))
                    {
                        errors.Add("Please enter User ID");
                    }
                    if (string.IsNullOrEmpty(Role))
                    {
                        errors.Add("Please select the Role");
                    }

                    return Json(new { success = false, message = errors });

                }

            }
            catch (Exception EX)
            {
                ErrorLogging.LogWritter("Admin", "AddUser", EX.Message, "Details: " + str);
                return Json(new { success = false, message = EX.Message });
            }

        }
        [HttpPost]

        public JsonResult DeleteUser(string CWOPAID)
        {
            string str = "CWOPAID:" + CWOPAID;
            try
            {
                _dataAccess.DeleteUser(CWOPAID);

                var totalUsers = _dataAccess.GetUsers().Count();
                int totalPages = (int)Math.Ceiling((double)totalUsers / 10);

                return Json(new { success = true, pages = totalPages });
            }
            catch (Exception EX)
            {
                ErrorLogging.LogWritter("Admin", "DeleteUser", EX.Message, "Details: " + str);
                return Json(new { success = false, message = EX.Message });
            }
        }
        [HttpPost]
        public JsonResult EditCategory(string CategoryName, string Email)
        {
            string str = "CategoryName: " + CategoryName + ";" + "Email:" + Email;
            try
            {
                var errors = new List<string>();
                var category = _dataAccess.GetCategories();
                var newCategory = category.Find(c => c.CategoryName == CategoryName);
                if (newCategory == null)
                {

                    errors.Add("Category not found.");
                }
                else if (Email == null || Email == "")
                {
                    errors.Add("Email cannot be null or empty");
                }
                if (!string.IsNullOrEmpty(Email))
                {
                    // Validate each email and collect errors in one go
                    var invalidEmails = Email
                        .Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries) // Split by semicolon
                        .Select(email => email.Trim()) // Trim whitespace from each email
                        .Where(email => !email.EndsWith("@pa.Gov", StringComparison.CurrentCultureIgnoreCase)) // Filter invalid emails
                        .ToList();
                    if (invalidEmails.Any())
                    {
                        errors.Add("Email must be in the format of @pa.gov");
                    }
                }
               
                if (errors.Any())
                {
                    return Json(new { success = false, message = errors });
                }
                else
                {
                    //update the email
                    newCategory.Email = Email;
                    //newCategory.LastChangedDate = DateTime.Now;
                    newCategory.LastChangedUser = "Chetana";

                    _dataAccess.EditCategory(newCategory);

                    return Json(new { sucess = true });
                }
            }
            catch (Exception EX)
            {
                ErrorLogging.LogWritter("Admin", "EditCategory", EX.Message, "Details: " + str);
                return Json(new { success = false, message = EX.Message });
            }
        }
    }
}