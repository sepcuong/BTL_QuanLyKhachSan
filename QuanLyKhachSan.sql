--create database QuanLyKhachSan
use QuanLyKhachSan

CREATE TABLE [user](
	[user_id] [varchar](50) NOT NULL PRIMARY KEY,
	[hoten] [nvarchar](150) NULL,
	[ngaysinh] [date] NULL,
	[diachi] [nvarchar](250) NULL,
	[gioitinh] [nvarchar](30) NULL,
	[email] [varchar](150) NULL,
	[taikhoan] [varchar](30) NULL,
	[matkhau] [varchar](60) NULL,
	[role] [varchar](30) NULL, -- 'Admin', 'LeTan', 'KeToan', 'Khach'
	[image_url] [varchar](300) NULL
);
GO

-- 2. Bảng Loại phòng
CREATE TABLE [loai_phong](
	[ma_loai_phong] [varchar](50) NOT NULL PRIMARY KEY,
	[ten_loai_phong] [nvarchar](150) NOT NULL,
	[gia_mac_dinh] [float] NOT NULL,
	[so_nguoi_chuan] [int] DEFAULT 2,
	[mo_ta] [nvarchar](max) NULL
);
GO

-- 3. Bảng Phòng
CREATE TABLE [phong](
	[ma_phong] [varchar](50) NOT NULL PRIMARY KEY,
	[ma_loai_phong] [varchar](50) NOT NULL,
	[ten_phong] [nvarchar](50) NOT NULL,
	[so_tang] [int] NOT NULL,
	[trang_thai] [nvarchar](50) DEFAULT N'Trống', -- 'Trống', 'Đang sử dụng', 'Đang dọn dẹp'
	CONSTRAINT [FK_phong_loai_phong] FOREIGN KEY([ma_loai_phong]) REFERENCES [loai_phong]([ma_loai_phong])
);
GO

-- 4. Bảng Giá theo mùa / ngày lễ
CREATE TABLE [gia_phong](
	[ma_gia] [varchar](50) NOT NULL PRIMARY KEY,
	[ma_loai_phong] [varchar](50) NOT NULL,
	[ten_chinh_sach] [nvarchar](150) NOT NULL,
	[tu_ngay] [date] NOT NULL,
	[den_ngay] [date] NOT NULL,
	[gia_theo_dem] [float] NOT NULL,
	[gia_theo_gio] [float] NOT NULL,
	CONSTRAINT [FK_gia_loai_phong] FOREIGN KEY([ma_loai_phong]) REFERENCES [loai_phong]([ma_loai_phong])
);
GO

CREATE TABLE [khach_hang](
	[ma_khach_hang] [varchar](50) NOT NULL PRIMARY KEY,
	[ho_ten] [nvarchar](150) NOT NULL,
	[cmnd_cccd] [varchar](20) NULL,
	[so_dien_thoai] [varchar](15) NULL,
	[email] [varchar](150) NULL,
	[dia_chi] [nvarchar](250) NULL
);
GO

CREATE TABLE [dat_phong](
	[ma_dat_phong] [varchar](50) NOT NULL PRIMARY KEY,
	[ma_khach_hang] [varchar](50) NOT NULL,
	[ma_phong] [varchar](50) NOT NULL,
	[ngay_dat] [datetime] DEFAULT GETDATE(),
	[ngay_nhan_du_kien] [datetime] NOT NULL,
	[ngay_tra_du_kien] [datetime] NOT NULL,
	[tien_coc] [float] DEFAULT 0,
	[trang_thai] [varchar](30) DEFAULT 'Pending', -- 'Pending', 'Confirmed', 'Checked-in', 'Checked-out', 'Cancelled'
	[ghi_chu] [nvarchar](max) NULL,
	CONSTRAINT [FK_dat_phong_khach_hang] FOREIGN KEY([ma_khach_hang]) REFERENCES [khach_hang]([ma_khach_hang]),
	CONSTRAINT [FK_dat_phong_phong] FOREIGN KEY([ma_phong]) REFERENCES [phong]([ma_phong])
);
GO

CREATE TABLE [dich_vu](
	[ma_dich_vu] [varchar](50) NOT NULL PRIMARY KEY,
	[ten_dich_vu] [nvarchar](150) NOT NULL,
	[don_gia] [float] NOT NULL,
	[don_vi_tinh] [nvarchar](50) NULL -- (Lon, Gói, Lần,...)
);
GO

CREATE TABLE [chi_tiet_su_dung_dv](
	[ma_chi_tiet_dv] [varchar](50) NOT NULL PRIMARY KEY,
	[ma_dat_phong] [varchar](50) NOT NULL,
	[ma_dich_vu] [varchar](50) NOT NULL,
	[so_luong] [int] NOT NULL,
	[thanh_tien] [float] NOT NULL,
	[ngay_su_dung] [datetime] DEFAULT GETDATE(),
	CONSTRAINT [FK_ct_dv_dat_phong] FOREIGN KEY([ma_dat_phong]) REFERENCES [dat_phong]([ma_dat_phong]),
	CONSTRAINT [FK_ct_dv_dich_vu] FOREIGN KEY([ma_dich_vu]) REFERENCES [dich_vu]([ma_dich_vu])
);
GO

CREATE TABLE [hoa_don_khach_san](
	[ma_hoa_don] [varchar](50) NOT NULL PRIMARY KEY,
	[ma_dat_phong] [varchar](50) NOT NULL,
	[ngay_lap] [datetime] DEFAULT GETDATE(),
	[tong_tien_phong] [float] NOT NULL,
	[tong_tien_dich_vu] [float] DEFAULT 0,
	[thue_vat] [float] DEFAULT 0,
	[tong_thanh_toan] [float] NOT NULL,
	[phuong_thuc_thanh_toan] [nvarchar](50) NULL, -- 'Tiền mặt', 'Chuyển khoản'
	CONSTRAINT [FK_hoa_don_dat_phong] FOREIGN KEY([ma_dat_phong]) REFERENCES [dat_phong]([ma_dat_phong])
);
GO


CREATE OR ALTER PROCEDURE dbo.sp_User_GetAll
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        user_id, hoten, ngaysinh, diachi, gioitinh,
        email, taikhoan, matkhau, role, image_url
    FROM dbo.[user]
    ORDER BY user_id;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_User_GetById
    @User_Id VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        user_id, hoten, ngaysinh, diachi, gioitinh,
        email, taikhoan, matkhau, role, image_url
    FROM dbo.[user]
    WHERE user_id = @User_Id;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_User_Search
    @Keyword NVARCHAR(150)
AS
BEGIN
    SET NOCOUNT ON;

    SET @Keyword = ISNULL(@Keyword, N'');

    SELECT
        user_id, hoten, ngaysinh, diachi, gioitinh,
        email, taikhoan, matkhau, role, image_url
    FROM dbo.[user]
    WHERE hoten LIKE N'%' + @Keyword + N'%'
       OR taikhoan LIKE N'%' + @Keyword + N'%'
       OR email LIKE N'%' + @Keyword + N'%'
       OR user_id LIKE N'%' + @Keyword + N'%'
    ORDER BY user_id;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_User_Create
    @User_Id VARCHAR(50),
    @Hoten NVARCHAR(150) = NULL,
    @Ngaysinh DATE = NULL,
    @Diachi NVARCHAR(250) = NULL,
    @Gioitinh NVARCHAR(30) = NULL,
    @Email VARCHAR(150) = NULL,
    @Taikhoan VARCHAR(30) = NULL,
    @Matkhau VARCHAR(60) = NULL,
    @Role VARCHAR(30) = NULL,
    @Image_Url VARCHAR(300) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM dbo.[user] WHERE user_id = @User_Id)
        THROW 50001, N'Mã người dùng đã tồn tại.', 1;

    IF @Taikhoan IS NOT NULL
       AND EXISTS (SELECT 1 FROM dbo.[user] WHERE taikhoan = @Taikhoan)
        THROW 50002, N'Tài khoản đã tồn tại.', 1;

    INSERT INTO dbo.[user]
    (
        user_id, hoten, ngaysinh, diachi, gioitinh,
        email, taikhoan, matkhau, role, image_url
    )
    VALUES
    (
        @User_Id, @Hoten, @Ngaysinh, @Diachi, @Gioitinh,
        @Email, @Taikhoan, @Matkhau, @Role, @Image_Url
    );
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_User_Update
    @User_Id VARCHAR(50),
    @Hoten NVARCHAR(150) = NULL,
    @Ngaysinh DATE = NULL,
    @Diachi NVARCHAR(250) = NULL,
    @Gioitinh NVARCHAR(30) = NULL,
    @Email VARCHAR(150) = NULL,
    @Taikhoan VARCHAR(30) = NULL,
    @Matkhau VARCHAR(60) = NULL,
    @Role VARCHAR(30) = NULL,
    @Image_Url VARCHAR(300) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.[user] WHERE user_id = @User_Id)
        THROW 50003, N'Không tìm thấy người dùng.', 1;

    IF @Taikhoan IS NOT NULL
       AND EXISTS
       (
           SELECT 1
           FROM dbo.[user]
           WHERE taikhoan = @Taikhoan
             AND user_id <> @User_Id
       )
        THROW 50004, N'Tài khoản đã được sử dụng.', 1;

    UPDATE dbo.[user]
    SET
        hoten = @Hoten,
        ngaysinh = @Ngaysinh,
        diachi = @Diachi,
        gioitinh = @Gioitinh,
        email = @Email,
        taikhoan = @Taikhoan,
        matkhau = @Matkhau,
        role = @Role,
        image_url = @Image_Url
    WHERE user_id = @User_Id;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_User_Delete
    @User_Id VARCHAR(50)
