using System;
using System.Data;
using System.Windows.Forms;
using POS_System.BLL;

namespace POS_System
{
    public partial class FrmLogin : Form
    {
        private readonly UserManager _userManager = new UserManager();

        public FrmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUser.Text) || string.IsNullOrWhiteSpace(txtPass.Text))
            {
                MessageBox.Show("يرجى إدخال اسم المستخدم وكلمة المرور", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataTable dt = _userManager.Login(txtUser.Text.Trim(), txtPass.Text.Trim());
            if (dt.Rows.Count > 0)
            {
                string role = dt.Rows[0]["Role"].ToString()!;
                string fullName = dt.Rows[0]["FullName"].ToString()!;

                // فتح الشاشة الرئيسية وتمرير الصلاحية
                FrmMain mainForm = new FrmMain(role, fullName);
                this.Hide();
                mainForm.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("اسم المستخدم أو كلمة المرور غير صحيحة", "خطأ في الدخول", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPass.Clear();
                txtPass.Focus();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txtPass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin_Click(null, null);
            }
        }

        private void btnServerSettings_Click(object sender, EventArgs e)
        {
            using (ServerSettingsForm frm = new ServerSettingsForm())
            {
                frm.ShowDialog();
            }
        }
    }
}