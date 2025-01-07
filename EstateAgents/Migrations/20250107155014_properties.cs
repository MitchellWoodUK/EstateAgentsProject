using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EstateAgents.Migrations
{
    /// <inheritdoc />
    public partial class properties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Properties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<double>(type: "REAL", nullable: false),
                    PropertyType = table.Column<int>(type: "INTEGER", nullable: false),
                    ListingDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AmountofBedrooms = table.Column<int>(type: "INTEGER", nullable: false),
                    AmountofBathrooms = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Image = table.Column<string>(type: "TEXT", nullable: false),
                    M2 = table.Column<double>(type: "REAL", nullable: false),
                    AmountofParking = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Properties");
        }
    }
}
