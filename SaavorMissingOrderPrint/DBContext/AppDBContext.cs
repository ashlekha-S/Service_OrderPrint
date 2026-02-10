using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SaavorMissingOrderPrint.DBContext
{
    /// <summary>
    /// AppDBContext
    /// </summary>
    public class AppDBContext
    {
        /// <summary>
        /// ConnectionString
        /// </summary>
        private readonly string ConnectionString = ConfigurationManager.ConnectionStrings["ServerConnection"].ToString();

        /// <summary>
        /// ExecuteProcedure
        /// </summary>
        /// <param name="proceddureName"></param>
        /// <param name="sqlParameters"></param>
        /// <returns></returns>
        public SqlDataReader ExecuteProcedure(string proceddureName, SqlParameter[] sqlParameters)
        {
            SqlDataReader sdr = null;
            try
            {
                SqlConnection connection = new SqlConnection(ConnectionString);

                SqlCommand cmd = new SqlCommand(proceddureName, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                if (sqlParameters != null && sqlParameters.Length > 0)
                {
                    cmd.Parameters.AddRange(sqlParameters);
                }
                connection.Open();
                sdr = cmd.ExecuteReader();
                return sdr;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Occurred: {ex.Message}");
                return sdr;
            }
        }

        /// <summary>
        /// ExecuteProcedureReturnDataTable
        /// </summary>
        /// <param name="proceddureName"></param>
        /// <param name="sqlParameters"></param>
        /// <returns></returns>
        public DataTable ExecuteProcedureReturnDataTable(string proceddureName, SqlParameter[] sqlParameters)
        {
            var dataTable = new DataTable();
            SqlDataReader sdr = null;
            SqlConnection connection = new SqlConnection(ConnectionString);
            try
            {
                SqlCommand cmd = new SqlCommand(proceddureName, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                if (sqlParameters != null && sqlParameters.Length > 0)
                {
                    cmd.Parameters.AddRange(sqlParameters);
                }
                connection.Open();
                sdr = cmd.ExecuteReader();
                dataTable.Load(sdr);
                return dataTable;
            }
            catch(Exception ex) {
                Console.WriteLine(ex.Message);
                return new DataTable();
            }
            finally
            {
                connection.Close();
                sdr.Close();
            }

        }

        /// <summary>
        /// GetUniversities
        /// </summary>
        /// <param name="universityName"></param>
        /// <returns></returns>
        public DataTable GetOrders(int profileId)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>(){
                        new SqlParameter("@profileId",0)
            };
            try
            {
                return ExecuteProcedureReturnDataTable("SU_Get_Missing_Order_Print", sqlParameters.ToArray());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new DataTable();
            }
        }
    }
}
