using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
  /// <inheritdoc />
  public partial class changeDBConfig : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
          name: "FK_M03ProductStock_M03ProductLocation_LocationID",
          table: "M03ProductStock");

      migrationBuilder.AlterColumn<string>(
          name: "SupplierName",
          table: "M02Suppliers",
          type: "nvarchar(200)",
          maxLength: 200,
          nullable: true,
          oldClrType: typeof(string),
          oldType: "nvarchar(200)",
          oldMaxLength: 200);

      migrationBuilder.AlterColumn<string>(
          name: "CustomerName",
          table: "M01Customers",
          type: "nvarchar(200)",
          maxLength: 200,
          nullable: true,
          oldClrType: typeof(string),
          oldType: "nvarchar(200)",
          oldMaxLength: 200);

      migrationBuilder.AddForeignKey(
          name: "FK_M03ProductStock_M03ProductLocation_LocationID",
          table: "M03ProductStock",
          column: "LocationID",
          principalTable: "M03ProductLocation",
          principalColumn: "LocationID",
          onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
          name: "FK_M03ProductStock_M03ProductLocation_LocationID",
          table: "M03ProductStock");

      migrationBuilder.AlterColumn<string>(
          name: "SupplierName",
          table: "M02Suppliers",
          type: "nvarchar(200)",
          maxLength: 200,
          nullable: false,
          defaultValue: "",
          oldClrType: typeof(string),
          oldType: "nvarchar(200)",
          oldMaxLength: 200,
          oldNullable: true);

      migrationBuilder.AlterColumn<string>(
          name: "CustomerName",
          table: "M01Customers",
          type: "nvarchar(200)",
          maxLength: 200,
          nullable: false,
          defaultValue: "",
          oldClrType: typeof(string),
          oldType: "nvarchar(200)",
          oldMaxLength: 200,
          oldNullable: true);

      migrationBuilder.AddForeignKey(
          name: "FK_M03ProductStock_M03ProductLocation_LocationID",
          table: "M03ProductStock",
          column: "LocationID",
          principalTable: "M03ProductLocation",
          principalColumn: "LocationID");
    }
  }
}
