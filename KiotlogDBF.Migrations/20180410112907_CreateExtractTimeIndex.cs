using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KiotlogDBF.Migrations
{
    public partial class CreateExtractTimeIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string indexCreate = @"CREATE INDEX points_expr_idx ON points USING btree (date_part('epoch'::text, timezone('UTC'::text, ""time"")))";

            migrationBuilder.Sql(indexCreate);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string indexDrop = @"DROP INDEX points_expr_idx";

            migrationBuilder.Sql(indexDrop);

        }
    }
}
