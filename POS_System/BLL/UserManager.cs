using System;
using System.Data;
using System.Data.SqlClient;
using POS_System.DAL;

namespace POS_System.BLL
{
    public class UserManager
    {
        private readonly DataAccessLayer _dal = new DataAccessLayer();

        // التحقق من بيانات الدخول
        public DataTable Login(string username, string password)
        {
            string query = "SELECT UserID, FullName, UserName, Role FROM Users WHERE UserName = @UserName AND Password = @Password";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@UserName", username),
                new SqlParameter("@Password", password)
            };
            return _dal.SelectData(query, parameters);
        }
    }
}