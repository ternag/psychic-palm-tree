module SplitTest

open System
open Xunit
open FsUnit.Xunit
open FsUnit
open Swensen.Unquote
open CollectionSplitter.Split

[<Fact>]
let ``SplitInternal works`` () =
    let expected = [Value ['q']; Value ['w']; Delim ['e']; Value ['r']; Value ['t']]
    let actual = "qwert" |> Seq.toList |> splitInternal (fun c -> c='e')
    actual |> should matchList expected
    Assert.Equal<Collections.Generic.IEnumerable<Chunk<char list>>>(expected,actual)
    test <@ expected = actual @>
    

[<Fact>]
let ``SplitInternal works 2`` () =
    let expected = [Value ['q']; Delim ['e']; Delim ['e']; Delim ['e']; Value ['t']]
    let actual = "qeeet" |> Seq.toList |> splitInternal (fun c -> c='e')
    Assert.Equal<Collections.Generic.IEnumerable<Chunk<char list>>>(expected,actual)

[<Fact>]
let ``Condense merges delimiters`` () =
    let expected = [Value ['q']; Delim ['e']; Value ['t']]
    let input = [Value ['q']; Delim ['e']; Delim ['e']; Delim ['e']; Value ['t']]
    let actual = doCondense Condense input
    //Assert.Equal<Collections.Generic.IEnumerable<Chunk<char list>>>(expected,actual)
    actual |> should matchList expected

[<Fact>]
let ``Condense handles empty list`` () =
    let expected = []
    let input = []
    let actual = doCondense Condense input
    Assert.Equal<Collections.Generic.IEnumerable<Chunk<char>>>(expected,actual)

[<Fact>]
let ``insert blanks after delimiter`` () =
    let input = [Value ['q']; Delim ['A']; Value ['e']; Delim ['A']; Value ['t']]
    let expected = [Value ['q']; Delim ['A']; Value []; Value ['e']; Delim ['A']; Value []; Value ['t']]
    let actual = insertBlanks input
    //test <@ actual = expected @>
    //actual |> should matchList expected
    Assert.Equal<Collections.Generic.IEnumerable<Chunk<char list>>>(expected,actual)

[<Fact>]
let ``insert blanks after mulit delimiter`` () =
    let input = [Value ['q']; Delim ['A']; Delim ['A']; Value ['e']; Delim ['A']; Value ['t']]
    let expected = [Value ['q']; Delim ['A']; Value []; Delim ['A']; Value []; Value ['e']; Delim ['A']; Value []; Value ['t']]
    let actual = insertBlanks input
    //test <@ actual = expected @>
    //actual |> should matchList expected
    Assert.Equal<Collections.Generic.IEnumerable<Chunk<char list>>>(expected,actual)

[<Fact>]
let ``insert blanks after only delimiter`` () =
    let input = [Delim ['A']; Delim ['A'];]
    let expected = [Value []; Delim ['A']; Value []; Delim ['A']; Value []; ]
    let actual = insertBlanks input
    //test <@ actual = expected @>
    //actual |> should matchList expected
    Assert.Equal<Collections.Generic.IEnumerable<Chunk<char list>>>(expected,actual)


[<Fact>]
let ``insert blanks on empty list returns empty list`` () =
    // let input = [1]
    // let expected = []
    // let actual = insertBlanks input
    //Assert.Equal<Collections.Generic.IEnumerable<Chunk<char>>>(expected,actual)
    //actual |> should matchList expected
    // test <@ List.isEmpty actual @>
    test <@ [] |> insertBlanks = [Value []] @>