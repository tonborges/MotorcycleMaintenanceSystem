using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "delivery_people",
                schema: "public",
                columns: table => new
                {
                    identifier = table.Column<string>(type: "varchar(255)", nullable: false),
                    name = table.Column<string>(type: "varchar(255)", nullable: false),
                    ein = table.Column<string>(type: "varchar(255)", nullable: false),
                    date_of_birth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    drivers_license_number = table.Column<string>(type: "varchar(255)", nullable: false),
                    drivers_license_type = table.Column<string>(type: "text", nullable: false),
                    drivers_license_image = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_delivery_people", x => x.identifier);
                });

            migrationBuilder.CreateTable(
                name: "motorcycles",
                schema: "public",
                columns: table => new
                {
                    identifier = table.Column<string>(type: "varchar(255)", nullable: false),
                    year = table.Column<int>(type: "integer", nullable: false),
                    model = table.Column<string>(type: "varchar(255)", nullable: false),
                    plate = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_motorcycles", x => x.identifier);
                });

            migrationBuilder.CreateTable(
                name: "stored_events",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    event_type = table.Column<string>(type: "varchar(255)", nullable: true),
                    data = table.Column<string>(type: "varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_stored_events", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rents",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    delivery_person_id = table.Column<string>(type: "varchar(255)", nullable: false),
                    motorcycle_id = table.Column<string>(type: "varchar(255)", nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    forecast_date_end = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    return_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    plan = table.Column<int>(type: "integer", nullable: false),
                    daily_price = table.Column<decimal>(type: "numeric", nullable: true),
                    fine_value = table.Column<decimal>(type: "numeric", nullable: true),
                    total_value = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_rents", x => x.id);
                    table.ForeignKey(
                        name: "fk_rents_delivery_people_delivery_person_id",
                        column: x => x.delivery_person_id,
                        principalSchema: "public",
                        principalTable: "delivery_people",
                        principalColumn: "identifier");
                    table.ForeignKey(
                        name: "fk_rents_motorcycles_motorcycle_id",
                        column: x => x.motorcycle_id,
                        principalSchema: "public",
                        principalTable: "motorcycles",
                        principalColumn: "identifier");
                });

            migrationBuilder.CreateIndex(
                name: "ix_delivery_people_drivers_license_number",
                schema: "public",
                table: "delivery_people",
                column: "drivers_license_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_delivery_people_ein",
                schema: "public",
                table: "delivery_people",
                column: "ein",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_motorcycles_plate",
                schema: "public",
                table: "motorcycles",
                column: "plate",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_rents_delivery_person_id",
                schema: "public",
                table: "rents",
                column: "delivery_person_id");

            migrationBuilder.CreateIndex(
                name: "ix_rents_motorcycle_id",
                schema: "public",
                table: "rents",
                column: "motorcycle_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "rents",
                schema: "public");

            migrationBuilder.DropTable(
                name: "stored_events",
                schema: "public");

            migrationBuilder.DropTable(
                name: "delivery_people",
                schema: "public");

            migrationBuilder.DropTable(
                name: "motorcycles",
                schema: "public");
        }
    }
}
