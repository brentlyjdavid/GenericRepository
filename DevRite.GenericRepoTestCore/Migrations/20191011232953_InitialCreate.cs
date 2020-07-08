using Microsoft.EntityFrameworkCore.Migrations;

namespace DevRite.GenericRepoTestCore.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TestClasses",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestClasses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TestComposites",
                columns: table => new
                {
                    Id1 = table.Column<string>(nullable: false),
                    Id2 = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestComposites", x => new { x.Id1, x.Id2 });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestClasses");

            migrationBuilder.DropTable(
                name: "TestComposites");
        }
    }
}
