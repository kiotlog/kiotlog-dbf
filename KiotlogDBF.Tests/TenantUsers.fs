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

module KiotlogDBF.Tests.TenantUsers

open Xunit

open KiotlogDBF.Tests.Config

open KiotlogDBF.Context
open KiotlogDBF.Models

[<Theory>]
[<InlineData("officina", "Magliano Alfieri", "Building Solutions", "telmo", "The Maker")>]
let ``Insert Related `` (tenant, address, notes, username, usernotes) =
    use ctx = new KiotlogDBFContext(dbContextOptions)

    let newTenantUser =
        TenantUsers (
            User = Users (
                Username = username,
                Meta = UserMeta (Notes = usernotes)),
            Tenant = Tenants (
                Tenant = tenant,
                Meta = TenantMeta (Address = address, Notes = notes))
        )

    ctx.TenantUsers.Add newTenantUser |> ignore
    ctx.SaveChanges () |> ignore

    let savedTenantUser = ctx.TenantUsers.Find newTenantUser.Id

    Assert.Equal(savedTenantUser.Tenant.Tenant, tenant)
    Assert.Equal(savedTenantUser.Tenant.Meta.Notes, notes)
    Assert.Equal(savedTenantUser.Tenant.Meta.Address, address)
    Assert.Equal(savedTenantUser.User.Username, username)
    Assert.Equal(savedTenantUser.User.Meta.Notes, usernotes)

    ctx.Users.Remove savedTenantUser.User |> ignore
    ctx.Tenants.Remove savedTenantUser.Tenant |> ignore
    ctx.TenantUsers.Remove savedTenantUser |> ignore
    ctx.SaveChanges () |> ignore
