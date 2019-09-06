namespace PlaneteBleue

open System.Text.RegularExpressions
open FSharp.Data
open PlaneteBleueTypes

module Scraper =

    let GetEpisodeList =

        let extractEpisodeNum txt = 
            let result = Regex.Match(txt,"LPB\s(\d+)\s.*")
            if result.Success then 
                Success ({Id= (result.Groups.[1].Value|> int); Name = result.Groups.[0].Value})
            else 
                Error txt

        let results = HtmlDocument.Load("https://laplanetebleue.com/emissions")

        // find the dropdown (select) with id="emission"
        let select = 
            results.Descendants ["select"]
            |> Seq.find (fun x -> x.AttributeValue("id") = "emission")
     
        // extract the episodes from the options element
        select.Descendants ["option"]
            |> Seq.map (fun x -> extractEpisodeNum (x.InnerText()))
            |> Seq.choose (fun x-> 
                match x with
                    | Success e -> Some e
                    |_ -> None)

    let GetPlaylist (episodeNbr: int) =

        // let episodeNbr = episode.Id

        let doc = HtmlDocument.Load("https://laplanetebleue.com/emission-"+ (episodeNbr |> string))
        let songs = 
            doc.CssSelect("table")
            |> List.map (fun n ->
                let a = n.CssSelect("p a") 
                let t = n.CssSelect("p i")
                (a, t)) |> List.filter (fun (a,t) -> 
                    not <| Seq.isEmpty a && not <| Seq.isEmpty t
                ) |> List.map (fun (a, t) -> 
                    let artist = (a |> Seq.head).InnerText()  
                    let title = (t |> Seq.head).InnerText()
                    { Title = title; Artist = artist}
                )
        {Tracks= songs;Name= episodeNbr}