AS
BEGIN

    DELETE FROM dbo.[user]
    WHERE user_id = @User_Id;

	SELECT '' ;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_User_Login
    @Taikhoan VARCHAR(30),
    @Matkhau VARCHAR(60)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        user_id, hoten, ngaysinh, diachi, gioitinh,
        email, taikhoan, matkhau, role, image_url
    FROM dbo.[user]
    WHERE taikhoan = @Taikhoan
      AND matkhau = @Matkhau;
END;
GO



CREATE OR ALTER PROCEDURE dbo.sp_KhachHang_GetAll
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ma_khach_hang, ho_ten, cmnd_cccd,
        so_dien_thoai, email, dia_chi
    FROM dbo.khach_hang
    ORDER BY ma_khach_hang;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_KhachHang_GetById
    @Ma_Khach_Hang VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ma_khach_hang, ho_ten, cmnd_cccd,
        so_dien_thoai, email, dia_chi
    FROM dbo.khach_hang
    WHERE ma_khach_hang = @Ma_Khach_Hang;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_KhachHang_Search
    @Keyword NVARCHAR(250)
AS
BEGIN
    SET NOCOUNT ON;

    SET @Keyword = ISNULL(@Keyword, N'');

    SELECT
        ma_khach_hang, ho_ten, cmnd_cccd,
        so_dien_thoai, email, dia_chi
    FROM dbo.khach_hang
    WHERE ma_khach_hang LIKE '%' + @Keyword + '%'
       OR ho_ten LIKE N'%' + @Keyword + N'%'
       OR cmnd_cccd LIKE '%' + @Keyword + '%'
       OR so_dien_thoai LIKE '%' + @Keyword + '%'
       OR email LIKE '%' + @Keyword + '%'
       OR dia_chi LIKE N'%' + @Keyword + N'%'
    ORDER BY ma_khach_hang;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_KhachHang_Create
    @Ma_Khach_Hang VARCHAR(50),
    @Ho_Ten NVARCHAR(150),
    @Cmnd_Cccd VARCHAR(20) = NULL,
    @So_Dien_Thoai VARCHAR(15) = NULL,
    @Email VARCHAR(150) = NULL,
    @Dia_Chi NVARCHAR(250) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM dbo.khach_hang WHERE ma_khach_hang = @Ma_Khach_Hang)
        THROW 50101, N'Mã khách hàng đã tồn tại.', 1;

    INSERT INTO dbo.khach_hang
    (
        ma_khach_hang, ho_ten, cmnd_cccd,
        so_dien_thoai, email, dia_chi
    )
    VALUES
    (
        @Ma_Khach_Hang, @Ho_Ten, @Cmnd_Cccd,
        @So_Dien_Thoai, @Email, @Dia_Chi
    );
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_KhachHang_Update
    @Ma_Khach_Hang VARCHAR(50),
    @Ho_Ten NVARCHAR(150),
    @Cmnd_Cccd VARCHAR(20) = NULL,
    @So_Dien_Thoai VARCHAR(15) = NULL,
    @Email VARCHAR(150) = NULL,
    @Dia_Chi NVARCHAR(250) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.khach_hang WHERE ma_khach_hang = @Ma_Khach_Hang)
        THROW 50102, N'Không tìm thấy khách hàng.', 1;

    UPDATE dbo.khach_hang
    SET
        ho_ten = @Ho_Ten,
        cmnd_cccd = @Cmnd_Cccd,
        so_dien_thoai = @So_Dien_Thoai,
        email = @Email,
        dia_chi = @Dia_Chi
    WHERE ma_khach_hang = @Ma_Khach_Hang;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_KhachHang_Delete
    @Ma_Khach_Hang VARCHAR(50)
AS
BEGIN

    IF EXISTS
    (
        SELECT 1
        FROM dbo.dat_phong
        WHERE ma_khach_hang = @Ma_Khach_Hang
    )
        THROW 50103, N'Không thể xóa khách hàng vì đã có dữ liệu đặt phòng.', 1;

    DELETE FROM dbo.khach_hang
    WHERE ma_khach_hang = @Ma_Khach_Hang;

	SELECT '';
END;
GO


CREATE OR ALTER PROCEDURE dbo.sp_LoaiPhong_GetAll
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ma_loai_phong, ten_loai_phong, gia_mac_dinh,
        so_nguoi_chuan, mo_ta
    FROM dbo.loai_phong
    ORDER BY ma_loai_phong;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_LoaiPhong_GetById
    @Ma_Loai_Phong VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ma_loai_phong, ten_loai_phong, gia_mac_dinh,
        so_nguoi_chuan, mo_ta
    FROM dbo.loai_phong
    WHERE ma_loai_phong = @Ma_Loai_Phong;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_LoaiPhong_Search
    @Keyword NVARCHAR(150)
AS
BEGIN
    SET NOCOUNT ON;

    SET @Keyword = ISNULL(@Keyword, N'');

    SELECT
        ma_loai_phong, ten_loai_phong, gia_mac_dinh,
        so_nguoi_chuan, mo_ta
    FROM dbo.loai_phong
    WHERE ma_loai_phong LIKE '%'+@Keyword+'%'
       OR ten_loai_phong LIKE N'%'+@Keyword+N'%'
       OR mo_ta LIKE N'%'+@Keyword+N'%'
    ORDER BY ma_loai_phong;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_LoaiPhong_Create
    @Ma_Loai_Phong VARCHAR(50),
    @Ten_Loai_Phong NVARCHAR(150),
    @Gia_Mac_Dinh FLOAT,
    @So_Nguoi_Chuan INT = NULL,
    @Mo_Ta NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM dbo.loai_phong WHERE ma_loai_phong = @Ma_Loai_Phong)
        THROW 50201, N'Mã loại phòng đã tồn tại.', 1;

    INSERT INTO dbo.loai_phong
    (
        ma_loai_phong, ten_loai_phong, gia_mac_dinh,
        so_nguoi_chuan, mo_ta
    )
    VALUES
    (
        @Ma_Loai_Phong, @Ten_Loai_Phong, @Gia_Mac_Dinh,
        @So_Nguoi_Chuan, @Mo_Ta
    );
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_LoaiPhong_Update
    @Ma_Loai_Phong VARCHAR(50),
    @Ten_Loai_Phong NVARCHAR(150),
    @Gia_Mac_Dinh FLOAT,
    @So_Nguoi_Chuan INT = NULL,
    @Mo_Ta NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.loai_phong WHERE ma_loai_phong = @Ma_Loai_Phong)
        THROW 50202, N'Không tìm thấy loại phòng.', 1;

    UPDATE dbo.loai_phong
    SET
        ten_loai_phong = @Ten_Loai_Phong,
        gia_mac_dinh = @Gia_Mac_Dinh,
        so_nguoi_chuan = @So_Nguoi_Chuan,
        mo_ta = @Mo_Ta
    WHERE ma_loai_phong = @Ma_Loai_Phong;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_LoaiPhong_Delete
    @Ma_Loai_Phong VARCHAR(50)
AS
BEGIN

    IF EXISTS (SELECT 1 FROM dbo.phong WHERE ma_loai_phong = @Ma_Loai_Phong)
        THROW 50203, N'Không thể xóa loại phòng vì đang có phòng sử dụng loại này.', 1;

    IF EXISTS (SELECT 1 FROM dbo.gia_phong WHERE ma_loai_phong = @Ma_Loai_Phong)
        THROW 50204, N'Không thể xóa loại phòng vì đang có bảng giá.', 1;

    DELETE FROM dbo.loai_phong
    WHERE ma_loai_phong = @Ma_Loai_Phong;

	SELECT '' ;
END;
GO


CREATE OR ALTER PROCEDURE dbo.sp_Phong_GetAll
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ma_phong, ma_loai_phong, ten_phong,
        so_tang, trang_thai
    FROM dbo.phong
    ORDER BY ma_phong;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Phong_GetById
    @Ma_Phong VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ma_phong, ma_loai_phong, ten_phong,
        so_tang, trang_thai
    FROM dbo.phong
    WHERE ma_phong = @Ma_Phong;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Phong_Search
    @Keyword NVARCHAR(150)
