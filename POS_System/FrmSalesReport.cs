using System;
using System.Data;
using System.Windows.Forms;
using POS_System.BLL;

namespace POS_System
{
    public partial class FrmSalesReport : Form
    {
        private readonly SalesManager _salesManager = new SalesManager();

        public FrmSalesReport()
        {
            InitializeComponent();
        }

        private void FrmSalesReport_Load(object sender, EventArgs e)
        {
            dtpFrom.Value = DateTime.Now;
            dtpTo.Value = DateTime.Now;
            LoadInvoices();
        }

        private void btnToday_Click(object sender, EventArgs e)
        {
            dtpFrom.Value = DateTime.Now;
            dtpTo.Value = DateTime.Now;
            LoadInvoices();
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            LoadInvoices();
        }

        private void LoadInvoices()
        {
            DataTable dt = _salesManager.GetInvoicesByDate(dtpFrom.Value, dtpTo.Value);
            dgvInvoices.DataSource = dt;

            if (dgvInvoices.Columns["InvoiceID"] != null) dgvInvoices.Columns["InvoiceID"].HeaderText = "رقم الفاتورة";
            if (dgvInvoices.Columns["CustomerName"] != null) dgvInvoices.Columns["CustomerName"].HeaderText = "اسم الزبون / العميل";
            if (dgvInvoices.Columns["InvoiceDate"] != null) dgvInvoices.Columns["InvoiceDate"].HeaderText = "التاريخ والوقت";
            if (dgvInvoices.Columns["TotalAmount"] != null) dgvInvoices.Columns["TotalAmount"].HeaderText = "المبلغ قبل الخصم";
            if (dgvInvoices.Columns["Discount"] != null) dgvInvoices.Columns["Discount"].HeaderText = "الخصم";
            if (dgvInvoices.Columns["FinalAmount"] != null) dgvInvoices.Columns["FinalAmount"].HeaderText = "الصافي المطلوب";
            if (dgvInvoices.Columns["PaidAmount"] != null) dgvInvoices.Columns["PaidAmount"].HeaderText = "المدفوع";
            if (dgvInvoices.Columns["RemainingAmount"] != null) dgvInvoices.Columns["RemainingAmount"].HeaderText = "المتبقي";

            // حساب الإجماليات أسفل الشاشة
            txtInvoicesCount.Text = dt.Rows.Count.ToString();
            decimal sumFinal = 0;
            foreach (DataRow row in dt.Rows)
            {
                sumFinal += Convert.ToDecimal(row["FinalAmount"]);
            }
            txtFinalSum.Text = sumFinal.ToString("N2");

            if (dt.Rows.Count == 0)
            {
                dgvDetails.DataSource = null;
            }
        }

        private void dgvInvoices_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvInvoices.SelectedRows.Count > 0)
            {
                int invoiceId = Convert.ToInt32(dgvInvoices.SelectedRows[0].Cells["InvoiceID"].Value);
                LoadDetails(invoiceId);
            }
        }

        private void LoadDetails(int invoiceId)
        {
            DataTable dt = _salesManager.GetInvoiceDetails(invoiceId);
            dgvDetails.DataSource = dt;

            if (dgvDetails.Columns["DetailID"] != null) dgvDetails.Columns["DetailID"].Visible = false;
            if (dgvDetails.Columns["Barcode"] != null) dgvDetails.Columns["Barcode"].HeaderText = "الباركود";
            if (dgvDetails.Columns["ProductName"] != null) dgvDetails.Columns["ProductName"].HeaderText = "اسم الدواء";
            if (dgvDetails.Columns["Quantity"] != null) dgvDetails.Columns["Quantity"].HeaderText = "الكمية المباعة";
            if (dgvDetails.Columns["UnitPrice"] != null) dgvDetails.Columns["UnitPrice"].HeaderText = "سعر الوحدة";
            if (dgvDetails.Columns["TotalPrice"] != null) dgvDetails.Columns["TotalPrice"].HeaderText = "الإجمالي";
        }
    }
}