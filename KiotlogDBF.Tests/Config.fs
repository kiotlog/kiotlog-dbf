(*
    Copyright (C) 2017 Giampaolo Mancini, Trampoline SRL.
    Copyright (C) 2017 Francesco Varano, Trampoline SRL.

    This file is part of Kiotlog.

    Kiotlog is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Kiotlog is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*)

module KiotlogDBF.Tests.Config

open Npgsql
open Microsoft.EntityFrameworkCore
open Microsoft.Extensions.Configuration

open KiotlogDBF.Context

let defaultConfig =
    Map [
        ("PgHost", "localhost")
        ("PgPort", "5432")
        ("PgUser", "postgres")
        ("PgPass", "postgres")
        ("PgDatabase", "kl_tests")
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
    csb.ApplicationName <- "KiotlogDBF.Tests"
    csb.ConnectionString


let dbContextOptions =
    DbContextOptionsBuilder<KiotlogDBFContext>()
        .UseNpgsql(connectionString)
        .Options
