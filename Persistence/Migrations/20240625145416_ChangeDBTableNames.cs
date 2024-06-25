using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDBTableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_M03Product_M03ProductCategory_CategoryID",
                table: "M03Product");

            migrationBuilder.DropForeignKey(
                name: "FK_M03ProductStock_M03ProductLocation_LocationID",
                table: "M03ProductStock");

            migrationBuilder.DropForeignKey(
                name: "FK_M03ProductStock_M03Product_ProductID",
                table: "M03ProductStock");

            migrationBuilder.DropPrimaryKey(
                name: "PK_M03ProductLocation",
                table: "M03ProductLocation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_M03ProductCategory",
                table: "M03ProductCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_M03Product",
                table: "M03Product");

            migrationBuilder.RenameTable(
                name: "M03ProductLocation",
                newName: "M03ProductLocations");

            migrationBuilder.RenameTable(
                name: "M03ProductCategory",
                newName: "M03ProductCategories");

            migrationBuilder.RenameTable(
                name: "M03Product",
                newName: "M03Products");

            migrationBuilder.RenameIndex(
                name: "IX_M03Product_CategoryID",
                table: "M03Products",
                newName: "IX_M03Products_CategoryID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_M03ProductLocations",
                table: "M03ProductLocations",
                column: "LocationID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_M03ProductCategories",
                table: "M03ProductCategories",
                column: "CategoryID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_M03Products",
                table: "M03Products",
                column: "ProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_M03Products_M03ProductCategories_CategoryID",
                table: "M03Products",
                column: "CategoryID",
                principalTable: "M03ProductCategories",
                principalColumn: "CategoryID");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_M03Products_M03ProductCategories_CategoryID",
                table: "M03Products");

            migrationBuilder.DropForeignKey(
                name: "FK_M03ProductStock_M03ProductLocations_LocationID",
                table: "M03ProductStock");

            migrationBuilder.DropForeignKey(
                name: "FK_M03ProductStock_M03Products_ProductID",
                table: "M03ProductStock");

            migrationBuilder.DropPrimaryKey(
                name: "PK_M03Products",
                table: "M03Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_M03ProductLocations",
                table: "M03ProductLocations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_M03ProductCategories",
                table: "M03ProductCategories");

            migrationBuilder.RenameTable(
                name: "M03Products",
                newName: "M03Product");

            migrationBuilder.RenameTable(
                name: "M03ProductLocations",
                newName: "M03ProductLocation");

            migrationBuilder.RenameTable(
                name: "M03ProductCategories",
                newName: "M03ProductCategory");

            migrationBuilder.RenameIndex(
                name: "IX_M03Products_CategoryID",
                table: "M03Product",
                newName: "IX_M03Product_CategoryID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_M03Product",
                table: "M03Product",
                column: "ProductID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_M03ProductLocation",
                table: "M03ProductLocation",
                column: "LocationID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_M03ProductCategory",
                table: "M03ProductCategory",
                column: "CategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_M03Product_M03ProductCategory_CategoryID",
                table: "M03Product",
                column: "CategoryID",
                principalTable: "M03ProductCategory",
                principalColumn: "CategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_M03ProductStock_M03ProductLocation_LocationID",
                table: "M03ProductStock",
                column: "LocationID",
                principalTable: "M03ProductLocation",
                principalColumn: "LocationID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_M03ProductStock_M03Product_ProductID",
                table: "M03ProductStock",
                column: "ProductID",
                principalTable: "M03Product",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
