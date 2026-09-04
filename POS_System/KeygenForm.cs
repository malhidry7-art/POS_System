using System;
using System.Drawing;
using System.Windows.Forms;

namespace POS_System
{
    public partial class KeygenForm : Form
    {
        private TextBox txtHwid;
        private NumericUpDown numDays;
        private TextBox txtResultKey;
        private Button btnGenerate;
        private Button btnCopy;

        public KeygenForm()
        {
            InitializeCustomUI();
        }

        private void InitializeCustomUI()
        {
            this.Text = "مولّد تراخيص فارما تك - المطور محمد";
            this.Size = new Size(500, 380);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.RightToLeft = RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.BackColor = Color.FromArgb(245, 247, 250);

            Label lblTitle = new Label
            {
                Text = "لوحة إدارة وتوليد مفاتيح الترخيص",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(41, 128, 185),
                Location = new Point(20, 15),
                AutoSize = true
            };

            Label lblHwid = new Label
            {
                Text = "معرّف جهاز العميل (HWID):",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Location = new Point(20, 60),
                AutoSize = true
            };

            txtHwid = new TextBox
            {
                Location = new Point(20, 85),
                Size = new Size(440, 27),
                Font = new Font("Consolas", 11F, FontStyle.Bold),
                TextAlign = HorizontalAlignment.Center
            };

            Label lblDays = new Label
            {
                Text = "مدة الترخيص (بالأيام):",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Location = new Point(20, 125),
                AutoSize = true
            };

            numDays = new NumericUpDown
            {
                Location = new Point(20, 150),
                Size = new Size(120, 27),
                Minimum = 1,
                Maximum = 3650,
                Value = 3 // القيمة الافتراضية 3 أيام للتجربة
            };

            Label lblHint = new Label
            {
                Text = "(3 أيام تجربة | 365 يوم لسنة | 3650 يوم لمدى الحياة)",
                Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                ForeColor = Color.Gray,
                Location = new Point(150, 153),
                AutoSize = true
            };

            btnGenerate = new Button
            {
                Text = "توليد مفتاح الترخيص 🔑",
                Location = new Point(20, 195),
                Size = new Size(440, 38),
                BackColor = Color.FromArgb(41, 128, 185),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnGenerate.FlatAppearance.BorderSize = 0;
            btnGenerate.Click += BtnGenerate_Click;

            Label lblResult = new Label
            {
                Text = "المفتاح المولد (أرسله للعميل):",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Location = new Point(20, 245),
                AutoSize = true
            };

            txtResultKey = new TextBox
            {
                Location = new Point(130, 270),
                Size = new Size(330, 27),
                ReadOnly = true,
                Font = new Font("Consolas", 9.5F, FontStyle.Bold),
                TextAlign = HorizontalAlignment.Center,
                BackColor = Color.FromArgb(236, 240, 241)
            };

            btnCopy = new Button
            {
                Text = "نسخ المفتاح",
                Location = new Point(20, 268),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(39, 174, 96),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnCopy.FlatAppearance.BorderSize = 0;
            btnCopy.Click += (s, e) =>
            {
                if (!string.IsNullOrEmpty(txtResultKey.Text))
                {
                    Clipboard.SetText(txtResultKey.Text);
                    MessageBox.Show("تم نسخ المفتاح بنجاح!", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblHwid);
            this.Controls.Add(txtHwid);
            this.Controls.Add(lblDays);
            this.Controls.Add(numDays);
            this.Controls.Add(lblHint);
            this.Controls.Add(btnGenerate);
            this.Controls.Add(lblResult);
            this.Controls.Add(txtResultKey);
            this.Controls.Add(btnCopy);
        }

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            string hwid = txtHwid.Text.Trim();
            if (string.IsNullOrEmpty(hwid))
            {
                MessageBox.Show("يرجى إدخال معرّف جهاز العميل (HWID) أولاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int days = (int)numDays.Value;
            DateTime expiry = DateTime.Now.AddDays(days);
            string key = LicenseManager.GenerateLicenseKey(hwid, expiry);
            txtResultKey.Text = key;
        }
    }
}