using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAnCoSo.Migrations
{
    /// <inheritdoc />
    public partial class ngay12_5moinhat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DonHangs_VanChuyens_MaVanChuyenID",
                table: "DonHangs");

            migrationBuilder.DropTable(
                name: "ChiTietCangs");

            migrationBuilder.DropTable(
                name: "ChiTietDanhBats");

            migrationBuilder.DropTable(
                name: "VanChuyens");

            migrationBuilder.DropTable(
                name: "Cangs");

            migrationBuilder.DropTable(
                name: "DuLieuDanhBats");

            migrationBuilder.DropTable(
                name: "NguDans");

            migrationBuilder.DropColumn(
                name: "TrangThai",
                table: "PhuongThucThanhToans");

            migrationBuilder.AddColumn<int>(
                name: "MaTrangThaiID",
                table: "DonHangs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayCapNhat",
                table: "ChiTietTrangThaiDonHangs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "NhaCungCaps",
                columns: table => new
                {
                    MaNCC = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNCC = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DiaChiNCC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SDT = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhaCungCaps", x => x.MaNCC);
                });

            migrationBuilder.CreateTable(
                name: "PhuongTienVanChuyens",
                columns: table => new
                {
                    MaPhuongTien = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoaiPhuongTien = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TrongTai = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhuongTienVanChuyens", x => x.MaPhuongTien);
                });

            migrationBuilder.CreateTable(
                name: "TrangThaiThanhToans",
                columns: table => new
                {
                    MaTrangThai = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenTrangThai = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrangThaiThanhToans", x => x.MaTrangThai);
                });

            migrationBuilder.CreateTable(
                name: "PhieuNhapHangs",
                columns: table => new
                {
                    MaPhieuNhap = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TongSoLuong = table.Column<int>(type: "int", nullable: false),
                    MaNCCID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhieuNhapHangs", x => x.MaPhieuNhap);
                    table.ForeignKey(
                        name: "FK_PhieuNhapHangs_NhaCungCaps_MaNCCID",
                        column: x => x.MaNCCID,
                        principalTable: "NhaCungCaps",
                        principalColumn: "MaNCC",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PhieuVanChuyens",
                columns: table => new
                {
                    MaVanChuyen = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaPhuongTienID = table.Column<int>(type: "int", nullable: false),
                    MaNhanVienID = table.Column<int>(type: "int", nullable: false),
                    NgayVanChuyen = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhieuVanChuyens", x => x.MaVanChuyen);
                    table.ForeignKey(
                        name: "FK_PhieuVanChuyens_NhanViens_MaNhanVienID",
                        column: x => x.MaNhanVienID,
                        principalTable: "NhanViens",
                        principalColumn: "MaNhanVien",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PhieuVanChuyens_PhuongTienVanChuyens_MaPhuongTienID",
                        column: x => x.MaPhuongTienID,
                        principalTable: "PhuongTienVanChuyens",
                        principalColumn: "MaPhuongTien",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietPhieuNhaps",
                columns: table => new
                {
                    MaPhieuNhap = table.Column<int>(type: "int", nullable: false),
                    MaSanPham = table.Column<int>(type: "int", nullable: false),
                    SoLuongSanPham = table.Column<int>(type: "int", nullable: false),
                    GiaNhapHang = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietPhieuNhaps", x => new { x.MaPhieuNhap, x.MaSanPham });
                    table.ForeignKey(
                        name: "FK_ChiTietPhieuNhaps_PhieuNhapHangs_MaPhieuNhap",
                        column: x => x.MaPhieuNhap,
                        principalTable: "PhieuNhapHangs",
                        principalColumn: "MaPhieuNhap",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietPhieuNhaps_SanPhams_MaSanPham",
                        column: x => x.MaSanPham,
                        principalTable: "SanPhams",
                        principalColumn: "MaSanPham",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DonHangs_MaTrangThaiID",
                table: "DonHangs",
                column: "MaTrangThaiID");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietPhieuNhaps_MaSanPham",
                table: "ChiTietPhieuNhaps",
                column: "MaSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuNhapHangs_MaNCCID",
                table: "PhieuNhapHangs",
                column: "MaNCCID");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuVanChuyens_MaNhanVienID",
                table: "PhieuVanChuyens",
                column: "MaNhanVienID");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuVanChuyens_MaPhuongTienID",
                table: "PhieuVanChuyens",
                column: "MaPhuongTienID");

            migrationBuilder.AddForeignKey(
                name: "FK_DonHangs_PhieuVanChuyens_MaVanChuyenID",
                table: "DonHangs",
                column: "MaVanChuyenID",
                principalTable: "PhieuVanChuyens",
                principalColumn: "MaVanChuyen",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DonHangs_TrangThaiThanhToans_MaTrangThaiID",
                table: "DonHangs",
                column: "MaTrangThaiID",
                principalTable: "TrangThaiThanhToans",
                principalColumn: "MaTrangThai",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DonHangs_PhieuVanChuyens_MaVanChuyenID",
                table: "DonHangs");

            migrationBuilder.DropForeignKey(
                name: "FK_DonHangs_TrangThaiThanhToans_MaTrangThaiID",
                table: "DonHangs");

            migrationBuilder.DropTable(
                name: "ChiTietPhieuNhaps");

            migrationBuilder.DropTable(
                name: "PhieuVanChuyens");

            migrationBuilder.DropTable(
                name: "TrangThaiThanhToans");

            migrationBuilder.DropTable(
                name: "PhieuNhapHangs");

            migrationBuilder.DropTable(
                name: "PhuongTienVanChuyens");

            migrationBuilder.DropTable(
                name: "NhaCungCaps");

            migrationBuilder.DropIndex(
                name: "IX_DonHangs_MaTrangThaiID",
                table: "DonHangs");

            migrationBuilder.DropColumn(
                name: "MaTrangThaiID",
                table: "DonHangs");

            migrationBuilder.DropColumn(
                name: "NgayCapNhat",
                table: "ChiTietTrangThaiDonHangs");

            migrationBuilder.AddColumn<string>(
                name: "TrangThai",
                table: "PhuongThucThanhToans",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Cangs",
                columns: table => new
                {
                    MaCang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenCang = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cangs", x => x.MaCang);
                });

            migrationBuilder.CreateTable(
                name: "NguDans",
                columns: table => new
                {
                    MaNguDan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SoGiayPhep = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoLuongDuKien = table.Column<int>(type: "int", nullable: false),
                    TenNguDan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenTau = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VungDanhBat = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NguDans", x => x.MaNguDan);
                });

            migrationBuilder.CreateTable(
                name: "VanChuyens",
                columns: table => new
                {
                    MaVanChuyen = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNhanVienID = table.Column<int>(type: "int", nullable: true),
                    LoaiPhuongTien = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrangThaiVanChuyen = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                name: "DuLieuDanhBats",
                columns: table => new
                {
                    MaDuLieuDanhBat = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguDanID = table.Column<int>(type: "int", nullable: true),
                    NgayDanhBat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoLuongDanhBat = table.Column<int>(type: "int", nullable: false),
                    ThoiTiet = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                name: "ChiTietDanhBats",
                columns: table => new
                {
                    MaDuLieuDanhBatID = table.Column<int>(type: "int", nullable: false),
                    MaSanPhamID = table.Column<int>(type: "int", nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoLuongSanPham = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.UpdateData(
                table: "PhuongThucThanhToans",
                keyColumn: "MaPTTT",
                keyValue: 1,
                column: "TrangThai",
                value: "Hoạt động");

            migrationBuilder.UpdateData(
                table: "PhuongThucThanhToans",
                keyColumn: "MaPTTT",
                keyValue: 2,
                column: "TrangThai",
                value: "Hoạt động");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietCangs_MaCangID",
                table: "ChiTietCangs",
                column: "MaCangID");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDanhBats_MaSanPhamID",
                table: "ChiTietDanhBats",
                column: "MaSanPhamID");

            migrationBuilder.CreateIndex(
                name: "IX_DuLieuDanhBats_MaNguDanID",
                table: "DuLieuDanhBats",
                column: "MaNguDanID");

            migrationBuilder.CreateIndex(
                name: "IX_VanChuyens_MaNhanVienID",
                table: "VanChuyens",
                column: "MaNhanVienID");

            migrationBuilder.AddForeignKey(
                name: "FK_DonHangs_VanChuyens_MaVanChuyenID",
                table: "DonHangs",
                column: "MaVanChuyenID",
                principalTable: "VanChuyens",
                principalColumn: "MaVanChuyen",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
