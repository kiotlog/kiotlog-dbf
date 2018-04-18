using Microsoft.EntityFrameworkCore.Migrations;

namespace KiotlogDBF.Migrations
{
    public partial class CreateConversions : Migration
    {
        private readonly object[] _initialConversions = { "id", "x10", "x100", "float_to_int16", "float_to_uint16" };
        protected override void Up(MigrationBuilder migrationBuilder) =>
            migrationBuilder.InsertData("conversions", "fun", _initialConversions);

        protected override void Down(MigrationBuilder migrationBuilder) =>
            migrationBuilder.DeleteData("conversions", "fun", _initialConversions);
    }
}
