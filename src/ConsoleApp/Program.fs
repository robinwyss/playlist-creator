open System
open PlaneteBleue
open CommandLine

// [<Verb("pb", HelpText = "PlaneteBleue")>]
type PlaneteBleueOptions = {
    [<Option('e', "episode", HelpText = "Episode number")>]  episodeNbr : int
}

let printReleases = 
    lazy (
        let releases = Scraper.GetEpisodeList 
        releases |> Seq.iter (fun r -> printfn "%s" r.Name )
    )

let printPlaylist e = 
     Scraper.GetPlaylist e |> (fun p -> 
        (printfn "   %d") p.Name 
        p.Tracks |> Seq.iter (fun t -> (printfn "    %s - %s") t.Artist t.Title)
        )

let run (options:PlaneteBleueOptions) = 
    match options.episodeNbr with
        | 0 -> () // printReleases
        | e -> printPlaylist e

let fail (errors:System.Collections.Generic.IEnumerable<Error>) = 
    printfn "Couldn't parse options"
    // errors |> Seq.iter (fun e -> (printfn "%s") e.Tag.GetTypeCode(). )

[<EntryPoint>]
let main argv =
    let result = CommandLine.Parser.Default.ParseArguments<PlaneteBleueOptions>(argv)
    match result with 
        | :? Parsed<PlaneteBleueOptions> as parsed -> run parsed.Value
        | :? NotParsed<PlaneteBleueOptions> as notParsed -> fail notParsed.Errors
        | _ -> printfn "No option specified"


 


    
    // printfn "Hello World from F#!"
    0 // return an integer exit code
