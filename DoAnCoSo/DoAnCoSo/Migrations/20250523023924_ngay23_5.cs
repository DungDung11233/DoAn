using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAnCoSo.Migrations
{
    /// <inheritdoc />
    public partial class ngay23_5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DanhGias_NguoiDungs_MaNguoiDungID",
                table: "DanhGias");

            migrationBuilder.DropForeignKey(
                name: "FK_DonHangs_NguoiDungs_MaNguoiDungID",
                table: "DonHangs");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDons_NguoiDungs_MaNguoiDungID",
                table: "HoaDons");

            migrationBuilder.DropTable(
                name: "NguoiDungs");

            migrationBuilder.CreateTable(
                name: "KhachHangs",
                columns: table => new
                {
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNguoiDung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhachHangs", x => x.MaNguoiDung);
                    table.ForeignKey(
                        name: "FK_KhachHangs_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_KhachHangs_UserID",
                table: "KhachHangs",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_DanhGias_KhachHangs_MaNguoiDungID",
                table: "DanhGias",
                column: "MaNguoiDungID",
                principalTable: "KhachHangs",
                principalColumn: "MaNguoiDung",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DonHangs_KhachHangs_MaNguoiDungID",
                table: "DonHangs",
                column: "MaNguoiDungID",
                principalTable: "KhachHangs",
                principalColumn: "MaNguoiDung",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDons_KhachHangs_MaNguoiDungID",
                table: "HoaDons",
                column: "MaNguoiDungID",
                principalTable: "KhachHangs",
                principalColumn: "MaNguoiDung",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DanhGias_KhachHangs_MaNguoiDungID",
                table: "DanhGias");

            migrationBuilder.DropForeignKey(
                name: "FK_DonHangs_KhachHangs_MaNguoiDungID",
                table: "DonHangs");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDons_KhachHangs_MaNguoiDungID",
                table: "HoaDons");

            migrationBuilder.DropTable(
                name: "KhachHangs");

            migrationBuilder.CreateTable(
                name: "NguoiDungs",
                columns: table => new
                {
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenNguoiDung = table.Column<string>(type: "nvarchar(max)", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_NguoiDungs_UserID",
                table: "NguoiDungs",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_DanhGias_NguoiDungs_MaNguoiDungID",
                table: "DanhGias",
                column: "MaNguoiDungID",
                principalTable: "NguoiDungs",
                principalColumn: "MaNguoiDung",
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
        }
    }
}
