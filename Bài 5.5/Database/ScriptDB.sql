
CREATE DATABASE IF NOT EXISTS InventoryDB
CHARACTER SET utf8mb4
COLLATE utf8mb4_unicode_ci;

USE InventoryDB;

CREATE TABLE NhaCungCap (
    MaNCC INT AUTO_INCREMENT PRIMARY KEY,
    TenNCC VARCHAR(150) NOT NULL,
    DienThoai VARCHAR(15) UNIQUE,
    DiaChi VARCHAR(255)
);

CREATE TABLE VatTu (
    MaVatTu VARCHAR(20) PRIMARY KEY,
    TenVatTu VARCHAR(150) NOT NULL,
    DVT VARCHAR(20),
    SoLuongTon DECIMAL(18,2) NOT NULL DEFAULT 0,
    CHECK (SoLuongTon >= 0)
);

CREATE TABLE PhieuKho (
    MaPhieu INT AUTO_INCREMENT PRIMARY KEY,
    LoaiPhieu VARCHAR(10) NOT NULL,
    NgayLap DATETIME DEFAULT CURRENT_TIMESTAMP,
    MaNCC INT NULL,
    TongTien DECIMAL(18,2) NOT NULL DEFAULT 0,
    CHECK (LoaiPhieu IN ('NHAP', 'XUAT')),
    FOREIGN KEY (MaNCC) REFERENCES NhaCungCap(MaNCC)
);

CREATE TABLE ChiTietPhieuKho (
    MaPhieu INT NOT NULL,
    MaVatTu VARCHAR(20) NOT NULL,
    SoLuong DECIMAL(18,2) NOT NULL,
    DonGia DECIMAL(18,2) NOT NULL DEFAULT 0,
    PRIMARY KEY (MaPhieu, MaVatTu),
    CHECK (SoLuong > 0),
    CHECK (DonGia >= 0),
    FOREIGN KEY (MaPhieu) REFERENCES PhieuKho(MaPhieu),
    FOREIGN KEY (MaVatTu) REFERENCES VatTu(MaVatTu)
);

USE InventoryDB;

INSERT INTO NhaCungCap (TenNCC, DienThoai, DiaChi)
VALUES
('Công ty An Phát', '0901111222', 'Hà Nội'),
('Công ty Minh Long', '0903333444', 'Hải Phòng');

INSERT INTO VatTu (MaVatTu, TenVatTu, DVT, SoLuongTon)
VALUES
('VT01', 'Xi măng', 'Bao', 100),
('VT02', 'Sắt thép', 'Kg', 500),
('VT03', 'Cát xây dựng', 'M3', 20),
('VT04', 'Gạch đỏ', 'Viên', 1000);

SELECT * FROM NhaCungCap;
SELECT * FROM VatTu;
SELECT * FROM PhieuKho;
SELECT * FROM ChiTietPhieuKho;
