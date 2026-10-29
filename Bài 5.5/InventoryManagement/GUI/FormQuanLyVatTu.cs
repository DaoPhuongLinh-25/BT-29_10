
using System;
using System.Windows.Forms;
using InventoryManagement.BUS;
using InventoryManagement.DTO;

namespace InventoryManagement.GUI
{
    public class FormQuanLyVatTu : Form
    {
        VatTuBUS bus = new VatTuBUS();

        DataGridView dgv = new DataGridView();
        TextBox txtMa = new TextBox();
        TextBox txtTen = new TextBox();
        TextBox txtDVT = new TextBox();
        TextBox txtTon = new TextBox();
        TextBox txtTim = new TextBox();

        public FormQuanLyVatTu()
        {
            Text = "Quản lý vật tư";
            Width = 850;
            Height = 550;
            StartPosition = FormStartPosition.CenterScreen;

            TaoGiaoDien();
            TaiDuLieu();
        }

        void TaoGiaoDien()
        {
            Label title = new Label();
            title.Text = "QUẢN LÝ VẬT TƯ";
            title.Font = new System.Drawing.Font(
                "Arial", 18, System.Drawing.FontStyle.Bold);
            title.SetBounds(250, 10, 350, 35);
            Controls.Add(title);

            ThemLabel("Mã vật tư:", 20, 65);
            ThemLabel("Tên vật tư:", 20, 105);
            ThemLabel("Đơn vị tính:", 20, 145);
            ThemLabel("Số lượng tồn:", 20, 185);

            txtMa.SetBounds(130, 65, 180, 25);
            txtTen.SetBounds(130, 105, 180, 25);
            txtDVT.SetBounds(130, 145, 180, 25);
            txtTon.SetBounds(130, 185, 180, 25);

            Controls.AddRange(new Control[]
                { txtMa, txtTen, txtDVT, txtTon });

            Button btnThem = TaoNut("Thêm", 340, 65);
            Button btnSua = TaoNut("Sửa", 430, 65);
            Button btnXoa = TaoNut("Xóa", 520, 65);
            Button btnMoi = TaoNut("Làm mới", 610, 65);

            btnThem.Click += (s, e) => Them();
            btnSua.Click += (s, e) => Sua();
            btnXoa.Click += (s, e) => Xoa();
            btnMoi.Click += (s, e) => LamMoi();

            ThemLabel("Tìm mã hoặc tên:", 20, 235);
            txtTim.SetBounds(150, 235, 220, 25);
            Controls.Add(txtTim);

            Button btnTim = TaoNut("Tìm kiếm", 390, 230);
            btnTim.Click += (s, e) =>
            {
                dgv.DataSource = bus.TimKiem(txtTim.Text.Trim());
            };

            dgv.SetBounds(20, 280, 790, 220);
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgv.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            dgv.CellClick += ChonDong;

            Controls.AddRange(new Control[] { btnThem, btnSua,
                btnXoa, btnMoi, btnTim, dgv });
        }

        void ThemLabel(string text, int x, int y)
        {
            Label lb = new Label();
            lb.Text = text;
            lb.SetBounds(x, y, 110, 25);
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

        VatTuDTO LayVatTu()
        {
            decimal ton;

            if (!decimal.TryParse(txtTon.Text, out ton) || ton < 0)
                throw new Exception("Số lượng tồn phải là số không âm.");

            return new VatTuDTO
            {
                MaVatTu = txtMa.Text.Trim(),
                TenVatTu = txtTen.Text.Trim(),
                DVT = txtDVT.Text.Trim(),
                SoLuongTon = ton
            };
        }

        void Them()
        {
            try
            {
                if (bus.Them(LayVatTu()))
                {
                    MessageBox.Show("Thêm vật tư thành công.");
                    TaiDuLieu();
                }
                else
                    MessageBox.Show("Vui lòng nhập mã và tên vật tư hợp lệ.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm vật tư: " + ex.Message);
            }
        }

        void Sua()
        {
            try
            {
                if (bus.Sua(LayVatTu()))
                {
                    MessageBox.Show("Sửa vật tư thành công.");
                    TaiDuLieu();
                }
                else
                    MessageBox.Show("Không tìm thấy vật tư hoặc dữ liệu không hợp lệ.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi sửa vật tư: " + ex.Message);
            }
        }

        void Xoa()
        {
            if (txtMa.Text.Trim() == "")
            {
                MessageBox.Show("Hãy chọn vật tư cần xóa.");
                return;
            }

            if (MessageBox.Show("Bạn chắc chắn muốn xóa?",
                "Xác nhận", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            try
            {
                if (bus.Xoa(txtMa.Text.Trim()))
                {
                    MessageBox.Show("Xóa thành công.");
                    LamMoi();
                }
                else
                    MessageBox.Show("Không tìm thấy vật tư.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể xóa vật tư: " + ex.Message);
            }
        }

        void LamMoi()
        {
            txtMa.Clear();
            txtTen.Clear();
            txtDVT.Clear();
            txtTon.Clear();
            txtTim.Clear();
            TaiDuLieu();
        }

        void ChonDong(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            txtMa.Text = dgv.Rows[e.RowIndex]
                .Cells["MaVatTu"].Value?.ToString() ?? "";
            txtTen.Text = dgv.Rows[e.RowIndex]
                .Cells["TenVatTu"].Value?.ToString() ?? "";
            txtDVT.Text = dgv.Rows[e.RowIndex]
                .Cells["DVT"].Value?.ToString() ?? "";
            txtTon.Text = dgv.Rows[e.RowIndex]
                .Cells["SoLuongTon"].Value?.ToString() ?? "";
        }
    }
}
