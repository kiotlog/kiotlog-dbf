namespace KiotlogDBF.Context

open Microsoft.EntityFrameworkCore
open Microsoft.EntityFrameworkCore.Design
open Microsoft.Extensions.Configuration

open Npgsql

type KiotlogDBFContextFactory () =
    interface IDesignTimeDbContextFactory<KiotlogDBFContext> with
        member __.CreateDbContext(_args) =

            let defaultConfig =
                Map [
                    ("PgHost", "localhost")
                    ("PgPort", "5432")
                    ("PgUser", "postgres")
                    ("PgPass", "")
                    ("PgDatabase", "efcore")
                ]

            let config =
                ConfigurationBuilder()
                    .AddInMemoryCollection(defaultConfig)
                    .AddEnvironmentVariables("KL_")
                    .Build()

            let connectionString =
                let csb = NpgsqlConnectionStringBuilder()
                csb.Host <- config.["PgHost"]
                csb.Port <- int config.["PgPort"]
                csb.Username <- config.["PgUser"]
                csb.Password <- config.["PgPass"]
                csb.Database <- config.["PgDatabase"]
                csb.ApplicationName <- "KiotlogDBF Migration Tool"
                csb.ConnectionString

            let options =
                DbContextOptionsBuilder<KiotlogDBFContext>()
                    .UseNpgsql(connectionString,
                        fun opt -> opt.MigrationsAssembly("KiotlogDBF.Migrations") |> ignore)
                    .Options

            new KiotlogDBFContext(options)
