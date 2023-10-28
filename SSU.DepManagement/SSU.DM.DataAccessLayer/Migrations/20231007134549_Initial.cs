using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SSU.DM.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Faculties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faculties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SavedFiles",
                columns: table => new
                {
                    Key = table.Column<string>(type: "text", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: true),
                    Bytes = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedFiles", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationForms",
                columns: table => new
                {
                    ApplicationFormId = table.Column<Guid>(type: "uuid", nullable: false),
                    DateTimeCreated = table.Column<DateTime>(type: "timestamp", nullable: false),
                    FileKey = table.Column<string>(type: "text", nullable: true),
                    FacultyId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationForms", x => x.ApplicationFormId);
                    table.ForeignKey(
                        name: "FK_ApplicationForms_Faculties_FacultyId",
                        column: x => x.FacultyId,
                        principalTable: "Faculties",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ApplicationForms_SavedFiles_FileKey",
                        column: x => x.FileKey,
                        principalTable: "SavedFiles",
                        principalColumn: "Key");
                });

            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NameDiscipline = table.Column<string>(type: "text", nullable: true),
                    Direction = table.Column<string>(type: "text", nullable: true),
                    Semester = table.Column<int>(type: "integer", nullable: false),
                    BudgetCount = table.Column<int>(type: "integer", nullable: false),
                    CommercialCount = table.Column<int>(type: "integer", nullable: false),
                    GroupNumber = table.Column<string>(type: "text", nullable: true),
                    GroupForm = table.Column<string>(type: "text", nullable: true),
                    TotalHours = table.Column<int>(type: "integer", nullable: false),
                    LectureHours = table.Column<int>(type: "integer", nullable: false),
                    PracticalHours = table.Column<int>(type: "integer", nullable: false),
                    LaboratoryHours = table.Column<int>(type: "integer", nullable: false),
                    IndependentWorkHours = table.Column<int>(type: "integer", nullable: false),
                    Reporting = table.Column<int>(type: "integer", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true),
                    ApplicationFormId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Requests_ApplicationForms_ApplicationFormId",
                        column: x => x.ApplicationFormId,
                        principalTable: "ApplicationForms",
                        principalColumn: "ApplicationFormId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationForms_FacultyId",
                table: "ApplicationForms",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationForms_FileKey",
                table: "ApplicationForms",
                column: "FileKey");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_ApplicationFormId",
                table: "Requests",
                column: "ApplicationFormId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.DropTable(
                name: "ApplicationForms");

            migrationBuilder.DropTable(
                name: "Faculties");

            migrationBuilder.DropTable(
                name: "SavedFiles");
        }
    }
}
