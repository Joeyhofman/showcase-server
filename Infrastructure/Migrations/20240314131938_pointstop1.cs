using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class pointstop1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DataflowAssociations_DataflowPoints_Point1Id",
                table: "DataflowAssociations");

            migrationBuilder.DropForeignKey(
                name: "FK_DataflowAssociations_DataflowPoints_Point2Id",
                table: "DataflowAssociations");

            migrationBuilder.RenameColumn(
                name: "Point2Id",
                table: "DataflowAssociations",
                newName: "P2Id");

            migrationBuilder.RenameColumn(
                name: "Point1Id",
                table: "DataflowAssociations",
                newName: "P1Id");

            migrationBuilder.RenameIndex(
                name: "IX_DataflowAssociations_Point2Id",
                table: "DataflowAssociations",
                newName: "IX_DataflowAssociations_P2Id");

            migrationBuilder.RenameIndex(
                name: "IX_DataflowAssociations_Point1Id",
                table: "DataflowAssociations",
                newName: "IX_DataflowAssociations_P1Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DataflowAssociations_DataflowPoints_P1Id",
                table: "DataflowAssociations",
                column: "P1Id",
                principalTable: "DataflowPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DataflowAssociations_DataflowPoints_P2Id",
                table: "DataflowAssociations",
                column: "P2Id",
                principalTable: "DataflowPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DataflowAssociations_DataflowPoints_P1Id",
                table: "DataflowAssociations");

            migrationBuilder.DropForeignKey(
                name: "FK_DataflowAssociations_DataflowPoints_P2Id",
                table: "DataflowAssociations");

            migrationBuilder.RenameColumn(
                name: "P2Id",
                table: "DataflowAssociations",
                newName: "Point2Id");

            migrationBuilder.RenameColumn(
                name: "P1Id",
                table: "DataflowAssociations",
                newName: "Point1Id");

            migrationBuilder.RenameIndex(
                name: "IX_DataflowAssociations_P2Id",
                table: "DataflowAssociations",
                newName: "IX_DataflowAssociations_Point2Id");

            migrationBuilder.RenameIndex(
                name: "IX_DataflowAssociations_P1Id",
                table: "DataflowAssociations",
                newName: "IX_DataflowAssociations_Point1Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DataflowAssociations_DataflowPoints_Point1Id",
                table: "DataflowAssociations",
                column: "Point1Id",
                principalTable: "DataflowPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DataflowAssociations_DataflowPoints_Point2Id",
                table: "DataflowAssociations",
                column: "Point2Id",
                principalTable: "DataflowPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
