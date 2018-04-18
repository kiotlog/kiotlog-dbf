using Microsoft.EntityFrameworkCore.Migrations;

namespace KiotlogDBF.Migrations
{
    public partial class CreateSensorTypes : Migration
    {
        private readonly string[] _columnNames = {"name", "meta", "type"};

        private readonly string[,] _initSensorTypes = {
            {"Generic", @"{}", "generic"},
            
            {"Generic_Temperature", @"{}", "temperature"},
            {"ATA8520_Temperature", @"{""max"": 60, ""min"": -60}", "temperature"},

            {"DHT11_Temperature", @"{""max"": 50, ""min"": 0}", "temperature"},
            {"DHT11_Humidity", @"{""max"": 80, ""min"": 20}", "humidity"},
            
            {"DHT22_Humidity", @"{""max"": 100, ""min"": 0}", "humidity"},
            {"DHT22_Temperature", @"{""max"": 125, ""min"": -40}", "temperature"},
            
            {"BME280_Temperature", @"{""max"": 85, ""min"": -40}", "temperature"},
            {"BME280_Humidity", @"{""max"": 100, ""min"": 0}", "humidity"},
            {"BME280_Pressure", @"{""max"": 1100, ""min"": 300}", "pressure"},
            
            {"Generic_milliVolts", @"{}", "voltage"},
            {"MKRFOX_Battery", @"{""max"": 5000, ""min"": 0}", "voltage"},
        };
        
        protected override void Up(MigrationBuilder migrationBuilder) =>
            migrationBuilder.InsertData("sensor_types", _columnNames, _initSensorTypes);

        protected override void Down(MigrationBuilder migrationBuilder) =>
            migrationBuilder.DeleteData("sensor_types", _columnNames, _initSensorTypes);
    }
}
