
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using InventoryManagement.BUS;
using InventoryManagement.DTO;

namespace InventoryManagement.GUI
{
    public class FormNhapXuatKho : Form
    {
        ComboBox cboLoai = new ComboBox();
        ComboBox cboNCC = new ComboBox();
        ComboBox cboVatTu = new ComboBox();
        NumericUpDown numSoLuong = new NumericUpDown();
        NumericUpDown numDonGia = new NumericUpDown();
        DataGridView dgv = new DataGridView();

        List<ChiTietPhieuKhoDTO> danhSach = new List<ChiTietPhieuKhoDTO>();
        DataTable bang = new DataTable();

        VatTuBUS vatTuBUS = new VatTuBUS();
        NhaCungCapBUS nccBUS = new NhaCungCapBUS();
        PhieuKhoBUS phieuBUS = new PhieuKhoBUS();

        public FormNhapXuatKho()
        {
            Text = "Lập phiếu nhập xuất kho";
            Size = new Size(900, 600);
            StartPosition = FormStartPosition.CenterScreen;

            TaoGiaoDien();
            TaoBang();
            TaiDanhMuc();
        }

        void TaoGiaoDien()
        {
            ThemLabel("Loại phiếu:", 20, 20);
            cboLoai.SetBounds(130, 20, 180, 28);
            cboLoai.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLoai.Items.AddRange(new object[] { "NHAP", "XUAT" });
            cboLoai.SelectedIndex = 0;
            cboLoai.SelectedIndexChanged += (s, e) =>
            {
                cboNCC.Enabled = cboLoai.Text == "NHAP";
            };
            Controls.Add(cboLoai);

            ThemLabel("Nhà cung cấp:", 350, 20);
            cboNCC.SetBounds(470, 20, 260, 28);
            cboNCC.DropDownStyle = ComboBoxStyle.DropDownList;
            Controls.Add(cboNCC);

            ThemLabel("Vật tư:", 20, 70);
            cboVatTu.SetBounds(130, 70, 300, 28);
            cboVatTu.DropDownStyle = ComboBoxStyle.DropDownList;
            Controls.Add(cboVatTu);

            ThemLabel("Số lượng:", 20, 115);
            numSoLuong.SetBounds(130, 115, 120, 28);
            numSoLuong.Minimum = 1;
            numSoLuong.Maximum = 100000000;
            numSoLuong.DecimalPlaces = 2;
            Controls.Add(numSoLuong);

            ThemLabel("Đơn giá:", 300, 115);
            numDonGia.SetBounds(380, 115, 150, 28);
            numDonGia.Minimum = 0;
            numDonGia.Maximum = 1000000000;
            numDonGia.DecimalPlaces = 2;
            Controls.Add(numDonGia);

            Button btnThem = new Button();
            btnThem.Text = "Thêm vật tư";
            btnThem.SetBounds(560, 110, 130, 35);
            btnThem.Click += (s, e) => ThemVatTu();
            Controls.Add(btnThem);

            dgv.SetBounds(20, 165, 840, 300);
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            Controls.Add(dgv);

            Button btnXoa = new Button();
            btnXoa.Text = "Xóa dòng chọn";
            btnXoa.SetBounds(20, 480, 140, 35);
            btnXoa.Click += (s, e) => XoaDong();
            Controls.Add(btnXoa);

            Button btnLuu = new Button();
            btnLuu.Text = "Lưu phiếu";
            btnLuu.SetBounds(590, 480, 120, 35);
            btnLuu.Click += (s, e) => LuuPhieu();
            Controls.Add(btnLuu);

            Button btnDong = new Button();
            btnDong.Text = "Đóng";
            btnDong.SetBounds(730, 480, 120, 35);
            btnDong.Click += (s, e) => Close();
            Controls.Add(btnDong);
        }

        void ThemLabel(string text, int x, int y)
        {
            Label lb = new Label();
            lb.Text = text;
            lb.SetBounds(x, y, 110, 28);
            lb.TextAlign = ContentAlignment.MiddleLeft;
            Controls.Add(lb);
        }

        void TaoBang()
        {
            bang.Columns.Add("Mã vật tư");
            bang.Columns.Add("Tên vật tư");
            bang.Columns.Add("Đơn vị tính");
            bang.Columns.Add("Số lượng");
            bang.Columns.Add("Đơn giá");
            bang.Columns.Add("Thành tiền");
            dgv.DataSource = bang;
        }

        void TaiDanhMuc()
        {
            try
            {
                DataTable dtVatTu = vatTuBUS.LayDanhSach();
                cboVatTu.DataSource = dtVatTu;
                cboVatTu.DisplayMember = "TenVatTu";
                cboVatTu.ValueMember = "MaVatTu";

                DataTable dtNCC = nccBUS.LayDanhSach();
                cboNCC.DataSource = dtNCC;
                cboNCC.DisplayMember = "TenNCC";
                cboNCC.ValueMember = "MaNCC";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không tải được danh mục: " + ex.Message);
            }
        }

        void ThemVatTu()
        {
            if (cboVatTu.SelectedItem == null)
            {
                MessageBox.Show("Chưa có vật tư để chọn.");
                return;
            }

            DataRowView dong = cboVatTu.SelectedItem as DataRowView;
            if (dong == null) return;

            string ma = dong["MaVatTu"].ToString();

            foreach (ChiTietPhieuKhoDTO ct in danhSach)
            {
                if (ct.MaVatTu == ma)
                {
                    MessageBox.Show("Vật tư này đã có trong phiếu.");
                    return;
                }
            }

            ChiTietPhieuKhoDTO chiTiet = new ChiTietPhieuKhoDTO();
            chiTiet.MaVatTu = ma;
            chiTiet.SoLuong = numSoLuong.Value;
            chiTiet.DonGia = numDonGia.Value;
            danhSach.Add(chiTiet);

            decimal thanhTien = chiTiet.SoLuong * chiTiet.DonGia;

            bang.Rows.Add(
                ma,
                dong["TenVatTu"].ToString(),
                dong["DVT"].ToString(),
                chiTiet.SoLuong,
                chiTiet.DonGia,
                thanhTien
            );
        }

        void XoaDong()
        {
            if (dgv.CurrentRow == null || dgv.CurrentRow.Index < 0)
                return;

            int viTri = dgv.CurrentRow.Index;

            if (viTri < danhSach.Count)
            {
                danhSach.RemoveAt(viTri);
                bang.Rows.RemoveAt(viTri);
            }
        }

        void LuuPhieu()
        {
            if (danhSach.Count == 0)
            {
                MessageBox.Show("Hãy thêm ít nhất một vật tư.");
                return;
            }

            try
            {
                int? maNCC = null;

                if (cboLoai.Text == "NHAP")
                {
                    if (cboNCC.SelectedValue == null)
                    {
                        MessageBox.Show("Hãy chọn nhà cung cấp.");
                        return;
                    }

                    maNCC = Convert.ToInt32(cboNCC.SelectedValue);
                }

                int maPhieu = phieuBUS.LuuPhieu(
                    cboLoai.Text,
                    maNCC,
                    danhSach
                );

                MessageBox.Show("Lưu phiếu thành công. Mã phiếu: " + maPhieu);

                danhSach.Clear();
                bang.Rows.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu phiếu: " + ex.Message);
            }
        }
    }
}
