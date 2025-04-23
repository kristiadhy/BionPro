using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
  /// <inheritdoc />
  public partial class editCascadeOnPurchaseDetailAndModel3 : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
          name: "FK_T01PurchaseDetails_M03Products_ProductID",
          table: "T01PurchaseDetails");

      migrationBuilder.AddForeignKey(
          name: "FK_T01PurchaseDetails_M03Products_ProductID",
          table: "T01PurchaseDetails",
          column: "ProductID",
          principalTable: "M03Products",
          principalColumn: "ProductID",
          onDelete: ReferentialAction.Restrict);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
          name: "FK_T01PurchaseDetails_M03Products_ProductID",
          table: "T01PurchaseDetails");

      migrationBuilder.AddForeignKey(
          name: "FK_T01PurchaseDetails_M03Products_ProductID",
          table: "T01PurchaseDetails",
          column: "ProductID",
          principalTable: "M03Products",
          principalColumn: "ProductID",
          onDelete: ReferentialAction.Cascade);
    }
  }
}
