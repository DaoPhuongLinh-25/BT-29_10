
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

        List<ChiTietPhieuKhoDTO> chiTiet = new List<ChiTietPhieuKhoDTO>();
        DataTable bangChiTiet = new DataTable();

        VatTuBUS vatTuBUS = new VatTuBUS();
        NhaCungCapBUS nccBUS = new NhaCungCapBUS();
        PhieuKhoBUS phieuBUS = new PhieuKhoBUS();

        public FormNhapXuatKho()
        {
            Text = "LẬP PHIẾU NHẬP / XUẤT KHO";
            Size = new Size(900, 600);
            StartPosition = FormStartPosition.CenterScreen;

            TaoGiaoDien();
            TaoBangChiTiet();
            TaiDuLieu();

            cboLoai.SelectedIndexChanged += (s, e) =>
            {
                cboNCC.Enabled = cboLoai.Text == "NHAP";
            };
        }

        void TaoGiaoDien()
        {
            ThemLabel("Loại phiếu:", 20, 20);
            cboLoai.SetBounds(130, 20, 180, 28);
            cboLoai.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLoai.Items.AddRange(new object[] { "NHAP", "XUAT" });
            cboLoai.SelectedIndex = 0;
            Controls.Add(cboLoai);

            ThemLabel("Nhà cung cấp:", 350, 20);
            cboNCC.SetBounds(470, 20, 250, 28);
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
            numSoLuong.Value = 1;
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
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
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

        void ThemLabel(string noiDung, int x, int y)
        {
            Label lb = new Label();
            lb.Text = noiDung;
            lb.SetBounds(x, y, 110, 28);
            lb.TextAlign = ContentAlignment.MiddleLeft;
            Controls.Add(lb);
        }

        void TaoBangChiTiet()
        {
            bangChiTiet.Columns.Add("MaVatTu");
            bangChiTiet.Columns.Add("TenVatTu");
            bangChiTiet.Columns.Add("DVT");
            bangChiTiet.Columns.Add("SoLuong");
            bangChiTiet.Columns.Add("DonGia");
            bangChiTiet.Columns.Add("ThanhTien");
            dgv.DataSource = bangChiTiet;
        }

        void TaiDuLieu()
        {
            try
            {
                cboVatTu.DataSource = vatTuBUS.LayDanhSach();
                cboVatTu.DisplayMember = "TenVatTu";
                cboVatTu.ValueMember = "MaVatTu";

                cboNCC.DataSource = nccBUS.LayDanhSach();
                cboNCC.DisplayMember = "TenNCC";
                cboNCC.ValueMember = "MaNCC";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không tải được dữ liệu: " + ex.Message);
            }
        }

        void ThemVatTu()
        {
            if (cboVatTu.SelectedItem == null)
            {
                MessageBox.Show("Chưa có vật tư để chọn.");
                return;
            }

            string ma = cboVatTu.SelectedValue.ToString();

            foreach (ChiTietPhieuKhoDTO ct in chiTiet)
            {
                if (ct.MaVatTu == ma)
                {
                    MessageBox.Show("Vật tư này đã có trong phiếu.");
                    return;
                }
            }

            DataRowView dong = cboVatTu.SelectedItem as DataRowView;
            if (dong == null) return;

            ChiTietPhieuKhoDTO ctMoi = new ChiTietPhieuKhoDTO();
            ctMoi.MaVatTu = ma;
            ctMoi.SoLuong = numSoLuong.Value;
            ctMoi.DonGia = numDonGia.Value;
            chiTiet.Add(ctMoi);

            decimal thanhTien = ctMoi.SoLuong * ctMoi.DonGia;

            bangChiTiet.Rows.Add(
                ma,
                dong["TenVatTu"].ToString(),
                dong["DVT"].ToString(),
                ctMoi.SoLuong,
                ctMoi.DonGia,
                thanhTien
            );
        }

        void XoaDong()
        {
            if (dgv.CurrentRow == null || dgv.CurrentRow.Index < 0)
                return;

            int viTri = dgv.CurrentRow.Index;

            if (viTri < chiTiet.Count)
            {
                chiTiet.RemoveAt(viTri);
                bangChiTiet.Rows.RemoveAt(viTri);
            }
        }

        void LuuPhieu()
        {
            if (chiTiet.Count == 0)
            {
                MessageBox.Show("Hãy thêm ít nhất một vật tư.");
                return;
            }

            try
            {
                string loai = cboLoai.Text;
                int? maNCC = null;

                if (loai == "NHAP")
                {
                    if (cboNCC.SelectedValue == null)
                    {
                        MessageBox.Show("Hãy chọn nhà cung cấp.");
                        return;
                    }

                    maNCC = Convert.ToInt32(cboNCC.SelectedValue);
                }

                int maPhieu = phieuBUS.LuuPhieu(loai, maNCC, chiTiet);

                MessageBox.Show(
                    "Lưu phiếu thành công!\nMã phiếu: " + maPhieu,
                    "Thông báo"
                );

                chiTiet.Clear();
                bangChiTiet.Rows.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể lưu phiếu: " + ex.Message);
            }
        }
    }
}
