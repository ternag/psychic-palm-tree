module SplitTest

open System
open Xunit
open FsUnit.Xunit
open CollectionSplitter.Split

[<Fact>]
let ``SplitInternal works`` () =
    let expected = [Value 'q'; Value 'w'; Delim 'e'; Value 'r'; Value 't']
    let actual = "qwert" |> Seq.toList |> splitInternal (fun c -> c='e')
    Assert.Equal<Collections.Generic.IEnumerable<Chunk<char>>>(expected,actual)

[<Fact>]
let ``SplitInternal works 2`` () =
    let expected = [Value 'q'; Delim 'e'; Delim 'e'; Delim 'e'; Value 't']
    let actual = "qeeet" |> Seq.toList |> splitInternal (fun c -> c='e')
    Assert.Equal<Collections.Generic.IEnumerable<Chunk<char>>>(expected,actual)

[<Fact>]
let ``Condense merges delimiters`` () =
    let expected = [Value 'q'; Delim 'e'; Value 't']
    let input = [Value 'q'; Delim 'e'; Delim 'e'; Delim 'e'; Value 't']
    let actual = doCondense Condense input
    Assert.Equal<Collections.Generic.IEnumerable<Chunk<char>>>(expected,actual)

[<Fact>]
let ``Condense handles empty list`` () =
    let expected = []
    let input = []
    let actual = doCondense Condense input
    Assert.Equal<Collections.Generic.IEnumerable<Chunk<char>>>(expected,actual)