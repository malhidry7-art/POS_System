using System;
using System.Windows.Forms;

namespace POS_System
{
    public partial class FrmMain : Form
    {
        private readonly string _userRole = "Admin";
        private readonly string _userName = "";

        public FrmMain(string role = "Admin", string userName = "المدير")
        {
            InitializeComponent();
            _userRole = role;
            _userName = userName;
            ApplyPermissions();
        }

        private void ApplyPermissions()
        {
            this.Text = $"نظام الصيدلية المتكامل - المستخدم: {_userName} ({_userRole})";

            // إذا كان المستخدم كاشير عادي، يتم قفل الشاشات الإدارية وفتح الكاشير مباشرة
            if (_userRole.Equals("Cashier", StringComparison.OrdinalIgnoreCase))
            {
                btnCategories.Enabled = false;
                btnProducts.Enabled = false;
                btnReports.Enabled = false;
                btnAlerts.Enabled = false;

                // فتح شاشة الكاشير تلقائياً
                LoadForm(new FrmPOS());
            }
        }

        private void LoadForm(Form frm)
        {
            if (pnlContainer.Controls.Count > 0)
                pnlContainer.Controls.Clear();

            frm.TopLevel = false;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;
            pnlContainer.Controls.Add(frm);
            pnlContainer.Tag = frm;
            frm.Show();
        }

        private void btnCategories_Click(object sender, EventArgs e)
        {
            LoadForm(new FrmCategories());
        }

        private void btnProducts_Click(object sender, EventArgs e)
        {
            LoadForm(new FrmProducts());
        }

        private void btnPOS_Click(object sender, EventArgs e)
        {
            LoadForm(new FrmPOS());
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            LoadForm(new FrmSalesReport());
        }

        private void btnAlerts_Click(object sender, EventArgs e)
        {
            LoadForm(new FrmAlerts());
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}