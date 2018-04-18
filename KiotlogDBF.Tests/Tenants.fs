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

module KiotlogDBF.Tests.Tenants

open Xunit

open KiotlogDBF.Tests.Config

open KiotlogDBF.Context
open KiotlogDBF.Models

[<Theory>]
[<InlineData("toolbox", "ToolBox Cowoking", "Connecting People")>]
let ``Insert with Meta (JSONB) `` (tenant: string, address: string, notes: string) =
    use ctx = new KiotlogDBFContext(dbContextOptions)

    let newTenant =
        Tenants (
            Tenant = tenant,
            Meta = TenantMeta ( Address = address, Notes = notes)
        )

    ctx.Tenants.Add newTenant |> ignore
    ctx.SaveChanges () |> ignore

    let tenant = ctx.Tenants.Find newTenant.Id

    Assert.Equal(tenant.Meta.Notes, notes)
    Assert.Equal(tenant.Meta.Address, address)

    ctx.Tenants.Remove tenant |> ignore
    ctx.SaveChanges () |> ignore
