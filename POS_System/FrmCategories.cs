using System;
using System.Data;
using System.Windows.Forms;
using POS_System.BLL;

namespace POS_System
{
    public partial class FrmCategories : Form
    {
        private readonly CategoriesManager _manager = new CategoriesManager();
        private int _selectedCategoryId = 0; // للاحتفاظ برقم الفئة المحددة

        public FrmCategories()
        {
            InitializeComponent();
        }

        private void FrmCategories_Load(object sender, EventArgs e)
        {
            LoadCategories();
        }

        // دالة تحميل وتحديث البيانات في الجدول
        private void LoadCategories()
        {
            try
            {
                DataTable dt = _manager.GetAllCategories();
                dgvCategories.DataSource = dt;

                // تحسين تسمية الأعمدة في الجدول
                if (dgvCategories.Columns["CategoryID"] != null)
                    dgvCategories.Columns["CategoryID"].HeaderText = "رقم الفئة";
                if (dgvCategories.Columns["CategoryName"] != null)
                    dgvCategories.Columns["CategoryName"].HeaderText = "اسم الفئة";
                if (dgvCategories.Columns["Description"] != null)
                    dgvCategories.Columns["Description"].HeaderText = "الوصف";
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء جلب البيانات: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // دالة تنظيف الحقول
        private void ClearInputs()
        {
            _selectedCategoryId = 0;
            txtCategoryName.Clear();
            txtDescription.Clear();
            txtCategoryName.Focus();
        }

        // 1. زر إضافة (Add)
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCategoryName.Text))
            {
                MessageBox.Show("يرجى إدخال اسم الفئة أولاً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int result = _manager.AddCategory(txtCategoryName.Text.Trim(), txtDescription.Text.Trim());
                if (result > 0)
                {
                    MessageBox.Show("تمت إضافة الفئة بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCategories();
                    ClearInputs();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("فشلت عملية الإضافة: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvCategories_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvCategories.Rows[e.RowIndex].Cells[0].Value != null)
            {
                DataGridViewRow row = dgvCategories.Rows[e.RowIndex];

                // جلب رقم المعرف من العمود الأول
                _selectedCategoryId = Convert.ToInt32(row.Cells[0].Value);

                // جلب الاسم والوصف من العمودين الثاني والثالث
                txtCategoryName.Text = row.Cells[1].Value?.ToString();
                txtDescription.Text = row.Cells[2].Value?.ToString();
            }
        }
        // 3. زر تعديل (Update)
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (_selectedCategoryId == 0)
            {
                MessageBox.Show("يرجى تحديد فئة من الجدول لتعديلها", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCategoryName.Text))
            {
                MessageBox.Show("لا يمكن ترك اسم الفئة فارغاً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int result = _manager.UpdateCategory(_selectedCategoryId, txtCategoryName.Text.Trim(), txtDescription.Text.Trim());
                if (result > 0)
                {
                    MessageBox.Show("تم تعديل بيانات الفئة بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCategories();
                    ClearInputs();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("فشلت عملية التعديل: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 4. زر حذف (Delete)
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedCategoryId == 0)
            {
                MessageBox.Show("يرجى تحديد فئة من الجدول لحذفها", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show("هل أنت متأكد من رغبتك في حذف هذه الفئة نهائياً؟", "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    int result = _manager.DeleteCategory(_selectedCategoryId);
                    if (result > 0)
                    {
                        MessageBox.Show("تم حذف الفئة بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadCategories();
                        ClearInputs();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("تعذر الحذف (قد تكون الفئة مرتبطة بأدوية مسجلة مسبقاً): " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // 5. زر جديد / مسح الحقول (New / Clear)
        private void btnNew_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }
    }
}