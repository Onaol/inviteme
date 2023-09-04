using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InviteMe.Migrations
{
    /// <inheritdoc />
    public partial class add_uniquekeyto_invites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UniqueInviteKey",
                table: "Invites",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UniqueInviteKey",
                table: "Invites");
        }
    }
}