AS
BEGIN
    SET NOCOUNT ON;

    SET @Keyword = ISNULL(@Keyword, N'');

    SELECT
        ma_phong, ma_loai_phong, ten_phong,
        so_tang, trang_thai
    FROM dbo.phong
    WHERE ma_phong LIKE '%'+@Keyword+'%'
       OR ma_loai_phong LIKE '%'+@Keyword+'%'
       OR ten_phong LIKE N'%'+@Keyword+N'%'
       OR trang_thai LIKE N'%'+@Keyword+N'%'
    ORDER BY ma_phong;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Phong_GetByTrangThai
    @Trang_Thai NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ma_phong, ma_loai_phong, ten_phong,
        so_tang, trang_thai
    FROM dbo.phong
    WHERE trang_thai = @Trang_Thai
    ORDER BY ma_phong;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Phong_GetAvailable
    @Ngay_Nhan DATETIME,
    @Ngay_Tra DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.ma_phong,
        p.ma_loai_phong,
        p.ten_phong,
        p.so_tang,
        p.trang_thai
    FROM dbo.phong p
    WHERE p.trang_thai = N'Trống'
      AND NOT EXISTS
      (
          SELECT 1
          FROM dbo.dat_phong d
          WHERE d.ma_phong = p.ma_phong
            AND d.trang_thai NOT IN ('Cancelled', 'Completed')
            AND d.ngay_nhan_du_kien < @Ngay_Tra
            AND d.ngay_tra_du_kien > @Ngay_Nhan
      )
    ORDER BY p.ma_phong;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Phong_Create
    @Ma_Phong VARCHAR(50),
    @Ma_Loai_Phong VARCHAR(50),
    @Ten_Phong NVARCHAR(50),
    @So_Tang INT,
    @Trang_Thai NVARCHAR(50) = N'Trống'
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM dbo.phong WHERE ma_phong = @Ma_Phong)
        THROW 50301, N'Mã phòng đã tồn tại.', 1;

    IF NOT EXISTS (SELECT 1 FROM dbo.loai_phong WHERE ma_loai_phong = @Ma_Loai_Phong)
        THROW 50302, N'Loại phòng không tồn tại.', 1;

    INSERT INTO dbo.phong
    (
        ma_phong, ma_loai_phong, ten_phong,
        so_tang, trang_thai
    )
    VALUES
    (
        @Ma_Phong, @Ma_Loai_Phong, @Ten_Phong,
        @So_Tang, @Trang_Thai
    );
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Phong_Update
    @Ma_Phong VARCHAR(50),
    @Ma_Loai_Phong VARCHAR(50),
    @Ten_Phong NVARCHAR(50),
    @So_Tang INT,
    @Trang_Thai NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.phong WHERE ma_phong = @Ma_Phong)
        THROW 50303, N'Không tìm thấy phòng.', 1;

    IF NOT EXISTS (SELECT 1 FROM dbo.loai_phong WHERE ma_loai_phong = @Ma_Loai_Phong)
        THROW 50304, N'Loại phòng không tồn tại.', 1;

    UPDATE dbo.phong
    SET
        ma_loai_phong = @Ma_Loai_Phong,
        ten_phong = @Ten_Phong,
        so_tang = @So_Tang,
        trang_thai = @Trang_Thai
    WHERE ma_phong = @Ma_Phong;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Phong_UpdateTrangThai
    @Ma_Phong VARCHAR(50),
    @Trang_Thai NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.phong
    SET trang_thai = @Trang_Thai
    WHERE ma_phong = @Ma_Phong;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Phong_Delete
    @Ma_Phong VARCHAR(50)
AS
BEGIN

    IF EXISTS (SELECT 1 FROM dbo.dat_phong WHERE ma_phong = @Ma_Phong)
        THROW 50305, N'Không thể xóa phòng vì đã có dữ liệu đặt phòng.', 1;

    DELETE FROM dbo.phong
    WHERE ma_phong = @Ma_Phong;

	SELECT '' ;
END;
GO


-- 1. Đăng nhập hệ thống
CREATE PROCEDURE [sp_user_login]
(
    @taikhoan VARCHAR(30), 
    @matkhau VARCHAR(60)
)
AS
BEGIN
    SELECT [user_id], hoten, email, taikhoan, [role], image_url  
    FROM [user]
    WHERE taikhoan = @taikhoan AND matkhau = @matkhau;
END;
GO

-- 2. Thêm mới phòng
CREATE PROCEDURE [sp_phong_create]
(
    @ma_phong VARCHAR(50),
    @ma_loai_phong VARCHAR(50),
    @ten_phong NVARCHAR(50),
    @so_tang INT,
    @trang_thai NVARCHAR(50)
)
AS
BEGIN
    INSERT INTO [phong] (ma_phong, ma_loai_phong, ten_phong, so_tang, trang_thai)
    VALUES (@ma_phong, @ma_loai_phong, @ten_phong, @so_tang, @trang_thai);
    SELECT '';
END;
GO

-- 3. Tìm kiếm, lọc và phân trang danh sách phòng (Chuẩn form mẫu)
CREATE PROCEDURE [sp_phong_search]
(
    @page_index INT,
    @page_size INT,
    @ten_phong NVARCHAR(50),
    @ma_loai_phong VARCHAR(50),
    @trang_thai NVARCHAR(50)
)
AS
BEGIN
    DECLARE @RecordCount BIGINT;
    SET NOCOUNT ON;

    SELECT (ROW_NUMBER() OVER(ORDER BY p.ten_phong ASC)) AS RowNumber,
           p.ma_phong,
           p.ten_phong,
           p.so_tang,
           p.trang_thai,
           lp.ma_loai_phong,
           lp.ten_loai_phong,
           lp.gia_mac_dinh
    INTO #Results1
    FROM [phong] p
    INNER JOIN [loai_phong] lp ON p.ma_loai_phong = lp.ma_loai_phong
    WHERE (@ten_phong = '' OR p.ten_phong LIKE N'%' + @ten_phong + '%')
      AND (@ma_loai_phong = '' OR p.ma_loai_phong = @ma_loai_phong)
      AND (@trang_thai = '' OR p.trang_thai = @trang_thai);

    SELECT @RecordCount = COUNT(*) FROM #Results1;

    SELECT *, @RecordCount AS RecordCount
    FROM #Results1
    WHERE (@page_size <> 0 AND RowNumber BETWEEN (@page_index - 1) * @page_size + 1 AND @page_index * @page_size)
       OR (@page_size = 0)
       OR (@page_index = -1);

    DROP TABLE #Results1;
