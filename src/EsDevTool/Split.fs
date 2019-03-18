// https://hackage.haskell.org/package/split-0.1.1/docs/src/Data-List-Split-Internals.html

namespace CollectionSplitter

module Split =

    type DelimiterPredicate<'T> = 'T -> bool

    type Splitter<'T> = { delimiter:DelimiterPredicate<'T> }

    type Chunk<'T> = 
        | Delim of 'T
        | Value of 'T

    type SplitList<'T> = Chunk<'T> list

    let isDelim a =
        match a with
        | Delim a -> true
        | _ -> false

    let isValue a =
        match a with
        | Value a -> true
        | _ -> false

    let matchDelimiter predicate a =
        match a with
        | a when predicate a -> Delim a
        | _ -> Value a

    let splitInternal (f:DelimiterPredicate<'T>) a =
        List.map (matchDelimiter f) a

    //let qwert = "qwert" |> Seq.toList |> splitInternal (fun c -> (c='e'))
    //let qwert = "qeeet" |> Seq.toList |> splitInternal (fun c -> (c='e'))

    // split :: Splitter a -> [a] -> [[a]]
    // split s = map fromElem . postProcess s . splitInternal (delimiter s)

    let split splitter sequence =
        splitInternal splitter.delimiter sequence

    type CondensePolicy =
        | Condense
        | KeepDelimiters

    let doCondense policy ss = 

        let rec condense splitlist prevChunk =
            match splitlist with
            | x::xs when isValue x -> x::(condense xs x)
            | x::xs when isDelim x -> if(prevChunk=x) then (condense xs x) else x::(condense xs x)
            | _ -> []

        match policy with
        | KeepDelimiters -> ss
        | Condense -> 
            match ss with
            | [] -> []
            | x::xs -> condense ss x



    // -- > splitWhen (<0) [1,3,-4,5,7,-9,0,2] == [[1,3],[5,7],[0,2]]
    // splitWhen :: (a -> Bool) -> [a] -> [[a]]
    // splitWhen = split . dropDelims . whenElt

    // let splitWhen f =
    //     split << dropDelims << whenElt

    // -- | Given a split list in the internal tagged representation, produce
    // --   a new internal tagged representation corresponding to the final
    // --   output, according to the strategy defined by the given
    // --   'Splitter'.
    // postProcess :: Splitter a -> SplitList a -> SplitList a
    // postProcess s = dropFinal (finalBlankPolicy s)
    //               . dropInitial (initBlankPolicy s)
    //               . doMerge (delimPolicy s)
    //               . doDrop (delimPolicy s)
    //               . insertBlanks
    //               . doCondense (condensePolicy s)
    let postProcess splitter =
        splitter





