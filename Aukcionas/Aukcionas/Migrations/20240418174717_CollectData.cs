using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;
#nullable disable

namespace Aukcionas.Migrations
{
    [ExcludeFromCodeCoverage]
    /// <inheritdoc />
    public partial class CollectData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CollectData",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CollectData",
                table: "AspNetUsers");
        }
    }
}
