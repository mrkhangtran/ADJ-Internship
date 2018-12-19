using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ADJ.DataAccess.Migrations
{
    public partial class test1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDateUtc = table.Column<DateTime>(nullable: true),
                    Order = table.Column<string>(nullable: true),
                    OrderId = table.Column<int>(nullable: false),
                    Line = table.Column<string>(nullable: true),
                    Item = table.Column<string>(nullable: true),
                    Carier = table.Column<string>(nullable: true),
                    Vessel = table.Column<string>(nullable: true),
                    ETD = table.Column<DateTime>(nullable: false),
                    ETA = table.Column<DateTime>(nullable: false),
                    Voyage = table.Column<string>(nullable: true),
                    Quantity = table.Column<decimal>(nullable: false),
                    Cartoons = table.Column<int>(nullable: false),
                    Cube = table.Column<decimal>(nullable: false),
                    PackType = table.Column<string>(nullable: true),
                    LoadingType = table.Column<string>(nullable: true),
                    Mode = table.Column<string>(nullable: true),
                    FreightTerms = table.Column<string>(nullable: true),
                    Consignee = table.Column<string>(nullable: true),
                    GrossWeight = table.Column<decimal>(nullable: false),
                    BookingDate = table.Column<DateTime>(nullable: false),
                    BookingType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DCBookings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDateUtc = table.Column<DateTime>(nullable: true),
                    DeliveryMethod = table.Column<string>(maxLength: 20, nullable: true),
                    WareHouse = table.Column<string>(maxLength: 50, nullable: true),
                    BookingRef = table.Column<string>(maxLength: 12, nullable: true),
                    BookingDate = table.Column<DateTime>(nullable: false),
                    BookingTime = table.Column<string>(maxLength: 12, nullable: true),
                    Haulier = table.Column<string>(maxLength: 30, nullable: true),
                    Created = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DCBookings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DCConfirmations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDateUtc = table.Column<DateTime>(nullable: true),
                    DeliveryDate = table.Column<DateTime>(nullable: false),
                    DeliveryTime = table.Column<string>(maxLength: 12, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DCConfirmations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDateUtc = table.Column<DateTime>(nullable: true),
                    PONumber = table.Column<string>(nullable: true),
                    OrderDate = table.Column<DateTime>(nullable: false),
                    Company = table.Column<string>(maxLength: 30, nullable: true),
                    Supplier = table.Column<string>(maxLength: 30, nullable: true),
                    Origin = table.Column<string>(nullable: false),
                    PortOfLoading = table.Column<string>(nullable: false),
                    PortOfDelivery = table.Column<string>(nullable: false),
                    Buyer = table.Column<string>(maxLength: 30, nullable: true),
                    Department = table.Column<string>(maxLength: 30, nullable: true),
                    OrderType = table.Column<string>(maxLength: 30, nullable: true),
                    Season = table.Column<string>(nullable: true),
                    Factory = table.Column<string>(maxLength: 30, nullable: true),
                    Currency = table.Column<int>(nullable: false),
                    ShipDate = table.Column<DateTime>(nullable: false),
                    LatestShipDate = table.Column<DateTime>(nullable: false),
                    DeliveryDate = table.Column<DateTime>(nullable: false),
                    Mode = table.Column<string>(nullable: true),
                    Vendor = table.Column<string>(maxLength: 30, nullable: true),
                    POQuantity = table.Column<decimal>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDateUtc = table.Column<DateTime>(nullable: true),
                    Test = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArriveOfDepacths",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDateUtc = table.Column<DateTime>(nullable: true),
                    BookingId = table.Column<int>(nullable: false),
                    Carrier = table.Column<string>(nullable: true),
                    Vessel = table.Column<string>(nullable: true),
                    Voyage = table.Column<string>(nullable: true),
                    CTD = table.Column<DateTime>(nullable: false),
                    ETA = table.Column<DateTime>(nullable: false),
                    OriginPort = table.Column<string>(nullable: true),
                    DestinationPort = table.Column<string>(nullable: true),
                    Mode = table.Column<string>(nullable: true),
                    Confirmed = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArriveOfDepacths", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArriveOfDepacths_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CAs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDateUtc = table.Column<DateTime>(nullable: true),
                    BookingId = table.Column<int>(nullable: false),
                    ArrivalDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CAs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CAs_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Manifests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDateUtc = table.Column<DateTime>(nullable: true),
                    BookingId = table.Column<int>(nullable: false),
                    Seal = table.Column<string>(nullable: true),
                    Container = table.Column<string>(nullable: true),
                    Loading = table.Column<string>(nullable: true),
                    Bars = table.Column<int>(nullable: false),
                    Equipment = table.Column<string>(nullable: true),
                    Quantity = table.Column<decimal>(nullable: false),
                    Cartoons = table.Column<int>(nullable: false),
                    Cartons = table.Column<string>(nullable: true),
                    Cube = table.Column<decimal>(nullable: false),
                    KGS = table.Column<decimal>(nullable: false),
                    FreightTerms = table.Column<string>(nullable: true),
                    ChargeableKGS = table.Column<decimal>(nullable: false),
                    PackType = table.Column<string>(nullable: true),
                    NetKGS = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manifests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Manifests_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DCConfirmationDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDateUtc = table.Column<DateTime>(nullable: true),
                    DCConfirmationId = table.Column<int>(nullable: false),
                    Order = table.Column<string>(maxLength: 30, nullable: true),
                    Line = table.Column<string>(maxLength: 30, nullable: true),
                    Quantity = table.Column<decimal>(nullable: false),
                    Cartons = table.Column<int>(nullable: false),
                    Item = table.Column<string>(maxLength: 50, nullable: true),
                    Container = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DCConfirmationDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DCConfirmationDetails_DCConfirmations_DCConfirmationId",
                        column: x => x.DCConfirmationId,
                        principalTable: "DCConfirmations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDateUtc = table.Column<DateTime>(nullable: true),
                    ItemNumber = table.Column<string>(nullable: true),
                    Line = table.Column<string>(nullable: true),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    Warehouse = table.Column<string>(maxLength: 30, nullable: true),
                    Colour = table.Column<string>(maxLength: 30, nullable: true),
                    Size = table.Column<string>(maxLength: 30, nullable: true),
                    Item = table.Column<string>(nullable: true),
                    Quantity = table.Column<decimal>(nullable: false),
                    ReviseQuantity = table.Column<decimal>(nullable: false),
                    Cartons = table.Column<float>(nullable: false),
                    Cube = table.Column<float>(nullable: false),
                    KGS = table.Column<float>(nullable: false),
                    UnitPrice = table.Column<float>(nullable: false),
                    RetailPrice = table.Column<float>(nullable: false),
                    Tariff = table.Column<string>(nullable: true),
                    OrderId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProgressChecks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDateUtc = table.Column<DateTime>(nullable: true),
                    Complete = table.Column<bool>(nullable: false),
                    OnSchedule = table.Column<bool>(nullable: false),
                    IntendedShipDate = table.Column<DateTime>(nullable: false),
                    EstQtyToShip = table.Column<decimal>(nullable: false),
                    InspectionDate = table.Column<DateTime>(nullable: false),
                    Comments = table.Column<string>(nullable: true),
                    OrderId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgressChecks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProgressChecks_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDateUtc = table.Column<DateTime>(nullable: true),
                    Test = table.Column<string>(nullable: true),
                    PurchaseOrderId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderItems_PurchaseOrders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalTable: "PurchaseOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DCBookingDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDateUtc = table.Column<DateTime>(nullable: true),
                    Line = table.Column<string>(maxLength: 30, nullable: true),
                    Quantity = table.Column<decimal>(nullable: false),
                    Cartons = table.Column<int>(nullable: false),
                    Cube = table.Column<decimal>(nullable: false),
                    Item = table.Column<string>(maxLength: 50, nullable: true),
                    OrderId = table.Column<int>(nullable: false),
                    Container = table.Column<string>(maxLength: 20, nullable: true),
                    DCBookingId = table.Column<int>(nullable: false),
                    DCConfirmationDetailId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DCBookingDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DCBookingDetails_DCBookings_DCBookingId",
                        column: x => x.DCBookingId,
                        principalTable: "DCBookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DCBookingDetails_DCConfirmationDetails_DCConfirmationDetailId",
                        column: x => x.DCConfirmationDetailId,
                        principalTable: "DCConfirmationDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DCBookingDetails_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArriveOfDepacths_BookingId",
                table: "ArriveOfDepacths",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_CAs_BookingId",
                table: "CAs",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_DCBookingDetails_DCBookingId",
                table: "DCBookingDetails",
                column: "DCBookingId");

            migrationBuilder.CreateIndex(
                name: "IX_DCBookingDetails_DCConfirmationDetailId",
                table: "DCBookingDetails",
                column: "DCConfirmationDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_DCBookingDetails_OrderId",
                table: "DCBookingDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_DCConfirmationDetails_DCConfirmationId",
                table: "DCConfirmationDetails",
                column: "DCConfirmationId");

            migrationBuilder.CreateIndex(
                name: "IX_Manifests_BookingId",
                table: "Manifests",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgressChecks_OrderId",
                table: "ProgressChecks",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderItems_PurchaseOrderId",
                table: "PurchaseOrderItems",
                column: "PurchaseOrderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArriveOfDepacths");

            migrationBuilder.DropTable(
                name: "CAs");

            migrationBuilder.DropTable(
                name: "DCBookingDetails");

            migrationBuilder.DropTable(
                name: "Manifests");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "ProgressChecks");

            migrationBuilder.DropTable(
                name: "PurchaseOrderItems");

            migrationBuilder.DropTable(
                name: "DCBookings");

            migrationBuilder.DropTable(
                name: "DCConfirmationDetails");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "PurchaseOrders");

            migrationBuilder.DropTable(
                name: "DCConfirmations");
        }
    }
}
