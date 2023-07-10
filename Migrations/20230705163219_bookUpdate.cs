using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace book_store_server_side.Migrations
{
    /// <inheritdoc />
    public partial class bookUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_AspNetUsers_AppUserId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_AppUserId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Books");

            migrationBuilder.CreateTable(
                name: "AppUserBook",
                columns: table => new
                {
                    AppUsersId = table.Column<string>(type: "varchar(255)", nullable: false),
                    BookInCartId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserBook", x => new { x.AppUsersId, x.BookInCartId });
                    table.ForeignKey(
                        name: "FK_AppUserBook_AspNetUsers_AppUsersId",
                        column: x => x.AppUsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUserBook_Books_BookInCartId",
                        column: x => x.BookInCartId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserBook_BookInCartId",
                table: "AppUserBook",
                column: "BookInCartId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUserBook");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Books",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_AppUserId",
                table: "Books",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_AspNetUsers_AppUserId",
                table: "Books",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
