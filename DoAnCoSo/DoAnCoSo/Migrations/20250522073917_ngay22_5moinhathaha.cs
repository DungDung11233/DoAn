using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAnCoSo.Migrations
{
    /// <inheritdoc />
    public partial class ngay22_5moinhathaha : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HinhAnh",
                table: "ChiTietPhieuNhaps");

            migrationBuilder.AddColumn<string>(
                name: "AnhSanPham",
                table: "PhieuNhapHangs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnhSanPham",
                table: "PhieuNhapHangs");

            migrationBuilder.AddColumn<string>(
                name: "HinhAnh",
                table: "ChiTietPhieuNhaps",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
