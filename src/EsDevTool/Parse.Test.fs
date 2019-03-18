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
[<InlineData("GET", "GET")>]
[<InlineData("get", "GET")>]
[<InlineData("POST", "POST")>]
[<InlineData("post", "POST")>]
[<InlineData("delete", "DELETE")>]
[<InlineData("DELETE", "DELETE")>]
[<InlineData("put", "PUT")>]
[<InlineData("PUT", "PUT")>]
[<InlineData("       PUT jsdfhglkjshdfg", "PUT")>]
let ``GetHttpMethod gets the correct http method`` (input, expected) =
    GetHttpMethod input |> should equal (Some expected)

[<Theory>]
[<InlineData(" FET _search")>]
[<InlineData(" /_search")>]
[<InlineData("")>]
[<InlineData("   ")>]
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
let ``Partitions a seq`` =
    ["item1"; "item 2"; ""; " item3"; "          "; "item 4 "; "item5"]
    |> PartitionWhile (String.IsNullOrWhiteSpace >> not) |> should matchList [["item1"; "item 2"]; ["item3"]; ["item 4 "; "item5"]]

// [<Fact>]
// let ``Partitions a seq 2`` =
//     let expected = [["item1"; "item 2"], ["item3"], ["item 4 "; "item5"]]
//     let actual = ["item1"; "item 2"; ""; " item3"; "          "; "item 4 "; "item5"] |> PartitionWhile (String.IsNullOrWhiteSpace >> not)
//     Assert.Equal<Collections.Generic.IEnumerable<string list>>(expected, actual)