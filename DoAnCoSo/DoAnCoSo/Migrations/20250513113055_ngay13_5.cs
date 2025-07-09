using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAnCoSo.Migrations
{
    /// <inheritdoc />
    public partial class ngay13_5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DanhGias_AspNetUsers_MaNguoiDungID",
                table: "DanhGias");

            migrationBuilder.DropForeignKey(
                name: "FK_DonHangs_AspNetUsers_MaNguoiDungID",
                table: "DonHangs");

            migrationBuilder.DropForeignKey(
                name: "FK_DonHangs_PhieuVanChuyens_MaVanChuyenID",
                table: "DonHangs");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDons_AspNetUsers_MaNguoiDungID",
                table: "HoaDons");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDons_AspNetUsers_MaNhanVienID",
                table: "HoaDons");

            migrationBuilder.DropForeignKey(
                name: "FK_PhieuNhapHangs_AspNetUsers_MaNhaCungCapID",
                table: "PhieuNhapHangs");

            migrationBuilder.DropForeignKey(
                name: "FK_PhieuVanChuyens_AspNetUsers_MaNhanVienID",
                table: "PhieuVanChuyens");

            migrationBuilder.DropIndex(
                name: "IX_DonHangs_MaVanChuyenID",
                table: "DonHangs");

            migrationBuilder.DropColumn(
                name: "MaVanChuyenID",
                table: "DonHangs");

            migrationBuilder.AlterColumn<int>(
                name: "MaNhanVienID",
                table: "PhieuVanChuyens",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "MaNhaCungCapID",
                table: "PhieuNhapHangs",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NhaCungCapMaNCC",
                table: "PhieuNhapHangs",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MaNhanVienID",
                table: "HoaDons",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MaNguoiDungID",
                table: "HoaDons",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MaNguoiDungID",
                table: "DonHangs",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserID",
                table: "DonHangs",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MaNguoiDungID",
                table: "DanhGias",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ChiTietVanChuyens",
                columns: table => new
                {
                    MaDonHang = table.Column<int>(type: "int", nullable: false),
                    MaVanChuyen = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietVanChuyens", x => new { x.MaDonHang, x.MaVanChuyen });
                    table.ForeignKey(
                        name: "FK_ChiTietVanChuyens_DonHangs_MaDonHang",
                        column: x => x.MaDonHang,
                        principalTable: "DonHangs",
                        principalColumn: "MaDonHang",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietVanChuyens_PhieuVanChuyens_MaVanChuyen",
                        column: x => x.MaVanChuyen,
                        principalTable: "PhieuVanChuyens",
                        principalColumn: "MaVanChuyen",
                        onDelete: ReferentialAction.Cascade);
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
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NguoiDungs", x => x.MaNguoiDung);
                    table.ForeignKey(
                        name: "FK_NguoiDungs_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "NhaCungCaps",
                columns: table => new
                {
                    MaNCC = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNCC = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DiaChiNCC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SDT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhaCungCaps", x => x.MaNCC);
                    table.ForeignKey(
                        name: "FK_NhaCungCaps_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
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
                    ThoiGianLamViec = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhanViens", x => x.MaNhanVien);
                    table.ForeignKey(
                        name: "FK_NhanViens_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PhieuNhapHangs_NhaCungCapMaNCC",
                table: "PhieuNhapHangs",
                column: "NhaCungCapMaNCC");

            migrationBuilder.CreateIndex(
                name: "IX_DonHangs_UserID",
                table: "DonHangs",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietVanChuyens_MaVanChuyen",
                table: "ChiTietVanChuyens",
                column: "MaVanChuyen");

            migrationBuilder.CreateIndex(
                name: "IX_NguoiDungs_UserID",
                table: "NguoiDungs",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_NhaCungCaps_UserID",
                table: "NhaCungCaps",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_NhanViens_UserID",
                table: "NhanViens",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_DanhGias_NguoiDungs_MaNguoiDungID",
                table: "DanhGias",
                column: "MaNguoiDungID",
                principalTable: "NguoiDungs",
                principalColumn: "MaNguoiDung",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DonHangs_AspNetUsers_UserID",
                table: "DonHangs",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DonHangs_NguoiDungs_MaNguoiDungID",
                table: "DonHangs",
                column: "MaNguoiDungID",
                principalTable: "NguoiDungs",
                principalColumn: "MaNguoiDung",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDons_NguoiDungs_MaNguoiDungID",
                table: "HoaDons",
                column: "MaNguoiDungID",
                principalTable: "NguoiDungs",
                principalColumn: "MaNguoiDung",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDons_NhanViens_MaNhanVienID",
                table: "HoaDons",
                column: "MaNhanVienID",
                principalTable: "NhanViens",
                principalColumn: "MaNhanVien",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PhieuNhapHangs_NhaCungCaps_MaNhaCungCapID",
                table: "PhieuNhapHangs",
                column: "MaNhaCungCapID",
                principalTable: "NhaCungCaps",
                principalColumn: "MaNCC",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PhieuNhapHangs_NhaCungCaps_NhaCungCapMaNCC",
                table: "PhieuNhapHangs",
                column: "NhaCungCapMaNCC",
                principalTable: "NhaCungCaps",
                principalColumn: "MaNCC");

            migrationBuilder.AddForeignKey(
                name: "FK_PhieuVanChuyens_NhanViens_MaNhanVienID",
                table: "PhieuVanChuyens",
                column: "MaNhanVienID",
                principalTable: "NhanViens",
                principalColumn: "MaNhanVien",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DanhGias_NguoiDungs_MaNguoiDungID",
                table: "DanhGias");

            migrationBuilder.DropForeignKey(
                name: "FK_DonHangs_AspNetUsers_UserID",
                table: "DonHangs");

            migrationBuilder.DropForeignKey(
                name: "FK_DonHangs_NguoiDungs_MaNguoiDungID",
                table: "DonHangs");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDons_NguoiDungs_MaNguoiDungID",
                table: "HoaDons");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDons_NhanViens_MaNhanVienID",
                table: "HoaDons");

            migrationBuilder.DropForeignKey(
                name: "FK_PhieuNhapHangs_NhaCungCaps_MaNhaCungCapID",
                table: "PhieuNhapHangs");

            migrationBuilder.DropForeignKey(
                name: "FK_PhieuNhapHangs_NhaCungCaps_NhaCungCapMaNCC",
                table: "PhieuNhapHangs");

            migrationBuilder.DropForeignKey(
                name: "FK_PhieuVanChuyens_NhanViens_MaNhanVienID",
                table: "PhieuVanChuyens");

            migrationBuilder.DropTable(
                name: "ChiTietVanChuyens");

            migrationBuilder.DropTable(
                name: "NguoiDungs");

            migrationBuilder.DropTable(
                name: "NhaCungCaps");

            migrationBuilder.DropTable(
                name: "NhanViens");

            migrationBuilder.DropIndex(
                name: "IX_PhieuNhapHangs_NhaCungCapMaNCC",
                table: "PhieuNhapHangs");

            migrationBuilder.DropIndex(
                name: "IX_DonHangs_UserID",
                table: "DonHangs");

            migrationBuilder.DropColumn(
                name: "NhaCungCapMaNCC",
                table: "PhieuNhapHangs");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "DonHangs");

            migrationBuilder.AlterColumn<string>(
                name: "MaNhanVienID",
                table: "PhieuVanChuyens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "MaNhaCungCapID",
                table: "PhieuNhapHangs",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaNhanVienID",
                table: "HoaDons",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaNguoiDungID",
                table: "HoaDons",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaNguoiDungID",
                table: "DonHangs",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaVanChuyenID",
                table: "DonHangs",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaNguoiDungID",
                table: "DanhGias",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DonHangs_MaVanChuyenID",
                table: "DonHangs",
                column: "MaVanChuyenID");

            migrationBuilder.AddForeignKey(
                name: "FK_DanhGias_AspNetUsers_MaNguoiDungID",
                table: "DanhGias",
                column: "MaNguoiDungID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DonHangs_AspNetUsers_MaNguoiDungID",
                table: "DonHangs",
                column: "MaNguoiDungID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DonHangs_PhieuVanChuyens_MaVanChuyenID",
                table: "DonHangs",
                column: "MaVanChuyenID",
                principalTable: "PhieuVanChuyens",
                principalColumn: "MaVanChuyen",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDons_AspNetUsers_MaNguoiDungID",
                table: "HoaDons",
                column: "MaNguoiDungID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDons_AspNetUsers_MaNhanVienID",
                table: "HoaDons",
                column: "MaNhanVienID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PhieuNhapHangs_AspNetUsers_MaNhaCungCapID",
                table: "PhieuNhapHangs",
                column: "MaNhaCungCapID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PhieuVanChuyens_AspNetUsers_MaNhanVienID",
                table: "PhieuVanChuyens",
                column: "MaNhanVienID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
