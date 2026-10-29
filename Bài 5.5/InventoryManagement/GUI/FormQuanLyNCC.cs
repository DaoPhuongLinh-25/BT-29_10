
using System;
using System.Windows.Forms;
using InventoryManagement.BUS;
using InventoryManagement.DTO;

namespace InventoryManagement.GUI
{
    public class FormQuanLyNCC : Form
    {
        NhaCungCapBUS bus = new NhaCungCapBUS();

        DataGridView dgv = new DataGridView();
        TextBox txtMa = new TextBox();
        TextBox txtTen = new TextBox();
        TextBox txtSDT = new TextBox();
        TextBox txtDiaChi = new TextBox();
        TextBox txtTim = new TextBox();

        public FormQuanLyNCC()
        {
            Text = "Quản lý nhà cung cấp";
            Width = 850;
            Height = 520;
            StartPosition = FormStartPosition.CenterScreen;

            TaoGiaoDien();
            TaiDuLieu();
        }

        void TaoGiaoDien()
        {
            Label title = new Label();
            title.Text = "QUẢN LÝ NHÀ CUNG CẤP";
            title.Font = new System.Drawing.Font(
                "Arial", 18, System.Drawing.FontStyle.Bold);
            title.SetBounds(220, 10, 450, 35);
            Controls.Add(title);

            ThemLabel("Mã NCC:", 20, 65);
            ThemLabel("Tên NCC:", 20, 105);
            ThemLabel("Điện thoại:", 20, 145);
            ThemLabel("Địa chỉ:", 20, 185);

            txtMa.SetBounds(120, 65, 180, 25);
            txtMa.ReadOnly = true;
            txtTen.SetBounds(120, 105, 180, 25);
            txtSDT.SetBounds(120, 145, 180, 25);
            txtDiaChi.SetBounds(120, 185, 180, 25);

            Controls.AddRange(new Control[]
                { txtMa, txtTen, txtSDT, txtDiaChi });

            Button btnThem = TaoNut("Thêm", 340, 65);
            Button btnSua = TaoNut("Sửa", 430, 65);
            Button btnXoa = TaoNut("Xóa", 520, 65);
            Button btnMoi = TaoNut("Làm mới", 610, 65);

            btnThem.Click += (s, e) => Them();
            btnSua.Click += (s, e) => Sua();
            btnXoa.Click += (s, e) => Xoa();
            btnMoi.Click += (s, e) => LamMoi();

            ThemLabel("Tìm kiếm:", 20, 235);
            txtTim.SetBounds(120, 235, 220, 25);
            Controls.Add(txtTim);

            Button btnTim = TaoNut("Tìm kiếm", 360, 230);
            btnTim.Click += (s, e) =>
            {
                dgv.DataSource = bus.TimKiem(txtTim.Text.Trim());
            };

            dgv.SetBounds(20, 280, 790, 190);
            dgv.ReadOnly = true;
            dgv.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgv.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            dgv.CellClick += ChonDong;

            Controls.AddRange(new Control[]
                { btnThem, btnSua, btnXoa, btnMoi, btnTim, dgv });
        }

        void ThemLabel(string text, int x, int y)
        {
            Label lb = new Label();
            lb.Text = text;
            lb.SetBounds(x, y, 100, 25);
            Controls.Add(lb);
        }

        Button TaoNut(string text, int x, int y)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.SetBounds(x, y, 80, 30);
            return btn;
        }

        void TaiDuLieu()
        {
            dgv.DataSource = bus.LayDanhSach();
        }

        NhaCungCapDTO LayNCC()
        {
            return new NhaCungCapDTO
            {
                MaNCC = int.TryParse(txtMa.Text, out int ma) ? ma : 0,
                TenNCC = txtTen.Text.Trim(),
                DienThoai = txtSDT.Text.Trim(),
                DiaChi = txtDiaChi.Text.Trim()
            };
        }

        void Them()
        {
            try
            {
                if (bus.Them(LayNCC()))
                {
                    MessageBox.Show("Thêm nhà cung cấp thành công.");
                    TaiDuLieu();
                    LamMoi();
                }
                else
                    MessageBox.Show("Vui lòng nhập tên nhà cung cấp.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm nhà cung cấp: " + ex.Message);
            }
        }

        void Sua()
        {
            try
            {
                if (bus.Sua(LayNCC()))
                {
                    MessageBox.Show("Sửa thành công.");
                    TaiDuLieu();
                }
                else
                    MessageBox.Show("Hãy chọn nhà cung cấp cần sửa.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi sửa: " + ex.Message);
            }
        }

        void Xoa()
        {
            if (!int.TryParse(txtMa.Text, out int ma))
            {
                MessageBox.Show("Hãy chọn nhà cung cấp cần xóa.");
                return;
            }

            if (MessageBox.Show("Bạn chắc chắn muốn xóa?",
                "Xác nhận", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            try
            {
                if (bus.Xoa(ma))
                {
                    MessageBox.Show("Xóa thành công.");
                    LamMoi();
                }
                else
                    MessageBox.Show("Không tìm thấy nhà cung cấp.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể xóa: " + ex.Message);
            }
        }

        void LamMoi()
        {
            txtMa.Clear();
            txtTen.Clear();
            txtSDT.Clear();
            txtDiaChi.Clear();
            txtTim.Clear();
            TaiDuLieu();
        }

        void ChonDong(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            txtMa.Text = dgv.Rows[e.RowIndex]
                .Cells["MaNCC"].Value?.ToString() ?? "";
            txtTen.Text = dgv.Rows[e.RowIndex]
                .Cells["TenNCC"].Value?.ToString() ?? "";
            txtSDT.Text = dgv.Rows[e.RowIndex]
                .Cells["DienThoai"].Value?.ToString() ?? "";
            txtDiaChi.Text = dgv.Rows[e.RowIndex]
                .Cells["DiaChi"].Value?.ToString() ?? "";
        }
    }
}
