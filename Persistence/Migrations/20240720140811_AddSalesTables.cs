using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
  /// <inheritdoc />
  public partial class AddSalesTables : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
          name: "FK_M03ProductBarcodes_M03Products_ProductID",
          table: "M03ProductBarcodes");

      migrationBuilder.DropForeignKey(
          name: "FK_M03ProductStock_M03ProductLocations_LocationID",
          table: "M03ProductStock");

      migrationBuilder.DropForeignKey(
          name: "FK_M03ProductStock_M03Products_ProductID",
          table: "M03ProductStock");

      migrationBuilder.DropForeignKey(
          name: "FK_T01Purchases_M02Suppliers_SupplierID",
          table: "T01Purchases");

      migrationBuilder.AlterColumn<Guid>(
          name: "ProductID",
          table: "M03ProductStock",
          type: "uniqueidentifier",
          nullable: true,
          oldClrType: typeof(Guid),
          oldType: "uniqueidentifier");

      migrationBuilder.AlterColumn<int>(
          name: "LocationID",
          table: "M03ProductStock",
          type: "int",
          nullable: true,
          oldClrType: typeof(int),
          oldType: "int");

      migrationBuilder.AlterColumn<string>(
          name: "SKU",
          table: "M03Products",
          type: "nvarchar(20)",
          maxLength: 20,
          nullable: true,
          oldClrType: typeof(string),
          oldType: "nvarchar(20)",
          oldMaxLength: 20);

      migrationBuilder.AlterColumn<string>(
          name: "Name",
          table: "M03Products",
          type: "nvarchar(255)",
          maxLength: 255,
          nullable: true,
          oldClrType: typeof(string),
          oldType: "nvarchar(255)",
          oldMaxLength: 255);

      migrationBuilder.AlterColumn<string>(
          name: "Name",
          table: "M03ProductLocations",
          type: "nvarchar(200)",
          maxLength: 200,
          nullable: true,
          oldClrType: typeof(string),
          oldType: "nvarchar(200)",
          oldMaxLength: 200);

      migrationBuilder.AlterColumn<string>(
          name: "Description",
          table: "M03ProductLocations",
          type: "nvarchar(max)",
          maxLength: 2147483647,
          nullable: true,
          oldClrType: typeof(string),
          oldType: "nvarchar(max)",
          oldMaxLength: 2147483647);

      migrationBuilder.AlterColumn<string>(
          name: "Name",
          table: "M03ProductCategories",
          type: "nvarchar(200)",
          maxLength: 200,
          nullable: true,
          oldClrType: typeof(string),
          oldType: "nvarchar(200)",
          oldMaxLength: 200);

      migrationBuilder.AlterColumn<Guid>(
          name: "ProductID",
          table: "M03ProductBarcodes",
          type: "uniqueidentifier",
          nullable: true,
          oldClrType: typeof(Guid),
          oldType: "uniqueidentifier");

      migrationBuilder.CreateTable(
          name: "T02Sales",
          columns: table => new
          {
            SaleID = table.Column<int>(type: "int", nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            TransactionCode = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
            Date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
            DiscountPercentage = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
            DiscountAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
            Description = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: true),
            CustomerID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            UpdatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            EntityStatus = table.Column<int>(type: "int", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_T02Sales", x => x.SaleID);
            table.ForeignKey(
                      name: "FK_T02Sales_M01Customers_CustomerID",
                      column: x => x.CustomerID,
                      principalTable: "M01Customers",
                      principalColumn: "CustomerID",
                      onDelete: ReferentialAction.Restrict);
          });

      migrationBuilder.CreateTable(
          name: "T02SaleDetails",
          columns: table => new
          {
            SaleID = table.Column<int>(type: "int", nullable: false),
            ProductID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            Quantity = table.Column<int>(type: "int", nullable: false),
            Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
            DiscountPercentage = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
            DiscountAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
            SubTotal = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_T02SaleDetails", x => new { x.ProductID, x.SaleID });
            table.ForeignKey(
                      name: "FK_T02SaleDetails_M03Products_ProductID",
                      column: x => x.ProductID,
                      principalTable: "M03Products",
                      principalColumn: "ProductID",
                      onDelete: ReferentialAction.Restrict);
            table.ForeignKey(
                      name: "FK_T02SaleDetails_T02Sales_SaleID",
                      column: x => x.SaleID,
                      principalTable: "T02Sales",
                      principalColumn: "SaleID",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateIndex(
          name: "IX_T02SaleDetails_SaleID",
          table: "T02SaleDetails",
          column: "SaleID");

      migrationBuilder.CreateIndex(
          name: "IX_T02Sales_CustomerID",
          table: "T02Sales",
          column: "CustomerID");

      migrationBuilder.AddForeignKey(
          name: "FK_M03ProductBarcodes_M03Products_ProductID",
          table: "M03ProductBarcodes",
          column: "ProductID",
          principalTable: "M03Products",
          principalColumn: "ProductID");

      migrationBuilder.AddForeignKey(
          name: "FK_M03ProductStock_M03ProductLocations_LocationID",
          table: "M03ProductStock",
          column: "LocationID",
          principalTable: "M03ProductLocations",
          principalColumn: "LocationID");

      migrationBuilder.AddForeignKey(
          name: "FK_M03ProductStock_M03Products_ProductID",
          table: "M03ProductStock",
          column: "ProductID",
          principalTable: "M03Products",
          principalColumn: "ProductID");

      migrationBuilder.AddForeignKey(
          name: "FK_T01Purchases_M02Suppliers_SupplierID",
          table: "T01Purchases",
          column: "SupplierID",
          principalTable: "M02Suppliers",
          principalColumn: "SupplierID",
          onDelete: ReferentialAction.Restrict);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
          name: "FK_M03ProductBarcodes_M03Products_ProductID",
          table: "M03ProductBarcodes");

      migrationBuilder.DropForeignKey(
          name: "FK_M03ProductStock_M03ProductLocations_LocationID",
          table: "M03ProductStock");

      migrationBuilder.DropForeignKey(
          name: "FK_M03ProductStock_M03Products_ProductID",
          table: "M03ProductStock");

      migrationBuilder.DropForeignKey(
          name: "FK_T01Purchases_M02Suppliers_SupplierID",
          table: "T01Purchases");

      migrationBuilder.DropTable(
          name: "T02SaleDetails");

      migrationBuilder.DropTable(
          name: "T02Sales");

      migrationBuilder.AlterColumn<Guid>(
          name: "ProductID",
          table: "M03ProductStock",
          type: "uniqueidentifier",
          nullable: false,
          defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
          oldClrType: typeof(Guid),
          oldType: "uniqueidentifier",
          oldNullable: true);

      migrationBuilder.AlterColumn<int>(
          name: "LocationID",
          table: "M03ProductStock",
          type: "int",
          nullable: false,
          defaultValue: 0,
          oldClrType: typeof(int),
          oldType: "int",
          oldNullable: true);

      migrationBuilder.AlterColumn<string>(
          name: "SKU",
          table: "M03Products",
          type: "nvarchar(20)",
          maxLength: 20,
          nullable: false,
          defaultValue: "",
          oldClrType: typeof(string),
          oldType: "nvarchar(20)",
          oldMaxLength: 20,
          oldNullable: true);

      migrationBuilder.AlterColumn<string>(
          name: "Name",
          table: "M03Products",
          type: "nvarchar(255)",
          maxLength: 255,
          nullable: false,
          defaultValue: "",
          oldClrType: typeof(string),
          oldType: "nvarchar(255)",
          oldMaxLength: 255,
          oldNullable: true);

      migrationBuilder.AlterColumn<string>(
          name: "Name",
          table: "M03ProductLocations",
          type: "nvarchar(200)",
          maxLength: 200,
          nullable: false,
          defaultValue: "",
          oldClrType: typeof(string),
          oldType: "nvarchar(200)",
          oldMaxLength: 200,
          oldNullable: true);

      migrationBuilder.AlterColumn<string>(
          name: "Description",
          table: "M03ProductLocations",
          type: "nvarchar(max)",
          maxLength: 2147483647,
          nullable: false,
          defaultValue: "",
          oldClrType: typeof(string),
          oldType: "nvarchar(max)",
          oldMaxLength: 2147483647,
          oldNullable: true);

      migrationBuilder.AlterColumn<string>(
          name: "Name",
          table: "M03ProductCategories",
          type: "nvarchar(200)",
          maxLength: 200,
          nullable: false,
          defaultValue: "",
          oldClrType: typeof(string),
          oldType: "nvarchar(200)",
          oldMaxLength: 200,
          oldNullable: true);

      migrationBuilder.AlterColumn<Guid>(
          name: "ProductID",
          table: "M03ProductBarcodes",
          type: "uniqueidentifier",
          nullable: false,
          defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
          oldClrType: typeof(Guid),
          oldType: "uniqueidentifier",
          oldNullable: true);

      migrationBuilder.AddForeignKey(
          name: "FK_M03ProductBarcodes_M03Products_ProductID",
          table: "M03ProductBarcodes",
          column: "ProductID",
          principalTable: "M03Products",
          principalColumn: "ProductID",
          onDelete: ReferentialAction.Cascade);

      migrationBuilder.AddForeignKey(
          name: "FK_M03ProductStock_M03ProductLocations_LocationID",
          table: "M03ProductStock",
          column: "LocationID",
          principalTable: "M03ProductLocations",
          principalColumn: "LocationID",
          onDelete: ReferentialAction.Cascade);

      migrationBuilder.AddForeignKey(
          name: "FK_M03ProductStock_M03Products_ProductID",
          table: "M03ProductStock",
          column: "ProductID",
          principalTable: "M03Products",
          principalColumn: "ProductID",
          onDelete: ReferentialAction.Cascade);

      migrationBuilder.AddForeignKey(
          name: "FK_T01Purchases_M02Suppliers_SupplierID",
          table: "T01Purchases",
          column: "SupplierID",
          principalTable: "M02Suppliers",
          principalColumn: "SupplierID");
    }
  }
}
