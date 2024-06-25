using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updateBaseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "M03ProductStock");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "M03ProductStock");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "M03ProductLocation");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "M03ProductLocation");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "M03ProductCategory");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "M03ProductCategory");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "M03Product");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "M03Product");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "M02Suppliers");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "M02Suppliers");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "M01Customers");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "M01Customers");

            migrationBuilder.AddColumn<int>(
                name: "EntityStatus",
                table: "M03ProductStock",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EntityStatus",
                table: "M03ProductLocation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EntityStatus",
                table: "M03ProductCategory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EntityStatus",
                table: "M03Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EntityStatus",
                table: "M02Suppliers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EntityStatus",
                table: "M01Customers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EntityStatus",
                table: "M03ProductStock");

            migrationBuilder.DropColumn(
                name: "EntityStatus",
                table: "M03ProductLocation");

            migrationBuilder.DropColumn(
                name: "EntityStatus",
                table: "M03ProductCategory");

            migrationBuilder.DropColumn(
                name: "EntityStatus",
                table: "M03Product");

            migrationBuilder.DropColumn(
                name: "EntityStatus",
                table: "M02Suppliers");

            migrationBuilder.DropColumn(
                name: "EntityStatus",
                table: "M01Customers");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateDeleted",
                table: "M03ProductStock",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "M03ProductStock",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateDeleted",
                table: "M03ProductLocation",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "M03ProductLocation",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateDeleted",
                table: "M03ProductCategory",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "M03ProductCategory",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateDeleted",
                table: "M03Product",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "M03Product",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateDeleted",
                table: "M02Suppliers",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "M02Suppliers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateDeleted",
                table: "M01Customers",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "M01Customers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
