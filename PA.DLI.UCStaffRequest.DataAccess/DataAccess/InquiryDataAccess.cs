using PA.DLI.UCStaffRequest.DataAccess.DataObjects;
using PA.DLI.UCStaffRequest.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PA.DLI.UCStaffRequest.DataAccess.DataAccess
{
    public class InquiryDataAccess
    {
        private readonly AdoDataProvider _dataProvider;

        public InquiryDataAccess()
        {
            _dataProvider = new AdoDataProvider();
        }

        public void AddInquiry(Inquiry inquiry)
        {

            using (var connection = _dataProvider.GetDbConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Validate required fields
                        if (inquiry.FromEmail == null)
                        {
                            throw new ArgumentNullException(nameof(inquiry.FromEmail));
                        }
                        if (inquiry.Category == 0)
                        {
                            throw new ArgumentNullException(nameof(inquiry.Category));
                        }
                        if (inquiry.Subject == null)
                        {
                            throw new ArgumentNullException(nameof(inquiry.Subject));
                        }
                        if (inquiry.Message == null)
                        {
                            throw new ArgumentNullException(nameof(inquiry.Message));
                        }

                        // Create a DataTable for files
                        DataTable fileTable = new DataTable();
                        fileTable.Columns.Add("FileName", typeof(string));
                        fileTable.Columns.Add("FileType", typeof(string));
                        fileTable.Columns.Add("FileConLen", typeof(int));
                        fileTable.Columns.Add("FileData", typeof(byte[]));
                        if (inquiry.files != null && inquiry.files.Any())
                        {
                            foreach (var file in inquiry.files)
                            {
                                if (file != null)
                                {
                                    using (var binaryReader = new BinaryReader(file.InputStream))
                                    {
                                        byte[] fileData = binaryReader.ReadBytes(file.ContentLength);
                                        fileTable.Rows.Add(file.FileName, file.ContentType, file.ContentLength, fileData);
                                    }
                                }
                            }
                        }

                        // Prepare parameters
                        var parameters = new List<SqlParameter>
            {
                new SqlParameter("@CategoryID", inquiry.Category),
                new SqlParameter("@FromEmail", inquiry.FromEmail),
                new SqlParameter("@Cc", (object)inquiry.CC ?? DBNull.Value),
                new SqlParameter("@Subject", inquiry.Subject),
                new SqlParameter("@Message", inquiry.Message),
                new SqlParameter("@LastChangedDate", DateTime.Now),
                new SqlParameter("@LastChangeUser", inquiry.LastChangeUser),
                new SqlParameter("@IsDeleted", "N"),
                new SqlParameter
                {
                    ParameterName = "@FileDetails",
                    SqlDbType = SqlDbType.Structured,
                    TypeName = "dbo.FileDetailsType", // TVP name
                    Value = fileTable
                }
            };

                        // Execute stored procedure
                        using (var command = connection.CreateCommand())
                        {
                            command.Transaction = transaction;
                            command.CommandType = CommandType.StoredProcedure;
                            command.CommandText = "USP_INSERT_INQUERY";

                            // Add parameters to the command
                            command.Parameters.AddRange(parameters.ToArray());

                            // Execute the command
                            command.ExecuteNonQuery();
                        }

                        // Commit the transaction
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction in case of error
                        transaction.Rollback();
                        throw;
                    }
                }
            }


        }

        public List<SearchResult> Search(SearchRequest search)
        {
            var response = new List<SearchResult>();
            using (var connection = _dataProvider.GetDbConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {

                        var parameters = new Dictionary<string, object>
                            {
                                {"@TicketNumber", search.TicketNumber },
                                {"@Category ", search.Category },
                                 {"@SubmittedDate", search.SubmittedDate },
                                {"@CwopaId", search.CwopaId },
                            };
                        var dataTable = _dataProvider.ExecuteQuery("EXEC USP_GET_INQUIRIES @TicketNumber, @Category,@SubmittedDate,@CwopaId", parameters);
                        foreach (DataRow row in dataTable.Rows)
                        {
                            var inquiry = new SearchResult();
                            if (!row["DTE_CHNG_LAST"].Equals(DBNull.Value))
                            {
                                inquiry.SubmissionDate = Convert.ToDateTime(row["DTE_CHNG_LAST"]);
                            }
                            if (!row["TXT_FROM_EMAIL"].Equals(DBNull.Value))
                            {
                                inquiry.FromEmail = row["TXT_FROM_EMAIL"].ToString();
                            }
                            if (!row["CATEGORY_NAME"].Equals(DBNull.Value))
                            {
                                inquiry.Category = row["CATEGORY_NAME"].ToString();
                            }
                            if (!row["IDN_TICKET"].Equals(DBNull.Value))
                            {
                                inquiry.TicketId = Convert.ToInt32(row["IDN_TICKET"]);
                            }
                            response. Add(inquiry);
                        }
                    }


                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                    return response;
                }
            }

        }
    }
}