END;
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [sp_dat_phong_create]
(
    @ma_dat_phong VARCHAR(50),
    @ma_khach_hang VARCHAR(50),
    @ma_phong VARCHAR(50),
    @ngay_nhan_du_kien DATETIME,
    @ngay_tra_du_kien DATETIME,
    @tien_coc FLOAT,
    @ghi_chu NVARCHAR(MAX)
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        -- Bắt đầu Transaction để đảm bảo tính toàn vẹn
        BEGIN TRANSACTION;

        DECLARE @is_booked INT = 0;

        -- KIỂM TRA TRÙNG PHÒNG (OVERBOOKING)
        -- Sử dụng UPDLOCK, SERIALIZABLE để khóa bảng trong lúc check, tránh các transaction khác chen ngang
        SELECT @is_booked = COUNT(*)
        FROM [dat_phong] WITH (UPDLOCK, SERIALIZABLE)
        WHERE ma_phong = @ma_phong
          AND trang_thai NOT IN ('Cancelled', 'Checked-out')
          AND (
              (@ngay_nhan_du_kien BETWEEN ngay_nhan_du_kien AND ngay_tra_du_kien)
              OR (@ngay_tra_du_kien BETWEEN ngay_nhan_du_kien AND ngay_tra_du_kien)
              OR (ngay_nhan_du_kien BETWEEN @ngay_nhan_du_kien AND @ngay_tra_du_kien)
          );

        -- Nếu có kết quả > 0 nghĩa là phòng đã bị đặt trước
        IF @is_booked > 0
        BEGIN
            ROLLBACK TRANSACTION;
            -- Bắn lỗi về cho C# bắt (try-catch)
            RAISERROR(N'Phòng đã được đặt trong khoảng thời gian này! (Overbooking)', 16, 1);
            RETURN;
        END

        -- Nếu phòng trống, tiến hành Insert
        INSERT INTO [dat_phong]
        (ma_dat_phong, ma_khach_hang, ma_phong, ngay_nhan_du_kien, ngay_tra_du_kien, tien_coc, trang_thai, ghi_chu)
        VALUES
        (@ma_dat_phong, @ma_khach_hang, @ma_phong, @ngay_nhan_du_kien, @ngay_tra_du_kien, @tien_coc, 'Confirmed', @ghi_chu);

        -- Đặt thành công
        COMMIT TRANSACTION;
        SELECT '';
    END TRY
    BEGIN CATCH
        -- Xử lý lỗi và Rollback nếu có exception
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END;
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [sp_hoa_don_khach_san_create]
(
    @ma_hoa_don VARCHAR(50),
    @ma_dat_phong VARCHAR(50),
    @tong_tien_phong FLOAT,
    @thue_vat FLOAT,
    @phuong_thuc_thanh_toan NVARCHAR(50),
    @listjson_dichvu NVARCHAR(MAX) -- Format: [{"ma_chi_tiet_dv": "...", "ma_dich_vu": "...", "so_luong": 2, "thanh_tien": 50000}]
)
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @tong_tien_dich_vu FLOAT = 0;
        DECLARE @tong_thanh_toan FLOAT = 0;

        -- 1. Tính tổng tiền dịch vụ từ mảng JSON (nếu có)
        IF (@listjson_dichvu IS NOT NULL AND @listjson_dichvu <> '')
        BEGIN
            SELECT @tong_tien_dich_vu = SUM(CAST(JSON_VALUE(p.value, '$.thanh_tien') AS FLOAT))
            FROM OPENJSON(@listjson_dichvu) AS p;
        END

        -- Tính tổng thanh toán cuối cùng
        SET @tong_thanh_toan = @tong_tien_phong + ISNULL(@tong_tien_dich_vu, 0) + @thue_vat;

        -- 2. Insert Hóa đơn (Master)
        INSERT INTO [hoa_don_khach_san]
        (ma_hoa_don, ma_dat_phong, tong_tien_phong, tong_tien_dich_vu, thue_vat, tong_thanh_toan, phuong_thuc_thanh_toan)
        VALUES
        (@ma_hoa_don, @ma_dat_phong, @tong_tien_phong, ISNULL(@tong_tien_dich_vu, 0), @thue_vat, @tong_thanh_toan, @phuong_thuc_thanh_toan);

        -- 3. Insert Chi tiết dịch vụ (Detail)
        IF (@listjson_dichvu IS NOT NULL AND @listjson_dichvu <> '')
        BEGIN
            INSERT INTO [chi_tiet_su_dung_dv]
            (ma_chi_tiet_dv, ma_dat_phong, ma_dich_vu, so_luong, thanh_tien)
            SELECT 
                JSON_VALUE(p.value, '$.ma_chi_tiet_dv'),
                @ma_dat_phong,
                JSON_VALUE(p.value, '$.ma_dich_vu'),
                CAST(JSON_VALUE(p.value, '$.so_luong') AS INT),
                CAST(JSON_VALUE(p.value, '$.thanh_tien') AS FLOAT)
            FROM OPENJSON(@listjson_dichvu) AS p;
        END

        -- 4. Cập nhật trạng thái đặt phòng thành Checked-out
        UPDATE [dat_phong]
        SET trang_thai = 'Checked-out'
        WHERE ma_dat_phong = @ma_dat_phong;

        -- 5. Cập nhật trạng thái phòng thành Đang dọn dẹp
        UPDATE p
        SET p.trang_thai = N'Đang dọn dẹp'
        FROM [phong] p
        INNER JOIN [dat_phong] dp ON p.ma_phong = dp.ma_phong
        WHERE dp.ma_dat_phong = @ma_dat_phong;

        COMMIT TRANSACTION;
        SELECT '';
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END;
GO


CREATE OR ALTER PROCEDURE dbo.sp_Phong_GetAll
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ma_phong, ma_loai_phong, ten_phong,
        so_tang, trang_thai
    FROM dbo.phong
    ORDER BY ma_phong;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Phong_GetById
    @MaPhong VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ma_phong, ma_loai_phong, ten_phong,
        so_tang, trang_thai
    FROM dbo.phong
    WHERE ma_phong = @MaPhong;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Phong_Search
    @Keyword NVARCHAR(150)
AS
BEGIN
    SET NOCOUNT ON;

    SET @Keyword = ISNULL(@Keyword, N'');

    SELECT
        ma_phong, ma_loai_phong, ten_phong,
        so_tang, trang_thai
    FROM dbo.phong
    WHERE ma_phong LIKE '%'+@Keyword+'%'
       OR ma_loai_phong LIKE '%'+@Keyword+'%'
       OR ten_phong LIKE N'%'+@Keyword+N'%'
       OR trang_thai LIKE N'%'+@Keyword+N'%'
    ORDER BY ma_phong;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Phong_GetByTrangThai
    @TrangThai NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ma_phong, ma_loai_phong, ten_phong,
        so_tang, trang_thai
    FROM dbo.phong
    WHERE trang_thai = @TrangThai
    ORDER BY ma_phong;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Phong_GetAvailable
    @NgayNhan DATETIME,
    @NgayTra DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.ma_phong,
        p.ma_loai_phong,
        p.ten_phong,
        p.so_tang,
        p.trang_thai
    FROM dbo.phong p
    WHERE p.trang_thai = N'Trống'
      AND NOT EXISTS
      (
          SELECT 1
          FROM dbo.dat_phong d
          WHERE d.ma_phong = p.ma_phong
            AND d.trang_thai NOT IN ('Cancelled', 'Completed')
            AND d.ngay_nhan_du_kien < @NgayTra
            AND d.ngay_tra_du_kien > @NgayNhan
      )
    ORDER BY p.ma_phong;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Phong_Create
    @MaPhong VARCHAR(50),
    @MaLoaiPhong VARCHAR(50),
    @TenPhong NVARCHAR(50),
    @SoTang INT,
    @TrangThai NVARCHAR(50) = N'Trống'
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM dbo.phong WHERE ma_phong = @MaPhong)
        THROW 50301, N'Mã phòng đã tồn tại.', 1;

    IF NOT EXISTS (SELECT 1 FROM dbo.loai_phong WHERE ma_loai_phong = @MaLoaiPhong)
        THROW 50302, N'Loại phòng không tồn tại.', 1;

    INSERT INTO dbo.phong
    (
        ma_phong, ma_loai_phong, ten_phong,
        so_tang, trang_thai
    )
    VALUES
    (
        @MaPhong, @MaLoaiPhong, @TenPhong,
        @SoTang, @TrangThai
    );
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Phong_Update
    @MaPhong VARCHAR(50),
    @MaLoaiPhong VARCHAR(50),
    @TenPhong NVARCHAR(50),
    @SoTang INT,
    @TrangThai NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.phong WHERE ma_phong = @MaPhong)
        THROW 50303, N'Không tìm thấy phòng.', 1;

    IF NOT EXISTS (SELECT 1 FROM dbo.loai_phong WHERE ma_loai_phong = @MaLoaiPhong)
        THROW 50304, N'Loại phòng không tồn tại.', 1;

    UPDATE dbo.phong
    SET
        ma_loai_phong = @MaLoaiPhong,
        ten_phong = @TenPhong,
        so_tang = @SoTang,
        trang_thai = @TrangThai
    WHERE ma_phong = @MaPhong;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Phong_UpdateTrangThai
    @MaPhong VARCHAR(50),
    @TrangThai NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.phong
    SET trang_thai = @TrangThai
    WHERE ma_phong = @MaPhong;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Phong_Delete
    @MaPhong VARCHAR(50)
AS
BEGIN

    IF EXISTS (SELECT 1 FROM dbo.dat_phong WHERE ma_phong = @MaPhong)
        THROW 50305, N'Không thể xóa phòng vì đã có dữ liệu đặt phòng.', 1;

    DELETE FROM dbo.phong
    WHERE ma_phong = @MaPhong;

	SELECT '' ;
END;
GO


CREATE OR ALTER PROCEDURE dbo.sp_GiaPhong_GetAll
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ma_gia, ma_loai_phong, ten_chinh_sach,
        tu_ngay, den_ngay, gia_theo_dem, gia_theo_gio
    FROM dbo.gia_phong
    ORDER BY tu_ngay DESC, ma_gia;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_GiaPhong_GetById
    @Ma_Gia VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ma_gia, ma_loai_phong, ten_chinh_sach,
        tu_ngay, den_ngay, gia_theo_dem, gia_theo_gio
    FROM dbo.gia_phong
    WHERE ma_gia = @Ma_Gia;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_GiaPhong_GetByLoaiPhong
    @Ma_Loai_Phong VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ma_gia, ma_loai_phong, ten_chinh_sach,
        tu_ngay, den_ngay, gia_theo_dem, gia_theo_gio
    FROM dbo.gia_phong
    WHERE ma_loai_phong = @Ma_Loai_Phong
    ORDER BY tu_ngay DESC;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_GiaPhong_GetCurrent
    @Ma_Loai_Phong VARCHAR(50),
    @Ngay DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1
        ma_gia, ma_loai_phong, ten_chinh_sach,
        tu_ngay, den_ngay, gia_theo_dem, gia_theo_gio
    FROM dbo.gia_phong
    WHERE ma_loai_phong = @Ma_Loai_Phong
      AND tu_ngay <= @Ngay
      AND den_ngay >= @Ngay
    ORDER BY tu_ngay DESC;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_GiaPhong_Create
    @Ma_Gia VARCHAR(50),
    @Ma_Loai_Phong VARCHAR(50),
    @Ten_Chinh_Sach NVARCHAR(150),
    @Tu_Ngay DATE,
    @Den_Ngay DATE,
    @Gia_Theo_Dem FLOAT,
    @Gia_Theo_Gio FLOAT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM dbo.gia_phong WHERE ma_gia = @Ma_Gia)
        THROW 50401, N'Mã giá đã tồn tại.', 1;

    IF @Den_Ngay < @Tu_Ngay
        THROW 50402, N'Đến ngày phải lớn hơn hoặc bằng từ ngày.', 1;

    IF NOT EXISTS (SELECT 1 FROM dbo.loai_phong WHERE ma_loai_phong = @Ma_Loai_Phong)
        THROW 50403, N'Loại phòng không tồn tại.', 1;

    INSERT INTO dbo.gia_phong
    (
        ma_gia, ma_loai_phong, ten_chinh_sach,
        tu_ngay, den_ngay, gia_theo_dem, gia_theo_gio
    )
    VALUES
    (
        @Ma_Gia, @Ma_Loai_Phong, @Ten_Chinh_Sach,
        @Tu_Ngay, @Den_Ngay, @Gia_Theo_Dem, @Gia_Theo_Gio
    );
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_GiaPhong_Update
    @Ma_Gia VARCHAR(50),
    @Ma_Loai_Phong VARCHAR(50),
    @Ten_Chinh_Sach NVARCHAR(150),
    @Tu_Ngay DATE,
    @Den_Ngay DATE,
    @Gia_Theo_Dem FLOAT,
    @Gia_Theo_Gio FLOAT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.gia_phong WHERE ma_gia = @Ma_Gia)
        THROW 50404, N'Không tìm thấy bảng giá.', 1;

    IF @Den_Ngay < @Tu_Ngay
        THROW 50405, N'Đến ngày phải lớn hơn hoặc bằng từ ngày.', 1;

    UPDATE dbo.gia_phong
    SET
        ma_loai_phong = @Ma_Loai_Phong,
        ten_chinh_sach = @Ten_Chinh_Sach,
        tu_ngay = @Tu_Ngay,
        den_ngay = @Den_Ngay,
        gia_theo_dem = @Gia_Theo_Dem,
        gia_theo_gio = @Gia_Theo_Gio
    WHERE ma_gia = @Ma_Gia;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_GiaPhong_Delete
    @Ma_Gia VARCHAR(50)
