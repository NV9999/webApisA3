using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAssignment3.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    description = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    image = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    pricing = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    shipping_cost = table.Column<decimal>(type: "decimal(10,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__products__3213E83FD87EA57E", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    email = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    password = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    username = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    purchase_history = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    shipping_address = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__users__3213E83F534F33A0", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "cart",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_ids = table.Column<int>(type: "int", nullable: false),
                    quantities = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    user_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__cart__3213E83FC03D9464", x => x.id);
                    table.ForeignKey(
                        name: "FK_cart_product",
                        column: x => x.product_ids,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_cart_user",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "comments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_id = table.Column<int>(type: "int", nullable: true),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    rating = table.Column<int>(type: "int", nullable: true),
                    image = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    text = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__comments__3213E83F1E718B74", x => x.id);
                    table.ForeignKey(
                        name: "FK_comments_product",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_comments_user",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cart_id = table.Column<int>(type: "int", nullable: true),
                    order_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    total = table.Column<decimal>(type: "decimal(10,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__orders__3213E83F15802FAF", x => x.id);
                    table.ForeignKey(
                        name: "FK_orders_cart",
                        column: x => x.cart_id,
                        principalTable: "cart",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_cart_product_ids",
                table: "cart",
                column: "product_ids");

            migrationBuilder.CreateIndex(
                name: "IX_cart_user_id",
                table: "cart",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_comments_product_id",
                table: "comments",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_comments_user_id",
                table: "comments",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_cart_id",
                table: "orders",
                column: "cart_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "comments");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "cart");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
