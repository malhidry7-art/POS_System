using System;
using System.Data;
using System.Windows.Forms;
using POS_System.BLL;

namespace POS_System
{
    public partial class FrmProducts : Form
    {
        private readonly ProductsManager _productsManager = new ProductsManager();
        private readonly CategoriesManager _categoriesManager = new CategoriesManager();
        private int _selectedProductId = 0;

        public FrmProducts()
        {
            InitializeComponent();
        }

        private void FrmProducts_Load(object sender, EventArgs e)
        {
            LoadCategoriesComboBox();
            LoadProducts();
        }

        private void LoadCategoriesComboBox()
        {
            try
            {
                DataTable dt = _categoriesManager.GetAllCategories();
                cmbCategories.DataSource = dt;
                cmbCategories.DisplayMember = "CategoryName";
                cmbCategories.ValueMember = "CategoryID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("فشل تحميل التصنيفات: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadProducts()
        {
            try
            {
                DataTable dt = _productsManager.GetAllProducts();
                dgvProducts.DataSource = dt;

                if (dgvProducts.Columns["ProductID"] != null) dgvProducts.Columns["ProductID"].HeaderText = "رقم الدواء";
                if (dgvProducts.Columns["Barcode"] != null) dgvProducts.Columns["Barcode"].HeaderText = "الباركود";
                if (dgvProducts.Columns["ProductName"] != null) dgvProducts.Columns["ProductName"].HeaderText = "الاسم التجاري";
                if (dgvProducts.Columns["GenericName"] != null) dgvProducts.Columns["GenericName"].HeaderText = "الاسم العلمي";
                if (dgvProducts.Columns["CategoryName"] != null) dgvProducts.Columns["CategoryName"].HeaderText = "التصنيف";
                if (dgvProducts.Columns["BuyPrice"] != null) dgvProducts.Columns["BuyPrice"].HeaderText = "سعر الشراء";
                if (dgvProducts.Columns["SellPrice"] != null) dgvProducts.Columns["SellPrice"].HeaderText = "سعر البيع";
                if (dgvProducts.Columns["Quantity"] != null) dgvProducts.Columns["Quantity"].HeaderText = "الكمية";
                if (dgvProducts.Columns["ExpiryDate"] != null) dgvProducts.Columns["ExpiryDate"].HeaderText = "تاريخ الانتهاء";

                if (dgvProducts.Columns["CategoryID"] != null)
                    dgvProducts.Columns["CategoryID"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("فشل تحميل الأدوية: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearInputs()
        {
            _selectedProductId = 0;
            txtBarcode.Clear();
            txtProductName.Clear();
            txtGenericName.Clear();
            txtBuyPrice.Text = "0.00";
            txtSellPrice.Text = "0.00";
            txtQuantity.Text = "0";
            dtpExpiryDate.Value = DateTime.Now;
            if (cmbCategories.Items.Count > 0) cmbCategories.SelectedIndex = 0;
            txtBarcode.Focus();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                MessageBox.Show("يرجى إدخال اسم الدواء التجاري أولاً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbCategories.SelectedValue == null)
            {
                MessageBox.Show("يرجى اختيار التصنيف", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                decimal buyPrice = decimal.TryParse(txtBuyPrice.Text, out decimal bp) ? bp : 0;
                decimal sellPrice = decimal.TryParse(txtSellPrice.Text, out decimal sp) ? sp : 0;
                int qty = int.TryParse(txtQuantity.Text, out int q) ? q : 0;
                int catId = Convert.ToInt32(cmbCategories.SelectedValue);

                int result = _productsManager.AddProduct(
                    txtBarcode.Text.Trim(),
                    txtProductName.Text.Trim(),
                    txtGenericName.Text.Trim(),
                    catId,
                    buyPrice,
                    sellPrice,
                    qty,
                    dtpExpiryDate.Value
                );

                if (result > 0)
                {
                    MessageBox.Show("تمت إضافة الدواء بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProducts();
                    ClearInputs();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("فشلت عملية الإضافة: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvProducts.Rows[e.RowIndex].Cells["ProductID"].Value != null)
            {
                DataGridViewRow row = dgvProducts.Rows[e.RowIndex];
                _selectedProductId = Convert.ToInt32(row.Cells["ProductID"].Value);
                txtBarcode.Text = row.Cells["Barcode"].Value?.ToString();
                txtProductName.Text = row.Cells["ProductName"].Value?.ToString();
                txtGenericName.Text = row.Cells["GenericName"].Value?.ToString();
                txtBuyPrice.Text = row.Cells["BuyPrice"].Value?.ToString();
                txtSellPrice.Text = row.Cells["SellPrice"].Value?.ToString();
                txtQuantity.Text = row.Cells["Quantity"].Value?.ToString();

                if (row.Cells["ExpiryDate"].Value != DBNull.Value && row.Cells["ExpiryDate"].Value != null)
                {
                    dtpExpiryDate.Value = Convert.ToDateTime(row.Cells["ExpiryDate"].Value);
                }

                if (row.Cells["CategoryID"].Value != DBNull.Value)
                {
                    cmbCategories.SelectedValue = row.Cells["CategoryID"].Value;
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (_selectedProductId == 0)
            {
                MessageBox.Show("يرجى اختيار دواء من الجدول لتعديله", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                decimal buyPrice = decimal.TryParse(txtBuyPrice.Text, out decimal bp) ? bp : 0;
                decimal sellPrice = decimal.TryParse(txtSellPrice.Text, out decimal sp) ? sp : 0;
                int qty = int.TryParse(txtQuantity.Text, out int q) ? q : 0;
                int catId = Convert.ToInt32(cmbCategories.SelectedValue);

                int result = _productsManager.UpdateProduct(
                    _selectedProductId,
                    txtBarcode.Text.Trim(),
                    txtProductName.Text.Trim(),
                    txtGenericName.Text.Trim(),
                    catId,
                    buyPrice,
                    sellPrice,
                    qty,
                    dtpExpiryDate.Value
                );

                if (result > 0)
                {
                    MessageBox.Show("تم تعديل بيانات الدواء بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProducts();
                    ClearInputs();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("فشلت عملية التعديل: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedProductId == 0)
            {
                MessageBox.Show("يرجى اختيار دواء من الجدول لحذفه", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show("هل أنت متأكد من حذف هذا الدواء؟", "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    int result = _productsManager.DeleteProduct(_selectedProductId);
                    if (result > 0)
                    {
                        MessageBox.Show("تم حذف الدواء بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadProducts();
                        ClearInputs();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("فشلت عملية الحذف: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }
    }
}