using DoAnCoSo.Repositories;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DoAnCoSo.Models
{
    public class SeafoodDbContext : IdentityDbContext<ApplicationUser>
    {
        public SeafoodDbContext(DbContextOptions<SeafoodDbContext> options) : base(options)
        {
        }

        public DbSet<KhachHang> KhachHangs { get; set; }
        public DbSet<NhaCungCap> NhaCungCaps { get; set; }
        public DbSet<NhanVien> NhanViens { get; set; }
        public DbSet<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; }
        public DbSet<SanPham> SanPhams { get; set; }
        public DbSet<PhieuNhapHang> PhieuNhapHangs { get; set; }
        public DbSet<DonHang> DonHangs { get; set; }
        public DbSet<HoaDon> HoaDons { get; set; }
        public DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public DbSet<GiamGia> GiamGias { get; set; }
        public DbSet<NhaKho> NhaKhos { get; set; }
        public DbSet<ChiTietKhoHang> ChiTietKhoHangs { get; set; }
        public DbSet<PhieuVanChuyen> PhieuVanChuyens { get; set; }
        public DbSet<DanhMuc> DanhMucs { get; set; }
        public DbSet<DanhGia> DanhGias { get; set; }
        public DbSet<HinhAnhSanPham> HinhAnhSanPhams { get; set; }
        public DbSet<Mua> Muas { get; set; }
        public DbSet<ChiTietMua> ChiTietMuas { get; set; }
        public DbSet<PhuongThucThanhToan> PhuongThucThanhToans { get; set; }
        public DbSet<TrangThaiDonHang> TrangThaiDonHangs { get; set; }
        public DbSet<LoaiSanPham> LoaiSanPhams { get; set; }
        public DbSet<ChiTietTrangThaiDonHang> ChiTietTrangThaiDonHangs { get; set; }
        public DbSet<TrangThaiThanhToan> TrangThaiThanhToans { get; set; }
        public DbSet<PhuongTienVanChuyen> PhuongTienVanChuyens { get; set; }
        public DbSet<ChiTietVanChuyen> ChiTietVanChuyens { get; set; }
        public DbSet<ThongKeDoanhThu> ThongKeDoanhThus { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);


            // DuLieuDanhBat -> NguDan
            modelBuilder.Entity<PhieuNhapHang>()
                .HasOne(p => p.NhaCungCap)
                .WithMany(nc => nc.PhieuNhapHangs)
                .HasForeignKey(p => p.MaNhaCungCapID)
                .OnDelete(DeleteBehavior.Restrict);



            // ChiTietSanPham: many-to-many giữa SanPham và phieunhaphang
            modelBuilder.Entity<ChiTietPhieuNhap>()
                .HasKey(ct => new { ct.MaPhieuNhap, ct.MaSanPham });

            modelBuilder.Entity<ChiTietPhieuNhap>()
                .HasOne(ct => ct.SanPham)
                .WithMany(sp => sp.ChiTietPhieuNhaps)
                .HasForeignKey(ct => ct.MaSanPham);

            modelBuilder.Entity<ChiTietPhieuNhap>()
                .HasOne(ct => ct.PhieuNhapHang)
                .WithMany(dl => dl.ChiTietPhieuNhaps)
                .HasForeignKey(ct => ct.MaPhieuNhap);

            // Quan hệ: PhieuVanChuyen - PhuongTienVanChuyen (Nhiều - 1)
            modelBuilder.Entity<PhieuVanChuyen>()
                .HasOne(p => p.PhuongTienVanChuyen)
                .WithMany(t => t.PhieuVanChuyens)
                .HasForeignKey(p => p.MaPhuongTienID)
                .OnDelete(DeleteBehavior.Restrict); // hoặc Cascade nếu muốn xóa lan theo
                                                    // DONHANG->TRANGTHAITHANHTOAN
            modelBuilder.Entity<DonHang>()
            .HasOne(d => d.TrangThaiThanhToan)
            .WithMany(t => t.DonHangs)
            .HasForeignKey(d => d.MaTrangThaiID)
            .OnDelete(DeleteBehavior.Restrict); // hoặc DeleteBehavior.Cascade nếu bạn muốn xóa lan theo
        
        // DonHang -> NguoiDung



            modelBuilder.Entity<DonHang>()
              .HasOne(dh => dh.KhachHang)
              .WithMany(nd => nd.DonHangs)
              .HasForeignKey(dh => dh.MaKhachHangID)
              .OnDelete(DeleteBehavior.Restrict);

            // DonHang -> GiamGia
            modelBuilder.Entity<DonHang>()
                .HasOne(dh => dh.GiamGia)
                .WithMany(gg => gg.DonHangs)
                .HasForeignKey(dh => dh.MaGiamGiaID)
                .OnDelete(DeleteBehavior.Restrict);

            // HoaDon -> NhanVien
            modelBuilder.Entity<HoaDon>()
              .HasOne(hd => hd.NhanVien)
              .WithMany(nv => nv.HoaDons)
              .HasForeignKey(hd => hd.MaNhanVienID)
              .OnDelete(DeleteBehavior.Restrict);


            // HoaDon -> NhanVien
            modelBuilder.Entity<HoaDon>()
                .HasOne(hd => hd.KhachHang)
                .WithMany(nv => nv.HoaDons)
                .HasForeignKey(hd => hd.MaKhachHangID)
                .OnDelete(DeleteBehavior.Restrict);

            // ChiTietDonHang: many-to-many giữa SanPham và DonHang, liên kết với HoaDon
            modelBuilder.Entity<ChiTietDonHang>()
               .HasKey(ct => new { ct.MaDonHangID, ct.MaSanPhamID });

            modelBuilder.Entity<ChiTietDonHang>()
                .HasOne(ct => ct.SanPham)
                .WithMany(sp => sp.ChiTietDonHangs)
                .HasForeignKey(ct => ct.MaSanPhamID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChiTietDonHang>()
                .HasOne(ct => ct.DonHang)
                .WithMany(dh => dh.ChiTietDonHangs)
                .HasForeignKey(ct => ct.MaDonHangID)
                .OnDelete(DeleteBehavior.Restrict);

            // ChiTietKhoHang: many-to-many giữa SanPham và NhaKho
            modelBuilder.Entity<ChiTietKhoHang>()
                .HasKey(ct => new { ct.MaSanPhamID, ct.MaNhaKhoID });

            modelBuilder.Entity<ChiTietKhoHang>()
                .HasOne(ct => ct.SanPham)
                .WithMany(sp => sp.ChiTietKhoHangs)
                .HasForeignKey(ct => ct.MaSanPhamID);

            modelBuilder.Entity<ChiTietKhoHang>()
                .HasOne(ct => ct.NhaKho)
                .WithMany(nk => nk.ChiTietKhoHangs)
                .HasForeignKey(ct => ct.MaNhaKhoID);

            // DonHang -> VanChuyen N:N
            modelBuilder.Entity<ChiTietVanChuyen>()
            .HasKey(ct => new { ct.MaDonHang, ct.MaVanChuyen });

            modelBuilder.Entity<ChiTietVanChuyen>()
                .HasOne(ct => ct.DonHang)
                .WithMany(dh => dh.ChiTietVanChuyens)
                .HasForeignKey(ct => ct.MaDonHang)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ChiTietVanChuyen>()
                .HasOne(ct => ct.PhieuVanChuyen)
                .WithMany(pvc => pvc.ChiTietVanChuyens)
                .HasForeignKey(ct => ct.MaVanChuyen)
                .OnDelete(DeleteBehavior.Cascade);


            // VanChuyen -> NhanVien
            modelBuilder.Entity<PhieuVanChuyen>()
                .HasOne(vc => vc.NhanVien)
                .WithMany(nv => nv.PhieuVanChuyens)
                .HasForeignKey(vc => vc.MaNhanVienID)
                .OnDelete(DeleteBehavior.Restrict);

            // HoaDon -> ThongKeDoanhThu
            modelBuilder.Entity<ThongKeDoanhThu>()
                .HasOne(tk => tk.HoaDon)
                .WithMany(hd => hd.ThongKeDoanhThus)
                .HasForeignKey(tk => tk.MaHoaDonID)
                .OnDelete(DeleteBehavior.Restrict);
           
            // DanhGia -> SanPham
            modelBuilder.Entity<DanhGia>()
                .HasOne(dg => dg.SanPham)
                .WithMany(sp => sp.DanhGias)
                .HasForeignKey(dg => dg.MaSanPhamID)
                .OnDelete(DeleteBehavior.Restrict);

            // DanhGia -> NguoiDung
            modelBuilder.Entity<DanhGia>()
                .HasOne(dg => dg.KhachHang)
                .WithMany(nd => nd.DanhGias)
                .HasForeignKey(dg => dg.MaKhachHangID)
                .OnDelete(DeleteBehavior.Restrict);


            // HinhAnhSanPham -> SanPham
            modelBuilder.Entity<HinhAnhSanPham>()
                .HasOne(ha => ha.SanPham)
                .WithMany(sp => sp.HinhAnhSanPhams)
                .HasForeignKey(ha => ha.MaSanPhamID)
                .OnDelete(DeleteBehavior.Restrict);

            // ChiTietMua: many-to-many giữa SanPham và Mua
            modelBuilder.Entity<ChiTietMua>()
                .HasKey(ct => new { ct.MaSanPhamID, ct.MaMuaID });

            modelBuilder.Entity<ChiTietMua>()
                .HasOne(ct => ct.SanPham)
                .WithMany(sp => sp.ChiTietMuas)
                .HasForeignKey(ct => ct.MaSanPhamID);

            modelBuilder.Entity<ChiTietMua>()
                .HasOne(ct => ct.Mua)
                .WithMany(m => m.ChiTietMuas)
                .HasForeignKey(ct => ct.MaMuaID);

            //DonHang va PhuongThucThanhToan
            modelBuilder.Entity<DonHang>()
                .HasOne(sp => sp.PhuongThucThanhToan)
                .WithMany(dm => dm.DonHangs)
                .HasForeignKey(sp => sp.MaPTTTID)
                .OnDelete(DeleteBehavior.Restrict);

            //DonHang va HoaDon
            modelBuilder.Entity<HoaDon>()
                .HasOne(sp => sp.DonHang)
                .WithMany(dm => dm.HoaDons)
                .HasForeignKey(sp => sp.MaDonHangID)
                .OnDelete(DeleteBehavior.Restrict);


            // SanPham -> DanhMuc (1 DanhMuc có nhiều SanPham)
            modelBuilder.Entity<SanPham>()
                .HasOne(sp => sp.DanhMuc)
                .WithMany(dm => dm.SanPhams)
                .HasForeignKey(sp => sp.DanhMucID)
                .OnDelete(DeleteBehavior.Cascade);

            // SanPham -> LoaiSanPham (1 LoaiSanPham có nhiều SanPham)
            modelBuilder.Entity<SanPham>()
                .HasOne(sp => sp.LoaiSanPham)
                .WithMany(c => c.SanPhams)
                .HasForeignKey(sp => sp.MaLoaiID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GiamGia>()
                .HasMany(gg => gg.DonHangs)          
                .WithOne(dh => dh.GiamGia)           
                .HasForeignKey(dh => dh.MaGiamGiaID) 
                .OnDelete(DeleteBehavior.Restrict);
          // ChiTietTrangThaiDonHang->DonHang
                modelBuilder.Entity<ChiTietTrangThaiDonHang>()
                    .HasKey(ct => new { ct.MaDonHang, ct.MaTrangThai });

                modelBuilder.Entity<ChiTietTrangThaiDonHang>()
                .HasOne(ct => ct.DonHang)
                .WithMany(dh => dh.ChiTietTrangThaiDonHangs)
                .HasForeignKey(ct => ct.MaDonHang)
                .OnDelete(DeleteBehavior.Restrict);

                modelBuilder.Entity<ChiTietTrangThaiDonHang>()
                .HasOne(ct => ct.TrangThaiDonHang)
                .WithMany(tt => tt.ChiTietTrangThaiDonHangs)
                .HasForeignKey(ct => ct.MaTrangThai)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PhuongThucThanhToan>().HasData(
                new PhuongThucThanhToan { MaPTTT = 1, TenPTTT = "Thanh toán khi nhận hàng" },
                new PhuongThucThanhToan { MaPTTT = 2, TenPTTT = "PayPal" }
            );
            modelBuilder.Entity<TrangThaiDonHang>().HasData(
            new TrangThaiDonHang { MaTrangThai = 1, TenTrangThai = "Đã đặt hàng", GhiChu = "Đơn hàng vừa được tạo" },
            new TrangThaiDonHang { MaTrangThai = 2, TenTrangThai = "Đã xác nhận", GhiChu = "Đơn hàng đã được xác nhận" },
            new TrangThaiDonHang { MaTrangThai = 3, TenTrangThai = "Đang gói hàng", GhiChu = "Đơn hàng đang được chuẩn bị" },
            new TrangThaiDonHang { MaTrangThai = 5, TenTrangThai = "Đang giao hàng", GhiChu = "Đơn hàng đang trên đường giao" },
            new TrangThaiDonHang { MaTrangThai = 6, TenTrangThai = "Đã giao hàng", GhiChu = "Đơn hàng đã giao thành công" },
            new TrangThaiDonHang { MaTrangThai = 7, TenTrangThai = "Đã hủy", GhiChu = "Đơn hàng bị hủy" }
            );
            modelBuilder.Entity<TrangThaiThanhToan>().HasData(
            new TrangThaiThanhToan { MaTrangThai = 1, TenTrangThai = "Chưa thanh toán" },
            new TrangThaiThanhToan { MaTrangThai = 2, TenTrangThai = "Đã thanh toán" }
            );

            //// Thêm dữ liệu mẫu cho bảng SanPham
            //modelBuilder.Entity<SanPham>().HasData(
            //    new SanPham
            //    {
            //        MaSanPham = 1,
            //        NguonGoc = "Việt Nam",
            //        GiaTheoKG = 200000,
            //        NgayThuHoach = new DateTime(2025, 4, 1),
            //        LoaiBaoQuan = "Đông lạnh",
            //        MoTa = "Cá hồi tươi, giàu omega-3",
            //        TenSanPham = "Cá hồi",
            //        TinhTrang = "Mới"
            //    },
            //    new SanPham
            //    {
            //        MaSanPham = 2,
            //        NguonGoc = "Đài Loan",
            //        GiaTheoKG = 150000,
            //        NgayThuHoach = new DateTime(2025, 4, 5),
            //        LoaiBaoQuan = "Bảo quản lạnh",
            //        MoTa = "Tôm sú tươi, ngọt và chắc thịt",
            //        TenSanPham = "Tôm sú",
            //        TinhTrang = "Mới"
            //    },
            //    new SanPham
            //    {
            //        MaSanPham = 3,
            //        NguonGoc = "Thái Lan",
            //        GiaTheoKG = 80000,
            //        NgayThuHoach = new DateTime(2025, 4, 3),
            //        LoaiBaoQuan = "Lạnh đông",
            //        MoTa = "Cá tra, tươi ngon, dễ chế biến",
            //        TenSanPham = "Cá tra",
            //        TinhTrang = "Mới"
            //    }
            //);

            //modelBuilder.Entity<Cang>().HasData(
            //    new Cang
            //    {
            //        MaCang = 1,
            //        TenCang = "Cảng Vũng Tàu",
            //        MoTa = "Cảng chuyên phục vụ hải sản Vũng Tàu"
            //    }
            //);

            //modelBuilder.Entity<ChiTietCang>().HasData(
            //    new ChiTietCang
            //    {
            //        MaDanhMuc = 1,
            //        MaCang = 1
            //    }
            //);


            //// Thêm dữ liệu mẫu cho bảng NhanVien
            //modelBuilder.Entity<NhanVien>().HasData(
            //    new NhanVien
            //    {
            //        MaNhanVien = 1,
            //        TenNhanVien = "Nguyễn Văn A",
            //        SoDienThoai = "0901234567",
            //        Email = "nvana@example.com",
            //        NgayTuyenDung = new DateTime(2020, 1, 1),
            //        ThoiGianLamViec = "8:00 AM - 5:00 PM",
            //        MaNhaKho = 1
            //    },
            //    new NhanVien
            //    {
            //        MaNhanVien = 2,
            //        TenNhanVien = "Trần Thị B",
            //        SoDienThoai = "0912345678",
            //        Email = "nvb@example.com",
            //        NgayTuyenDung = new DateTime(2021, 6, 15),
            //        ThoiGianLamViec = "9:00 AM - 6:00 PM",
            //        MaNhaKho = 2
            //    },
            //    new NhanVien
            //    {
            //        MaNhanVien = 3,
            //        TenNhanVien = "Phạm Minh C",
            //        SoDienThoai = "0923456789",
            //        Email = "nvc@example.com",
            //        NgayTuyenDung = new DateTime(2022, 3, 10),
            //        ThoiGianLamViec = "7:00 AM - 4:00 PM",
            //        MaNhaKho = 3
            //    }
            //);
            // Thêm dữ liệu mẫu cho bảng Cang
            //modelBuilder.Entity<Cang>().HasData(
            //    new Cang
            //    {
            //        MaCang = 1,
            //        TenCang = "Cảng Hải Phòng",
            //        MoTa = "Cảng chính của miền Bắc, giao thương quốc tế."
            //    },
            //    new Cang
            //    {
            //        MaCang = 2,
            //        TenCang = "Cảng Đà Nẵng",
            //        MoTa = "Cảng trung tâm miền Trung, kết nối các cảng trong khu vực."
            //    },
            //    new Cang
            //    {
            //        MaCang = 3,
            //        TenCang = "Cảng Cần Thơ",
            //        MoTa = "Cảng quan trọng của miền Tây, kết nối với đồng bằng sông Cửu Long."
            //    }
            //);

            //// Thêm dữ liệu mẫu cho bảng ChucVu
            //modelBuilder.Entity<ChucVu>().HasData(
            //    new ChucVu
            //    {
            //        MaChucVu = 1,
            //        TenChucVu = "Giám Đốc",
            //        MaNhanVien = 1 // Ví dụ MaNhanVien là 1
            //    },
            //    new ChucVu
            //    {
            //        MaChucVu = 2,
            //        TenChucVu = "Trưởng Phòng",
            //        MaNhanVien = 2 // Ví dụ MaNhanVien là 2
            //    },
            //    new ChucVu
            //    {
            //        MaChucVu = 3,
            //        TenChucVu = "Nhân Viên",
            //        MaNhanVien = 3 // Ví dụ MaNhanVien là 3
            //    }
            //);

            //// Thêm dữ liệu mẫu cho bảng DanhGia
            //modelBuilder.Entity<DanhGia>().HasData(
            //    new DanhGia
            //    {
            //        MaDanhGia = 1,
            //        XepHang = 5,
            //        BinhLuan = "Sản phẩm rất tốt, chất lượng tuyệt vời!",
            //        NgayDanhGia = new DateTime(2025, 4, 17),
            //        MaSanPham = 1, // Giả sử sản phẩm có MaSanPham là 1
            //        MaNguoiDung = 1 // Giả sử người dùng có MaNguoiDung là 1
            //    },
            //    new DanhGia
            //    {
            //        MaDanhGia = 2,
            //        XepHang = 4,
            //        BinhLuan = "Sản phẩm ổn, nhưng có thể cải thiện chất lượng.",
            //        NgayDanhGia = new DateTime(2025, 4, 16),
            //        MaSanPham = 2, // Giả sử sản phẩm có MaSanPham là 2
            //        MaNguoiDung = 2 // Giả sử người dùng có MaNguoiDung là 2
            //    },
            //    new DanhGia
            //    {
            //        MaDanhGia = 3,
            //        XepHang = 3,
            //        BinhLuan = "Sản phẩm tạm được, có thể tốt hơn.",
            //        NgayDanhGia = new DateTime(2025, 4, 15),
            //        MaSanPham = 3, // Giả sử sản phẩm có MaSanPham là 3
            //        MaNguoiDung = 3 // Giả sử người dùng có MaNguoiDung là 3
            //    }
            //);

            //// Thêm dữ liệu mẫu cho bảng DanhMuc
            //modelBuilder.Entity<DanhMuc>().HasData(
            //    new DanhMuc
            //    {
            //        MaDanhMuc = 1,
            //        TenDanhMuc = "Hải sản tươi sống",
            //        MoTa = "Danh mục bao gồm các loại hải sản tươi sống như cá, tôm, cua, sò,..."
            //    }
            //);
            //    new DanhMuc
            //    {
            //        MaDanhMuc = 2,
            //        TenDanhMuc = "Hải sản khô",
            //        MoTa = "Danh mục bao gồm các loại hải sản khô, tôm khô, mực khô, cá khô,..."
            //    },
            //    new DanhMuc
            //    {
            //        MaDanhMuc = 3,
            //        TenDanhMuc = "Nguyên liệu chế biến",
            //        MoTa = "Danh mục bao gồm các nguyên liệu chế biến hải sản như gia vị, dầu ăn, bột chiên,..."
            //    }
            //);

            //// Thêm dữ liệu mẫu cho bảng DonHang
            //modelBuilder.Entity<DonHang>().HasData(
            //    new DonHang
            //    {
            //        MaDonHang = 1,
            //        NgayDatHang = DateTime.Parse("2025-04-10"),
            //        TongSoTien = 500000m,
            //        MaNguoiDung = 1,  // Giả sử có một NguoiDung với MaNguoiDung = 1
            //        MaNhaKho = 1,     // Giả sử có một NhaKho với MaNhaKho = 1
            //        MaGiamGia = 1,    // Giả sử có một GiamGia với MaGiamGia = 1
            //    },
            //    new DonHang
            //    {
            //        MaDonHang = 2,
            //        NgayDatHang = DateTime.Parse("2025-04-12"),
            //        TongSoTien = 700000m,
            //        MaNguoiDung = 2,  // Giả sử có một NguoiDung với MaNguoiDung = 2
            //        MaNhaKho = 2,     // Giả sử có một NhaKho với MaNhaKho = 2
            //        MaGiamGia = 2,    // Giả sử có một GiamGia với MaGiamGia = 2
            //    }
            //);

            //// Thêm dữ liệu mẫu cho bảng DuLieuDanhBat
            //modelBuilder.Entity<DuLieuDanhBat>().HasData(
            //    new DuLieuDanhBat
            //    {
            //        MaDuLieuDanhBat = 1,
            //        NgayDanhBat = DateTime.Parse("2025-04-10"),
            //        SoLuongDanhBat = 100,
            //        ThoiTiet = "Nắng",
            //        MaNguDan = 1,  // Giả sử có một NguDan với MaNguDan = 1
            //        MaNhanVien = 1, // Giả sử có một NhanVien với MaNhanVien = 1
            //        MaNguoiDung = 1, // Giả sử có một NguoiDung với MaNguoiDung = 1
            //    },
            //    new DuLieuDanhBat
            //    {
            //        MaDuLieuDanhBat = 2,
            //        NgayDanhBat = DateTime.Parse("2025-04-12"),
            //        SoLuongDanhBat = 150,
            //        ThoiTiet = "Mưa",
            //        MaNguDan = 2,  // Giả sử có một NguDan với MaNguDan = 2
            //        MaNhanVien = 2, // Giả sử có một NhanVien với MaNhanVien = 2
            //        MaNguoiDung = 2, // Giả sử có một NguoiDung với MaNguoiDung = 2
            //    }
            //);

            //// Thêm dữ liệu mẫu cho bảng GiamGia
            //modelBuilder.Entity<GiamGia>().HasData(
            //    new GiamGia
            //    {
            //        MaGiamGia = 1,
            //        SoLuongConLai = 100,
            //    },
            //    new GiamGia
            //    {
            //        MaGiamGia = 2,
            //        SoLuongConLai = 50,
            //    }
            //);


            //// Thêm dữ liệu mẫu cho bảng HinhAnhSanPham
            //modelBuilder.Entity<HinhAnhSanPham>().HasData(
            //    new HinhAnhSanPham
            //    {
            //        MaHinhAnhSanPham = 1,
            //        LoaiHinhAnh = "Hình ảnh chính",
            //        MaSanPham = 1
            //    },
            //    new HinhAnhSanPham
            //    {
            //        MaHinhAnhSanPham = 2,
            //        LoaiHinhAnh = "Hình ảnh phụ",
            //        MaSanPham = 1
            //    }
            //);

            //// Thêm dữ liệu mẫu cho bảng HoaDon
            //modelBuilder.Entity<HoaDon>().HasData(
            //    new HoaDon
            //    {
            //        MaHoaDon = 1,
            //        TongSoTien = 150000,
            //        NgayTaoHoaDon = new DateTime(2025, 4, 15),
            //        MaNhanVien = 1 // Giả sử nhân viên có MaNhanVien = 1
            //    },
            //    new HoaDon
            //    {
            //        MaHoaDon = 2,
            //        TongSoTien = 200000,
            //        NgayTaoHoaDon = new DateTime(2025, 4, 16),
            //        MaNhanVien = 2 // Giả sử nhân viên có MaNhanVien = 2
            //    }
            //);

            //// Thêm dữ liệu mẫu cho bảng Mua với các mùa
            //modelBuilder.Entity<Mua>().HasData(
            //    new Mua
            //    {
            //        MaMua = 1,
            //        TenMua = "Mùa Xuân"
            //    },
            //    new Mua
            //    {
            //        MaMua = 2,
            //        TenMua = "Mùa Hạ"
            //    },
            //    new Mua
            //    {
            //        MaMua = 3,
            //        TenMua = "Mùa Thu"
            //    },
            //    new Mua
            //    {
            //        MaMua = 4,
            //        TenMua = "Mùa Đông"
            //    }
            //);

            //// Thêm dữ liệu mẫu cho bảng NguDan
            //modelBuilder.Entity<NguDan>().HasData(
            //    new NguDan
            //    {
            //        MaNguDan = 1,
            //        TenTau = "Tau A123",
            //        ViTriCang = "Cảng Hải Phòng",
            //        LoaiDanhBatPhoBien = "Cá cơm",
            //        SoLuongDuKien = 1000,
            //        VungDanhBat = "Vùng 1",
            //        SoGiayPhep = "GP123456",
            //        MaNguoiDung = 1  // Liên kết với người dùng (MaNguoiDung)
            //    },
            //    new NguDan
            //    {
            //        MaNguDan = 2,
            //        TenTau = "Tau B456",
            //        ViTriCang = "Cảng Đà Nẵng",
            //        LoaiDanhBatPhoBien = "Tôm hùm",
            //        SoLuongDuKien = 500,
            //        VungDanhBat = "Vùng 2",
            //        SoGiayPhep = "GP654321",
            //        MaNguoiDung = 2  // Liên kết với người dùng khác (MaNguoiDung)
            //    }
            //);

            //// Thêm dữ liệu mẫu cho bảng NguoiDung
            //modelBuilder.Entity<NguoiDung>().HasData(
            //    new NguoiDung
            //    {
            //        MaNguoiDung = 1,
            //        TenNguoiDung = "Nguyen Van A",
            //        SoDienThoai = "0123456789",
            //        Email = "nguyenvana@example.com",
            //        MatKhau = "password123",  // Trong thực tế, mật khẩu nên được mã hóa
            //        VaiTro = "Ngư dân",
            //        DiaChi = "Hải Phòng, Việt Nam",
            //        NgaySinh = new DateTime(1990, 5, 15)
            //    },
            //    new NguoiDung
            //    {
            //        MaNguoiDung = 2,
            //        TenNguoiDung = "Tran Thi B",
            //        SoDienThoai = "0987654321",
            //        Email = "tranthib@example.com",
            //        MatKhau = "password456",  // Trong thực tế, mật khẩu nên được mã hóa
            //        VaiTro = "Khách hàng",
            //        DiaChi = "Hà Nội, Việt Nam",
            //        NgaySinh = new DateTime(1985, 10, 25)
            //    }
            //);

            //// Thêm dữ liệu mẫu cho bảng NhaKho
            //modelBuilder.Entity<NhaKho>().HasData(
            //    new NhaKho
            //    {
            //        MaNhaKho = 1,
            //        TenNhaKho = "Kho Hải Sản A",
            //        DiaChi = "Số 123, Đường ABC, Hải Phòng",
            //        SoDienThoai = "0987654321",
            //        NgayTao = new DateTime(2020, 1, 1)
            //    },
            //    new NhaKho
            //    {
            //        MaNhaKho = 2,
            //        TenNhaKho = "Kho Hải Sản B",
            //        DiaChi = "Số 456, Đường XYZ, Hà Nội",
            //        SoDienThoai = "0912345678",
            //        NgayTao = new DateTime(2021, 3, 15)
            //    },
            //    new NhaKho
            //    {
            //        MaNhaKho = 3,
            //        TenNhaKho = "Kho Hải Sản C",
            //        DiaChi = "Số 789, Đường LMN, Đà Nẵng",
            //        SoDienThoai = "0976543210",
            //        NgayTao = new DateTime(2022, 6, 10)
            //    }
            //);

            //// Thêm dữ liệu mẫu cho bảng PhanTichNhuCau
            //modelBuilder.Entity<PhanTichNhuCau>().HasData(
            //    new PhanTichNhuCau
            //    {
            //        MaPhanTich = 1,
            //        NgayPhanTich = new DateTime(2024, 4, 1),
            //        UocTinhNhuCau = "Tăng trưởng mạnh trong mùa hè 2024 với nhu cầu cao về hải sản tươi sống.",
            //        XuHuongThiTruong = "Tăng cường nhập khẩu hải sản từ các nước Đông Nam Á, với xu hướng tiêu thụ sản phẩm hữu cơ và sạch."
            //    },
            //    new PhanTichNhuCau
            //    {
            //        MaPhanTich = 2,
            //        NgayPhanTich = new DateTime(2024, 4, 10),
            //        UocTinhNhuCau = "Nhu cầu tăng trưởng cho sản phẩm cá hồi và tôm vào mùa đông 2024.",
            //        XuHuongThiTruong = "Sự gia tăng tiêu thụ sản phẩm đông lạnh, với thị trường mở rộng tại các quốc gia Châu Âu."
            //    },
            //    new PhanTichNhuCau
            //    {
            //        MaPhanTich = 3,
            //        NgayPhanTich = new DateTime(2024, 4, 15),
            //        UocTinhNhuCau = "Nhu cầu giảm cho các sản phẩm cá ngừ, nhưng tăng cho các sản phẩm thủy hải sản chế biến sẵn.",
            //        XuHuongThiTruong = "Tăng trưởng mạnh mẽ trong phân khúc thực phẩm chế biến sẵn và đóng gói sẵn."
            //    }
            //);

            //// Thêm dữ liệu mẫu cho bảng PhuongThucThanhToan
            //modelBuilder.Entity<PhuongThucThanhToan>().HasData(
            //    new PhuongThucThanhToan
            //    {
            //        MaPTTT = 1,
            //        TrangThaiThanhToan = "Đã thanh toán",
            //        MaDonHang = 1  // Mã đơn hàng phù hợp
            //    },
            //    new PhuongThucThanhToan
            //    {
            //        MaPTTT = 2,
            //        TrangThaiThanhToan = "Chưa thanh toán",
            //        MaDonHang = 2  // Mã đơn hàng phù hợp
            //    },
            //    new PhuongThucThanhToan
            //    {
            //        MaPTTT = 3,
            //        TrangThaiThanhToan = "Đang xử lý",
            //        MaDonHang = 3  // Mã đơn hàng phù hợp
            //    }
            //);

            //// Thêm dữ liệu mẫu cho bảng VanChuyen
            //modelBuilder.Entity<VanChuyen>().HasData(
            //    new VanChuyen
            //    {
            //        MaVanChuyen = 1,
            //        DiaDiemBatDau = "Cảng A",
            //        DiemDen = "Cảng B",
            //        LoaiPhuongTien = "Xe tải",
            //        NhietDo = "30°C",
            //        DoAm = "80%",
            //        MaDonHang = 1,  // Mã đơn hàng tương ứng
            //        MaNhanVien = 1  // Mã nhân viên tương ứng
            //    },
            //    new VanChuyen
            //    {
            //        MaVanChuyen = 2,
            //        DiaDiemBatDau = "Cảng C",
            //        DiemDen = "Cảng D",
            //        LoaiPhuongTien = "Tàu thủy",
            //        NhietDo = "25°C",
            //        DoAm = "70%",
            //        MaDonHang = 2,  // Mã đơn hàng tương ứng
            //        MaNhanVien = 2  // Mã nhân viên tương ứng
            //    },
            //    new VanChuyen
            //    {
            //        MaVanChuyen = 3,
            //        DiaDiemBatDau = "Cảng E",
            //        DiemDen = "Cảng F",
            //        LoaiPhuongTien = "Xe container",
            //        NhietDo = "28°C",
            //        DoAm = "75%",
            //        MaDonHang = 3,  // Mã đơn hàng tương ứng
            //        MaNhanVien = 3  // Mã nhân viên tương ứng
            //    }
            //);

            //// Thêm dữ liệu mẫu cho bảng ChiTietCang
            //modelBuilder.Entity<ChiTietCang>().HasData(
            //    new ChiTietCang
            //    {
            //        MaDanhMuc = 1,   // Mã danh mục
            //        MaCang = 1,      // Mã cảng (phải có dữ liệu trong bảng Cang)
            //    },
            //    new ChiTietCang
            //    {
            //        MaDanhMuc = 2,   // Mã danh mục
            //        MaCang = 2,      // Mã cảng (phải có dữ liệu trong bảng Cang)
            //    },
            //    new ChiTietCang
            //    {
            //        MaDanhMuc = 3,   // Mã danh mục
            //        MaCang = 3,      // Mã cảng (phải có dữ liệu trong bảng Cang)
            //    }
            //);

            //// Thêm dữ liệu mẫu cho bảng ChiTietDanhMuc
            //modelBuilder.Entity<ChiTietDanhMuc>().HasData(
            //    new ChiTietDanhMuc
            //    {
            //        MaSanPham = 1,  // Mã sản phẩm tương ứng
            //        MaDanhMuc = 1,  // Mã danh mục tương ứng
            //        HetHan = DateTime.Now.AddMonths(6), // Hạn sử dụng 6 tháng
            //    },
            //    new ChiTietDanhMuc
            //    {
            //        MaSanPham = 2,  // Mã sản phẩm tương ứng
            //        MaDanhMuc = 2,  // Mã danh mục tương ứng
            //        HetHan = DateTime.Now.AddMonths(12), // Hạn sử dụng 12 tháng
            //    },
            //    new ChiTietDanhMuc
            //    {
            //        MaSanPham = 3,  // Mã sản phẩm tương ứng
            //        MaDanhMuc = 3,  // Mã danh mục tương ứng
            //        HetHan = DateTime.Now.AddMonths(3), // Hạn sử dụng 3 tháng
            //    }
            //);

            //// Thêm dữ liệu mẫu cho bảng ChiTietDonHang
            //modelBuilder.Entity<ChiTietDonHang>().HasData(
            //    new ChiTietDonHang
            //    {
            //        MaSanPham = 1,  // Mã sản phẩm tương ứng
            //        MaDonHang = 1,  // Mã đơn hàng tương ứng
            //        SoLuong = 5,    // Số lượng sản phẩm
            //        MaHoaDon = 1    // Mã hóa đơn tương ứng
            //    },
            //    new ChiTietDonHang
            //    {
            //        MaSanPham = 2,  // Mã sản phẩm tương ứng
            //        MaDonHang = 1,  // Mã đơn hàng tương ứng
            //        SoLuong = 3,    // Số lượng sản phẩm
            //        MaHoaDon = 1    // Mã hóa đơn tương ứng
            //    },
            //    new ChiTietDonHang
            //    {
            //        MaSanPham = 3,  // Mã sản phẩm tương ứng
            //        MaDonHang = 2,  // Mã đơn hàng tương ứng
            //        SoLuong = 7,    // Số lượng sản phẩm
            //        MaHoaDon = 2    // Mã hóa đơn tương ứng
            //    }
            //);

            //// Thêm dữ liệu mẫu cho bảng ChiTietKhoHang
            //modelBuilder.Entity<ChiTietKhoHang>().HasData(
            //    new ChiTietKhoHang
            //    {
            //        MaSanPham = 1, // Mã sản phẩm tương ứng
            //        MaNhaKho = 1,  // Mã nhà kho tương ứng
            //        SoLuong = 100, // Số lượng sản phẩm trong kho
            //        NgayHetHan = new DateTime(2025, 12, 31) // Ngày hết hạn của sản phẩm
            //    },
            //    new ChiTietKhoHang
            //    {
            //        MaSanPham = 2, // Mã sản phẩm tương ứng
            //        MaNhaKho = 1,  // Mã nhà kho tương ứng
            //        SoLuong = 150, // Số lượng sản phẩm trong kho
            //        NgayHetHan = new DateTime(2025, 11, 30) // Ngày hết hạn của sản phẩm
            //    },
            //    new ChiTietKhoHang
            //    {
            //        MaSanPham = 3, // Mã sản phẩm tương ứng
            //        MaNhaKho = 2,  // Mã nhà kho tương ứng
            //        SoLuong = 200, // Số lượng sản phẩm trong kho
            //        NgayHetHan = new DateTime(2026, 01, 15) // Ngày hết hạn của sản phẩm
            //    }
            //);

            //// Thêm dữ liệu mẫu cho bảng ChiTietMua
            //modelBuilder.Entity<ChiTietMua>().HasData(
            //    new ChiTietMua
            //    {
            //        MaSanPham = 1, // Mã sản phẩm tương ứng
            //        MaMua = 1,     // Mã mùa tương ứng
            //    },
            //    new ChiTietMua
            //    {
            //        MaSanPham = 2, // Mã sản phẩm tương ứng
            //        MaMua = 1,     // Mã mùa tương ứng
            //    },
            //    new ChiTietMua
            //    {
            //        MaSanPham = 3, // Mã sản phẩm tương ứng
            //        MaMua = 2,     // Mã mùa tương ứng
            //    }
            //);

            //// Thêm dữ liệu mẫu cho bảng ChiTietNhanVien
            //modelBuilder.Entity<ChiTietNhanVien>().HasData(
            //    new ChiTietNhanVien
            //    {
            //        MaSanPham = 1,    // Mã sản phẩm tương ứng
            //        MaNhanVien = 1,   // Mã nhân viên tương ứng
            //        LoaiHanhDong = "Xuất kho" // Loại hành động tương ứng
            //    },
            //    new ChiTietNhanVien
            //    {
            //        MaSanPham = 2,    // Mã sản phẩm tương ứng
            //        MaNhanVien = 2,   // Mã nhân viên tương ứng
            //        LoaiHanhDong = "Nhập kho" // Loại hành động tương ứng
            //    },
            //    new ChiTietNhanVien
            //    {
            //        MaSanPham = 3,    // Mã sản phẩm tương ứng
            //        MaNhanVien = 1,   // Mã nhân viên tương ứng
            //        LoaiHanhDong = "Kiểm tra" // Loại hành động tương ứng
            //    }
            //);

            //// Thêm dữ liệu mẫu cho bảng ChiTietSanPham
            //modelBuilder.Entity<ChiTietSanPham>().HasData(
            //    new ChiTietSanPham
            //    {
            //        MaDuLieuDanhBat = 1,    // Mã dữ liệu danh bắt
            //        MaSanPham = 1,         // Mã sản phẩm
            //    },
            //    new ChiTietSanPham
            //    {
            //        MaDuLieuDanhBat = 2,    // Mã dữ liệu danh bắt
            //        MaSanPham = 2,         // Mã sản phẩm
            //    },
            //    new ChiTietSanPham
            //    {
            //        MaDuLieuDanhBat = 3,    // Mã dữ liệu danh bắt
            //        MaSanPham = 3,         // Mã sản phẩm
            //    }
            //);
        }
    }
}
