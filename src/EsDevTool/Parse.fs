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


let partitionOn pred sl= 
  let merger pred list obj =
    match list with
    | [] -> [obj] :: list
    | _ when pred obj -> []::list
    | head::rest -> (obj::head)::rest

  List.fold (merger pred) [] sl |> 
  List.map List.rev |> 
  List.rev  