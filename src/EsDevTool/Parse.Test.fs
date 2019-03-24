module ParseTest

open System
open Xunit
open FsUnit.Xunit
open Parse

[<Theory>]
[<InlineData("_search")>]
[<InlineData("/_search")>]
let ``EnsureStartsWithSlash works`` (input) =
    EnsureStartsWithSlash input |> should equal "/_search"

[<Theory>]
[<InlineData("GET _search", "GET")>]
[<InlineData(" POST /_search", "POST")>]
[<InlineData("GET _search", "GET")>]
[<InlineData("get _search", "GET")>]
[<InlineData("POST _search", "POST")>]
[<InlineData("post _search", "POST")>]
[<InlineData("delete _search", "DELETE")>]
[<InlineData("DELETE _search", "DELETE")>]
[<InlineData("put _search", "PUT")>]
[<InlineData("PUT _search", "PUT")>]
[<InlineData("       PUT jsdfhglkjshdfg", "PUT")>]
let ``GetHttpMethod gets the correct http method`` (input, expected) =
    GetHttpMethod input |> should equal (Some expected)

[<Theory>]
[<InlineData(" FET _search")>]
[<InlineData(" /_search")>]
[<InlineData("")>]
[<InlineData("   ")>]
[<InlineData("PUTTY")>]
let ``GetHttpMethod returns none when no http method is present`` (input) =
    GetHttpMethod input |> should equal None

[<Theory>]
[<InlineData("  GET _search   ", "_search")>]
[<InlineData(" POST /_search ", "/_search")>]
[<InlineData("GET /st-msg/messagesearchdto/_search?typed_keys=true", "/st-msg/messagesearchdto/_search?typed_keys=true")>]
let ``GetPathQuery gets the correct path/query`` (input, expected) =
    GetPathQuery input |> should equal (Some expected)

[<Theory>]
[<InlineData("POST/_search")>]
[<InlineData("")>]
let ``GetPathQuery returns None when no second word is present`` (input) =
    GetPathQuery input |> should equal None

[<Fact>]
let ``Partition a list`` () =
    let input = ["item1"; "item 2"; ""; " item3"; "          "; "item 4 "; "item5"]
    let expected = [["item1"; "item 2"]; [" item3"]; ["item 4 "; "item5"]]
    let actual =  partitionOn (String.IsNullOrWhiteSpace) input
    Assert.Equal<Collections.Generic.IEnumerable<string list>>(expected, actual)
