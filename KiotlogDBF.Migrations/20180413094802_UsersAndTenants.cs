using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KiotlogDBF.Migrations
{
    public partial class UsersAndTenants : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tenants",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false, defaultValueSql: "gen_random_uuid()"),
                    tenant = table.Column<string>(nullable: false),
                    meta = table.Column<string>(type: "jsonb", nullable: true, defaultValueSql: "'{}'::jsonb")
                },
                constraints: table =>
                {
                    table.PrimaryKey("tenants_pkey", x => x.id);
                    table.UniqueConstraint("AK_tenants_tenant", x => x.tenant);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false, defaultValueSql: "gen_random_uuid()"),
                    username = table.Column<string>(nullable: false),
                    meta = table.Column<string>(type: "jsonb", nullable: true, defaultValueSql: "'{}'::jsonb"),
                    auth = table.Column<string>(type: "jsonb", nullable: true, defaultValueSql: "json_build_object('passwd', encode(gen_random_bytes(32), 'base64'))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("users_pkey", x => x.id);
                    table.UniqueConstraint("AK_users_username", x => x.username);
                });

            migrationBuilder.CreateTable(
                name: "tenant_users",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false, defaultValueSql: "gen_random_uuid()"),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("tenant_users_pkey", x => x.id);
                    table.ForeignKey(
                        name: "tenant_users_tenant_id_fkey",
                        column: x => x.tenant_id,
                        principalTable: "tenants",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "tenant_users_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tenant_users_tenant_id",
                table: "tenant_users",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_tenant_users_user_id",
                table: "tenant_users",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tenant_users");

            migrationBuilder.DropTable(
                name: "tenants");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
