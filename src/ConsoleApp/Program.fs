open System
open PlaneteBleue
open HotNewHipHop
open CommandLine

[<Verb("pb", HelpText = "Planete Bleue")>]
type PlaneteBleueOptions = {
    [<Option('e', "episode", HelpText = "Episode number")>]  episodeNbr : int
}

[<Verb("hnhh", HelpText = "Hot New Hip Hop")>]
type HotNewHipHopOptions = {
    [<Option('e', "episode", HelpText = "Episode number")>]  episodeNbr : int
}


let printReleases = 
    lazy (
        let releases = Scraper.GetEpisodeList()
        releases |> Seq.iter (fun r -> printfn "%s" r.Name )
    )

let printPlaylist e = 
     Scraper.GetPlaylist e |> (fun p -> 
        (printfn "   %d") p.Name 
        p.Tracks |> Seq.iter (fun t -> (printfn "    %s - %s") t.Artist t.Title)
        )

let printHotNewHipHopReleases = 
    printf "printing HNHH"
    lazy (
        let songs = HotNewHipHopScraper.GetNewestSongs()
        let nbrOfSongs = (Seq.length songs)
        for i in 1..nbrOfSongs  do
            let s = songs.[i - 1]
            printfn "%i %s - %s" i s.Artist s.Title
        //songs |> Seq.iter (fun s -> printfn "%s - %s" s.Artist s.Title)
    )

let planeteBleue (options:PlaneteBleueOptions) = 
    match options.episodeNbr with
        | 0 -> printReleases.Force()
        | e -> printPlaylist e

let hotNewHipHop (options:HotNewHipHopOptions) =
    match options.episodeNbr with
        | 0 -> printHotNewHipHopReleases.Force()
        | e -> printf ("not yet implemented")

[<EntryPoint>]
let main argv =
    let result = CommandLine.Parser.Default.ParseArguments<PlaneteBleueOptions, HotNewHipHopOptions>(argv)
    match result with 
        | :? CommandLine.Parsed<obj> as command ->
            match command.Value with 
                | :? PlaneteBleueOptions as opts -> planeteBleue opts
                | :? HotNewHipHopOptions as opts -> hotNewHipHop opts
                | _ -> ()
        | :? CommandLine.NotParsed<obj> -> printfn "No option specified"
        | _ -> ()                  
    0 // return an integer exit code
