using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderFood_BE.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceIsActiveWithStatusInShop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Đổi tên IsActive thành OldIsActive
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Shops",
                newName: "OldIsActive");

            // 2. Thêm cột Status
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Shops",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Pending");

            // 3. Cập nhật dữ liệu từ OldIsActive sang Status
            migrationBuilder.Sql(@"
        UPDATE Shops
        SET Status = CASE 
                        WHEN OldIsActive = 1 THEN 'Approved'
                        ELSE 'Pending'
                     END
    ");

            // 4. Xoá cột OldIsActive
            migrationBuilder.DropColumn(
                name: "OldIsActive",
                table: "Shops");
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Shops");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Shops",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
