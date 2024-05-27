using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace Aukcionas.Migrations
{
    [ExcludeFromCodeCoverage]
    /// <inheritdoc />
    public partial class auctionLikes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LikedBy",
                table: "Auctions",
                newName: "auction_likes_list");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "auction_likes_list",
                table: "Auctions",
                newName: "LikedBy");
        }
    }
}
