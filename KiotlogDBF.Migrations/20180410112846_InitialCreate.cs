using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KiotlogDBF.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:pgcrypto", ",,");

            migrationBuilder.CreateTable(
                name: "conversions",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false, defaultValueSql: "gen_random_uuid()"),
                    fun = table.Column<string>(nullable: false, defaultValueSql: "'id'::text")
                },
                constraints: table =>
                {
                    table.PrimaryKey("convertions_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "devices",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false, defaultValueSql: "gen_random_uuid()"),
                    device = table.Column<string>(nullable: false, defaultValueSql: "'device'"),
                    meta = table.Column<string>(type: "jsonb", nullable: false, defaultValueSql: "'{}'::jsonb"),
                    auth = table.Column<string>(type: "jsonb", nullable: false, defaultValueSql: "json_build_object('klsn', json_build_object('key', encode(gen_random_bytes(32), 'base64')), 'basic', json_build_object('token', encode(gen_random_bytes(32), 'base64')))"),
                    frame = table.Column<string>(type: "jsonb", nullable: false, defaultValueSql: " '{\"bigendian\": true, \"bitfields\": false}'::jsonb ")
                },
                constraints: table =>
                {
                    table.PrimaryKey("devices_pkey", x => x.id);
                    table.UniqueConstraint("devices_device_key", x => x.device);
                });

            migrationBuilder.CreateTable(
                name: "sensor_types",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(nullable: false, defaultValueSql: "'generic'::text"),
                    type = table.Column<string>(nullable: false, defaultValueSql: "'generic'::text"),
                    meta = table.Column<string>(type: "jsonb", nullable: true, defaultValueSql: "'{}'::jsonb")
                },
                constraints: table =>
                {
                    table.PrimaryKey("sensor_types_pkey", x => x.id);
                    table.UniqueConstraint("sensor_types_name_key", x => x.name);
                });

            migrationBuilder.CreateTable(
                name: "points",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false, defaultValueSql: "gen_random_uuid()"),
                    device_id = table.Column<Guid>(nullable: false),
                    time = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    flags = table.Column<string>(type: "jsonb", nullable: false, defaultValueSql: "'{}'::jsonb"),
                    data = table.Column<string>(type: "jsonb", nullable: false, defaultValueSql: "'{}'::jsonb")
                },
                constraints: table =>
                {
                    table.PrimaryKey("points_pkey", x => x.id);
                    table.ForeignKey(
                        name: "points_device_fkey",
                        column: x => x.device_id,
                        principalTable: "devices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "sensors",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false, defaultValueSql: "gen_random_uuid()"),
                    device_id = table.Column<Guid>(type: "uuid", nullable: false),
                    sensor_type_id = table.Column<Guid>(nullable: false),
                    conversion_id = table.Column<Guid>(nullable: false),
                    meta = table.Column<string>(type: "jsonb", nullable: true, defaultValueSql: "'{}'::jsonb"),
                    fmt = table.Column<string>(type: "jsonb", nullable: true, defaultValueSql: "'{}'::jsonb")
                },
                constraints: table =>
                {
                    table.PrimaryKey("sensors_pkey", x => x.id);
                    table.ForeignKey(
                        name: "sensors_conversion_fkey",
                        column: x => x.conversion_id,
                        principalTable: "conversions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "sensors_device_id_fkey",
                        column: x => x.device_id,
                        principalTable: "devices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "sensors_sensor_type_fkey",
                        column: x => x.sensor_type_id,
                        principalTable: "sensor_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_points_device_id",
                table: "points",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "IX_sensors_conversion_id",
                table: "sensors",
                column: "conversion_id");

            migrationBuilder.CreateIndex(
                name: "IX_sensors_device_id",
                table: "sensors",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "IX_sensors_sensor_type_id",
                table: "sensors",
                column: "sensor_type_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "points");

            migrationBuilder.DropTable(
                name: "sensors");

            migrationBuilder.DropTable(
                name: "conversions");

            migrationBuilder.DropTable(
                name: "devices");

            migrationBuilder.DropTable(
                name: "sensor_types");
        }
    }
}
