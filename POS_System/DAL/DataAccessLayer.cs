using System;
using System.Data;
using System.Data.SqlClient;

namespace POS_System.DAL
{
    public class DataAccessLayer
    {
        private readonly string conString = @"Data Source=.\SQL2022;Initial Catalog=POS_InventoryDB;Integrated Security=True;Encrypt=False;";

        // دالة فحص الاتصال بقاعدة البيانات
        public bool TestConnection()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        // دالة جلب البيانات مع أو بدون باراميترات
        public DataTable SelectData(string query, SqlParameter[]? parameters = null)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(conString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }

        // دالة تنفيذ الأوامر مع باراميترات (إضافة، تعديل، حذف)
        public int ExecuteCommand(string query, SqlParameter[]? parameters)
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    con.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        // دالة تنفيذ الأوامر البسيطة بدون باراميترات
        public int ExecuteCommand(string query)
        {
            return ExecuteCommand(query, null);
        }
    }
}