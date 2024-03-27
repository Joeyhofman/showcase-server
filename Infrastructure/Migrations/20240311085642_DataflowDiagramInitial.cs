using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DataflowDiagramInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataflowDiagrams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ProjectId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataflowDiagrams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataflowDiagrams_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DataflowPoints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    X = table.Column<int>(type: "INTEGER", nullable: false),
                    Y = table.Column<int>(type: "INTEGER", nullable: false),
                    Width = table.Column<int>(type: "INTEGER", nullable: false),
                    Height = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Color = table.Column<string>(type: "TEXT", nullable: true),
                    DataflowDiagramId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataflowPoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataflowPoints_DataflowDiagrams_DataflowDiagramId",
                        column: x => x.DataflowDiagramId,
                        principalTable: "DataflowDiagrams",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DataflowAssociations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Point1Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Point2Id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataflowAssociations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataflowAssociations_DataflowPoints_Point1Id",
                        column: x => x.Point1Id,
                        principalTable: "DataflowPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DataflowAssociations_DataflowPoints_Point2Id",
                        column: x => x.Point2Id,
                        principalTable: "DataflowPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DataflowAssociations_Point1Id",
                table: "DataflowAssociations",
                column: "Point1Id");

            migrationBuilder.CreateIndex(
                name: "IX_DataflowAssociations_Point2Id",
                table: "DataflowAssociations",
                column: "Point2Id");

            migrationBuilder.CreateIndex(
                name: "IX_DataflowDiagrams_ProjectId",
                table: "DataflowDiagrams",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_DataflowPoints_DataflowDiagramId",
                table: "DataflowPoints",
                column: "DataflowDiagramId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataflowAssociations");

            migrationBuilder.DropTable(
                name: "DataflowPoints");

            migrationBuilder.DropTable(
                name: "DataflowDiagrams");
        }
    }
}