AS
BEGIN
    DELETE FROM dbo.gia_phong
    WHERE ma_gia = @Ma_Gia;

	SELECT '';
END;
GO


CREATE OR ALTER PROCEDURE dbo.sp_DatPhong_GetAll
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ma_dat_phong, ma_khach_hang, ma_phong,
        ngay_dat, ngay_nhan_du_kien, ngay_tra_du_kien,
        tien_coc, trang_thai, ghi_chu
    FROM dbo.dat_phong
    ORDER BY ngay_dat DESC, ma_dat_phong;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_DatPhong_GetById
    @Ma_Dat_Phong VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ma_dat_phong, ma_khach_hang, ma_phong,
        ngay_dat, ngay_nhan_du_kien, ngay_tra_du_kien,
        tien_coc, trang_thai, ghi_chu
    FROM dbo.dat_phong
    WHERE ma_dat_phong = @Ma_Dat_Phong;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_DatPhong_GetByKhachHang
    @Ma_Khach_Hang VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ma_dat_phong, ma_khach_hang, ma_phong,
        ngay_dat, ngay_nhan_du_kien, ngay_tra_du_kien,
        tien_coc, trang_thai, ghi_chu
    FROM dbo.dat_phong
    WHERE ma_khach_hang = @Ma_Khach_Hang
    ORDER BY ngay_dat DESC;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_DatPhong_GetByPhong
    @Ma_Phong VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ma_dat_phong, ma_khach_hang, ma_phong,
        ngay_dat, ngay_nhan_du_kien, ngay_tra_du_kien,
        tien_coc, trang_thai, ghi_chu
    FROM dbo.dat_phong
    WHERE ma_phong = @Ma_Phong
    ORDER BY ngay_dat DESC;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_DatPhong_Search
    @Keyword NVARCHAR(150)
AS
BEGIN
    SET NOCOUNT ON;

    SET @Keyword = ISNULL(@Keyword, N'');

    SELECT
        ma_dat_phong, ma_khach_hang, ma_phong,
        ngay_dat, ngay_nhan_du_kien, ngay_tra_du_kien,
        tien_coc, trang_thai, ghi_chu
    FROM dbo.dat_phong
    WHERE ma_dat_phong LIKE '%'+@Keyword+'%'
       OR ma_khach_hang LIKE '%'+@Keyword+'%'
       OR ma_phong LIKE '%'+@Keyword+'%'
       OR trang_thai LIKE '%'+@Keyword+'%'
       OR ghi_chu LIKE N'%'+@Keyword+N'%'
    ORDER BY ngay_dat DESC;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_DatPhong_Create
    @Ma_Dat_Phong VARCHAR(50),
    @Ma_Khach_Hang VARCHAR(50),
    @Ma_Phong VARCHAR(50),
    @Ngay_Dat DATETIME = NULL,
    @Ngay_Nhan_Du_Kien DATETIME,
    @Ngay_Tra_Du_Kien DATETIME,
    @Tien_Coc FLOAT = 0,
    @Trang_Thai VARCHAR(30) = 'Pending',
    @Ghi_Chu NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM dbo.dat_phong WHERE ma_dat_phong = @Ma_Dat_Phong)
        THROW 50501, N'Mã đặt phòng đã tồn tại.', 1;

    IF NOT EXISTS (SELECT 1 FROM dbo.khach_hang WHERE ma_khach_hang = @Ma_Khach_Hang)
        THROW 50502, N'Khách hàng không tồn tại.', 1;

    IF NOT EXISTS (SELECT 1 FROM dbo.phong WHERE ma_phong = @Ma_Phong)
        THROW 50503, N'Phòng không tồn tại.', 1;

    IF @Ngay_Tra_Du_Kien <= @Ngay_Nhan_Du_Kien
        THROW 50504, N'Ngày trả phải lớn hơn ngày nhận.', 1;

    IF EXISTS
    (
        SELECT 1
        FROM dbo.dat_phong
        WHERE ma_phong = @Ma_Phong
          AND trang_thai NOT IN ('Cancelled', 'Completed')
          AND ngay_nhan_du_kien < @Ngay_Tra_Du_Kien
          AND ngay_tra_du_kien > @Ngay_Nhan_Du_Kien
    )
        THROW 50505, N'Phòng đã được đặt trong khoảng thời gian này.', 1;

    INSERT INTO dbo.dat_phong
    (
        ma_dat_phong, ma_khach_hang, ma_phong,
        ngay_dat, ngay_nhan_du_kien, ngay_tra_du_kien,
        tien_coc, trang_thai, ghi_chu
    )
    VALUES
    (
        @Ma_Dat_Phong, @Ma_Khach_Hang, @Ma_Phong,
        ISNULL(@Ngay_Dat, GETDATE()), @Ngay_Nhan_Du_Kien, @Ngay_Tra_Du_Kien,
        ISNULL(@Tien_Coc, 0), ISNULL(@Trang_Thai, 'Pending'), @Ghi_Chu
    );

    UPDATE dbo.phong
    SET trang_thai = N'Đã đặt'
    WHERE ma_phong = @Ma_Phong
      AND ISNULL(@Trang_Thai, 'Pending') IN ('Pending', 'Confirmed');
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_DatPhong_Update
    @Ma_Dat_Phong VARCHAR(50),
    @Ma_Khach_Hang VARCHAR(50),
    @Ma_Phong VARCHAR(50),
    @Ngay_Dat DATETIME = NULL,
    @Ngay_Nhan_Du_Kien DATETIME,
    @Ngay_Tra_Du_Kien DATETIME,
    @Tien_Coc FLOAT = 0,
    @Trang_Thai VARCHAR(30) = NULL,
    @Ghi_Chu NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.dat_phong WHERE ma_dat_phong = @Ma_Dat_Phong)
        THROW 50506, N'Không tìm thấy đặt phòng.', 1;

    IF @Ngay_Tra_Du_Kien <= @Ngay_Nhan_Du_Kien
        THROW 50507, N'Ngày trả phải lớn hơn ngày nhận.', 1;

    UPDATE dbo.dat_phong
    SET
        ma_khach_hang = @Ma_Khach_Hang,
        ma_phong = @Ma_Phong,
        ngay_dat = @Ngay_Dat,
        ngay_nhan_du_kien = @Ngay_Nhan_Du_Kien,
        ngay_tra_du_kien = @Ngay_Tra_Du_Kien,
        tien_coc = @Tien_Coc,
        trang_thai = @Trang_Thai,
        ghi_chu = @Ghi_Chu
    WHERE ma_dat_phong = @Ma_Dat_Phong;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_DatPhong_Delete
    @Ma_Dat_Phong VARCHAR(50)
AS
BEGIN

    DECLARE @Ma_Phong VARCHAR(50);

    SELECT @Ma_Phong = ma_phong
    FROM dbo.dat_phong
    WHERE ma_dat_phong = @Ma_Dat_Phong;

    IF @Ma_Phong IS NULL
        THROW 50508, N'Không tìm thấy đặt phòng.', 1;

    DELETE FROM dbo.dat_phong
    WHERE ma_dat_phong = @Ma_Dat_Phong;

    IF NOT EXISTS
    (
        SELECT 1
        FROM dbo.dat_phong
        WHERE ma_phong = @Ma_Phong
          AND trang_thai NOT IN ('Cancelled', 'Completed')
    )
    BEGIN
        UPDATE dbo.phong
        SET trang_thai = N'Trống'
        WHERE ma_phong = @Ma_Phong;
    END;

	SELECT '';
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_DatPhong_Cancel
    @Ma_Dat_Phong VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Ma_Phong VARCHAR(50);

    SELECT @Ma_Phong = ma_phong
    FROM dbo.dat_phong
    WHERE ma_dat_phong = @Ma_Dat_Phong;

    IF @Ma_Phong IS NULL
        THROW 50509, N'Không tìm thấy đặt phòng.', 1;

    UPDATE dbo.dat_phong
    SET trang_thai = 'Cancelled'
    WHERE ma_dat_phong = @Ma_Dat_Phong;

    IF NOT EXISTS
    (
        SELECT 1
        FROM dbo.dat_phong
        WHERE ma_phong = @Ma_Phong
          AND trang_thai NOT IN ('Cancelled', 'Completed')
    )
    BEGIN
        UPDATE dbo.phong
        SET trang_thai = N'Trống'
        WHERE ma_phong = @Ma_Phong;
    END;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_DatPhong_CheckIn
    @Ma_Dat_Phong VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Ma_Phong VARCHAR(50);

    SELECT @Ma_Phong = ma_phong
    FROM dbo.dat_phong
    WHERE ma_dat_phong = @Ma_Dat_Phong;

    IF @Ma_Phong IS NULL
        THROW 50510, N'Không tìm thấy đặt phòng.', 1;

    UPDATE dbo.dat_phong
    SET trang_thai = 'CheckedIn'
    WHERE ma_dat_phong = @Ma_Dat_Phong;

    UPDATE dbo.phong
    SET trang_thai = N'Đang sử dụng'
    WHERE ma_phong = @Ma_Phong;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_DatPhong_CheckOut
    @Ma_Dat_Phong VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Ma_Phong VARCHAR(50);

    SELECT @Ma_Phong = ma_phong
    FROM dbo.dat_phong
    WHERE ma_dat_phong = @Ma_Dat_Phong;

    IF @Ma_Phong IS NULL
        THROW 50511, N'Không tìm thấy đặt phòng.', 1;

    UPDATE dbo.dat_phong
    SET trang_thai = 'Completed'
    WHERE ma_dat_phong = @Ma_Dat_Phong;

    UPDATE dbo.phong
    SET trang_thai = N'Trống'
    WHERE ma_phong = @Ma_Phong;
