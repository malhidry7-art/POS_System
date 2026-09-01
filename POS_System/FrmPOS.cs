using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using POS_System.BLL;

namespace POS_System
{
    public partial class FrmPOS : Form
    {
        private readonly SalesManager _salesManager = new SalesManager();
        private DataTable _cartTable;

        // متغيرات الطباعة
        private PrintDocument _printDoc = new PrintDocument();
        private PrintPreviewDialog _previewDlg = new PrintPreviewDialog();
        private string _lastCustomerName = "";
        private string _lastInvoiceDate = "";
        private string _lastTotal = "";
        private string _lastDiscount = "";
        private string _lastFinal = "";
        private string _lastPaid = "";
        private string _lastRemaining = "";
        private DataTable _lastPrintDetails;

        public FrmPOS()
        {
            InitializeComponent();
            _printDoc.PrintPage += new PrintPageEventHandler(PrintDocument_PrintPage);
        }

        private void FrmPOS_Load(object sender, EventArgs e)
        {
            InitCartTable();
            txtBarcode.Focus();
        }

        private void InitCartTable()
        {
            _cartTable = new DataTable();
            _cartTable.Columns.Add("ProductID", typeof(int));
            _cartTable.Columns.Add("Barcode", typeof(string));
            _cartTable.Columns.Add("ProductName", typeof(string));
            _cartTable.Columns.Add("UnitPrice", typeof(decimal));
            _cartTable.Columns.Add("Quantity", typeof(int));
            _cartTable.Columns.Add("TotalPrice", typeof(decimal), "UnitPrice * Quantity");

            dgvCart.DataSource = _cartTable;

            if (dgvCart.Columns["ProductID"] != null) dgvCart.Columns["ProductID"].Visible = false;
            if (dgvCart.Columns["Barcode"] != null)
            {
                dgvCart.Columns["Barcode"].HeaderText = "الباركود";
                dgvCart.Columns["Barcode"].ReadOnly = true;
            }
            if (dgvCart.Columns["ProductName"] != null)
            {
                dgvCart.Columns["ProductName"].HeaderText = "اسم الدواء / الصنف";
                dgvCart.Columns["ProductName"].ReadOnly = true;
            }
            if (dgvCart.Columns["UnitPrice"] != null)
            {
                dgvCart.Columns["UnitPrice"].HeaderText = "سعر الوحدة";
                dgvCart.Columns["UnitPrice"].ReadOnly = true;
            }
            if (dgvCart.Columns["Quantity"] != null)
            {
                dgvCart.Columns["Quantity"].HeaderText = "الكمية المباعة";
                dgvCart.Columns["Quantity"].ReadOnly = false;
            }
            if (dgvCart.Columns["TotalPrice"] != null)
            {
                dgvCart.Columns["TotalPrice"].HeaderText = "الإجمالي";
                dgvCart.Columns["TotalPrice"].ReadOnly = true;
            }
        }

        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(txtBarcode.Text))
            {
                e.SuppressKeyPress = true;
                AddProductToCartByBarcode(txtBarcode.Text.Trim());
                txtBarcode.Clear();
            }
        }

        private void AddProductToCartByBarcode(string barcode)
        {
            DataTable dt = _salesManager.GetProductByBarcode(barcode);
            if (dt.Rows.Count > 0)
            {
                int productId = Convert.ToInt32(dt.Rows[0]["ProductID"]);
                string prodName = dt.Rows[0]["ProductName"].ToString()!;
                decimal sellPrice = Convert.ToDecimal(dt.Rows[0]["SellPrice"]);
                int stockQty = Convert.ToInt32(dt.Rows[0]["Quantity"]);

                if (stockQty <= 0)
                {
                    MessageBox.Show("عذراً، هذا الصنف نفد من المخزون تماماً!", "نفاد الكمية", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataRow[] existingRows = _cartTable.Select($"ProductID = {productId}");
                if (existingRows.Length > 0)
                {
                    int currentQty = Convert.ToInt32(existingRows[0]["Quantity"]);
                    if (currentQty + 1 > stockQty)
                    {
                        MessageBox.Show($"الكمية المتوفرة في المخزن ({stockQty}) فقط!", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    existingRows[0]["Quantity"] = currentQty + 1;
                }
                else
                {
                    DataRow newRow = _cartTable.NewRow();
                    newRow["ProductID"] = productId;
                    newRow["Barcode"] = barcode;
                    newRow["ProductName"] = prodName;
                    newRow["UnitPrice"] = sellPrice;
                    newRow["Quantity"] = 1;
                    _cartTable.Rows.Add(newRow);
                }

                CalculateInvoiceTotals(null, null);
            }
            else
            {
                MessageBox.Show("لم يتم العثور على دواء بهذا الباركود", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void CalculateInvoiceTotals(object sender, EventArgs e)
        {
            decimal total = 0;
            foreach (DataRow row in _cartTable.Rows)
            {
                if (row.RowState != DataRowState.Deleted)
                {
                    total += Convert.ToDecimal(row["TotalPrice"]);
                }
            }

            txtTotal.Text = total.ToString("N2");

            decimal.TryParse(txtDiscount.Text, out decimal discount);
            decimal finalAmount = total - discount;
            if (finalAmount < 0) finalAmount = 0;
            txtFinal.Text = finalAmount.ToString("N2");

            decimal.TryParse(txtPaid.Text, out decimal paid);
            decimal remaining = paid - finalAmount;
            txtRemaining.Text = remaining.ToString("N2");
        }

        private void dgvCart_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvCart.Columns[e.ColumnIndex].Name == "Quantity")
            {
                CalculateInvoiceTotals(null, null);
            }
        }

        private void btnDeleteRow_Click(object sender, EventArgs e)
        {
            if (dgvCart.SelectedRows.Count > 0)
            {
                dgvCart.Rows.RemoveAt(dgvCart.SelectedRows[0].Index);
                CalculateInvoiceTotals(null, null);
            }
        }

        private void btnNewInvoice_Click(object sender, EventArgs e)
        {
            _cartTable.Clear();
            txtCustomer.Text = "عميل نقدي";
            txtTotal.Text = "0.00";
            txtDiscount.Text = "0.00";
            txtFinal.Text = "0.00";
            txtPaid.Text = "0.00";
            txtRemaining.Text = "0.00";
            txtBarcode.Clear();
            txtBarcode.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_cartTable.Rows.Count == 0)
            {
                MessageBox.Show("لا توجد أصناف في الفاتورة لحفظها", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal.TryParse(txtTotal.Text, out decimal total);
            decimal.TryParse(txtDiscount.Text, out decimal discount);
            decimal.TryParse(txtFinal.Text, out decimal finalAmount);
            decimal.TryParse(txtPaid.Text, out decimal paid);
            decimal.TryParse(txtRemaining.Text, out decimal remaining);

            string customerName = string.IsNullOrWhiteSpace(txtCustomer.Text) ? "عميل نقدي" : txtCustomer.Text.Trim();

            bool success = _salesManager.SaveInvoice(customerName, total, discount, finalAmount, paid, remaining, _cartTable);
            if (success)
            {
                _lastCustomerName = customerName;
                _lastInvoiceDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                _lastTotal = txtTotal.Text;
                _lastDiscount = txtDiscount.Text;
                _lastFinal = txtFinal.Text;
                _lastPaid = txtPaid.Text;
                _lastRemaining = txtRemaining.Text;
                _lastPrintDetails = _cartTable.Copy();

                DialogResult dr = MessageBox.Show("تم حفظ الفاتورة بنجاح وتحديث المخزون!\n\nهل تريد طباعة إيصال الفاتورة الآن؟",
                    "حفظ الفاتورة", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dr == DialogResult.Yes)
                {
                    _previewDlg.Document = _printDoc;
                    _previewDlg.Width = 600;
                    _previewDlg.Height = 700;
                    _previewDlg.StartPosition = FormStartPosition.CenterScreen;
                    _previewDlg.ShowDialog();
                }

                btnNewInvoice_Click(null, null);
            }
            else
            {
                MessageBox.Show("حدث خطأ أثناء حفظ الفاتورة في قاعدة البيانات", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FrmPOS_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F10)
            {
                btnSave_Click(null, null);
            }
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics!;
            Font titleFont = new Font("Arial", 14, FontStyle.Bold);
            Font subTitleFont = new Font("Arial", 10, FontStyle.Regular);
            Font headerFont = new Font("Arial", 9, FontStyle.Bold);
            Font bodyFont = new Font("Arial", 9, FontStyle.Regular);
            Font boldFont = new Font("Arial", 10, FontStyle.Bold);

            StringFormat centerFormat = new StringFormat { Alignment = StringAlignment.Center };

            int y = 20;
            int pageWidth = 280;

            g.DrawString("صيدلية الشفاء النموذجية", titleFont, Brushes.Black, new RectangleF(0, y, pageWidth, 25), centerFormat);
            y += 26;
            g.DrawString("فاتورة مبيعات نقدية", subTitleFont, Brushes.Black, new RectangleF(0, y, pageWidth, 20), centerFormat);
            y += 20;
            g.DrawString($"العميل: {_lastCustomerName}", boldFont, Brushes.Black, new RectangleF(0, y, pageWidth, 20), centerFormat);
            y += 20;
            g.DrawString($"التاريخ: {_lastInvoiceDate}", bodyFont, Brushes.Black, new RectangleF(0, y, pageWidth, 18), centerFormat);
            y += 20;

            g.DrawLine(new Pen(Color.Black, 1) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash }, 5, y, pageWidth - 5, y);
            y += 8;

            g.DrawString("الصنف", headerFont, Brushes.Black, 180, y);
            g.DrawString("الكمية", headerFont, Brushes.Black, 115, y);
            g.DrawString("السعر", headerFont, Brushes.Black, 65, y);
            g.DrawString("الإجمالي", headerFont, Brushes.Black, 10, y);
            y += 20;

            g.DrawLine(Pens.Black, 5, y, pageWidth - 5, y);
            y += 6;

            if (_lastPrintDetails != null)
            {
                foreach (DataRow row in _lastPrintDetails.Rows)
                {
                    string name = row["ProductName"].ToString()!;
                    if (name.Length > 16) name = name.Substring(0, 16) + "..";

                    string qty = row["Quantity"].ToString()!;
                    string price = Convert.ToDecimal(row["UnitPrice"]).ToString("0.00");
                    string rowTotal = Convert.ToDecimal(row["TotalPrice"]).ToString("0.00");

                    g.DrawString(name, bodyFont, Brushes.Black, 160, y);
                    g.DrawString(qty, bodyFont, Brushes.Black, 125, y);
                    g.DrawString(price, bodyFont, Brushes.Black, 60, y);
                    g.DrawString(rowTotal, bodyFont, Brushes.Black, 10, y);
                    y += 20;
                }
            }

            g.DrawLine(new Pen(Color.Black, 1) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash }, 5, y, pageWidth - 5, y);
            y += 10;

            g.DrawString($"الإجمالي: {_lastTotal}", bodyFont, Brushes.Black, 20, y);
            y += 18;
            g.DrawString($"الخصم: {_lastDiscount}", bodyFont, Brushes.Black, 20, y);
            y += 18;
            g.DrawString($"الصافي المطلوب: {_lastFinal}", boldFont, Brushes.Black, 20, y);
            y += 22;
            g.DrawString($"المدفوع: {_lastPaid}", bodyFont, Brushes.Black, 20, y);
            y += 18;

            // تمييز المتبقي إذا كان ديناً أو باقياً
            decimal.TryParse(_lastRemaining, out decimal rem);
            if (rem < 0)
            {
                g.DrawString($"المتبقي (دين على العميل): {Math.Abs(rem):0.00}", boldFont, Brushes.Black, 20, y);
            }
            else
            {
                g.DrawString($"المتبقي للعميل: {_lastRemaining}", bodyFont, Brushes.Black, 20, y);
            }
            y += 25;

            g.DrawLine(Pens.Black, 5, y, pageWidth - 5, y);
            y += 8;
            g.DrawString("شكراً لزيارتكم ونتمنى لكم الشفاء العاجل", subTitleFont, Brushes.Black, new RectangleF(0, y, pageWidth, 20), centerFormat);
        }
    }
}