
CREATE DATABASE IF NOT EXISTS CafePOSDB;
USE CafePOSDB;

CREATE TABLE BanAn (
    MaBan INT AUTO_INCREMENT PRIMARY KEY,
    TenBan VARCHAR(50) NOT NULL,
    TrangThai VARCHAR(20) NOT NULL DEFAULT 'Trống'
);

CREATE TABLE DanhMuc (
    MaDanhMuc INT AUTO_INCREMENT PRIMARY KEY,
    TenDanhMuc VARCHAR(100) NOT NULL
);

CREATE TABLE MonAn (
    MaMon INT AUTO_INCREMENT PRIMARY KEY,
    TenMon VARCHAR(100) NOT NULL,
    MaDanhMuc INT NOT NULL,
    DonGia DECIMAL(18,2) NOT NULL CHECK (DonGia >= 0),
    FOREIGN KEY (MaDanhMuc) REFERENCES DanhMuc(MaDanhMuc)
);

CREATE TABLE HoaDon (
    MaHoaDon INT AUTO_INCREMENT PRIMARY KEY,
    MaBan INT NOT NULL,
    GioVao DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    GioRa DATETIME NULL,
    TongTien DECIMAL(18,2) NOT NULL DEFAULT 0,
    TrangThai BOOLEAN NOT NULL DEFAULT FALSE,
    FOREIGN KEY (MaBan) REFERENCES BanAn(MaBan)
);

CREATE TABLE ChiTietHoaDon (
    MaHoaDon INT NOT NULL,
    MaMon INT NOT NULL,
    SoLuong INT NOT NULL CHECK (SoLuong > 0),
    DonGia DECIMAL(18,2) NOT NULL CHECK (DonGia >= 0),
    PRIMARY KEY (MaHoaDon, MaMon),
    FOREIGN KEY (MaHoaDon) REFERENCES HoaDon(MaHoaDon),
    FOREIGN KEY (MaMon) REFERENCES MonAn(MaMon)
);

INSERT INTO BanAn (TenBan) VALUES
('Bàn 01'), ('Bàn 02'), ('Bàn 03'),
('Bàn 04'), ('Bàn 05'), ('Bàn 06'),
('Bàn 07'), ('Bàn 08'), ('Bàn 09'), ('Bàn 10');

INSERT INTO DanhMuc (TenDanhMuc) VALUES
('Cà phê'), ('Trà sữa'), ('Đồ ăn nhẹ');

INSERT INTO MonAn (TenMon, MaDanhMuc, DonGia) VALUES
('Cà phê đen', 1, 20000),
('Cà phê sữa', 1, 25000),
('Bạc xỉu', 1, 30000),
('Trà sữa truyền thống', 2, 35000),
('Bánh ngọt', 3, 25000);

USE cafeposdb;

SELECT * FROM BanAn;
SELECT * FROM DanhMuc;
SELECT * FROM MonAn;
SELECT * FROM HoaDon;
SELECT * FROM ChiTietHoaDon;
