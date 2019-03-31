module ParseTest

open System
open Xunit
open FsUnit.Xunit
open Elastic

[<Theory>]
[<InlineData("_search")>]
[<InlineData("/_search")>]
let ``EnsureStartsWithSlash works`` (input) =
    EnsureStartsWithSlash input |> should equal "/_search"

[<Theory>]
[<InlineData("GET _search", "GET")>]
[<InlineData(" POST /_search", "POST")>]
[<InlineData("get _search", "GET")>]
[<InlineData("POST _search", "POST")>]
[<InlineData("post _search", "POST")>]
[<InlineData("delete _search", "DELETE")>]
[<InlineData("DELETE _search", "DELETE")>]
[<InlineData("put _search", "PUT")>]
[<InlineData("PUT _search", "PUT")>]
[<InlineData("       PUT jsdfhglkjshdfg", "PUT")>]
let ``GetHttpMethod gets the correct http method`` (input, expected) =
    toHttpMethod input |> should equal (Some expected)

[<Theory>]
[<InlineData(" FET _search")>]
[<InlineData(" /_search")>]
[<InlineData("")>]
[<InlineData("   ")>]
[<InlineData("PUTTY")>]
let ``GetHttpMethod returns none when no http method is present`` (input) =
    toHttpMethod input |> should equal None

[<Theory>]
[<InlineData("  GET _search   ", "_search")>]
[<InlineData(" POST /_search ", "/_search")>]
[<InlineData("GET /st-msg/messagesearchdto/_search?typed_keys=true", "/st-msg/messagesearchdto/_search?typed_keys=true")>]
let ``GetPathQuery gets the correct path/query`` (input, expected) =
    toPathQuery input |> should equal (Some expected)

[<Theory>]
[<InlineData("POST/_search")>]
[<InlineData("")>]
let ``GetPathQuery returns None when no second word is present`` (input) =
    toPathQuery input |> should equal None

[<Fact>]
let ``Partition a list`` () =
    let input = ["item1"; "item 2"; ""; " item3"; "          "; "item 4 "; "item5"]
    let expected = [["item1"; "item 2"]; [" item3"]; ["item 4 "; "item5"]]
    let actual =  PartitionOn (String.IsNullOrWhiteSpace) input
    Assert.Equal<Collections.Generic.IEnumerable<string list>>(expected, actual)

[<Fact>]
let ``Partition an empty list yields an empty list`` () =
    let input = []
    let expected = []
    let actual =  PartitionOn (String.IsNullOrWhiteSpace) input
    Assert.Equal<Collections.Generic.IEnumerable<string list>>(expected, actual)

[<Fact>]
let ``Partition a list without delimiters yields a list with one list element`` () =
    let input = ["item1"; "item 2"; "item3";]
    let expected = [["item1"; "item 2"; "item3";]]
    let actual =  PartitionOn (String.IsNullOrWhiteSpace) input
    Assert.Equal<Collections.Generic.IEnumerable<string list>>(expected, actual)

[<Fact>]
let ``Partition a list with only delimiters yields an empty list`` () =
    let input = [null; ""; "\n"; "   \n"; "\t\n"; "    ";]
    let expected = []
    let actual =  PartitionOn (String.IsNullOrWhiteSpace) input
    Assert.Equal<Collections.Generic.IEnumerable<string list>>(expected, actual)

