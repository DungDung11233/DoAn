using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAnCoSo.Migrations
{
    /// <inheritdoc />
    public partial class ngay22_5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoaDons_DonHangs_MaDonHangID",
                table: "HoaDons");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDons_DonHangs_MaDonHangID",
                table: "HoaDons",
                column: "MaDonHangID",
                principalTable: "DonHangs",
                principalColumn: "MaDonHang",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoaDons_DonHangs_MaDonHangID",
                table: "HoaDons");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDons_DonHangs_MaDonHangID",
                table: "HoaDons",
                column: "MaDonHangID",
                principalTable: "DonHangs",
                principalColumn: "MaDonHang");
        }
    }
}
