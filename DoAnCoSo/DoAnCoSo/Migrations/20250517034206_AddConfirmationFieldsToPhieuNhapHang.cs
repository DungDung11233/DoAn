using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAnCoSo.Migrations
{
    /// <inheritdoc />
    public partial class AddConfirmationFieldsToPhieuNhapHang : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DaXacNhan",
                table: "PhieuNhapHangs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "NguoiXacNhan",
                table: "PhieuNhapHangs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ThoiGianXacNhan",
                table: "PhieuNhapHangs",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DaXacNhan",
                table: "PhieuNhapHangs");

            migrationBuilder.DropColumn(
                name: "NguoiXacNhan",
                table: "PhieuNhapHangs");

            migrationBuilder.DropColumn(
                name: "ThoiGianXacNhan",
                table: "PhieuNhapHangs");
        }
    }
}
