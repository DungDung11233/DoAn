using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DoAnCoSo.Migrations
{
    /// <inheritdoc />
    public partial class taomoi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Age = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cangs",
                columns: table => new
                {
                    MaCang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenCang = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cangs", x => x.MaCang);
                });

            migrationBuilder.CreateTable(
                name: "DanhMucs",
                columns: table => new
                {
                    MaDanhMuc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDanhMuc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhMucs", x => x.MaDanhMuc);
                });

            migrationBuilder.CreateTable(
                name: "GiamGias",
                columns: table => new
                {
                    MaGiamGia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    GiamTheoSoTien = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GiamTheoPhanTram = table.Column<double>(type: "float", nullable: true),
                    NgayApDung = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoLanToiDaDungMa = table.Column<int>(type: "int", nullable: false),
                    SoLanDungMa = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiamGias", x => x.MaGiamGia);
                });

            migrationBuilder.CreateTable(
                name: "LoaiSanPhams",
                columns: table => new
                {
                    MaLoai = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenLoai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoaiSanPhams", x => x.MaLoai);
                });

            migrationBuilder.CreateTable(
                name: "Muas",
                columns: table => new
                {
                    MaMua = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenMua = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ThoiGianVaoMua = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThoiGianHetMua = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Muas", x => x.MaMua);
                });

            migrationBuilder.CreateTable(
                name: "NguDans",
                columns: table => new
                {
                    MaNguDan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNguDan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenTau = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoLuongDuKien = table.Column<int>(type: "int", nullable: false),
                    VungDanhBat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoGiayPhep = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NguDans", x => x.MaNguDan);
                });

            migrationBuilder.CreateTable(
                name: "NguoiDungs",
                columns: table => new
                {
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNguoiDung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VaiTro = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NguoiDungs", x => x.MaNguoiDung);
                });

            migrationBuilder.CreateTable(
                name: "NhaKhos",
                columns: table => new
                {
                    MaNhaKho = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNhaKho = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LoaiKho = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhaKhos", x => x.MaNhaKho);
                });

            migrationBuilder.CreateTable(
                name: "NhanViens",
                columns: table => new
                {
                    MaNhanVien = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNhanVien = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayTuyenDung = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThoiGianLamViec = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhanViens", x => x.MaNhanVien);
                });

            migrationBuilder.CreateTable(
                name: "PhuongThucThanhToans",
                columns: table => new
                {
                    MaPTTT = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenPTTT = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhuongThucThanhToans", x => x.MaPTTT);
                });

            migrationBuilder.CreateTable(
                name: "TrangThaiDonHangs",
                columns: table => new
                {
                    MaTrangThai = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenTrangThai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrangThaiDonHangs", x => x.MaTrangThai);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SanPhams",
                columns: table => new
                {
                    MaSanPham = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenSanPham = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: true),
                    NguonGoc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GiaTheoKG = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NgayThuHoach = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LoaiBaoQuan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TinhTrang = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DanhMucID = table.Column<int>(type: "int", nullable: true),
                    MaLoaiID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanPhams", x => x.MaSanPham);
                    table.ForeignKey(
                        name: "FK_SanPhams_DanhMucs_DanhMucID",
                        column: x => x.DanhMucID,
                        principalTable: "DanhMucs",
                        principalColumn: "MaDanhMuc",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SanPhams_LoaiSanPhams_MaLoaiID",
                        column: x => x.MaLoaiID,
                        principalTable: "LoaiSanPhams",
                        principalColumn: "MaLoai",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DuLieuDanhBats",
                columns: table => new
                {
                    MaDuLieuDanhBat = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NgayDanhBat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoLuongDanhBat = table.Column<int>(type: "int", nullable: false),
                    ThoiTiet = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaNguDanID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DuLieuDanhBats", x => x.MaDuLieuDanhBat);
                    table.ForeignKey(
                        name: "FK_DuLieuDanhBats_NguDans_MaNguDanID",
                        column: x => x.MaNguDanID,
                        principalTable: "NguDans",
                        principalColumn: "MaNguDan",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VanChuyens",
                columns: table => new
                {
                    MaVanChuyen = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoaiPhuongTien = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrangThaiVanChuyen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaNhanVienID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VanChuyens", x => x.MaVanChuyen);
                    table.ForeignKey(
                        name: "FK_VanChuyens_NhanViens_MaNhanVienID",
                        column: x => x.MaNhanVienID,
                        principalTable: "NhanViens",
                        principalColumn: "MaNhanVien",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietCangs",
                columns: table => new
                {
                    MaSanPhamID = table.Column<int>(type: "int", nullable: false),
                    MaCangID = table.Column<int>(type: "int", nullable: false),
                    NgayNhapCang = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SoLuongNhap = table.Column<int>(type: "int", nullable: true),
                    TrangThaiHang = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietCangs", x => new { x.MaSanPhamID, x.MaCangID });
                    table.ForeignKey(
                        name: "FK_ChiTietCangs_Cangs_MaCangID",
                        column: x => x.MaCangID,
                        principalTable: "Cangs",
                        principalColumn: "MaCang",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietCangs_SanPhams_MaSanPhamID",
                        column: x => x.MaSanPhamID,
                        principalTable: "SanPhams",
                        principalColumn: "MaSanPham",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietKhoHangs",
                columns: table => new
                {
                    MaSanPhamID = table.Column<int>(type: "int", nullable: false),
                    MaNhaKhoID = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    NgayNhap = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietKhoHangs", x => new { x.MaSanPhamID, x.MaNhaKhoID });
                    table.ForeignKey(
                        name: "FK_ChiTietKhoHangs_NhaKhos_MaNhaKhoID",
                        column: x => x.MaNhaKhoID,
                        principalTable: "NhaKhos",
                        principalColumn: "MaNhaKho",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietKhoHangs_SanPhams_MaSanPhamID",
                        column: x => x.MaSanPhamID,
                        principalTable: "SanPhams",
                        principalColumn: "MaSanPham",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietMuas",
                columns: table => new
                {
                    MaSanPhamID = table.Column<int>(type: "int", nullable: false),
                    MaMuaID = table.Column<int>(type: "int", nullable: false),
                    SoLuongTheoMua = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietMuas", x => new { x.MaSanPhamID, x.MaMuaID });
                    table.ForeignKey(
                        name: "FK_ChiTietMuas_Muas_MaMuaID",
                        column: x => x.MaMuaID,
                        principalTable: "Muas",
                        principalColumn: "MaMua",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietMuas_SanPhams_MaSanPhamID",
                        column: x => x.MaSanPhamID,
                        principalTable: "SanPhams",
                        principalColumn: "MaSanPham",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DanhGias",
                columns: table => new
                {
                    MaDanhGia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    XepHang = table.Column<int>(type: "int", nullable: false),
                    BinhLuan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayDanhGia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaSanPhamID = table.Column<int>(type: "int", nullable: true),
                    MaNguoiDungID = table.Column<int>(type: "int", nullable: true),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhGias", x => x.MaDanhGia);
                    table.ForeignKey(
                        name: "FK_DanhGias_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DanhGias_NguoiDungs_MaNguoiDungID",
                        column: x => x.MaNguoiDungID,
                        principalTable: "NguoiDungs",
                        principalColumn: "MaNguoiDung",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DanhGias_SanPhams_MaSanPhamID",
                        column: x => x.MaSanPhamID,
                        principalTable: "SanPhams",
                        principalColumn: "MaSanPham",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HinhAnhSanPhams",
                columns: table => new
                {
                    MaHinhAnhSanPham = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoaiHinhAnh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaSanPhamID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HinhAnhSanPhams", x => x.MaHinhAnhSanPham);
                    table.ForeignKey(
                        name: "FK_HinhAnhSanPhams_SanPhams_MaSanPhamID",
                        column: x => x.MaSanPhamID,
                        principalTable: "SanPhams",
                        principalColumn: "MaSanPham",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietDanhBats",
                columns: table => new
                {
                    MaDuLieuDanhBatID = table.Column<int>(type: "int", nullable: false),
                    MaSanPhamID = table.Column<int>(type: "int", nullable: false),
                    SoLuongSanPham = table.Column<int>(type: "int", nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietDanhBats", x => new { x.MaDuLieuDanhBatID, x.MaSanPhamID });
                    table.ForeignKey(
                        name: "FK_ChiTietDanhBats_DuLieuDanhBats_MaDuLieuDanhBatID",
                        column: x => x.MaDuLieuDanhBatID,
                        principalTable: "DuLieuDanhBats",
                        principalColumn: "MaDuLieuDanhBat",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietDanhBats_SanPhams_MaSanPhamID",
                        column: x => x.MaSanPhamID,
                        principalTable: "SanPhams",
                        principalColumn: "MaSanPham",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DonHangs",
                columns: table => new
                {
                    MaDonHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NgayDatHang = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TongSoTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiaChiGiaoHang = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaNguoiDungID = table.Column<int>(type: "int", nullable: true),
                    NguoiDungMaNguoiDung = table.Column<int>(type: "int", nullable: true),
                    MaGiamGiaID = table.Column<int>(type: "int", nullable: true),
                    MaVanChuyenID = table.Column<int>(type: "int", nullable: true),
                    MaPTTTID = table.Column<int>(type: "int", nullable: true),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonHangs", x => x.MaDonHang);
                    table.ForeignKey(
                        name: "FK_DonHangs_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DonHangs_GiamGias_MaGiamGiaID",
                        column: x => x.MaGiamGiaID,
                        principalTable: "GiamGias",
                        principalColumn: "MaGiamGia",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DonHangs_NguoiDungs_NguoiDungMaNguoiDung",
                        column: x => x.NguoiDungMaNguoiDung,
                        principalTable: "NguoiDungs",
                        principalColumn: "MaNguoiDung");
                    table.ForeignKey(
                        name: "FK_DonHangs_PhuongThucThanhToans_MaPTTTID",
                        column: x => x.MaPTTTID,
                        principalTable: "PhuongThucThanhToans",
                        principalColumn: "MaPTTT",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DonHangs_VanChuyens_MaVanChuyenID",
                        column: x => x.MaVanChuyenID,
                        principalTable: "VanChuyens",
                        principalColumn: "MaVanChuyen",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietDonHangs",
                columns: table => new
                {
                    MaSanPhamID = table.Column<int>(type: "int", nullable: false),
                    MaDonHangID = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietDonHangs", x => new { x.MaDonHangID, x.MaSanPhamID });
                    table.ForeignKey(
                        name: "FK_ChiTietDonHangs_DonHangs_MaDonHangID",
                        column: x => x.MaDonHangID,
                        principalTable: "DonHangs",
                        principalColumn: "MaDonHang",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChiTietDonHangs_SanPhams_MaSanPhamID",
                        column: x => x.MaSanPhamID,
                        principalTable: "SanPhams",
                        principalColumn: "MaSanPham",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietTrangThaiDonHangs",
                columns: table => new
                {
                    MaDonHang = table.Column<int>(type: "int", nullable: false),
                    MaTrangThai = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietTrangThaiDonHangs", x => new { x.MaDonHang, x.MaTrangThai });
                    table.ForeignKey(
                        name: "FK_ChiTietTrangThaiDonHangs_DonHangs_MaDonHang",
                        column: x => x.MaDonHang,
                        principalTable: "DonHangs",
                        principalColumn: "MaDonHang",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChiTietTrangThaiDonHangs_TrangThaiDonHangs_MaTrangThai",
                        column: x => x.MaTrangThai,
                        principalTable: "TrangThaiDonHangs",
                        principalColumn: "MaTrangThai",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HoaDons",
                columns: table => new
                {
                    MaHoaDon = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TongSoTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NgayTaoHoaDon = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaNhanVienID = table.Column<int>(type: "int", nullable: true),
                    MaNguoiDungID = table.Column<int>(type: "int", nullable: true),
                    MaDonHangID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoaDons", x => x.MaHoaDon);
                    table.ForeignKey(
                        name: "FK_HoaDons_DonHangs_MaDonHangID",
                        column: x => x.MaDonHangID,
                        principalTable: "DonHangs",
                        principalColumn: "MaDonHang");
                    table.ForeignKey(
                        name: "FK_HoaDons_NguoiDungs_MaNguoiDungID",
                        column: x => x.MaNguoiDungID,
                        principalTable: "NguoiDungs",
                        principalColumn: "MaNguoiDung",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HoaDons_NhanViens_MaNhanVienID",
                        column: x => x.MaNhanVienID,
                        principalTable: "NhanViens",
                        principalColumn: "MaNhanVien",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "PhuongThucThanhToans",
                columns: new[] { "MaPTTT", "TenPTTT", "TrangThai" },
                values: new object[,]
                {
                    { 1, "Thanh toán khi nhận hàng", "Hoạt động" },
                    { 2, "PayPal", "Hoạt động" }
                });

            migrationBuilder.InsertData(
                table: "TrangThaiDonHangs",
                columns: new[] { "MaTrangThai", "GhiChu", "TenTrangThai" },
                values: new object[,]
                {
                    { 1, "Đơn hàng vừa được tạo", "Đã đặt hàng" },
                    { 2, "Đơn hàng đã được xác nhận", "Đã xác nhận" },
                    { 3, "Đơn hàng đang được chuẩn bị", "Đang gói hàng" },
                    { 4, "Đơn hàng đã bàn giao vận chuyển", "Đã giao cho đơn vị vận chuyển" },
                    { 5, "Đơn hàng đang trên đường giao", "Đang giao hàng" },
                    { 6, "Đơn hàng đã giao thành công", "Đã giao hàng" },
                    { 7, "Đơn hàng bị hủy", "Đã hủy" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietCangs_MaCangID",
                table: "ChiTietCangs",
                column: "MaCangID");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDanhBats_MaSanPhamID",
                table: "ChiTietDanhBats",
                column: "MaSanPhamID");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonHangs_MaSanPhamID",
                table: "ChiTietDonHangs",
                column: "MaSanPhamID");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietKhoHangs_MaNhaKhoID",
                table: "ChiTietKhoHangs",
                column: "MaNhaKhoID");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietMuas_MaMuaID",
                table: "ChiTietMuas",
                column: "MaMuaID");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietTrangThaiDonHangs_MaTrangThai",
                table: "ChiTietTrangThaiDonHangs",
                column: "MaTrangThai");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGias_ApplicationUserId",
                table: "DanhGias",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGias_MaNguoiDungID",
                table: "DanhGias",
                column: "MaNguoiDungID");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGias_MaSanPhamID",
                table: "DanhGias",
                column: "MaSanPhamID");

            migrationBuilder.CreateIndex(
                name: "IX_DonHangs_MaGiamGiaID",
                table: "DonHangs",
                column: "MaGiamGiaID");

            migrationBuilder.CreateIndex(
                name: "IX_DonHangs_MaPTTTID",
                table: "DonHangs",
                column: "MaPTTTID");

            migrationBuilder.CreateIndex(
                name: "IX_DonHangs_MaVanChuyenID",
                table: "DonHangs",
                column: "MaVanChuyenID");

            migrationBuilder.CreateIndex(
                name: "IX_DonHangs_NguoiDungMaNguoiDung",
                table: "DonHangs",
                column: "NguoiDungMaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_DonHangs_UserID",
                table: "DonHangs",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_DuLieuDanhBats_MaNguDanID",
                table: "DuLieuDanhBats",
                column: "MaNguDanID");

            migrationBuilder.CreateIndex(
                name: "IX_HinhAnhSanPhams_MaSanPhamID",
                table: "HinhAnhSanPhams",
                column: "MaSanPhamID");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDons_MaDonHangID",
                table: "HoaDons",
                column: "MaDonHangID");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDons_MaNguoiDungID",
                table: "HoaDons",
                column: "MaNguoiDungID");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDons_MaNhanVienID",
                table: "HoaDons",
                column: "MaNhanVienID");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhams_DanhMucID",
                table: "SanPhams",
                column: "DanhMucID");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhams_MaLoaiID",
                table: "SanPhams",
                column: "MaLoaiID");

            migrationBuilder.CreateIndex(
                name: "IX_VanChuyens_MaNhanVienID",
                table: "VanChuyens",
                column: "MaNhanVienID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ChiTietCangs");

            migrationBuilder.DropTable(
                name: "ChiTietDanhBats");

            migrationBuilder.DropTable(
                name: "ChiTietDonHangs");

            migrationBuilder.DropTable(
                name: "ChiTietKhoHangs");

            migrationBuilder.DropTable(
                name: "ChiTietMuas");

            migrationBuilder.DropTable(
                name: "ChiTietTrangThaiDonHangs");

            migrationBuilder.DropTable(
                name: "DanhGias");

            migrationBuilder.DropTable(
                name: "HinhAnhSanPhams");

            migrationBuilder.DropTable(
                name: "HoaDons");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Cangs");

            migrationBuilder.DropTable(
                name: "DuLieuDanhBats");

            migrationBuilder.DropTable(
                name: "NhaKhos");

            migrationBuilder.DropTable(
                name: "Muas");

            migrationBuilder.DropTable(
                name: "TrangThaiDonHangs");

            migrationBuilder.DropTable(
                name: "SanPhams");

            migrationBuilder.DropTable(
                name: "DonHangs");

            migrationBuilder.DropTable(
                name: "NguDans");

            migrationBuilder.DropTable(
                name: "DanhMucs");

            migrationBuilder.DropTable(
                name: "LoaiSanPhams");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "GiamGias");

            migrationBuilder.DropTable(
                name: "NguoiDungs");

            migrationBuilder.DropTable(
                name: "PhuongThucThanhToans");

            migrationBuilder.DropTable(
                name: "VanChuyens");

            migrationBuilder.DropTable(
                name: "NhanViens");
        }
    }
}
