using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace POS_System
{
    public partial class ActivationForm : Form
    {
        private string _reasonMessage;

        // عناصر الواجهة
        private Label lblHeaderTitle;
        private Label lblHeaderSubtitle;
        private Label lblReason;
        private Label lblHwidTitle;
        private TextBox txtHardwareId;
        private Button btnCopyHwid;
        private Label lblKeyTitle;
        private TextBox txtLicenseKey;
        private Button btnActivate;
        private Button btnWhatsApp;
        private Button btnClose;

        public ActivationForm(string reasonMessage)
        {
            _reasonMessage = reasonMessage;
            InitializeModernUI();
        }

        private void InitializeModernUI()
        {
            // إعدادات النافذة الأساسية
            this.Text = "تنشيط نظام فارما تك - Pharma Tech";
            this.Size = new Size(520, 560);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.FromArgb(248, 249, 252);
            this.RightToLeft = RightToLeft.Yes;
            this.RightToLeftLayout = true;

            // لوحة العنوان العلوية (Header Panel)
            Panel pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 100,
                BackColor = Color.FromArgb(26, 82, 118) // أزرق كحلي ملكي راقي
            };

            lblHeaderTitle = new Label
            {
                Text = "نظام فارما تك لإدارة الصيدليات والمبيعات",
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                Size = new Size(460, 30),
                Location = new Point(30, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };

            lblHeaderSubtitle = new Label
            {
                Text = "بوابة التحقق وتفعيل رخصة الاستخدام",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
                ForeColor = Color.FromArgb(214, 234, 248),
                AutoSize = false,
                Size = new Size(460, 25),
                Location = new Point(30, 55),
                TextAlign = ContentAlignment.MiddleCenter
            };

            pnlHeader.Controls.Add(lblHeaderTitle);
            pnlHeader.Controls.Add(lblHeaderSubtitle);

            // لوحة المحتوى الرئيسية (Card Design)
            Panel pnlCard = new Panel
            {
                Location = new Point(25, 115),
                Size = new Size(470, 420),
                BackColor = Color.White
            };
            pnlCard.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(220, 224, 230), 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, pnlCard.Width - 1, pnlCard.Height - 1);
                }
            };

            // سبب ظهور الشاشة (تنبيه)
            lblReason = new Label
            {
                Text = "⚠️ " + _reasonMessage,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(192, 57, 43), // أحمر أنيق للتنبيه
                Location = new Point(20, 15),
                Size = new Size(430, 40),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // عنوان كود الجهاز
            lblHwidTitle = new Label
            {
                Text = "معرّف هذا الجهاز الفريد (Hardware ID):",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                Location = new Point(20, 65),
                AutoSize = true
            };

            // حقل كود الجهاز
            txtHardwareId = new TextBox
            {
                Location = new Point(110, 92),
                Size = new Size(340, 30),
                ReadOnly = true,
                BackColor = Color.FromArgb(240, 243, 244),
                ForeColor = Color.FromArgb(44, 62, 80),
                Font = new Font("Consolas", 11F, FontStyle.Bold),
                TextAlign = HorizontalAlignment.Center,
                BorderStyle = BorderStyle.FixedSingle,
                Text = LicenseManager.GetHardwareId()
            };

            // زر نسخ الكود
            btnCopyHwid = new Button
            {
                Text = "نسخ الكود",
                Location = new Point(20, 91),
                Size = new Size(85, 30),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnCopyHwid.FlatAppearance.BorderSize = 0;
            btnCopyHwid.Click += (s, e) =>
            {
                Clipboard.SetText(txtHardwareId.Text);
                MessageBox.Show("تم نسخ كود الجهاز بنجاح! يمكنك الآن إرساله للمطور لتوليد مفتاح التفعيل.", "تم النسخ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            // عنوان مفتاح التفعيل
            lblKeyTitle = new Label
            {
                Text = "أدخل مفتاح التفعيل والتنشيط هنا:",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                Location = new Point(20, 140),
                AutoSize = true
            };

            // حقل إدخال مفتاح التفعيل
            txtLicenseKey = new TextBox
            {
                Location = new Point(20, 168),
                Size = new Size(430, 32),
                Font = new Font("Consolas", 10F, FontStyle.Regular),
                TextAlign = HorizontalAlignment.Center,
                BorderStyle = BorderStyle.FixedSingle
            };

            // زر تفعيل النظام
            btnActivate = new Button
            {
                Text = "تنشيط وتفعيل النظام الآن ✔",
                Location = new Point(20, 225),
                Size = new Size(430, 42),
                BackColor = Color.FromArgb(39, 174, 96), // أخضر مميز للنجاح
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnActivate.FlatAppearance.BorderSize = 0;
            btnActivate.Click += BtnActivate_Click;

            // زر طلب التفعيل عبر واتساب
            btnWhatsApp = new Button
            {
                Text = "طلب مفتاح تفعيل عبر واتساب (المطور) 💬",
                Location = new Point(20, 280),
                Size = new Size(430, 38),
                BackColor = Color.FromArgb(37, 211, 102), // أخضر واتساب
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnWhatsApp.FlatAppearance.BorderSize = 0;
            btnWhatsApp.Click += (s, e) =>
            {
                string hwid = txtHardwareId.Text.Trim();
                string text = Uri.EscapeDataString($"السلام عليكم ورحمة الله، أرغب في تفعيل نظام فارما تك.\nمعرّف الجهاز (HWID): {hwid}");
                Process.Start(new ProcessStartInfo($"https://wa.me/967776749452?text={text}") { UseShellExecute = true });
            };

            // زر إغلاق البرنامج
            btnClose = new Button
            {
                Text = "إغلاق البرنامج",
                Location = new Point(140, 340),
                Size = new Size(190, 34),
                BackColor = Color.FromArgb(238, 240, 242),
                ForeColor = Color.FromArgb(127, 140, 141),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) =>
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            };

            // إضافة العناصر إلى الكرت
            pnlCard.Controls.Add(lblReason);
            pnlCard.Controls.Add(lblHwidTitle);
            pnlCard.Controls.Add(txtHardwareId);
            pnlCard.Controls.Add(btnCopyHwid);
            pnlCard.Controls.Add(lblKeyTitle);
            pnlCard.Controls.Add(txtLicenseKey);
            pnlCard.Controls.Add(btnActivate);
            pnlCard.Controls.Add(btnWhatsApp);
            pnlCard.Controls.Add(btnClose);

            // إضافة الكل إلى الفورم
            this.Controls.Add(pnlCard);
            this.Controls.Add(pnlHeader);

            // رسم إطار خارجي أنيق للفورم بالكامل
            this.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(189, 195, 199), 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, this.Width - 1, this.Height - 1);
                }
            };
        }

        private void BtnActivate_Click(object sender, EventArgs e)
        {
            string hwid = txtHardwareId.Text.Trim();
            string key = txtLicenseKey.Text.Trim();

            if (string.IsNullOrEmpty(key))
            {
                MessageBox.Show("يرجى لصق أو كتابة مفتاح التفعيل أولاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime expiry;
            if (LicenseManager.ValidateKey(hwid, key, out expiry))
            {
                if (DateTime.Now.Date > expiry.Date)
                {
                    MessageBox.Show("عذراً، هذا المفتاح منتهي الصلاحية بالفعل!", "مفتاح منتهي", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                LicenseManager.SaveLicense(key);
                MessageBox.Show($"تم تفعيل نظام فارما تك بنجاح!\nالترخيص سارٍ حتى تاريخ: {expiry:yyyy-MM-dd}\nنتمنى لكم تجربة موفقة.",
                                "تم التنشيط بنجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("مفتاح التفعيل المدخل غير صحيح أو غير مخصص لهذا الجهاز.\nيرجى مراجعة المطور.", "فشل التفعيل", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
