using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sos.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultValueOfUserLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Longitude",
                table: "User",
                type: "float",
                nullable: true,
                defaultValue: 16.462622,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Latitude",
                table: "User",
                type: "float",
                nullable: true,
                defaultValue: 107.585217,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Longitude",
                table: "User",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true,
                oldDefaultValue: 16.462622);

            migrationBuilder.AlterColumn<double>(
                name: "Latitude",
                table: "User",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true,
                oldDefaultValue: 107.585217);
        }
    }
}
