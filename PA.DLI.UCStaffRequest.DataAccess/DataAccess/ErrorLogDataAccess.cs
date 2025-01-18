using PA.DLI.UCStaffRequest.DataAccess.DataObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PA.DLI.UCStaffRequest.DataAccess.DataAccess
{
    public class ErrorLogDataAccess
    {
        private readonly AdoDataProvider _dataProvider;
        public ErrorLogDataAccess()
        {
            _dataProvider = new AdoDataProvider();
        }
        public string ErrorInfo(ErrorLogBO errorLog)
        {
            using (var connection = _dataProvider.GetDbConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var parameters = new Dictionary<string, object>
                            {
                                {"@ControllerName", errorLog.ControllerName  },
                                {"@ActionName", errorLog.ActionName},
                                {"@Message", errorLog.Message },
                                {"@Additional_Info", errorLog.Additional_Info},
                                {"@DateCreated", errorLog.DateCreated }
                               
                            };
                        
                        var returnValue = new SqlParameter("@ReturnValue", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        _dataProvider.ExecuteNonQuery("EXEC USP_ERR_LOG @ControllerName, @ActionName, @Message, @Additional_INFO, @DateCreated, @ReturnValue OUTPUT", parameters, returnValue);
                        transaction.Commit();
                        return returnValue.Value.ToString();
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