END;
GO


CREATE OR ALTER PROCEDURE dbo.sp_DichVu_GetAll
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ma_dich_vu, ten_dich_vu, don_gia, don_vi_tinh
    FROM dbo.dich_vu
    ORDER BY ma_dich_vu;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_DichVu_GetById
    @Ma_Dich_Vu VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ma_dich_vu, ten_dich_vu, don_gia, don_vi_tinh
    FROM dbo.dich_vu
    WHERE ma_dich_vu = @Ma_Dich_Vu;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_DichVu_Search
    @Keyword NVARCHAR(150)
AS
BEGIN
    SET NOCOUNT ON;

    SET @Keyword = ISNULL(@Keyword, N'');

    SELECT
        ma_dich_vu, ten_dich_vu, don_gia, don_vi_tinh
    FROM dbo.dich_vu
    WHERE ma_dich_vu LIKE '%'+@Keyword+'%'
       OR ten_dich_vu LIKE N'%'+@Keyword+N'%'
       OR don_vi_tinh LIKE N'%'+@Keyword+N'%'
    ORDER BY ma_dich_vu;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_DichVu_Create
    @Ma_Dich_Vu VARCHAR(50),
    @Ten_Dich_Vu NVARCHAR(150),
    @Don_Gia FLOAT,
    @Don_Vi_Tinh NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM dbo.dich_vu WHERE ma_dich_vu = @Ma_Dich_Vu)
        THROW 50601, N'Mã dịch vụ đã tồn tại.', 1;

    INSERT INTO dbo.dich_vu
    (
        ma_dich_vu, ten_dich_vu, don_gia, don_vi_tinh
    )
    VALUES
    (
        @Ma_Dich_Vu, @Ten_Dich_Vu, @Don_Gia, @Don_Vi_Tinh
    );
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_DichVu_Update
    @Ma_Dich_Vu VARCHAR(50),
    @Ten_Dich_Vu NVARCHAR(150),
    @Don_Gia FLOAT,
    @Don_Vi_Tinh NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.dich_vu WHERE ma_dich_vu = @Ma_Dich_Vu)
        THROW 50602, N'Không tìm thấy dịch vụ.', 1;

    UPDATE dbo.dich_vu
    SET
        ten_dich_vu = @Ten_Dich_Vu,
        don_gia = @Don_Gia,
        don_vi_tinh = @Don_Vi_Tinh
    WHERE ma_dich_vu = @Ma_Dich_Vu;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_DichVu_Delete
    @Ma_Dich_Vu VARCHAR(50)
AS
BEGIN

    IF EXISTS (SELECT 1 FROM dbo.chi_tiet_su_dung_dv WHERE ma_dich_vu = @Ma_Dich_Vu)
        THROW 50603, N'Không thể xóa dịch vụ vì đã có lịch sử sử dụng.', 1;

    DELETE FROM dbo.dich_vu
    WHERE ma_dich_vu = @Ma_Dich_Vu;
	SELECT '';
END;
GO


CREATE OR ALTER PROCEDURE dbo.sp_ChiTietDv_GetAll
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ma_chi_tiet_dv, ma_dat_phong, ma_dich_vu,
        so_luong, thanh_tien, ngay_su_dung
    FROM dbo.chi_tiet_su_dung_dv
    ORDER BY ngay_su_dung DESC, ma_chi_tiet_dv;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_ChiTietDv_GetById
    @Ma_Chi_Tiet_Dv VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ma_chi_tiet_dv, ma_dat_phong, ma_dich_vu,
        so_luong, thanh_tien, ngay_su_dung
    FROM dbo.chi_tiet_su_dung_dv
    WHERE ma_chi_tiet_dv = @Ma_Chi_Tiet_Dv;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_ChiTietDv_GetByDatPhong
    @Ma_Dat_Phong VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ct.ma_chi_tiet_dv,
        ct.ma_dat_phong,
        ct.ma_dich_vu,
        dv.ten_dich_vu,
        dv.don_gia,
        dv.don_vi_tinh,
        ct.so_luong,
        ct.thanh_tien,
        ct.ngay_su_dung
    FROM dbo.chi_tiet_su_dung_dv ct
    INNER JOIN dbo.dich_vu dv
        ON dv.ma_dich_vu = ct.ma_dich_vu
    WHERE ct.ma_dat_phong = @Ma_Dat_Phong
    ORDER BY ct.ngay_su_dung DESC;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_ChiTietDv_Create
    @Ma_Chi_Tiet_Dv VARCHAR(50),
    @Ma_Dat_Phong VARCHAR(50),
    @Ma_Dich_Vu VARCHAR(50),
    @So_Luong INT,
    @Thanh_Tien FLOAT = NULL,
    @Ngay_Su_Dung DATETIME = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Don_Gia FLOAT;

    IF EXISTS (SELECT 1 FROM dbo.chi_tiet_su_dung_dv WHERE ma_chi_tiet_dv = @Ma_Chi_Tiet_Dv)
        THROW 50701, N'Mã chi tiết dịch vụ đã tồn tại.', 1;

    SELECT @Don_Gia = don_gia
    FROM dbo.dich_vu
    WHERE ma_dich_vu = @Ma_Dich_Vu;

    IF @Don_Gia IS NULL
        THROW 50702, N'Dịch vụ không tồn tại.', 1;

    IF @So_Luong <= 0
        THROW 50703, N'Số lượng phải lớn hơn 0.', 1;

    INSERT INTO dbo.chi_tiet_su_dung_dv
    (
        ma_chi_tiet_dv, ma_dat_phong, ma_dich_vu,
        so_luong, thanh_tien, ngay_su_dung
    )
    VALUES
    (
        @Ma_Chi_Tiet_Dv, @Ma_Dat_Phong, @Ma_Dich_Vu,
        @So_Luong,
        ISNULL(@Thanh_Tien, @Don_Gia * @So_Luong),
        ISNULL(@Ngay_Su_Dung, GETDATE())
    );
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_ChiTietDv_Update
    @Ma_Chi_Tiet_Dv VARCHAR(50),
    @Ma_Dat_Phong VARCHAR(50),
    @Ma_Dich_Vu VARCHAR(50),
    @So_Luong INT,
    @Thanh_Tien FLOAT = NULL,
    @Ngay_Su_Dung DATETIME = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Don_Gia FLOAT;

    SELECT @Don_Gia = don_gia
    FROM dbo.dich_vu
    WHERE ma_dich_vu = @Ma_Dich_Vu;

    IF @Don_Gia IS NULL
        THROW 50704, N'Dịch vụ không tồn tại.', 1;

    IF @So_Luong <= 0
        THROW 50705, N'Số lượng phải lớn hơn 0.', 1;

    UPDATE dbo.chi_tiet_su_dung_dv
    SET
        ma_dat_phong = @Ma_Dat_Phong,
        ma_dich_vu = @Ma_Dich_Vu,
        so_luong = @So_Luong,
        thanh_tien = ISNULL(@Thanh_Tien, @Don_Gia * @So_Luong),
        ngay_su_dung = @Ngay_Su_Dung
    WHERE ma_chi_tiet_dv = @Ma_Chi_Tiet_Dv;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_ChiTietDv_Delete
    @Ma_Chi_Tiet_Dv VARCHAR(50)
AS
BEGIN

    DELETE FROM dbo.chi_tiet_su_dung_dv
    WHERE ma_chi_tiet_dv = @Ma_Chi_Tiet_Dv;
	SELECT '';
END;
GO


CREATE OR ALTER PROCEDURE dbo.sp_HoaDon_GetAll
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ma_hoa_don, ma_dat_phong, ngay_lap,
        tong_tien_phong, tong_tien_dich_vu,
        thue_vat, tong_thanh_toan, phuong_thuc_thanh_toan
    FROM dbo.hoa_don_khach_san
    ORDER BY ngay_lap DESC, ma_hoa_don;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_HoaDon_GetById
    @Ma_Hoa_Don VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ma_hoa_don, ma_dat_phong, ngay_lap,
        tong_tien_phong, tong_tien_dich_vu,
        thue_vat, tong_thanh_toan, phuong_thuc_thanh_toan
    FROM dbo.hoa_don_khach_san
    WHERE ma_hoa_don = @Ma_Hoa_Don;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_HoaDon_GetByDatPhong
    @Ma_Dat_Phong VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ma_hoa_don, ma_dat_phong, ngay_lap,
        tong_tien_phong, tong_tien_dich_vu,
        thue_vat, tong_thanh_toan, phuong_thuc_thanh_toan
    FROM dbo.hoa_don_khach_san
    WHERE ma_dat_phong = @Ma_Dat_Phong
    ORDER BY ngay_lap DESC;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_HoaDon_Search
    @Keyword NVARCHAR(150)
