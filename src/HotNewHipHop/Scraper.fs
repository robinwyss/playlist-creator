namespace HotNewHipHop

open System.Text.RegularExpressions
open FSharp.Data
open PlaylistTypes

module HotNewHipHopScraper =

    let private getInfo (node: HtmlNode) selector = 
        match node.CssSelect(selector) with
            | [x] -> Some(x.InnerText())
            | _   -> None

    let private getArtistName (node: HtmlNode) = getInfo node ".chartItem-artist-trackTitle"
    
    let private getTrackName (node: HtmlNode) = getInfo node ".chartItem-artist-trackTitle"

    let private extractTracks (node: HtmlNode)=
        node.CssSelect("li.chartItem") |> List.choose (fun li ->
            let title = getTrackName li 
            let artist = getArtistName li
            match (title, artist) with 
               | (Some(t), Some(a)) -> Some({Title = t; Artist = a })
               | _ -> None
            )

    let GetNewestSongsInternal (loader: string -> HtmlDocument) =
        let results = loader("https://www.hotnewhiphop.com/top100/")
        results.Elements() |> Seq.iter (fun e -> printfn "%s" (e.Name()) )
        match results.Descendants "body" with
            | s when Seq.isEmpty s -> failwith "no body tag found"
            | s-> extractTracks (Seq.head s)

    let loadDocument (url:string) =
        HtmlDocument.Load(url)

    let GetNewestSongs() = GetNewestSongsInternal loadDocument 
