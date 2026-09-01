namespace POS_System
{
    partial class FrmAlerts
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.tcAlerts = new System.Windows.Forms.TabControl();
            this.tabExpiry = new System.Windows.Forms.TabPage();
            this.dgvExpiry = new System.Windows.Forms.DataGridView();
            this.pnlExpiryTop = new System.Windows.Forms.Panel();
            this.btnFilterExpiry = new System.Windows.Forms.Button();
            this.cmbExpiryPeriod = new System.Windows.Forms.ComboBox();
            this.lblExpiryPeriod = new System.Windows.Forms.Label();
            this.tabShortage = new System.Windows.Forms.TabPage();
            this.dgvShortage = new System.Windows.Forms.DataGridView();
            this.pnlShortageTop = new System.Windows.Forms.Panel();
            this.btnFilterShortage = new System.Windows.Forms.Button();
            this.nudMinQty = new System.Windows.Forms.NumericUpDown();
            this.lblMinQty = new System.Windows.Forms.Label();
            this.tcAlerts.SuspendLayout();
            this.tabExpiry.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvExpiry)).BeginInit();
            this.pnlExpiryTop.SuspendLayout();
            this.tabShortage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvShortage)).BeginInit();
            this.pnlShortageTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinQty)).BeginInit();
            this.SuspendLayout();
            // 
            // tcAlerts
            // 
            this.tcAlerts.Controls.Add(this.tabExpiry);
            this.tcAlerts.Controls.Add(this.tabShortage);
            this.tcAlerts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcAlerts.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.tcAlerts.Location = new System.Drawing.Point(0, 0);
            this.tcAlerts.Name = "tcAlerts";
            this.tcAlerts.RightToLeftLayout = true;
            this.tcAlerts.SelectedIndex = 0;
            this.tcAlerts.Size = new System.Drawing.Size(950, 600);
            this.tcAlerts.TabIndex = 0;
            // 
            // tabExpiry
            // 
            this.tabExpiry.Controls.Add(this.dgvExpiry);
            this.tabExpiry.Controls.Add(this.pnlExpiryTop);
            this.tabExpiry.Location = new System.Drawing.Point(4, 34);
            this.tabExpiry.Name = "tabExpiry";
            this.tabExpiry.Padding = new System.Windows.Forms.Padding(3);
            this.tabExpiry.Size = new System.Drawing.Size(942, 562);
            this.tabExpiry.TabIndex = 0;
            this.tabExpiry.Text = "⏳ تنبيهات انتهاء الصلاحية";
            this.tabExpiry.UseVisualStyleBackColor = true;
            // 
            // dgvExpiry
            // 
            this.dgvExpiry.AllowUserToAddRows = false;
            this.dgvExpiry.AllowUserToDeleteRows = false;
            this.dgvExpiry.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvExpiry.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvExpiry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvExpiry.Location = new System.Drawing.Point(3, 63);
            this.dgvExpiry.Name = "dgvExpiry";
            this.dgvExpiry.ReadOnly = true;
            this.dgvExpiry.RowHeadersWidth = 51;
            this.dgvExpiry.RowTemplate.Height = 30;
            this.dgvExpiry.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvExpiry.Size = new System.Drawing.Size(936, 496);
            this.dgvExpiry.TabIndex = 1;
            // 
            // pnlExpiryTop
            // 
            this.pnlExpiryTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.pnlExpiryTop.Controls.Add(this.btnFilterExpiry);
            this.pnlExpiryTop.Controls.Add(this.cmbExpiryPeriod);
            this.pnlExpiryTop.Controls.Add(this.lblExpiryPeriod);
            this.pnlExpiryTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlExpiryTop.Location = new System.Drawing.Point(3, 3);
            this.pnlExpiryTop.Name = "pnlExpiryTop";
            this.pnlExpiryTop.Size = new System.Drawing.Size(936, 60);
            this.pnlExpiryTop.TabIndex = 0;
            // 
            // btnFilterExpiry
            // 
            this.btnFilterExpiry.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFilterExpiry.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnFilterExpiry.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFilterExpiry.ForeColor = System.Drawing.Color.White;
            this.btnFilterExpiry.Location = new System.Drawing.Point(440, 12);
            this.btnFilterExpiry.Name = "btnFilterExpiry";
            this.btnFilterExpiry.Size = new System.Drawing.Size(120, 36);
            this.btnFilterExpiry.TabIndex = 2;
            this.btnFilterExpiry.Text = "عرض التنبيهات";
            this.btnFilterExpiry.UseVisualStyleBackColor = false;
            this.btnFilterExpiry.Click += new System.EventHandler(this.btnFilterExpiry_Click);
            // 
            // cmbExpiryPeriod
            // 
            this.cmbExpiryPeriod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbExpiryPeriod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbExpiryPeriod.FormattingEnabled = true;
            this.cmbExpiryPeriod.Items.AddRange(new object[] {
            "الأدوية المنتهية بالفعل",
            "تنتهي خلال شهر (30 يوم)",
            "تنتهي خلال 3 أشهر (90 يوم)",
            "تنتهي خلال 6 أشهر (180 يوم)"});
            this.cmbExpiryPeriod.Location = new System.Drawing.Point(580, 14);
            this.cmbExpiryPeriod.Name = "cmbExpiryPeriod";
            this.cmbExpiryPeriod.Size = new System.Drawing.Size(220, 33);
            this.cmbExpiryPeriod.TabIndex = 1;
            // 
            // lblExpiryPeriod
            // 
            this.lblExpiryPeriod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblExpiryPeriod.AutoSize = true;
            this.lblExpiryPeriod.Location = new System.Drawing.Point(810, 18);
            this.lblExpiryPeriod.Name = "lblExpiryPeriod";
            this.lblExpiryPeriod.Size = new System.Drawing.Size(117, 25);
            this.lblExpiryPeriod.TabIndex = 0;
            this.lblExpiryPeriod.Text = "فترة الصلاحية:";
            // 
            // tabShortage
            // 
            this.tabShortage.Controls.Add(this.dgvShortage);
            this.tabShortage.Controls.Add(this.pnlShortageTop);
            this.tabShortage.Location = new System.Drawing.Point(4, 34);
            this.tabShortage.Name = "tabShortage";
            this.tabShortage.Padding = new System.Windows.Forms.Padding(3);
            this.tabShortage.Size = new System.Drawing.Size(942, 562);
            this.tabShortage.TabIndex = 1;
            this.tabShortage.Text = "📦 نواقص الأدوية (حد الطلب)";
            this.tabShortage.UseVisualStyleBackColor = true;
            // 
            // dgvShortage
            // 
            this.dgvShortage.AllowUserToAddRows = false;
            this.dgvShortage.AllowUserToDeleteRows = false;
            this.dgvShortage.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvShortage.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvShortage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvShortage.Location = new System.Drawing.Point(3, 63);
            this.dgvShortage.Name = "dgvShortage";
            this.dgvShortage.ReadOnly = true;
            this.dgvShortage.RowHeadersWidth = 51;
            this.dgvShortage.RowTemplate.Height = 30;
            this.dgvShortage.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvShortage.Size = new System.Drawing.Size(936, 496);
            this.dgvShortage.TabIndex = 1;
            // 
            // pnlShortageTop
            // 
            this.pnlShortageTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.pnlShortageTop.Controls.Add(this.btnFilterShortage);
            this.pnlShortageTop.Controls.Add(this.nudMinQty);
            this.pnlShortageTop.Controls.Add(this.lblMinQty);
            this.pnlShortageTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlShortageTop.Location = new System.Drawing.Point(3, 3);
            this.pnlShortageTop.Name = "pnlShortageTop";
            this.pnlShortageTop.Size = new System.Drawing.Size(936, 60);
            this.pnlShortageTop.TabIndex = 0;
            // 
            // btnFilterShortage
            // 
            this.btnFilterShortage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFilterShortage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(126)))), ((int)(((byte)(34)))));
            this.btnFilterShortage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFilterShortage.ForeColor = System.Drawing.Color.White;
            this.btnFilterShortage.Location = new System.Drawing.Point(510, 12);
            this.btnFilterShortage.Name = "btnFilterShortage";
            this.btnFilterShortage.Size = new System.Drawing.Size(120, 36);
            this.btnFilterShortage.TabIndex = 2;
            this.btnFilterShortage.Text = "عرض النواقص";
            this.btnFilterShortage.UseVisualStyleBackColor = false;
            this.btnFilterShortage.Click += new System.EventHandler(this.btnFilterShortage_Click);
            // 
            // nudMinQty
            // 
            this.nudMinQty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nudMinQty.Location = new System.Drawing.Point(650, 14);
            this.nudMinQty.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudMinQty.Name = "nudMinQty";
            this.nudMinQty.Size = new System.Drawing.Size(100, 32);
            this.nudMinQty.TabIndex = 1;
            this.nudMinQty.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // lblMinQty
            // 
            this.lblMinQty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMinQty.AutoSize = true;
            this.lblMinQty.Location = new System.Drawing.Point(760, 18);
            this.lblMinQty.Name = "lblMinQty";
            this.lblMinQty.Size = new System.Drawing.Size(167, 25);
            this.lblMinQty.TabIndex = 0;
            this.lblMinQty.Text = "الكميات الأقل من أو تساوي:";
            // 
            // FrmAlerts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(950, 600);
            this.Controls.Add(this.tcAlerts);
            this.Name = "FrmAlerts";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "تنبيهات الأدوية والنواقص";
            this.Load += new System.EventHandler(this.FrmAlerts_Load);
            this.tcAlerts.ResumeLayout(false);
            this.tabExpiry.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvExpiry)).EndInit();
            this.pnlExpiryTop.ResumeLayout(false);
            this.pnlExpiryTop.PerformLayout();
            this.tabShortage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvShortage)).EndInit();
            this.pnlShortageTop.ResumeLayout(false);
            this.pnlShortageTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinQty)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tcAlerts;
        private System.Windows.Forms.TabPage tabExpiry;
        private System.Windows.Forms.TabPage tabShortage;
        private System.Windows.Forms.Panel pnlExpiryTop;
        private System.Windows.Forms.ComboBox cmbExpiryPeriod;
        private System.Windows.Forms.Label lblExpiryPeriod;
        private System.Windows.Forms.Button btnFilterExpiry;
        private System.Windows.Forms.DataGridView dgvExpiry;
        private System.Windows.Forms.Panel pnlShortageTop;
        private System.Windows.Forms.NumericUpDown nudMinQty;
        private System.Windows.Forms.Label lblMinQty;
        private System.Windows.Forms.Button btnFilterShortage;
        private System.Windows.Forms.DataGridView dgvShortage;
    }
}