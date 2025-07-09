using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAnCoSo.Migrations
{
    /// <inheritdoc />
    public partial class ngay23_5moinhat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "TenNguoiDung",
                table: "KhachHangs",
                newName: "TenKhachHang");

            migrationBuilder.RenameColumn(
                name: "MaNguoiDung",
                table: "KhachHangs",
                newName: "MaKhachHang");

            migrationBuilder.RenameColumn(
                name: "MaNguoiDungID",
                table: "HoaDons",
                newName: "MaKhachHangID");

            migrationBuilder.RenameIndex(
                name: "IX_HoaDons_MaNguoiDungID",
                table: "HoaDons",
                newName: "IX_HoaDons_MaKhachHangID");

            migrationBuilder.RenameColumn(
                name: "MaNguoiDungID",
                table: "DonHangs",
                newName: "MaKhachHangID");

            migrationBuilder.RenameIndex(
                name: "IX_DonHangs_MaNguoiDungID",
                table: "DonHangs",
                newName: "IX_DonHangs_MaKhachHangID");

            migrationBuilder.RenameColumn(
                name: "MaNguoiDungID",
                table: "DanhGias",
                newName: "MaKhachHangID");

            migrationBuilder.RenameIndex(
                name: "IX_DanhGias_MaNguoiDungID",
                table: "DanhGias",
                newName: "IX_DanhGias_MaKhachHangID");

            migrationBuilder.AddForeignKey(
                name: "FK_DanhGias_KhachHangs_MaKhachHangID",
                table: "DanhGias",
                column: "MaKhachHangID",
                principalTable: "KhachHangs",
                principalColumn: "MaKhachHang",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DonHangs_KhachHangs_MaKhachHangID",
                table: "DonHangs",
                column: "MaKhachHangID",
                principalTable: "KhachHangs",
                principalColumn: "MaKhachHang",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDons_KhachHangs_MaKhachHangID",
                table: "HoaDons",
                column: "MaKhachHangID",
                principalTable: "KhachHangs",
                principalColumn: "MaKhachHang",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DanhGias_KhachHangs_MaKhachHangID",
                table: "DanhGias");

            migrationBuilder.DropForeignKey(
                name: "FK_DonHangs_KhachHangs_MaKhachHangID",
                table: "DonHangs");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDons_KhachHangs_MaKhachHangID",
                table: "HoaDons");

            migrationBuilder.RenameColumn(
                name: "TenKhachHang",
                table: "KhachHangs",
                newName: "TenNguoiDung");

            migrationBuilder.RenameColumn(
                name: "MaKhachHang",
                table: "KhachHangs",
                newName: "MaNguoiDung");

            migrationBuilder.RenameColumn(
                name: "MaKhachHangID",
                table: "HoaDons",
                newName: "MaNguoiDungID");

            migrationBuilder.RenameIndex(
                name: "IX_HoaDons_MaKhachHangID",
                table: "HoaDons",
                newName: "IX_HoaDons_MaNguoiDungID");

            migrationBuilder.RenameColumn(
                name: "MaKhachHangID",
                table: "DonHangs",
                newName: "MaNguoiDungID");

            migrationBuilder.RenameIndex(
                name: "IX_DonHangs_MaKhachHangID",
                table: "DonHangs",
                newName: "IX_DonHangs_MaNguoiDungID");

            migrationBuilder.RenameColumn(
                name: "MaKhachHangID",
                table: "DanhGias",
                newName: "MaNguoiDungID");

            migrationBuilder.RenameIndex(
                name: "IX_DanhGias_MaKhachHangID",
                table: "DanhGias",
                newName: "IX_DanhGias_MaNguoiDungID");

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
    }
}
