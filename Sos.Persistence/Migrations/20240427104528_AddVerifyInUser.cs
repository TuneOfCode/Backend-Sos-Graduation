using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sos.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddVerifyInUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VerifyCode",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "VerifyCodeExpired",
                table: "User",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VerifyCode",
                table: "User");

            migrationBuilder.DropColumn(
                name: "VerifyCodeExpired",
                table: "User");
        }
    }
}
