using System;
using System.Data;
using System.Data.SqlClient;
using POS_System.DAL;

namespace POS_System.BLL
{
    public class CategoriesManager
    {
        private readonly DataAccessLayer _dal = new DataAccessLayer();

        public bool CheckDatabaseConnection()
        {
            return _dal.TestConnection();
        }

        public DataTable GetAllCategories()
        {
            string query = "SELECT CategoryID, CategoryName, Description FROM Categories";
            return _dal.SelectData(query);
        }
        public int AddCategory(string categoryName, string description)
        {
            string name = (categoryName ?? "").Trim().Replace("'", "''");
            string desc = (description ?? "").Trim().Replace("'", "''");
            string query = $"INSERT INTO Categories (CategoryName, Description) VALUES (N'{name}', N'{desc}')";
            return _dal.ExecuteCommand(query);
        }

        public int UpdateCategory(int categoryId, string categoryName, string description)
        {
            string name = (categoryName ?? "").Trim().Replace("'", "''");
            string desc = (description ?? "").Trim().Replace("'", "''");
            string query = $"UPDATE Categories SET CategoryName = N'{name}', Description = N'{desc}' WHERE CategoryID = {categoryId}";
            return _dal.ExecuteCommand(query);
        }

        public int DeleteCategory(int categoryId)
        {
            string query = $"DELETE FROM Categories WHERE CategoryID = {categoryId}";
            return _dal.ExecuteCommand(query);
        }
    }
}