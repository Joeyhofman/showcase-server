using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class extensionOfDataflowpoint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "DataflowPoints",
                newName: "fillColor");

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "DataflowPoints",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Classname",
                table: "DataflowPoints",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InputValue",
                table: "DataflowPoints",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isEditing",
                table: "DataflowPoints",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Classname",
                table: "DataflowPoints");

            migrationBuilder.DropColumn(
                name: "InputValue",
                table: "DataflowPoints");

            migrationBuilder.DropColumn(
                name: "isEditing",
                table: "DataflowPoints");

            migrationBuilder.RenameColumn(
                name: "fillColor",
                table: "DataflowPoints",
                newName: "Name");

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "DataflowPoints",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }
    }
}
