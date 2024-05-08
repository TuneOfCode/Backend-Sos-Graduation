using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sos.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLocationInUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VerifiedOnUtc",
                table: "User",
                newName: "VerifiedAt");

            migrationBuilder.RenameColumn(
                name: "ModifiedOnUtc",
                table: "User",
                newName: "ModifiedAt");

            migrationBuilder.RenameColumn(
                name: "DeletedOnUtc",
                table: "User",
                newName: "DeletedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedOnUtc",
                table: "User",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "ModifiedOnUtc",
                table: "FriendshipRequest",
                newName: "ModifiedAt");

            migrationBuilder.RenameColumn(
                name: "DeletedOnUtc",
                table: "FriendshipRequest",
                newName: "DeletedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedOnUtc",
                table: "FriendshipRequest",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "ModifiedOnUtc",
                table: "Friendship",
                newName: "ModifiedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedOnUtc",
                table: "Friendship",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "User",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "User",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "VerifiedAt",
                table: "User",
                newName: "VerifiedOnUtc");

            migrationBuilder.RenameColumn(
                name: "ModifiedAt",
                table: "User",
                newName: "ModifiedOnUtc");

            migrationBuilder.RenameColumn(
                name: "DeletedAt",
                table: "User",
                newName: "DeletedOnUtc");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "User",
                newName: "CreatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "ModifiedAt",
                table: "FriendshipRequest",
                newName: "ModifiedOnUtc");

            migrationBuilder.RenameColumn(
                name: "DeletedAt",
                table: "FriendshipRequest",
                newName: "DeletedOnUtc");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "FriendshipRequest",
                newName: "CreatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "ModifiedAt",
                table: "Friendship",
                newName: "ModifiedOnUtc");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Friendship",
                newName: "CreatedOnUtc");
        }
    }
}
