using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class associatointodaigram : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DataflowDiagramId",
                table: "DataflowAssociations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DataflowAssociations_DataflowDiagramId",
                table: "DataflowAssociations",
                column: "DataflowDiagramId");

            migrationBuilder.AddForeignKey(
                name: "FK_DataflowAssociations_DataflowDiagrams_DataflowDiagramId",
                table: "DataflowAssociations",
                column: "DataflowDiagramId",
                principalTable: "DataflowDiagrams",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DataflowAssociations_DataflowDiagrams_DataflowDiagramId",
                table: "DataflowAssociations");

            migrationBuilder.DropIndex(
                name: "IX_DataflowAssociations_DataflowDiagramId",
                table: "DataflowAssociations");

            migrationBuilder.DropColumn(
                name: "DataflowDiagramId",
                table: "DataflowAssociations");
        }
    }
}
