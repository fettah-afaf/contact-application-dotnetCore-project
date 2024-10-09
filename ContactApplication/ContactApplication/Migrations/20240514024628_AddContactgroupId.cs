using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContactApplication.Migrations
{
    /// <inheritdoc />
    public partial class AddContactgroupId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContactGroupId",
                table: "ContactGroups",
                type: "int",
                nullable: false)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContactGroups",
                table: "ContactGroups",
                column: "ContactGroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ContactGroups",
                table: "ContactGroups");

            migrationBuilder.DropColumn(
                name: "ContactGroupId",
                table: "ContactGroups");
        }
    }
}
