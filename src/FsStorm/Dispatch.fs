﻿/// Dispatch loops for spouts and bolts
module Storm.Dispatch

open System
open Storm.Multilang

let private log out' level msg = Log(msg, level) |> out'

/// Dispatch spout commands and handle retries
let reliableSpoutLoop mkArgs mkAcker next getStream (in', out') conf = 
    async { 
        let args = mkArgs (log out') conf
        let ack, nack = mkAcker args
        while true do
            let! msg = in'()
            do! async { 
                    match msg with
                    | Next -> 
                        let! v = next args
                        match v with
                        | Some(tid, tuple) -> Emit(tuple, Some tid, [], (getStream tuple), None) |> out'
                        | _ -> ()
                    | Ack tid -> ack tid
                    | Nack tid -> nack tid
                    | _ -> failwithf "Unexpected command: %A" msg
                    Sync |> out'
                }
    }

/// Dispatch commands for spouts that don't provide unique ids to emitted tuples
let unreliableSpoutLoop mkArgs next getStream (in', out') conf = 
    async { 
        let args = mkArgs (log out') conf
        while true do
            let! msg = in'()
            do! async { 
                    match msg with
                    | Next -> 
                        let! v = next args
                        match v with
                        | Some(tuple) -> Emit(tuple, None, [], (getStream tuple), None) |> out'
                        | _ -> ()
                    | Ack _
                    | Nack _ -> ()
                    | _ -> failwithf "Unexpected command: %A" msg
                    Sync |> out'
                }
    }

/// Dispatch bolt commands and auto acks/nack handled messages
let autoAckBoltLoop mkArgs consume getAnchors getStream (in', out') conf = 
    async { 
        let args = mkArgs (log out') conf
        while true do
            let! msg = in'()
            match msg with
            | Heartbeat -> Sync |> out'
            | Tuple(tuple, id, src, stream, task) -> 
                let emit t = Emit(t, None, getAnchors stream id, (getStream t), None) |> out'
                let! res = consume (args tuple emit) |> Async.Catch
                match res with
                | Choice1Of2 _ -> Ok id
                | Choice2Of2 ex -> 
                    Fail id |> out'
                    Error("autoBoltRunner: ", ex)
                |> out'
            | _ -> failwithf "Unexpected command: %A" msg
    }