using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KiotlogDBF.Migrations
{
    public partial class CreateUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateRole("kl_readers");
            
            migrationBuilder.CreateUser("kl_grafana", "KlGr4f4n4");
            migrationBuilder.CreateUser("kl_snreceiver", "KlSnR3c3iv3r");
            migrationBuilder.CreateUser("kl_httpreceiver", "KlR3c3iv3r");

            migrationBuilder.GrantRoleToUser("kl_readers", "kl_grafana");
            migrationBuilder.GrantRoleToUser("kl_readers", "kl_snreceiver");
            migrationBuilder.GrantRoleToUser("kl_readers", "kl_httpreceiver");

            migrationBuilder.CreateRole("kl_writers");

            migrationBuilder.CreateUser("kl_webapi", "KlW3b4p1");
            migrationBuilder.CreateUser("kl_decoder", "KlD3c0d3r");

            migrationBuilder.GrantRoleToUser("kl_writers", "kl_webapi");
            migrationBuilder.GrantRoleToUser("kl_writers", "kl_decoder");

            migrationBuilder.SetOwner("devices", "kl_writers");
            migrationBuilder.SetOwner("points", "kl_writers");
            migrationBuilder.SetOwner("sensors", "kl_writers");
            migrationBuilder.SetOwner("sensor_types", "kl_writers");
            migrationBuilder.SetOwner("conversions", "kl_writers");

            migrationBuilder.GrantSelect("devices", "kl_readers");
            migrationBuilder.GrantSelect("points", "kl_readers");
            migrationBuilder.GrantSelect("sensors", "kl_readers");
            migrationBuilder.GrantSelect("sensor_types", "kl_readers");
            migrationBuilder.GrantSelect("conversions", "kl_readers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