AS
BEGIN
    SET NOCOUNT ON;

    SET @Keyword = ISNULL(@Keyword, N'');

    SELECT
        ma_hoa_don, ma_dat_phong, ngay_lap,
        tong_tien_phong, tong_tien_dich_vu,
        thue_vat, tong_thanh_toan, phuong_thuc_thanh_toan
    FROM dbo.hoa_don_khach_san
    WHERE ma_hoa_don LIKE '%'+@Keyword+'%'
       OR ma_dat_phong LIKE '%'+@Keyword+'%'
       OR phuong_thuc_thanh_toan LIKE N'%'+@Keyword+N'%'
    ORDER BY ngay_lap DESC;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_HoaDon_Create
    @Ma_Hoa_Don VARCHAR(50),
    @Ma_Dat_Phong VARCHAR(50),
    @Ngay_Lap DATETIME = NULL,
    @Tong_Tien_Phong FLOAT,
    @Tong_Tien_Dich_Vu FLOAT = 0,
    @Thue_Vat FLOAT = 0,
    @Tong_Thanh_Toan FLOAT,
    @Phuong_Thuc_Thanh_Toan NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM dbo.hoa_don_khach_san WHERE ma_hoa_don = @Ma_Hoa_Don)
        THROW 50801, N'Mã hóa đơn đã tồn tại.', 1;

    IF NOT EXISTS (SELECT 1 FROM dbo.dat_phong WHERE ma_dat_phong = @Ma_Dat_Phong)
        THROW 50802, N'Đặt phòng không tồn tại.', 1;

    INSERT INTO dbo.hoa_don_khach_san
    (
        ma_hoa_don, ma_dat_phong, ngay_lap,
        tong_tien_phong, tong_tien_dich_vu,
        thue_vat, tong_thanh_toan, phuong_thuc_thanh_toan
    )
    VALUES
    (
        @Ma_Hoa_Don, @Ma_Dat_Phong, ISNULL(@Ngay_Lap, GETDATE()),
        @Tong_Tien_Phong, ISNULL(@Tong_Tien_Dich_Vu, 0),
        ISNULL(@Thue_Vat, 0), @Tong_Thanh_Toan, @Phuong_Thuc_Thanh_Toan
    );
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_HoaDon_Update
    @Ma_Hoa_Don VARCHAR(50),
    @Ma_Dat_Phong VARCHAR(50),
    @Ngay_Lap DATETIME = NULL,
    @Tong_Tien_Phong FLOAT,
    @Tong_Tien_Dich_Vu FLOAT = 0,
    @Thue_Vat FLOAT = 0,
    @Tong_Thanh_Toan FLOAT,
    @Phuong_Thuc_Thanh_Toan NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.hoa_don_khach_san WHERE ma_hoa_don = @Ma_Hoa_Don)
        THROW 50803, N'Không tìm thấy hóa đơn.', 1;

    UPDATE dbo.hoa_don_khach_san
    SET
        ma_dat_phong = @Ma_Dat_Phong,
        ngay_lap = @Ngay_Lap,
        tong_tien_phong = @Tong_Tien_Phong,
        tong_tien_dich_vu = @Tong_Tien_Dich_Vu,
        thue_vat = @Thue_Vat,
        tong_thanh_toan = @Tong_Thanh_Toan,
        phuong_thuc_thanh_toan = @Phuong_Thuc_Thanh_Toan
    WHERE ma_hoa_don = @Ma_Hoa_Don;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_HoaDon_Delete
    @Ma_Hoa_Don VARCHAR(50)
AS
BEGIN
    DELETE FROM dbo.hoa_don_khach_san
    WHERE ma_hoa_don = @Ma_Hoa_Don;
	SELECT '';
END;
GO


CREATE OR ALTER PROCEDURE dbo.sp_DatPhong_GetDoanhThu
    @Tu_Ngay DATE,
    @Den_Ngay DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        CAST(ISNULL(SUM(hd.tong_tien_phong), 0) AS FLOAT) AS Tong_Tien_Phong,
        CAST(ISNULL(SUM(hd.tong_tien_dich_vu), 0) AS FLOAT) AS TongTien_Dich_Vu,
        CAST(ISNULL(SUM(hd.thue_vat), 0) AS FLOAT) AS Tong_Thue_Vat,
        CAST(ISNULL(SUM(hd.tong_thanh_toan), 0) AS FLOAT) AS Tong_Doanh_Thu
    FROM dbo.hoa_don_khach_san hd
    WHERE CAST(ISNULL(hd.ngay_lap, GETDATE()) AS DATE)
          BETWEEN @Tu_Ngay AND @Den_Ngay;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_ThongKe_Phong
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        trang_thai,
        COUNT(*) AS SoLuong
    FROM dbo.phong
    GROUP BY trang_thai
    ORDER BY trang_thai;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_ThongKe_LoaiPhong
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        lp.ma_loai_phong,
        lp.ten_loai_phong,
        COUNT(p.ma_phong) AS SoLuongPhong
    FROM dbo.loai_phong lp
    LEFT JOIN dbo.phong p
        ON p.ma_loai_phong = lp.ma_loai_phong
    GROUP BY
        lp.ma_loai_phong,
        lp.ten_loai_phong
    ORDER BY lp.ma_loai_phong;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_ThongKe_DichVu
    @Tu_Ngay DATE,
    @Den_Ngay DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        dv.ma_dich_vu,
        dv.ten_dich_vu,
        SUM(ct.so_luong) AS Tong_So_Luong,
        SUM(ct.thanh_tien) AS Tong_Tien
    FROM dbo.chi_tiet_su_dung_dv ct
    INNER JOIN dbo.dich_vu dv
        ON dv.ma_dich_vu = ct.ma_dich_vu
    WHERE CAST(ISNULL(ct.ngay_su_dung, GETDATE()) AS DATE)
          BETWEEN @Tu_Ngay AND @Den_Ngay
    GROUP BY
        dv.ma_dich_vu,
        dv.ten_dich_vu
    ORDER BY Tong_Tien DESC;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_ThongKe_DatPhong
    @Tu_Ngay DATE,
    @Den_Ngay DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        trang_thai,
        COUNT(*) AS So_Luong
    FROM dbo.dat_phong
    WHERE CAST(ISNULL(ngay_dat, GETDATE()) AS DATE)
          BETWEEN @Tu_Ngay AND @Den_Ngay
    GROUP BY trang_thai
    ORDER BY trang_thai;
END;
GO




INSERT INTO [user] 
([user_id], [hoten], [ngaysinh], [diachi], [gioitinh], [email], [taikhoan], [matkhau], [role], [image_url]) 
VALUES 
-- 1. Tài khoản Admin (Toàn quyền)
('0cba3670-9424-4bf2-bb57-caeadd45be61', N'Phạm Quốc Cường', '2006-04-23', N'Hưng Yên', N'Nam', 'admin@utehy.edu.vn', 'admin', '123456', 'Admin', NULL),
-- 2. Tài khoản Lễ tân (Quản lý Check-in/Check-out, Booking)
('6126ec27-cf7c-44fa-8da0-18faaddb4e31', N'Nguyễn Thị Lan', '1999-08-15', N'Hà Nội', N'Nữ', 'lan.nguyen@gmail.com', 'letan01', '123456', 'LeTan', NULL),
-- 3. Tài khoản Kế toán (Quản lý hóa đơn, báo cáo doanh thu)
('02090d4c-16e9-432e-a41a-86f3288e8379', N'Trần Văn Bình', '1995-12-10', N'Hải Dương', N'Nam', 'binh.tran@gmail.com', 'ketoan01', '123456', 'KeToan', NULL),
-- 4. Tài khoản Khách hàng (Dùng để test luồng khách tự đặt phòng online)
('381a6cbe-b247-4f75-aaa4-b85d128d8509', N'Lê Hoàng Anh', '2002-05-20', N'Hải Phòng', N'Nam', 'hoanganh@gmail.com', 'khach01', '123456', 'Khach', NULL);
GO
SELECT * FROM [dbo].[user]
SELECT * FROM [dbo].[loai_phong]
SELECT * FROM [dbo].[khach_hang]
SELECT COUNT(1) FROM [dbo].[user] WHERE [taikhoan] = 'qcuong35'

INSERT INTO [dbo].[khach_hang] 
([ma_khach_hang], [ho_ten], [cmnd_cccd], [so_dien_thoai], [email], [dia_chi]) 
VALUES 
('kh01-1111-2222-3333-aaaaaaaaaaaa', N'Nguyễn Văn An', '031099001234', '0901234567', 'an.nguyen@gmail.com', N'Hà Nội')
SELECT * FROM [dbo].[khach_hang]

INSERT INTO dbo.loai_phong
    (ma_loai_phong, ten_loai_phong, gia_mac_dinh, so_nguoi_chuan, mo_ta)
