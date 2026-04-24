using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoodHamburger.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class alter_table_order_item_add_product_id : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "product_id",
                table: "order_item",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_order_item_product_id",
                table: "order_item",
                column: "product_id");

            migrationBuilder.AddForeignKey(
                name: "FK_order_item_product_product_id",
                table: "order_item",
                column: "product_id",
                principalTable: "product",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_order_item_product_product_id",
                table: "order_item");

            migrationBuilder.DropIndex(
                name: "IX_order_item_product_id",
                table: "order_item");

            migrationBuilder.DropColumn(
                name: "product_id",
                table: "order_item");
        }
    }
}
