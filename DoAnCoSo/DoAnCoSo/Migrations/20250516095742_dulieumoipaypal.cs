using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DoAnCoSo.Migrations
{
    /// <inheritdoc />
    public partial class dulieumoipaypal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TrangThaiThanhToans",
                columns: new[] { "MaTrangThai", "TenTrangThai" },
                values: new object[,]
                {
                    { 1, "Chưa thanh toán" },
                    { 2, "Đã thanh toán" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TrangThaiThanhToans",
                keyColumn: "MaTrangThai",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TrangThaiThanhToans",
                keyColumn: "MaTrangThai",
                keyValue: 2);
        }
    }
}
