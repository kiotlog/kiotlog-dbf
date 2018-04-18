module AppDB.Queries

open System
open System.Linq
open Microsoft.EntityFrameworkCore
open Microsoft.Extensions.Configuration

open Npgsql
open Npgsql.EntityFrameworkCore.PostgreSQL
open Newtonsoft.Json

open FSharp.Control.Tasks

open KiotlogDBF.Context
open KiotlogDBF.Models

open KiotlogDBF.Json
open System.Collections.Generic

let runQueries (argv : string []) =
    let rnd = Random()

    let defaultConfig =
        Map [
            ("PgHost", "localhost")
            ("PgPort", "7433")
            ("PgUser", "kl_webapi")
            ("PgPass", "KlW3b4p1")
            ("PgDatabase", "trmpln")
            ("Device", "fake-efcs-" + string (rnd.Next(100)))
        ]

    let config =
        ConfigurationBuilder()
            .AddInMemoryCollection(defaultConfig)
            .AddEnvironmentVariables("KL_")
            .AddCommandLine(argv)
            .Build()

    let connectionString =
        let csb = NpgsqlConnectionStringBuilder()
        csb.Host <- config.["PgHost"]
        csb.Port <- int config.["PgPort"]
        csb.Username <- config.["PgUser"]
        csb.Password <- config.["PgPass"]
        csb.Database <- config.["PgDatabase"]
        csb.ApplicationName <- "EFCoreFSharp"
        csb.ConnectionString

    printfn "Connecting to: %s" connectionString

    let optionsBuilder = DbContextOptionsBuilder<KiotlogDBFContext>()
    let optionsAction =
        fun (options : Infrastructure.NpgsqlDbContextOptionsBuilder) ->
            options.MigrationsAssembly("KiotlogDBF.Migrations") |> ignore

    optionsBuilder.UseNpgsql(connectionString, optionsAction) |> ignore


    use ctx = new KiotlogDBFContext(optionsBuilder.Options)
    let devices = ctx.Devices

    printfn "Loading Devices:"
    devices |> Seq.iter (fun d -> printfn "%O" d)
// (*
    let devices =
        ctx.Devices
            .Include(fun d -> d.Sensors :> IEnumerable<_>)
                .ThenInclude(fun (s : Sensors) -> s.SensorType)
            .Include(fun d -> d.Sensors :> IEnumerable<_>)
                .ThenInclude(fun (s : Sensors) -> s.Conversion)

    printfn "Loading Devices With SensorType and Conversion:"
    devices |> Seq.iter (fun d -> printfn "%O" d)
// *)

// (*
    printfn "Adding Fake Device:"
    let fakeDevice =
        Devices (
            Device = config.["Device"],
            Meta = DevicesMeta ( Name = "Fake Device", Description = "", Kind = "Test"),
            Frame = DevicesFrame ( Bigendian = false, Bitfields = false),
            Sensors = HashSet [|
                Sensors (
                    Fmt = SensorsFmt ( FmtChr = "H", Index = 0 ),
                    Meta = SensorsMeta ( Name = "Fake Sensor " + string (rnd.Next(100)), Description = String.Empty ),
                    SensorType = SensorTypes (
                        Name = "Fake Type " + string (rnd.Next(100)),
                        Type = "Speed",
                        Meta = SensorTypesMeta ( Max = rnd.Next(0, 100), Min = rnd.Next(-250, 0))),
                    ConversionId = Guid("51eff0b3-4d3d-4495-ab05-edf072441723"))
                Sensors(
                    Fmt = SensorsFmt ( FmtChr = "B", Index = 1 ),
                    Meta = SensorsMeta ( Name = "Fake Sensor " + string (rnd.Next(100)), Description = String.Empty ),
                    SensorTypeId = Guid("4da4b178-c4de-4887-b6c8-7ef6f6e7c8dc"),
                    Conversion = Conversions(
                        Fun = "uint16_to_float_efc"))
            |]
        )

    printfn "%A" fakeDevice

    try
        try
            use ctx = new KiotlogDBFContext(optionsBuilder.Options)
            ctx.Devices.Add fakeDevice |> ignore
            ctx.SaveChanges() |> ignore
        with
            | ex  -> printfn "Unable to add Fake Device: %A: %A\n %A" ex.Message ex.InnerException.Message fakeDevice
    finally
        printfn "%A" fakeDevice
// *)
(*
    let oneDevice = ctx.Set<Devices>().Find fakeDevice.Id
    ctx.Entry(oneDevice).Collection("Sensors").Load()
    printfn "Loading Device: %A" fakeDevice.Id
    printfn "%O" oneDevice
*)
// (*
    let jsonDevice = """
        {
          "device": "mkr0-fona",
          "meta": {
              "name": "mkr0-jona",
              "description": "json test",
              "kind": "test"
          },
          "auth": {
            "basic": {
              "token": "fb2e2658-c6cd-498d-91e6-37ad92bbe89b"
            }
          },
          "frame": {
              "bitfields": false,
              "bigendian": true
          },
          "sensors": [
            {
              "meta": {
                "name": "temperture"
              },
              "fmt": {
                "index": 0,
                "fmtchr": "h"
              },
              "sensortypeid": "85b9a2e6-d944-4ca6-89ed-90c9771aad5c",
              "conversionid": "d67b67e9-45a1-4155-bffc-ef35c3df7242"
            }
          ]
        }
        """

    let newDevice = JsonConvert.DeserializeObject<Devices> (jsonDevice, camelSettings)
    printfn "Adding Json Device: %A" newDevice
    try
        try
            use ctx = new KiotlogDBFContext(optionsBuilder.Options)
            ctx.Devices.Add newDevice |> ignore
            ctx.SaveChanges() |> ignore
        with
            | ex  -> printfn "Unable to add Json Device %A: %A: %A" ex.Message ex.InnerException.Message newDevice
    finally
        printfn "%A" newDevice
// *)
(*
    let devId = newDevice.Id

    let oneDevice =
        ctx.Devices
            .Where(fun d -> d.Id = devId)
            .Include(fun d -> d.Sensors :> IEnumerable<_>)
                .ThenInclude(fun (s : Sensors) -> s.SensorType)
            .Include(fun d -> d.Sensors :> IEnumerable<_>)
                .ThenInclude(fun (s : Sensors) -> s.Conversion)
            .FirstOrDefault()

    printfn "Loading Device: %A" devId
    printfn "%O" oneDevice

    let t =
        let devices =
            ctx.Devices
                .Include(fun d -> d.Sensors :> IEnumerable<_>)
                    .ThenInclude(fun (s : Sensors) -> s.SensorType)
                .Include(fun d -> d.Sensors :> IEnumerable<_>)
                    .ThenInclude(fun (s : Sensors) -> s.Conversion)

        let oneDeviceQ =
            query {
                for d in devices do
                where (d.Id = devId)
                select d
            }
        task {
            printfn "Loading Device: %A" devId
            let! oneDevice = oneDeviceQ.SingleOrDefaultAsync ()
            return oneDevice
        }

    t.Wait()
    if t.IsCompleted then
        match t.Result with
            | null -> printfn "%O not found" devId
            | found -> printfn "%O" found
*)
