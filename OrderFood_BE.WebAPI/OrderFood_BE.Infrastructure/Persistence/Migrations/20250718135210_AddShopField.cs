using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderFood_BE.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddShopField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Shops",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AddColumn<string>(
                name: "BusinessLicenseImageUrl",
                table: "Shops",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Shops",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Shops",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Shops",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusinessLicenseImageUrl",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Shops");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Shops",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldNullable: true);
        }
    }
}
