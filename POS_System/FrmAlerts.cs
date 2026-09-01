using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using POS_System.BLL;

namespace POS_System
{
    public partial class FrmAlerts : Form
    {
        private readonly SalesManager _salesManager = new SalesManager();

        public FrmAlerts()
        {
            InitializeComponent();
        }

        private void FrmAlerts_Load(object sender, EventArgs e)
        {
            cmbExpiryPeriod.SelectedIndex = 1; // الافتراضي: 30 يوم
            LoadExpiryAlerts();
            LoadShortageAlerts();
        }

        private void btnFilterExpiry_Click(object sender, EventArgs e)
        {
            LoadExpiryAlerts();
        }

        private void LoadExpiryAlerts()
        {
            int days = 30;
            switch (cmbExpiryPeriod.SelectedIndex)
            {
                case 0: days = 0; break;
                case 1: days = 30; break;
                case 2: days = 90; break;
                case 3: days = 180; break;
            }

            DataTable dt = _salesManager.GetExpiringProducts(days);
            dgvExpiry.DataSource = dt;

            if (dgvExpiry.Columns["ProductID"] != null) dgvExpiry.Columns["ProductID"].Visible = false;
            if (dgvExpiry.Columns["Barcode"] != null) dgvExpiry.Columns["Barcode"].HeaderText = "الباركود";
            if (dgvExpiry.Columns["ProductName"] != null) dgvExpiry.Columns["ProductName"].HeaderText = "اسم الدواء";
            if (dgvExpiry.Columns["Quantity"] != null) dgvExpiry.Columns["Quantity"].HeaderText = "الكمية المتبقية";
            if (dgvExpiry.Columns["ExpiryDate"] != null) dgvExpiry.Columns["ExpiryDate"].HeaderText = "تاريخ الصلاحية";
            if (dgvExpiry.Columns["DaysRemaining"] != null) dgvExpiry.Columns["DaysRemaining"].HeaderText = "الأيام المتبقية";

            // تلوين الأدوية المنتهية بالفعل باللون الأحمر الفاتح
            foreach (DataGridViewRow row in dgvExpiry.Rows)
            {
                if (row.Cells["DaysRemaining"].Value != null && Convert.ToInt32(row.Cells["DaysRemaining"].Value) <= 0)
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 230, 230);
                    row.DefaultCellStyle.ForeColor = Color.DarkRed;
                }
            }
        }

        private void btnFilterShortage_Click(object sender, EventArgs e)
        {
            LoadShortageAlerts();
        }

        private void LoadShortageAlerts()
        {
            int minQty = Convert.ToInt32(nudMinQty.Value);
            DataTable dt = _salesManager.GetLowStockProducts(minQty);
            dgvShortage.DataSource = dt;

            if (dgvShortage.Columns["ProductID"] != null) dgvShortage.Columns["ProductID"].Visible = false;
            if (dgvShortage.Columns["Barcode"] != null) dgvShortage.Columns["Barcode"].HeaderText = "الباركود";
            if (dgvShortage.Columns["ProductName"] != null) dgvShortage.Columns["ProductName"].HeaderText = "اسم الدواء";
            if (dgvShortage.Columns["Quantity"] != null) dgvShortage.Columns["Quantity"].HeaderText = "الكمية الحالية";
            if (dgvShortage.Columns["SellPrice"] != null) dgvShortage.Columns["SellPrice"].HeaderText = "سعر البيع";
        }
    }
}