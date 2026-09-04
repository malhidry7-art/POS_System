using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient; // أو System.Data.SqlClient حسب المستخدم عندك

namespace POS_System
{
    public partial class ServerSettingsForm : Form
    {
        private TextBox txtServer;
        private TextBox txtDatabase;
        private RadioButton rbWindowsAuth;
        private RadioButton rbSqlAuth;
        private TextBox txtUser;
        private TextBox txtPassword;
        private Button btnTest;
        private Button btnSave;

        public ServerSettingsForm()
        {
            InitializeCustomUI();
            LoadCurrentSettings();
        }

        private void InitializeCustomUI()
        {
            this.Text = "إعدادات الاتصال بقاعدة البيانات - فارما تك";
            this.Size = new Size(460, 420);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.RightToLeft = RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.BackColor = Color.FromArgb(245, 247, 250);

            Label lblTitle = new Label
            {
                Text = "تهيئة خادم وقاعدة بيانات النظام",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(41, 128, 185),
                Location = new Point(20, 15),
                AutoSize = true
            };

            Label lblServer = new Label { Text = "اسم السيرفر (Server Name / IP):", Location = new Point(20, 55), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            txtServer = new TextBox { Location = new Point(20, 78), Size = new Size(400, 27), Font = new Font("Segoe UI", 10F) };

            Label lblDb = new Label { Text = "اسم قاعدة البيانات (Database):", Location = new Point(20, 115), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            txtDatabase = new TextBox { Location = new Point(20, 138), Size = new Size(400, 27), Font = new Font("Segoe UI", 10F) };

            GroupBox gbAuth = new GroupBox
            {
                Text = "نوع المصادقة (Authentication)",
                Location = new Point(20, 175),
                Size = new Size(400, 130),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };

            rbWindowsAuth = new RadioButton { Text = "Windows Authentication (محلي)", Location = new Point(180, 25), AutoSize = true, Checked = true };
            rbSqlAuth = new RadioButton { Text = "SQL Server Authentication (مستخدم وكلمة مرور)", Location = new Point(50, 50), AutoSize = true };

            Label lblUser = new Label { Text = "المستخدم:", Location = new Point(320, 85), AutoSize = true, Font = new Font("Segoe UI", 8.5F) };
            txtUser = new TextBox { Location = new Point(205, 82), Size = new Size(110, 25), Enabled = false, Text = "sa" };

            Label lblPass = new Label { Text = "كلمة المرور:", Location = new Point(135, 85), AutoSize = true, Font = new Font("Segoe UI", 8.5F) };
            txtPassword = new TextBox { Location = new Point(15, 82), Size = new Size(115, 25), UseSystemPasswordChar = true, Enabled = false };

            rbSqlAuth.CheckedChanged += (s, e) =>
            {
                txtUser.Enabled = rbSqlAuth.Checked;
                txtPassword.Enabled = rbSqlAuth.Checked;
            };

            gbAuth.Controls.Add(rbWindowsAuth);
            gbAuth.Controls.Add(rbSqlAuth);
            gbAuth.Controls.Add(lblUser);
            gbAuth.Controls.Add(txtUser);
            gbAuth.Controls.Add(lblPass);
            gbAuth.Controls.Add(txtPassword);

            btnTest = new Button
            {
                Text = "اختبار الاتصال 🔌",
                Location = new Point(230, 320),
                Size = new Size(190, 38),
                BackColor = Color.FromArgb(52, 73, 94),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnTest.FlatAppearance.BorderSize = 0;
            btnTest.Click += BtnTest_Click;

            btnSave = new Button
            {
                Text = "حفظ الإعدادات ✔",
                Location = new Point(20, 320),
                Size = new Size(190, 38),
                BackColor = Color.FromArgb(39, 174, 96),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblServer);
            this.Controls.Add(txtServer);
            this.Controls.Add(lblDb);
            this.Controls.Add(txtDatabase);
            this.Controls.Add(gbAuth);
            this.Controls.Add(btnTest);
            this.Controls.Add(btnSave);
        }

        private void LoadCurrentSettings()
        {
            var config = ServerConfig.Load();
            txtServer.Text = config.ServerName;
            txtDatabase.Text = config.DatabaseName;
            rbWindowsAuth.Checked = config.UseWindowsAuth;
            rbSqlAuth.Checked = !config.UseWindowsAuth;
            txtUser.Text = config.SqlUser;
            txtPassword.Text = config.SqlPassword;
        }

        private string BuildTempConnectionString()
        {
            if (rbWindowsAuth.Checked)
                return $@"Data Source={txtServer.Text.Trim()};Initial Catalog={txtDatabase.Text.Trim()};Integrated Security=True;TrustServerCertificate=True;";
            else
                return $@"Data Source={txtServer.Text.Trim()};Initial Catalog={txtDatabase.Text.Trim()};User ID={txtUser.Text.Trim()};Password={txtPassword.Text};TrustServerCertificate=True;";
        }

        private void BtnTest_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(BuildTempConnectionString()))
                {
                    conn.Open();
                    MessageBox.Show("تم الاتصال بالسيرفر وقاعدة البيانات بنجاح تام! ✔", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("فشل الاتصال! تأكد من اسم السيرفر وأن قاعدة البيانات موجودة.\nالخطأ: " + ex.Message, "خطأ في الاتصال", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var cfg = new ServerConfig
                {
                    ServerName = txtServer.Text.Trim(),
                    DatabaseName = txtDatabase.Text.Trim(),
                    UseWindowsAuth = rbWindowsAuth.Checked,
                    SqlUser = txtUser.Text.Trim(),
                    SqlPassword = txtPassword.Text
                };

                ServerConfig.Save(cfg);
                MessageBox.Show("تم حفظ إعدادات الاتصال بنجاح!", "تم الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}