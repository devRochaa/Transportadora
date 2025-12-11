using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transportadora.Infrasctructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterTable_Client_Orders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_Client_ClientId",
                table: "Address");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderTrack_ClientOrder_OrderId",
                table: "OrderTrack");

            migrationBuilder.DropTable(
                name: "ClientOrder");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Client",
                table: "Client");

            migrationBuilder.RenameTable(
                name: "Client",
                newName: "Person");

            migrationBuilder.RenameIndex(
                name: "IX_Client_Phone",
                table: "Person",
                newName: "IX_Person_Phone");

            migrationBuilder.RenameIndex(
                name: "IX_Client_NationalDocument",
                table: "Person",
                newName: "IX_Person_NationalDocument");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Person",
                table: "Person",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MaxDeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalAmount = table.Column<int>(type: "int", nullable: false),
                    WeightCategory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Freight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Distance = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    DestinataryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OriginId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DestinyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_Address_DestinyId",
                        column: x => x.DestinyId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order_Address_OriginId",
                        column: x => x.OriginId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order_Person_DestinataryId",
                        column: x => x.DestinataryId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_DestinataryId",
                table: "Order",
                column: "DestinataryId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_DestinyId",
                table: "Order",
                column: "DestinyId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_OriginId",
                table: "Order",
                column: "OriginId");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_Person_ClientId",
                table: "Address",
                column: "ClientId",
                principalTable: "Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderTrack_Order_OrderId",
                table: "OrderTrack",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_Person_ClientId",
                table: "Address");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderTrack_Order_OrderId",
                table: "OrderTrack");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Person",
                table: "Person");

            migrationBuilder.RenameTable(
                name: "Person",
                newName: "Client");

            migrationBuilder.RenameIndex(
                name: "IX_Person_Phone",
                table: "Client",
                newName: "IX_Client_Phone");

            migrationBuilder.RenameIndex(
                name: "IX_Person_NationalDocument",
                table: "Client",
                newName: "IX_Client_NationalDocument");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Client",
                table: "Client",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ClientOrder",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DestinataryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DestinyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OriginId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Distance = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Freight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxDeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalAmount = table.Column<int>(type: "int", nullable: false),
                    WeightCategory = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientOrder_Address_DestinyId",
                        column: x => x.DestinyId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClientOrder_Address_OriginId",
                        column: x => x.OriginId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClientOrder_Client_DestinataryId",
                        column: x => x.DestinataryId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientOrder_DestinataryId",
                table: "ClientOrder",
                column: "DestinataryId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientOrder_DestinyId",
                table: "ClientOrder",
                column: "DestinyId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientOrder_OriginId",
                table: "ClientOrder",
                column: "OriginId");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_Client_ClientId",
                table: "Address",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderTrack_ClientOrder_OrderId",
                table: "OrderTrack",
                column: "OrderId",
                principalTable: "ClientOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
