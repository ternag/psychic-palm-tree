[<RequireQualifiedAccess>]
module Result

  let isOk = function
    | Ok _ -> true
    | Error _ -> false

  let isError res = not (isOk res)
