using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderFood_BE.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldPayosOrderCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<long>(
                name: "PayosOrderCode",
                table: "Orders",
                type: "bigint",
                maxLength: 11,
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "PayosOrderCode",
                table: "Orders");
        }
    }
}
