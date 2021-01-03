using Microsoft.EntityFrameworkCore.Migrations;

namespace SamuraiApp.Data.Migrations
{
    public partial class battle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SamuraiBattles",
                table: "SamuraiBattles");

            migrationBuilder.DropIndex(
                name: "IX_SamuraiBattles_SamuraiId",
                table: "SamuraiBattles");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "SamuraiBattles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SamuraiBattles",
                table: "SamuraiBattles",
                columns: new[] { "SamuraiId", "BattleId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SamuraiBattles",
                table: "SamuraiBattles");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "SamuraiBattles",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SamuraiBattles",
                table: "SamuraiBattles",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_SamuraiBattles_SamuraiId",
                table: "SamuraiBattles",
                column: "SamuraiId");
        }
    }
}
