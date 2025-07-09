using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAnCoSo.Migrations
{
    /// <inheritdoc />
    public partial class ngay13_5moi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhieuNhapHangs_NhaCungCaps_NhaCungCapMaNCC",
                table: "PhieuNhapHangs");

            migrationBuilder.DropIndex(
                name: "IX_PhieuNhapHangs_NhaCungCapMaNCC",
                table: "PhieuNhapHangs");

            migrationBuilder.DropColumn(
                name: "NhaCungCapMaNCC",
                table: "PhieuNhapHangs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NhaCungCapMaNCC",
                table: "PhieuNhapHangs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PhieuNhapHangs_NhaCungCapMaNCC",
                table: "PhieuNhapHangs",
                column: "NhaCungCapMaNCC");

            migrationBuilder.AddForeignKey(
                name: "FK_PhieuNhapHangs_NhaCungCaps_NhaCungCapMaNCC",
                table: "PhieuNhapHangs",
                column: "NhaCungCapMaNCC",
                principalTable: "NhaCungCaps",
                principalColumn: "MaNCC");
        }
    }
}
