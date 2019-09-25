open System
open PlaneteBleue
open CommandLine

[<Verb("pb", HelpText = "PlaneteBleue")>]
type PlaneteBleueOptions = {
    [<Option('e', "episode", HelpText = "Episode number")>]  episodeNbr : int
}

[<Verb("search", HelpText = "PlaneteBleue")>]
type SearchOptions = {
    [<Option('e', "episode", HelpText = "Episode number")>]  episodeNbr : int
}


let printReleases = 
    // lazy (
        let releases = Scraper.GetEpisodeList 
        releases |> Seq.iter (fun r -> printfn "%s" r.Name )
    // )

let printPlaylist e = 
     Scraper.GetPlaylist e |> (fun p -> 
        (printfn "   %d") p.Name 
        p.Tracks |> Seq.iter (fun t -> (printfn "    %s - %s") t.Artist t.Title)
        )

let run (options:PlaneteBleueOptions) = 
    match options.episodeNbr with
        | 0 -> printReleases
        | e -> printPlaylist e

[<EntryPoint>]
let main argv =
    let result = CommandLine.Parser.Default.ParseArguments<PlaneteBleueOptions, SearchOptions>(argv)
    match result with 
        | :? CommandLine.Parsed<obj> as command ->
            match command.Value with 
                | :? PlaneteBleueOptions as opts -> run opts
                // | :? Parsed<PlaneteBleueOptions> as parsed -> run parsed.Value
                // | :? NotParsed<PlaneteBleueOptions> as notParsed -> fail notParsed.Errors
                | _ -> ()
        | :? CommandLine.NotParsed<obj> -> printfn "No option specified"
        | _ -> ()                  


 


    
    // printfn "Hello World from F#!"
    0 // return an integer exit code
