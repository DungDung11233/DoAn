using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAnCoSo.Migrations
{
    /// <inheritdoc />
    public partial class ngay12_5_15h30 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DanhGias_AspNetUsers_ApplicationUserId",
                table: "DanhGias");

            migrationBuilder.DropForeignKey(
                name: "FK_DanhGias_NguoiDungs_MaNguoiDungID",
                table: "DanhGias");

            migrationBuilder.DropForeignKey(
                name: "FK_DonHangs_AspNetUsers_UserID",
                table: "DonHangs");

            migrationBuilder.DropForeignKey(
                name: "FK_DonHangs_NguoiDungs_NguoiDungMaNguoiDung",
                table: "DonHangs");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDons_NguoiDungs_MaNguoiDungID",
                table: "HoaDons");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDons_NhanViens_MaNhanVienID",
                table: "HoaDons");

            migrationBuilder.DropForeignKey(
                name: "FK_PhieuNhapHangs_NhaCungCaps_MaNCCID",
                table: "PhieuNhapHangs");

            migrationBuilder.DropForeignKey(
                name: "FK_PhieuVanChuyens_NhanViens_MaNhanVienID",
                table: "PhieuVanChuyens");

            migrationBuilder.DropTable(
                name: "NguoiDungs");

            migrationBuilder.DropTable(
                name: "NhaCungCaps");

            migrationBuilder.DropTable(
                name: "NhanViens");

            migrationBuilder.DropIndex(
                name: "IX_PhieuNhapHangs_MaNCCID",
                table: "PhieuNhapHangs");

            migrationBuilder.DropIndex(
                name: "IX_DonHangs_NguoiDungMaNguoiDung",
                table: "DonHangs");

            migrationBuilder.DropIndex(
                name: "IX_DonHangs_UserID",
                table: "DonHangs");

            migrationBuilder.DropIndex(
                name: "IX_DanhGias_ApplicationUserId",
                table: "DanhGias");

            migrationBuilder.DropColumn(
                name: "MaNCCID",
                table: "PhieuNhapHangs");

            migrationBuilder.DropColumn(
                name: "NguoiDungMaNguoiDung",
                table: "DonHangs");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "DonHangs");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "DanhGias");

            migrationBuilder.AlterColumn<string>(
                name: "MaNhanVienID",
                table: "PhieuVanChuyens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "MaNhaCungCapID",
                table: "PhieuNhapHangs",
                type: "nvarchar(450)",
                nullable: true);

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

            migrationBuilder.AlterColumn<string>(
                name: "MaNguoiDungID",
                table: "DanhGias",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PhieuNhapHangs_MaNhaCungCapID",
                table: "PhieuNhapHangs",
                column: "MaNhaCungCapID");

            migrationBuilder.CreateIndex(
                name: "IX_DonHangs_MaNguoiDungID",
                table: "DonHangs",
                column: "MaNguoiDungID");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DanhGias_AspNetUsers_MaNguoiDungID",
                table: "DanhGias");

            migrationBuilder.DropForeignKey(
                name: "FK_DonHangs_AspNetUsers_MaNguoiDungID",
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
                name: "IX_PhieuNhapHangs_MaNhaCungCapID",
                table: "PhieuNhapHangs");

            migrationBuilder.DropIndex(
                name: "IX_DonHangs_MaNguoiDungID",
                table: "DonHangs");

            migrationBuilder.DropColumn(
                name: "MaNhaCungCapID",
                table: "PhieuNhapHangs");

            migrationBuilder.AlterColumn<int>(
                name: "MaNhanVienID",
                table: "PhieuVanChuyens",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "MaNCCID",
                table: "PhieuNhapHangs",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.AddColumn<int>(
                name: "NguoiDungMaNguoiDung",
                table: "DonHangs",
                type: "int",
                nullable: true);

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

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "DanhGias",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "NguoiDungs",
                columns: table => new
                {
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiaChi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenNguoiDung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VaiTro = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NguoiDungs", x => x.MaNguoiDung);
                });

            migrationBuilder.CreateTable(
                name: "NhaCungCaps",
                columns: table => new
                {
                    MaNCC = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiaChiNCC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SDT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenNCC = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhaCungCaps", x => x.MaNCC);
                });

            migrationBuilder.CreateTable(
                name: "NhanViens",
                columns: table => new
                {
                    MaNhanVien = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayTuyenDung = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenNhanVien = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ThoiGianLamViec = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhanViens", x => x.MaNhanVien);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PhieuNhapHangs_MaNCCID",
                table: "PhieuNhapHangs",
                column: "MaNCCID");

            migrationBuilder.CreateIndex(
                name: "IX_DonHangs_NguoiDungMaNguoiDung",
                table: "DonHangs",
                column: "NguoiDungMaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_DonHangs_UserID",
                table: "DonHangs",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGias_ApplicationUserId",
                table: "DanhGias",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DanhGias_AspNetUsers_ApplicationUserId",
                table: "DanhGias",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

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
                name: "FK_DonHangs_NguoiDungs_NguoiDungMaNguoiDung",
                table: "DonHangs",
                column: "NguoiDungMaNguoiDung",
                principalTable: "NguoiDungs",
                principalColumn: "MaNguoiDung");

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
                name: "FK_PhieuNhapHangs_NhaCungCaps_MaNCCID",
                table: "PhieuNhapHangs",
                column: "MaNCCID",
                principalTable: "NhaCungCaps",
                principalColumn: "MaNCC",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PhieuVanChuyens_NhanViens_MaNhanVienID",
                table: "PhieuVanChuyens",
                column: "MaNhanVienID",
                principalTable: "NhanViens",
                principalColumn: "MaNhanVien",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
