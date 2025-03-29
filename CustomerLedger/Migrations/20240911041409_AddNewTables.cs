using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerLedger.Migrations
{
    /// <inheritdoc />
    public partial class AddNewTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Notes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "")
                .Annotation("Relational:ColumnOrder", 3);

            migrationBuilder.AddColumn<string>(
                name: "Details",
                table: "Logs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "")
                .Annotation("Relational:ColumnOrder", 3);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "Details",
                table: "Logs");
        }
    }
}