VALUES
    ('a12f8c31-7b42-4d91-8e65-0c3a2f6b9101',
     N'Phòng đơn', 300000, 1, N'Phòng tiêu chuẩn dành cho 1 người'),

    ('b23a9d42-8c53-4e02-9f76-1d4b3a7c8202',
     N'Phòng đôi', 450000, 2, N'Phòng dành cho 2 người'),

    ('c34b0e53-9d64-4f13-a087-2e5c4b8d7303',
     N'Phòng Deluxe', 600000, 2, N'Phòng cao cấp với tiện nghi hiện đại'),

    ('d45c1f64-ae75-4014-b198-3f6d5c9e6404',
     N'Phòng gia đình', 800000, 4, N'Phòng rộng rãi dành cho gia đình'),

    ('e56d2075-bf86-4125-82a9-4a7e6d0f5505',
     N'Phòng VIP', 1200000, 2, N'Phòng cao cấp với dịch vụ đặc biệt');
GO

INSERT INTO dbo.phong
    (ma_phong, ma_loai_phong, ten_phong, so_tang, trang_thai)
VALUES
    ('f67e3186-c097-4236-93ba-5b8f7e1a4606',
     'a12f8c31-7b42-4d91-8e65-0c3a2f6b9101',
     N'P101', 1, N'Trống'),

    ('078f4297-d1a8-4347-a4cb-6c9a8f2b3707',
     'a12f8c31-7b42-4d91-8e65-0c3a2f6b9101',
     N'P102', 1, N'Trống'),

    ('189053a8-e2b9-4458-b5dc-7dab903c2808',
     'b23a9d42-8c53-4e02-9f76-1d4b3a7c8202',
     N'P201', 2, N'Trống'),

    ('29a164b9-f3ca-4569-86ed-8eb0a14d1909',
     'b23a9d42-8c53-4e02-9f76-1d4b3a7c8202',
     N'P202', 2, N'Trống'),

    ('3ab275ca-04db-467a-97fe-9fc1b25e0010',
     'c34b0e53-9d64-4f13-a087-2e5c4b8d7303',
     N'P301', 3, N'Trống'),

    ('4bc386db-15ec-478b-a80f-a0d2c36f1011',
     'c34b0e53-9d64-4f13-a087-2e5c4b8d7303',
     N'P302', 3, N'Trống'),

    ('5cd497ec-26fd-489c-b910-b1e3d4702012',
     'd45c1f64-ae75-4014-b198-3f6d5c9e6404',
     N'P401', 4, N'Trống'),

    ('6de5a8fd-370e-49ad-8a21-c2f4e5813013',
     'd45c1f64-ae75-4014-b198-3f6d5c9e6404',
     N'P402', 4, N'Trống'),

    ('7ef6b90e-481f-4abe-9b32-d305f6924014',
     'e56d2075-bf86-4125-82a9-4a7e6d0f5505',
     N'P501', 5, N'Trống'),

    ('8f07ca1f-5920-4bcf-ac43-e41607a35015',
     'e56d2075-bf86-4125-82a9-4a7e6d0f5505',
     N'P502', 5, N'Trống');
GO

INSERT INTO dbo.gia_phong
    (ma_gia, ma_loai_phong, ten_chinh_sach,
     tu_ngay, den_ngay, gia_theo_dem, gia_theo_gio)
VALUES
    ('9018db20-6a31-4cd0-bd54-f52718b46016',
     'a12f8c31-7b42-4d91-8e65-0c3a2f6b9101',
     N'Giá tiêu chuẩn phòng đơn',
     '2026-01-01', '2026-12-31', 300000, 50000),

    ('a129ec31-7b42-4de1-8e65-0c3a2f6b1017',
     'b23a9d42-8c53-4e02-9f76-1d4b3a7c8202',
     N'Giá tiêu chuẩn phòng đôi',
     '2026-01-01', '2026-12-31', 450000, 80000),

    ('b23afd42-8c53-4ef2-9f76-1d4b3a7c1018',
     'c34b0e53-9d64-4f13-a087-2e5c4b8d7303',
     N'Giá tiêu chuẩn phòng Deluxe',
     '2026-01-01', '2026-12-31', 600000, 100000),

    ('c34b0e53-9d64-4f13-a087-2e5c4b8d1019',
     'd45c1f64-ae75-4014-b198-3f6d5c9e6404',
     N'Giá tiêu chuẩn phòng gia đình',
     '2026-01-01', '2026-12-31', 800000, 150000),

    ('d45c1f64-ae75-4014-b198-3f6d5c9e1020',
     'e56d2075-bf86-4125-82a9-4a7e6d0f5505',
     N'Giá tiêu chuẩn phòng VIP',
     '2026-01-01', '2026-12-31', 1200000, 250000);
GO

INSERT INTO dbo.dat_phong
    (ma_dat_phong, ma_khach_hang, ma_phong,
     ngay_dat, ngay_nhan_du_kien, ngay_tra_du_kien,
     tien_coc, trang_thai, ghi_chu)
VALUES
    ('e56d2075-bf86-4125-82a9-4a7e6d0f1021',
     '366e7f8e-19e7-4c64-8bc6-9fccebed01f4',
     'f67e3186-c097-4236-93ba-5b8f7e1a4606',
     '2026-10-11 10:00:00',
     '2026-10-15 14:00:00',
     '2026-10-17 12:00:00',
     100000, 'Confirmed', N'Đặt phòng đơn 2 đêm'),

    ('f67e3186-c097-4236-93ba-5b8f7e1a1022',
     '4f9120ed-a92c-4d9a-953a-2e5f60962923',
     '189053a8-e2b9-4458-b5dc-7dab903c2808',
     '2026-10-11 11:00:00',
     '2026-10-18 14:00:00',
     '2026-10-20 12:00:00',
     150000, 'Pending', N'Đặt phòng đôi 2 đêm'),

    ('078f4297-d1a8-4347-a4cb-6c9a8f2b1023',
     'f8ef0a4a-da16-41d8-a54d-a7f5e4928f25',
     '3ab275ca-04db-467a-97fe-9fc1b25e0010',
     '2026-10-11 12:00:00',
     '2026-10-21 14:00:00',
     '2026-10-24 12:00:00',
     200000, 'Confirmed', N'Đặt phòng Deluxe 3 đêm');
GO
select * from dbo.dat_phong
INSERT INTO dbo.dich_vu ([ma_dich_vu], [ten_dich_vu], [don_gia], [don_vi_tinh])
VALUES 
('a3bb189e-8bf9-4888-9912-ace4e6543002', N'Nước suối Aquafina', 15000, N'Chai'),
('3f2504e0-4f89-41d3-9a0c-0305e82c3301', N'Nước tăng lực Vita500', 25000, N'Chai'),
('21ec2020-3aea-4069-a2dd-08002b30309d', N'Bò húc (RedBull)', 20000, N'Lon'),
('886313e1-3b8a-4372-9b90-0c9aee199e5d', N'Mì ly Hảo Hảo', 20000, N'Ly'),
('d9428888-122b-4bcd-8421-654321abcdef', N'Giặt ủi quần áo', 50000, N'Kg'),
('e02fd0e4-00fd-490a-ca30-0d00a0038ba0', N'Buffet sáng', 150000, N'Suất'),
('b4a7d9b0-2c4e-4f8a-9d6c-1e3f5a7b9c0d', N'Thuê xe máy', 150000, N'Ngày');
GO

INSERT INTO dbo.chi_tiet_su_dung_dv 
([ma_chi_tiet_dv], [ma_dat_phong], [ma_dich_vu], [so_luong], [thanh_tien], [ngay_su_dung])
VALUES 
('11a55b88-7c2d-4e9f-8a1b-3c4d5e6f7a8b', '078f4297-d1a8-4347-a4cb-6c9a8f2b1023', '3f2504e0-4f89-41d3-9a0c-0305e82c3301', 2, 50000, '2026-10-16 20:00:00'),
('22b66c99-8d3e-4f0a-9b2c-4d5e6f7a8b9c', '078f4297-d1a8-4347-a4cb-6c9a8f2b1023', '886313e1-3b8a-4372-9b90-0c9aee199e5d', 1, 20000, '2026-10-16 22:30:00'),
('33c77daa-9e4f-401b-8c3d-5e6f7a8b9c0d', '078f4297-d1a8-4347-a4cb-6c9a8f2b1023', 'd9428888-122b-4bcd-8421-654321abcdef', 1, 50000, '2026-10-17 09:00:00'),

('44d88ebb-0f5a-412c-9d4e-6f7a8b9c0d1e', '62facf73-af98-4e0c-a7c9-b9ab9c3bba0a', 'e02fd0e4-00fd-490a-ca30-0d00a0038ba0', 2, 300000, '2026-10-20 07:30:00'),
('55e99fcc-1a6b-423d-8e5f-7a8b9c0d1e2f', '62facf73-af98-4e0c-a7c9-b9ab9c3bba0a', 'a3bb189e-8bf9-4888-9912-ace4e6543002', 2, 30000, '2026-10-20 08:00:00');
GO

INSERT INTO dbo.hoa_don_khach_san 
([ma_hoa_don], [ma_dat_phong], [ngay_lap], [tong_tien_phong], [tong_tien_dich_vu], [thue_vat], [tong_thanh_toan], [phuong_thuc_thanh_toan])
VALUES 
('9f8e7d6c-5b4a-4321-8765-abcdef123456', '078f4297-d1a8-4347-a4cb-6c9a8f2b1023', '2026-10-18 12:15:00', 600000, 120000, 72000, 792000, N'Chuyển khoản'),

('0a1b2c3d-4e5f-4678-9012-fedcba654321', '62facf73-af98-4e0c-a7c9-b9ab9c3bba0a', '2026-10-20 12:10:00', 500000, 330000, 83000, 913000, N'Tiền mặt');
GO

select * from [dbo].[dich_vu]