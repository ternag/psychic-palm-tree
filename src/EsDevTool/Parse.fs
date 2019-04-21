module Elastic

open System
open FSharp.Data

type Method = GET|POST|PUT|DELETE
type Path = Path of string
type Body = Json of JsonValue|Empty

type ElasticRequest = {
  Method:Method
  Path:Path
  Body:Body
  }

let EnsureStartsWithSlash (str:string) = 
    match str with
    | str when str.StartsWith("/") -> str
    | _ -> "/" + str

let toHttpMethod (str:string) =
    let s = str.Trim().ToUpper()
    match s with
    | s when s.StartsWith("GET ") -> Ok(GET)
    | s when s.StartsWith("POST ") -> Ok(POST)
    | s when s.StartsWith("PUT ") -> Ok(PUT)
    | s when s.StartsWith("DELETE ") -> Ok(DELETE)
    | _ -> Error (sprintf "HTTP method not found in '%s'" str)

let toPathQuery (str:string) = 
    let words = str.Trim().Split(' ')
    match words.Length with
    | 2 -> Ok (Path (EnsureStartsWithSlash words.[1]))
    | _ -> Error (sprintf "Path not found in '%s'. Expected format is '<http-method> <path>?<query>'" str)

let PartitionOn pred sl= 

  let merger pred list obj =
    match list with
    | _ when pred obj -> []::list
    | [] -> [obj] :: list
    | head::rest -> (obj::head)::rest

  List.fold (merger pred) [] sl |> 
  List.filter (List.isEmpty >> not) |>
  List.map List.rev |> 
  List.rev  

let toBody str =
  match str with
  | str when String.IsNullOrWhiteSpace str -> Empty
  | str -> Json (JsonValue.Parse str)

let ParseSection sl =
  
  let validate head rest =
    let method = toHttpMethod head
    let path = toPathQuery head
    let section = toBody (rest |> List.fold (+) "")
    (method, path, section)

  match sl with
  | [] -> Error "Empty section"
  | head::rest -> 
    let tmp = validate head rest
    match tmp with
    | (Ok a, Ok b, Json c) -> Ok ({Method = a; Path = b; Body = Json c })
    | (Ok a, Ok b, _) -> Ok ({Method = a; Path = b; Body = Empty })
    | (Error a, _, _) -> Error a
    | (_, Error b, _) -> Error b


let Parse sl =
  PartitionOn (String.IsNullOrWhiteSpace) sl |> List.map ParseSection