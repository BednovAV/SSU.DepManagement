using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSU.DM.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class add_dative : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NameDat",
                table: "Faculties",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameDat",
                table: "Faculties");
        }
    }
}
