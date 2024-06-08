using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTablesRelatedToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_M02SUPPLIERS",
                table: "M02SUPPLIERS");

            migrationBuilder.DropPrimaryKey(
                name: "PK_M01CUSTOMERS",
                table: "M01CUSTOMERS");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "056c9142-0011-47e2-99a9-481a2adfe656");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "39a5cee2-7b91-4278-8135-987dbc959f36");

            migrationBuilder.RenameTable(
                name: "M02SUPPLIERS",
                newName: "M02Suppliers");

            migrationBuilder.RenameTable(
                name: "M01CUSTOMERS",
                newName: "M01Customers");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "M02Suppliers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateCreated",
                table: "M02Suppliers",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateDeleted",
                table: "M02Suppliers",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateUpdated",
                table: "M02Suppliers",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "M02Suppliers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "M02Suppliers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_M02Suppliers",
                table: "M02Suppliers",
                column: "SupplierID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_M01Customers",
                table: "M01Customers",
                column: "CustomerID");

            migrationBuilder.CreateTable(
                name: "M03ProductCategory",
                columns: table => new
                {
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M03ProductCategory", x => x.CategoryID);
                });

            migrationBuilder.CreateTable(
                name: "M03ProductLocation",
                columns: table => new
                {
                    LocationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M03ProductLocation", x => x.LocationID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "M03ProductCategory");

            migrationBuilder.DropTable(
                name: "M03ProductLocation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_M02Suppliers",
                table: "M02Suppliers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_M01Customers",
                table: "M01Customers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "13daf828-8497-4e9e-9182-d0d42eada310");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "304d9e18-6523-4526-b3f4-ef57e8227114");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "M02Suppliers");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "M02Suppliers");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "M02Suppliers");

            migrationBuilder.DropColumn(
                name: "DateUpdated",
                table: "M02Suppliers");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "M02Suppliers");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "M02Suppliers");

            migrationBuilder.RenameTable(
                name: "M02Suppliers",
                newName: "M02SUPPLIERS");

            migrationBuilder.RenameTable(
                name: "M01Customers",
                newName: "M01CUSTOMERS");

            migrationBuilder.AddPrimaryKey(
                name: "PK_M02SUPPLIERS",
                table: "M02SUPPLIERS",
                column: "SupplierID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_M01CUSTOMERS",
                table: "M01CUSTOMERS",
                column: "CustomerID");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "056c9142-0011-47e2-99a9-481a2adfe656", null, "Administrator", "ADMINISTRATOR" },
                    { "39a5cee2-7b91-4278-8135-987dbc959f36", null, "Manager", "MANAGER" }
                });
        }
    }
}
