using System;
using System.Data;
using System.Data.SqlClient;
using POS_System.DAL;

namespace POS_System.BLL
{
    public class ProductsManager
    {
        private readonly DataAccessLayer _dal = new DataAccessLayer();

        // جلب جميع الأدوية مع اسم تصنيفها
        public DataTable GetAllProducts()
        {
            string query = @"SELECT 
                                P.ProductID,
                                P.Barcode,
                                P.ProductName,
                                P.GenericName,
                                C.CategoryName,
                                P.BuyPrice,
                                P.SellPrice,
                                P.Quantity,
                                P.ExpiryDate,
                                P.CategoryID
                             FROM Products P
                             INNER JOIN Categories C ON P.CategoryID = C.CategoryID";
            return _dal.SelectData(query);
        }

        // إضافة دواء جديد
        public int AddProduct(string barcode, string name, string genericName, int categoryId, decimal buyPrice, decimal sellPrice, int quantity, DateTime expiryDate)
        {
            string query = @"INSERT INTO Products (Barcode, ProductName, GenericName, CategoryID, BuyPrice, SellPrice, Quantity, ExpiryDate) 
                             VALUES (@Barcode, @ProductName, @GenericName, @CategoryID, @BuyPrice, @SellPrice, @Quantity, @ExpiryDate)";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Barcode", string.IsNullOrEmpty(barcode) ? (object)DBNull.Value : barcode),
                new SqlParameter("@ProductName", name),
                new SqlParameter("@GenericName", string.IsNullOrEmpty(genericName) ? (object)DBNull.Value : genericName),
                new SqlParameter("@CategoryID", categoryId),
                new SqlParameter("@BuyPrice", buyPrice),
                new SqlParameter("@SellPrice", sellPrice),
                new SqlParameter("@Quantity", quantity),
                new SqlParameter("@ExpiryDate", expiryDate)
            };

            return _dal.ExecuteCommand(query, parameters);
        }

        // تعديل بيانات دواء
        public int UpdateProduct(int productId, string barcode, string name, string genericName, int categoryId, decimal buyPrice, decimal sellPrice, int quantity, DateTime expiryDate)
        {
            string query = @"UPDATE Products 
                             SET Barcode = @Barcode, 
                                 ProductName = @ProductName, 
                                 GenericName = @GenericName, 
                                 CategoryID = @CategoryID, 
                                 BuyPrice = @BuyPrice, 
                                 SellPrice = @SellPrice, 
                                 Quantity = @Quantity, 
                                 ExpiryDate = @ExpiryDate 
                             WHERE ProductID = @ProductID";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ProductID", productId),
                new SqlParameter("@Barcode", string.IsNullOrEmpty(barcode) ? (object)DBNull.Value : barcode),
                new SqlParameter("@ProductName", name),
                new SqlParameter("@GenericName", string.IsNullOrEmpty(genericName) ? (object)DBNull.Value : genericName),
                new SqlParameter("@CategoryID", categoryId),
                new SqlParameter("@BuyPrice", buyPrice),
                new SqlParameter("@SellPrice", sellPrice),
                new SqlParameter("@Quantity", quantity),
                new SqlParameter("@ExpiryDate", expiryDate)
            };

            return _dal.ExecuteCommand(query, parameters);
        }

        // حذف دواء
        public int DeleteProduct(int productId)
        {
            string query = "DELETE FROM Products WHERE ProductID = @ProductID";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ProductID", productId)
            };

            return _dal.ExecuteCommand(query, parameters);
        }
    }
}