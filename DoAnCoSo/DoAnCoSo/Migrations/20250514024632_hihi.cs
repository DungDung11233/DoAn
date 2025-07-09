using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAnCoSo.Migrations
{
    /// <inheritdoc />
    public partial class hihi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DonHangs_AspNetUsers_UserID",
                table: "DonHangs");

            migrationBuilder.DropIndex(
                name: "IX_DonHangs_UserID",
                table: "DonHangs");

            migrationBuilder.DeleteData(
                table: "TrangThaiDonHangs",
                keyColumn: "MaTrangThai",
                keyValue: 4);

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "DonHangs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserID",
                table: "DonHangs",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "TrangThaiDonHangs",
                columns: new[] { "MaTrangThai", "GhiChu", "TenTrangThai" },
                values: new object[] { 4, "Đơn hàng đã bàn giao vận chuyển", "Đã giao cho đơn vị vận chuyển" });

            migrationBuilder.CreateIndex(
                name: "IX_DonHangs_UserID",
                table: "DonHangs",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_DonHangs_AspNetUsers_UserID",
                table: "DonHangs",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
