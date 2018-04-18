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

module KiotlogDBF.Tests.Users

open System
open Xunit

open KiotlogDBF.Tests.Config

open KiotlogDBF.Context
open KiotlogDBF.Models

[<Theory>]
[<InlineData("foobar", "Foo Bar")>]
let ``Insert with Meta (JSONB) `` (username: string, notes: string) =
    use ctx = new KiotlogDBFContext(dbContextOptions)

    let newUser =
        Users (
            Username = username,
            Meta = UserMeta ( Notes = notes)
        )

    ctx.Users.Add newUser |> ignore
    ctx.SaveChanges () |> ignore

    let user = ctx.Users.Find newUser.Id

    Assert.Equal(user.Meta.Notes, notes)

    ctx.Users.Remove user |> ignore
    ctx.SaveChanges () |> ignore


[<Theory>]
[<InlineData("foo", "notes")>]
let ``Create and Destroy `` (username: string, notes: string) =
    use ctx = new KiotlogDBFContext(dbContextOptions)

    let newUser =
        Users (
            Username = username,
            Meta = UserMeta (Notes = notes)
        )

    ctx.Users.Add newUser |> ignore
    ctx.SaveChanges () |> ignore

    Assert.Equal(newUser.Username, username)
    Assert.Equal(newUser.Meta.Notes, notes)

    ctx.Users.Remove newUser |> ignore
    ctx.SaveChanges() |> ignore

    let user = ctx.Users.Find newUser.Id

    Assert.Equal(user, null)


[<Theory>]
[<InlineData("bar")>]
let ``Insert with Empty Meta `` (value: string) =
    use ctx = new KiotlogDBFContext(dbContextOptions)

    let newUser =
        Users (
            Username = value
        )

    ctx.Users.Add newUser |> ignore
    ctx.SaveChanges () |> ignore

    Assert.Equal(String.Empty, newUser.Meta.Notes)

    ctx.Users.Remove newUser |> ignore
    ctx.SaveChanges() |> ignore





