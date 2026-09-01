using System;
using System.Data;
using System.Data.SqlClient;
using POS_System.DAL;

namespace POS_System.BLL
{
    public class SalesManager
    {
        private readonly DataAccessLayer _dal = new DataAccessLayer();

        public DataTable GetProductByBarcode(string barcode)
        {
            string query = "SELECT ProductID, Barcode, ProductName, SellPrice, Quantity FROM Products WHERE Barcode = @Barcode";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Barcode", barcode)
            };
            return _dal.SelectData(query, parameters);
        }

        public DataTable GetAllProductsForSale()
        {
            string query = "SELECT ProductID, Barcode, ProductName, SellPrice, Quantity FROM Products WHERE Quantity > 0";
            return _dal.SelectData(query);
        }

        public bool SaveInvoice(string customerName, decimal totalAmount, decimal discount, decimal finalAmount, decimal paidAmount, decimal remainingAmount, DataTable invoiceDetails)
        {
            try
            {
                string insertInvoiceQuery = @"INSERT INTO SalesInvoices (CustomerName, TotalAmount, Discount, FinalAmount, PaidAmount, RemainingAmount) 
                                             VALUES (@CustomerName, @TotalAmount, @Discount, @FinalAmount, @PaidAmount, @RemainingAmount);
                                             SELECT SCOPE_IDENTITY();";

                SqlParameter[] invoiceParams = new SqlParameter[]
                {
                    new SqlParameter("@CustomerName", string.IsNullOrWhiteSpace(customerName) ? "عميل نقدي" : customerName),
                    new SqlParameter("@TotalAmount", totalAmount),
                    new SqlParameter("@Discount", discount),
                    new SqlParameter("@FinalAmount", finalAmount),
                    new SqlParameter("@PaidAmount", paidAmount),
                    new SqlParameter("@RemainingAmount", remainingAmount)
                };

                DataTable dt = _dal.SelectData(insertInvoiceQuery, invoiceParams);
                if (dt.Rows.Count == 0) return false;

                int invoiceId = Convert.ToInt32(dt.Rows[0][0]);

                foreach (DataRow row in invoiceDetails.Rows)
                {
                    int productId = Convert.ToInt32(row["ProductID"]);
                    int qty = Convert.ToInt32(row["Quantity"]);
                    decimal unitPrice = Convert.ToDecimal(row["UnitPrice"]);

                    string detailQuery = @"INSERT INTO SalesInvoiceDetails (InvoiceID, ProductID, Quantity, UnitPrice) 
                                          VALUES (@InvoiceID, @ProductID, @Quantity, @UnitPrice)";
                    SqlParameter[] detailParams = new SqlParameter[]
                    {
                        new SqlParameter("@InvoiceID", invoiceId),
                        new SqlParameter("@ProductID", productId),
                        new SqlParameter("@Quantity", qty),
                        new SqlParameter("@UnitPrice", unitPrice)
                    };
                    _dal.ExecuteCommand(detailQuery, detailParams);

                    string updateStockQuery = @"UPDATE Products 
                                               SET Quantity = Quantity - @Quantity 
                                               WHERE ProductID = @ProductID";
                    SqlParameter[] stockParams = new SqlParameter[]
                    {
                        new SqlParameter("@ProductID", productId),
                        new SqlParameter("@Quantity", qty)
                    };
                    _dal.ExecuteCommand(updateStockQuery, stockParams);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public DataTable GetExpiringProducts(int days)
        {
            string query = @"SELECT ProductID, Barcode, ProductName, Quantity, ExpiryDate,
                            DATEDIFF(day, GETDATE(), ExpiryDate) AS DaysRemaining
                            FROM Products 
                            WHERE ExpiryDate IS NOT NULL 
                            AND DATEDIFF(day, GETDATE(), ExpiryDate) <= @Days
                            ORDER BY ExpiryDate ASC";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Days", days)
            };
            return _dal.SelectData(query, parameters);
        }

        public DataTable GetLowStockProducts(int minQty)
        {
            string query = @"SELECT ProductID, Barcode, ProductName, Quantity, SellPrice 
                            FROM Products 
                            WHERE Quantity <= @MinQty 
                            ORDER BY Quantity ASC";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MinQty", minQty)
            };
            return _dal.SelectData(query, parameters);
        }

        public DataTable GetInvoicesByDate(DateTime fromDate, DateTime toDate)
        {
            string query = @"SELECT InvoiceID, CustomerName, InvoiceDate, TotalAmount, Discount, FinalAmount, PaidAmount, RemainingAmount 
                            FROM SalesInvoices 
                            WHERE InvoiceDate BETWEEN @FromDate AND @ToDate 
                            ORDER BY InvoiceDate DESC";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@FromDate", fromDate.Date),
                new SqlParameter("@ToDate", toDate.Date.AddDays(1).AddSeconds(-1))
            };
            return _dal.SelectData(query, parameters);
        }

        public DataTable GetInvoiceDetails(int invoiceId)
        {
            string query = @"SELECT D.DetailID, P.Barcode, P.ProductName, D.Quantity, D.UnitPrice, D.TotalPrice 
                            FROM SalesInvoiceDetails D
                            INNER JOIN Products P ON D.ProductID = P.ProductID
                            WHERE D.InvoiceID = @InvoiceID";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@InvoiceID", invoiceId)
            };
            return _dal.SelectData(query, parameters);
        }
    }
}