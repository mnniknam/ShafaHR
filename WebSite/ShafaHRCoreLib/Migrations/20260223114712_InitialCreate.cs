using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShafaHRCoreLib.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Firstname = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Lastname = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RecordKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    RecordCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RecordModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RecordDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admin", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "File",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Extension = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    RecordKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    RecordCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RecordModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RecordDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_File", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Page",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Code = table.Column<int>(type: "int", nullable: true),
                    BodyText = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: true),
                    BodyHTML = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: true),
                    RecordKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    RecordCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RecordModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RecordDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Page", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecordChangeLog",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecordType = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    RecordKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    DateOf = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AdminId = table.Column<long>(type: "bigint", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Action = table.Column<int>(type: "int", nullable: false),
                    RecordChangeAction = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecordChangeLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AdminRole",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Role = table.Column<int>(type: "int", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    AdminId = table.Column<long>(type: "bigint", nullable: false),
                    RecordKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    RecordCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RecordModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RecordDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminRole_Admin_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Admin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Publication",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: true),
                    ThumbnailId = table.Column<long>(type: "bigint", nullable: true),
                    PDFId = table.Column<long>(type: "bigint", nullable: true),
                    VideoId = table.Column<long>(type: "bigint", nullable: true),
                    ViewCount = table.Column<long>(type: "bigint", nullable: false),
                    RecordKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    RecordCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RecordModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RecordDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publication", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Publication_File_PDFId",
                        column: x => x.PDFId,
                        principalTable: "File",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Publication_File_ThumbnailId",
                        column: x => x.ThumbnailId,
                        principalTable: "File",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Publication_File_VideoId",
                        column: x => x.VideoId,
                        principalTable: "File",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminRole_AdminId",
                table: "AdminRole",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Publication_PDFId",
                table: "Publication",
                column: "PDFId");

            migrationBuilder.CreateIndex(
                name: "IX_Publication_ThumbnailId",
                table: "Publication",
                column: "ThumbnailId");

            migrationBuilder.CreateIndex(
                name: "IX_Publication_VideoId",
                table: "Publication",
                column: "VideoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminRole");

            migrationBuilder.DropTable(
                name: "Page");

            migrationBuilder.DropTable(
                name: "Publication");

            migrationBuilder.DropTable(
                name: "RecordChangeLog");

            migrationBuilder.DropTable(
                name: "Admin");

            migrationBuilder.DropTable(
                name: "File");
        }
    }
}
