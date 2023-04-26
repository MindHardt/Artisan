using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Artisan.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedCharacterSnapshots : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CharacterSnapshot",
                columns: table => new
                {
                    Name = table.Column<string>(type: "text", nullable: false),
                    Race = table.Column<string>(type: "text", nullable: true),
                    Gender = table.Column<string>(type: "text", nullable: true),
                    ApplicationUserId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterSnapshot", x => x.Name);
                    table.ForeignKey(
                        name: "FK_CharacterSnapshot_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharacterSnapshot_ApplicationUserId",
                table: "CharacterSnapshot",
                column: "ApplicationUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterSnapshot");
        }
    }
}
