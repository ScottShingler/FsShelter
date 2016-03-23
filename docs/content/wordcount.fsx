(*** hide ***)
#I "../../build"
#r "FsShelter.dll"

open System

(**
Defining the schema
========================

FsShelter uses F# discriminated unions to statically type streams:

*)

// data schema for the topology, every case is a unqiue stream
type Schema = 
    | Sentence of string
    | Word of string
    | WordCount of string*int64

(**
Defining unreliable spouts
========================

FsShelter spouts can be implemented as "reliable" or "unreliable". 
Either implementation is a single function, returning async Option, where None indicates there's nothing to emit from this spout at this moment:

*)

// sentences spout - feeds messages into the topology
let sentences source = async { return source() |> Sentence |> Some }

(**
Defining bolts
========================

Example of a FsShelter bolt that reads a tuple and emits another one:

*)

// split bolt - consumes sentences and emits words
let splitIntoWords (input, emit) = 
    async { 
        match input with
        | Sentence s -> s.Split([|' '|],StringSplitOptions.RemoveEmptyEntries) 
                        |> Seq.map Word 
                        |> Seq.iter emit
        | _ -> failwithf "unexpected input: %A" input
    }

// count words bolt 
let countWords (input, increment, emit) = 
    async { 
        match input with
        | Word word -> 
            let! count = increment word
            WordCount (word,count) |> emit
        | _ -> failwithf "unexpected input: %A" input
    }

(**
And a terminating bolt that reads a tuple, but doesn't emit anything:

*)

// log word count - terminating bolt 
let logResult (log, input) = 
    async { 
        match input with
        | WordCount (word,count) -> log (sprintf "%s: %d" word count)
        | _ -> failwithf "unexpected input: %A" input
    }


(**
And given these helper methods:

*)
let source = 
    let rnd = new System.Random()
    let sentences = [ "the cow jumped over the moon"
                      "an apple a day keeps the doctor away"
                      "four score and seven years ago"
                      "snow white and the seven dwarfs"
                      "i am at two with nature" ]

    fun () -> sentences.[ rnd.Next(0, sentences.Length) ]

let increment =
    let mkCountAgent () = 
        let cache = Collections.Generic.Dictionary()
        MailboxProcessor.Start(fun inbox -> 
            async { 
                while true do
                    let! (rc:AsyncReplyChannel<int64>,word) = inbox.Receive()
                    match cache.TryGetValue(word) with
                    | (true,count) -> cache.[word] <- count + 1L
                                      count + 1L
                    | _ -> cache.[word] <- 1L
                           1L
                    |> rc.Reply
            })
    let agentsNumber = 10
    let countAgents = seq {for i in 1..agentsNumber -> mkCountAgent()} |> Seq.toArray
    fun word -> 
        let agent = countAgents.[abs (word.GetHashCode() % agentsNumber)]
        agent.PostAndAsyncReply(fun rc -> rc,word)

(**

We are ready to define the topology.


Using F# DSL to define the topology
========================

Topologies are defined using an embedded DSL:
*)

open FsShelter.DSL
#nowarn "25" // for stream grouping expressions

//define the storm topology
let sampleTopology = 
    topology "WordCount" { 
        let sentencesSpout = 
            sentences |> runSpout (fun log cfg -> source)        // make arguments: ignoring Storm logging and cfg, passing our source function
        
        let splitBolt = 
            splitIntoWords
            |> runBolt (fun log cfg tuple emit -> (tuple, emit)) // make arguments: pass incoming tuple and emit function
            |> withParallelism 2
        
        let countBolt = 
            countWords
            |> runBolt (fun log cfg tuple emit -> (tuple, increment, emit))
            |> withParallelism 2
        
        let logBolt = 
            logResult
            |> runBolt (fun log cfg ->                           // make arguments: pass PID-log and incoming tuple 
                            let mylog = Common.Logging.asyncLog (Diagnostics.Process.GetCurrentProcess().Id.ToString()+"_count")
                            fun tuple emit -> (mylog, tuple))
            |> withParallelism 2
        
        yield sentencesSpout --> splitBolt |> shuffle.on Sentence               // emit from sentencesSpout to splitBolt on Sentence stream, shuffle among target task instances
        yield splitBolt --> countBolt |> group.by (function Word w -> w)        // emit from splitBolt into countBolt on Word stream, group by word (into the same task instance)
        yield countBolt --> logBolt |> group.by (function WordCount (w,c) -> w) // emit from countBolt into logBolt on WordCount stream, group by word value
    }

(**
Here we define a graph by declaring the components and connecting them with arrows. 
The lambdas following the "run" methods privdes the opportunity to carry out construction of the arguments that will be passed into the component functions, where:
* log is the Storm log factory
* cfg is the runtime configuration passed in from Storm 
* tuple is the instance of the schema DU coming in
* emit is the function to emit another tuple

"log" and "cfg" are fixed once (curried) and as demonstrated in logBolt mkArgs lambda, one time-initialization can be carried out by inserting arbitrary code before "tuple" and "emit" arguments.
This initialization will not be triggered unless the task execution is actually requsted by Storm for this specific instance of the process.
*)


