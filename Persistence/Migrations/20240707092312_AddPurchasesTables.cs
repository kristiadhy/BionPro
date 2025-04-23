using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
  /// <inheritdoc />
  public partial class AddPurchasesTables : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AlterColumn<string>(
          name: "ImageUrl",
          table: "M03Products",
          type: "nvarchar(500)",
          maxLength: 500,
          nullable: true,
          oldClrType: typeof(string),
          oldType: "nvarchar(255)",
          oldMaxLength: 255,
          oldNullable: true);

      migrationBuilder.CreateTable(
          name: "M03ProductBarcodes",
          columns: table => new
          {
            EAN13Barcode = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: true),
            BarcodeImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
            ProductID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            UpdatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            EntityStatus = table.Column<int>(type: "int", nullable: false)
          },
          constraints: table =>
          {
            table.ForeignKey(
                      name: "FK_M03ProductBarcodes_M03Products_ProductID",
                      column: x => x.ProductID,
                      principalTable: "M03Products",
                      principalColumn: "ProductID",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "T01Purchases",
          columns: table => new
          {
            PurchaseID = table.Column<int>(type: "int", nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            TransactionCode = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
            Date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
            DiscountPercentage = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
            DiscountAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
            Description = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: true),
            SupplierID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            UpdatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            EntityStatus = table.Column<int>(type: "int", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_T01Purchases", x => x.PurchaseID);
            table.ForeignKey(
                      name: "FK_T01Purchases_M02Suppliers_SupplierID",
                      column: x => x.SupplierID,
                      principalTable: "M02Suppliers",
                      principalColumn: "SupplierID");
          });

      migrationBuilder.CreateTable(
          name: "T01PurchaseDetails",
          columns: table => new
          {
            PurchaseID = table.Column<int>(type: "int", nullable: false),
            ProductID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            Quantity = table.Column<int>(type: "int", nullable: false),
            Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
            DiscountPercentage = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
            DiscountAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
            SubTotal = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_T01PurchaseDetails", x => new { x.ProductID, x.PurchaseID });
            table.ForeignKey(
                      name: "FK_T01PurchaseDetails_M03Products_ProductID",
                      column: x => x.ProductID,
                      principalTable: "M03Products",
                      principalColumn: "ProductID",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "FK_T01PurchaseDetails_T01Purchases_PurchaseID",
                      column: x => x.PurchaseID,
                      principalTable: "T01Purchases",
                      principalColumn: "PurchaseID",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateIndex(
          name: "IX_M03ProductBarcodes_ProductID",
          table: "M03ProductBarcodes",
          column: "ProductID");

      migrationBuilder.CreateIndex(
          name: "IX_T01PurchaseDetails_PurchaseID",
          table: "T01PurchaseDetails",
          column: "PurchaseID");

      migrationBuilder.CreateIndex(
          name: "IX_T01Purchases_SupplierID",
          table: "T01Purchases",
          column: "SupplierID");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "M03ProductBarcodes");

      migrationBuilder.DropTable(
          name: "T01PurchaseDetails");

      migrationBuilder.DropTable(
          name: "T01Purchases");

      migrationBuilder.AlterColumn<string>(
          name: "ImageUrl",
          table: "M03Products",
          type: "nvarchar(255)",
          maxLength: 255,
          nullable: true,
          oldClrType: typeof(string),
          oldType: "nvarchar(500)",
          oldMaxLength: 500,
          oldNullable: true);
    }
  }
}
