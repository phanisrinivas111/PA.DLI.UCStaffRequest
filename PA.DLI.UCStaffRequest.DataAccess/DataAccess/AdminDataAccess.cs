using PA.DLI.UCStaffRequest.DataAccess.DataObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PA.DLI.UCStaffRequest.DataAccess.DataAccess
{
    public class AdminDataAccess
    {
        private readonly AdoDataProvider _dataProvider;
        public AdminDataAccess()
        {
            _dataProvider = new AdoDataProvider();
        }

        public List<User> GetUsers()
        {
            var users = new List<User>();
            try
            {
                var dataTable = _dataProvider.ExecuteQuery("EXEC USP_GET_USERS");

                foreach (DataRow row in dataTable.Rows)
                {
                    var user = new User();
                    if (!row["IDN_USER"].Equals(DBNull.Value))
                    {
                        user.UserId = row["IDN_USER"].ToString();
                    }
                    if (!row["TXT_ROLE"].Equals(DBNull.Value))
                    {
                        user.RoleCode = row["TXT_ROLE"].ToString();
                    }
                    if (!row["DTE_CHNG_LAST"].Equals(DBNull.Value))
                    {
                        user.LastChangeDate = Convert.ToDateTime(row["DTE_CHNG_LAST"]);
                    }
                    if (!row["USER_CHNG_LAST"].Equals(DBNull.Value))
                    {
                        user.LastChangeUser = row["USER_CHNG_LAST"].ToString();
                    }
                    if (!row["IND_DEL"].Equals(DBNull.Value))
                    {
                        user.IsDeleted = row["IND_DEL"].ToString();
                    }
                    users.Add(user);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return users;
        }
        public List<User> GetAllUsers()
        {
            var users = new List<User>();
            try
            {
                var dataTable = _dataProvider.ExecuteQuery("EXEC USP_GETALL_USERS");
                    foreach (DataRow row in dataTable.Rows)
                {
                        var user = new User();
                        if (!row["IDN_USER"].Equals(DBNull.Value))
                        {
                            user.UserId = row["IDN_USER"].ToString();
                        }
                        if (!row["CDE_ROLE"].Equals(DBNull.Value))
                        {
                            user.RoleCode = row["CDE_ROLE"].ToString();
                        }
                        if (!row["DTE_CHNG_LAST"].Equals(DBNull.Value))
                        {
                            user.LastChangeDate = Convert.ToDateTime(row["DTE_CHNG_LAST"]);
                        }
                        if (!row["USER_CHNG_LAST"].Equals(DBNull.Value))
                        {
                            user.LastChangeUser = row["USER_CHNG_LAST"].ToString();
                        }
                        if (!row["IND_DEL"].Equals(DBNull.Value))
                        {
                            user.IsDeleted = row["IND_DEL"].ToString();
                        }
                        users.Add(user);
                    }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return users;
        }
        public List<Role> GetAllRoles()
        {
            var roles = new List<Role>();
            try
            {
                var dataTable = _dataProvider.ExecuteQuery("EXEC USP_GET_ROLES");
                foreach (DataRow row in dataTable.Rows)
                {
                    var role = new Role();
                    if (!row["CDE_ROLE"].Equals(DBNull.Value))
                    {
                        role.CDE_ROLE = row["CDE_ROLE"].ToString();
                    }
                    if (!row["TXT_ROLE"].Equals(DBNull.Value))
                    {
                        role.TXT_ROLE = row["TXT_ROLE"].ToString();
                    }
                    roles.Add(role);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return roles;
        }
        public void AddUser(User user)
        {

            using (var connection = _dataProvider.GetDbConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        if (user.UserId == null)
                        {
                            throw new ArgumentNullException(nameof(user.UserId));
                        }
                        if (user.RoleCode == null)
                        {
                            throw new ArgumentNullException(nameof(user.RoleCode));
                        }
                        if (user.LastChangeDate == null)
                        {
                            throw new ArgumentNullException(nameof(user.LastChangeDate));
                        }
                        if (user.LastChangeUser == null)
                        {
                            throw new ArgumentNullException(nameof(user.LastChangeUser));
                        }
                        // If IsDeleted is a nullable boolean, check it
                        if (user.IsDeleted == null)
                        {
                            throw new ArgumentNullException(nameof(user.IsDeleted));
                        }
                        var parameters = new Dictionary<string, object>
                            {
                                {"@UserId", user.UserId },
                                {"@RoleCode", user.RoleCode },
                                {"@LastChangedDate", user.LastChangeDate },
                                {"@LastChangeUser", user.LastChangeUser },
                                {"@IsDeleted", user.IsDeleted }
                            };
                        _dataProvider.ExecuteNonQuery("EXEC USP_ADD_USER @UserId, @RoleCode, @LastChangedDate, @LastChangeUser, @IsDeleted", parameters, null);
                        transaction.Commit();
                    }


                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }     
            
        }

        public void DeleteUser(string userId)
                {
                    using (var connection = _dataProvider.GetDbConnection())
                    {
                        connection.Open();
                        using (var transaction = connection.BeginTransaction())
                        {
                            try
                            {
                        if (userId == null)
                        {
                            throw new ArgumentNullException(nameof(userId));
                        }

                        var parameters = new Dictionary<string, object>
                                    {
                                        {"@UserId", userId}
                                    };
                                _dataProvider.ExecuteNonQuery("EXEC USP_DELETE_USER @UserId", parameters, null);
                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                throw ex;
                            }
                        }
                    }
                }

        public List<Category> GetCategories()
        {
            try
            {
                var categories = new List<Category>();
                var dataTable = _dataProvider.ExecuteQuery("EXEC USP_GET_CATEGORIES");
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        var category = new Category();
                        if (!row["CTRY_ID"].Equals(DBNull.Value))
                        {
                            category.CategoryId = Convert.ToInt32(row["CTRY_ID"]);
                        }
                        if (!row["CTRY_NAME"].Equals(DBNull.Value))
                        {
                            category.CategoryName = row["CTRY_NAME"].ToString();
                        }
                        if (!row["TXT_EMAIL"].Equals(DBNull.Value))
                        {
                            category.Email = row["TXT_EMAIL"].ToString();
                        }
                        if (!row["DTE_CHNG_LAST"].Equals(DBNull.Value))
                        {
                            category.LastChangedDate = Convert.ToDateTime(row["DTE_CHNG_LAST"]);
                        }
                        if (!row["USER_CHNG_LAST"].Equals(DBNull.Value))
                        {
                            category.LastChangedUser = row["USER_CHNG_LAST"].ToString();
                        }
                        if (!row["IND_DEL"].Equals(DBNull.Value))
                        {
                            category.IsDeleted = row["IND_DEL"].ToString();
                        }
                        categories.Add(category);
                    }
                }
            
                return categories;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddCategory(Category category)
        {
            using (var connection = _dataProvider.GetDbConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        if (category.CategoryName == null)
                        {
                            throw new ArgumentNullException(nameof(category.CategoryName));
                        }
                        if (category.Email == null)
                        {
                            throw new ArgumentNullException(nameof(category.Email));
                        }
                        if (category.LastChangedDate == null)
                        {
                            throw new ArgumentNullException(nameof(category.LastChangedDate));
                        }
                        // If IsDeleted is a nullable boolean, check it
                        if (category.IsDeleted == null)
                        {
                            throw new ArgumentNullException(nameof(category.IsDeleted));
                        }
                        var parameters = new Dictionary<string, object>
                    {
                        {"@CategoryName", category.CategoryName },
                        {"@Email", category.Email },
                        {"@LastChangedDate", category.LastChangedDate },
                        {"@LastChangeUser", category.LastChangedUser },
                        {"@IsDeleted", category.IsDeleted }
                    };
                        _dataProvider.ExecuteNonQuery("EXEC USP_ADD_CATEGORY @CategoryName, @Email, @LastChangedDate, @LastChangeUser, @IsDeleted", parameters, null);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }


        public void EditCategory(Category category)
        {
            using (var connection = _dataProvider.GetDbConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        if (category.CategoryName == null)
                        {
                            throw new ArgumentNullException(nameof(category.CategoryName));
                        }
                        if (category.Email == null)
                        {
                            throw new ArgumentNullException(nameof(category.Email));
                        }
                        var parameters = new Dictionary<string, object>
                        {
                            {"@CategoryName", category.CategoryName },
                            {"@Email", category.Email ?? string.Empty},
                            {"@LastChangeUser", category.LastChangedUser}
                        };
                        _dataProvider.ExecuteNonQuery("EXEC USP_EDIT_CATEGORY @CategoryName, @Email, @LastChangeUser", parameters, null);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }
        public void EditUser(User user)
        {
            using (var connection = _dataProvider.GetDbConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        if (user.UserId == null)
                        {
                            throw new ArgumentNullException(nameof(user.UserId));
                        }
                        if (user.RoleCode == null)
                        {
                            throw new ArgumentNullException(nameof(user.RoleCode));
                        }
                        if (user.LastChangeDate == null)
                        {
                            throw new ArgumentNullException(nameof(user.LastChangeDate));
                        }
                        if (user.LastChangeUser == null)
                        {
                            throw new ArgumentNullException(nameof(user.LastChangeUser));
                        }
                        if (user.IsDeleted == null)
                        {
                            throw new ArgumentNullException(nameof(user.IsDeleted));
                        }

                        var parameters = new Dictionary<string, object>
                        {
                            {"@UserId", user.UserId},
                            {"@RoleCode", user.RoleCode },
                            {"@LastChangedDate", user.LastChangeDate },
                            {"@LastChangeUser", user.LastChangeUser },
                            {"@IsDeleted", user.IsDeleted }
                        };
                        _dataProvider.ExecuteNonQuery("EXEC USP_UPDATE_USER @UserId, @RoleCode, @LastChangedDate, @LastChangeUser, @IsDeleted", parameters, null);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }
    }
}
