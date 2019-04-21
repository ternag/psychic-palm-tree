namespace MyPSModule

open System.Management.Automation
open Elastic

[<Cmdlet("Get", "Requests")>]
type GetRequestsCommand () =
    inherit PSCmdlet ()
    [<Parameter>]
    member val Strings : string list = [] with get, set
    override x.EndProcessing () =
        let requests = Parse x.Strings
        x.WriteObject (requests)
        base.EndProcessing ()