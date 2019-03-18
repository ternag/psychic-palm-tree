module Parse
open System

type Method = GET|POST|PUT|DELETE
type Path = Path of string
type Body = string seq
type ElasticRequest = {Method:Method; Path:Path; Body:Body}

let EnsureStartsWithSlash (str:string) = 
    match str with
    | str when str.StartsWith("/") -> str
    | _ -> "/" + str

let GetHttpMethod (str:string) =
    let s = str.Trim().ToUpper()
    match s with
    | s when s.StartsWith("GET") -> Some("GET")
    | s when s.StartsWith("POST") -> Some("POST")
    | s when s.StartsWith("PUT") -> Some("PUT")
    | s when s.StartsWith("DELETE") -> Some("DELETE")
    | _ -> None

let GetPathQuery (str:string) = 
    let words = str.Trim().Split(' ')
    match words.Length with
    | 2 -> Some(words.[1])
    | _ -> None

let PartitionWhile f =
    let rec loop acc = function
      | x::xs when f x -> loop (x::acc) xs
      | xs -> List.rev acc, xs
    loop [] 


let a = ["item1"; "item 2"; ""; " item3"; "          "; "item 4 "; "item5"] |> seq
