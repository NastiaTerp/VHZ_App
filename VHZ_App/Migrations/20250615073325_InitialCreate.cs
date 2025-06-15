using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VHZ_App.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.CreateTable(
                name: "Contact",
                columns: table => new
                {
                    id_contact = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    email = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    name_contact = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    number_phone = table.Column<string>(type: "char(11)", unicode: false, fixedLength: true, maxLength: 11, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact", x => x.id_contact);
                });
            */
            /*migrationBuilder.CreateTable(
                name: "Information",
                columns: table => new
                {
                    id_information = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    section_name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Information", x => x.id_information);
                });
            */
            migrationBuilder.CreateTable(
                name: "PasswordResetTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordResetTokens", x => x.Id);
                });

            /*migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    id_product = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    price = table.Column<decimal>(type: "money", nullable: false),
                    image = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    appointment = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: false),
                    name_product = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    product_compliance = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    type = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: false),
                    description_product = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.id_product);
                });
            */
            /*migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    id_user = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    surname = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    pathronimic = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    name_company = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: true),
                    inn = table.Column<string>(type: "char(12)", unicode: false, fixedLength: true, maxLength: 12, nullable: false),
                    kpp = table.Column<string>(type: "char(9)", unicode: false, fixedLength: true, maxLength: 9, nullable: true),
                    contact_number = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: false),
                    email = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    login = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    post = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    role = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.id_user);
                });

            migrationBuilder.CreateTable(
                name: "Technical_specifications",
                columns: table => new
                {
                    id_technical_specifications = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_product = table.Column<int>(type: "int", nullable: false),
                    name_indicator = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    standard = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Technical_specifications", x => x.id_technical_specifications);
                    table.ForeignKey(
                        name: "FK_Technical_specifications_Product",
                        column: x => x.id_product,
                        principalTable: "Product",
                        principalColumn: "id_product");
                });

            migrationBuilder.CreateTable(
                name: "Bank_card",
                columns: table => new
                {
                    id_bank_card = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_user = table.Column<int>(type: "int", nullable: false),
                    validity_period = table.Column<string>(type: "char(5)", unicode: false, fixedLength: true, maxLength: 5, nullable: false),
                    cvv_cvc = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: false),
                    card_number = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bank_card", x => x.id_bank_card);
                    table.ForeignKey(
                        name: "FK_Bank_card_User",
                        column: x => x.id_user,
                        principalTable: "User",
                        principalColumn: "id_user");
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    id_order = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_user = table.Column<int>(type: "int", nullable: false),
                    delivery_method = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    area = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    locality = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    street = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    house = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    total_price = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.id_order);
                    table.ForeignKey(
                        name: "FK_Order_User",
                        column: x => x.id_user,
                        principalTable: "User",
                        principalColumn: "id_user");
                });

            migrationBuilder.CreateTable(
                name: "Cart",
                columns: table => new
                {
                    id_cart = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_product = table.Column<int>(type: "int", nullable: false),
                    id_order = table.Column<int>(type: "int", nullable: true),
                    amount_products = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cart", x => x.id_cart);
                    table.ForeignKey(
                        name: "FK_Cart_Order",
                        column: x => x.id_order,
                        principalTable: "Order",
                        principalColumn: "id_order");
                    table.ForeignKey(
                        name: "FK_Cart_Product",
                        column: x => x.id_product,
                        principalTable: "Product",
                        principalColumn: "id_product");
                });
            */
            migrationBuilder.CreateIndex(
                name: "IX_Bank_card_id_user",
                table: "Bank_card",
                column: "id_user");

            migrationBuilder.CreateIndex(
                name: "IX_Cart_id_order",
                table: "Cart",
                column: "id_order");

            migrationBuilder.CreateIndex(
                name: "IX_Cart_id_product",
                table: "Cart",
                column: "id_product");

            migrationBuilder.CreateIndex(
                name: "IX_Order_id_user",
                table: "Order",
                column: "id_user");

            migrationBuilder.CreateIndex(
                name: "IX_Technical_specifications_id_product",
                table: "Technical_specifications",
                column: "id_product");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bank_card");

            migrationBuilder.DropTable(
                name: "Cart");

            migrationBuilder.DropTable(
                name: "Contact");

            migrationBuilder.DropTable(
                name: "Information");

            migrationBuilder.DropTable(
                name: "PasswordResetTokens");

            migrationBuilder.DropTable(
                name: "Technical_specifications");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
