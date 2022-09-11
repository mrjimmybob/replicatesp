using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading;

namespace replicatesp
{
    internal class DB
    {
        SqlConnectionStringBuilder dbData;
        string errorMessage;
        string errorDetail;
        bool error;
        int errorNumber;

        public DB(string datasource, string userid, string password, string catalog)
        {
            error = false;
            errorMessage = string.Empty;
            errorDetail = string.Empty;
            errorNumber = 0;
            try
            {
                dbData = new SqlConnectionStringBuilder();
                dbData.DataSource = datasource;
                dbData.UserID = userid;
                dbData.Password = password;
                dbData.InitialCatalog = catalog;
            }
            catch (Exception ex) {
                error = true;
                errorMessage = "Error creating dbData string";
                errorDetail = ex.Message;
            }
        }
 
        public bool RunQuery(string query)
        {
            try
            {
                using SqlConnection connection = new SqlConnection(dbData.ConnectionString);
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                //
                Thread.Sleep(2000);
                //
                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                error = true;
                errorMessage = ex.Number switch
                {
                    -1 => "Error connecting to remote database (" + ex.Number.ToString() + ")",
                    156 => "SQL syntax error. (" + ex.Number.ToString() + ")",
                    208 => "Invalid object name. (" + ex.Number.ToString() + ")",
                    233 => "Error connecting to remote database. Wrong credentials. (" + ex.Number.ToString() + ")",
                    2714 => "Error creating remote object. Object exists. (" + ex.Number.ToString() + ")",
                    2601 => "Error connecting to remote database (" + ex.Number.ToString() + ")",
                    _ => "General SQL exception (" + ex.Number.ToString() + ")",
                };
                errorNumber = ex.Number;
                errorDetail = ex.Message;
            }
            return error;
        }

        public bool RenameStoredProcedure(string oldName, string newName)
        {
            try
            {
                string renameCommand = "EXEC sp_rename '" + oldName + "', '" + newName + "';";

                using SqlConnection connection = new SqlConnection(dbData.ConnectionString);
                SqlCommand command = new SqlCommand(renameCommand, connection);
                command.Connection.Open();
                //
                Thread.Sleep(2000);
                //
                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                Print.Error("Error renaming Stored Procedure", "Error", "[" + ex.Number + "] " + ex.Message);
                error = true;
                errorMessage = ex.Number switch
                {
                    -1 => "Error connecting to remote database (" + ex.Number.ToString() + ")",
                    156 => "SQL syntax error. (" + ex.Number.ToString() + ")",
                    208 => "Invalid object name. (" + ex.Number.ToString() + ")",
                    233 => "Error connecting to remote database. Wrong credentials. (" + ex.Number.ToString() + ")",
                    2714 => "Error creating remote object. Object exists. (" + ex.Number.ToString() + ")",
                    2601 => "Error connecting to remote database (" + ex.Number.ToString() + ")",
                    _ => "General SQL exception (" + ex.Number.ToString() + ")",
                };
                errorNumber = ex.Number;
                errorDetail = ex.Message;
            }
            return error;
        }

        public string getErrorMessage()
        {
            return errorMessage;
        }
        
        public string getErrorDetail()
        {
            return errorDetail;
        }
        
        public int getErrorNumber()
        {
            return errorNumber;
        }

        public bool Error()
        {
            return error;
        }

        public void ResetError()
        {
            error = false;
            errorMessage = string.Empty;
            errorDetail = string.Empty;
            errorNumber = 0;
        }
    }
}
