namespace PlaneteBleue

open System.Text.RegularExpressions
open FSharp.Data
open PlaneteBleueTypes

module Scraper =

    let GetEpisodeListInternal (loader: string -> HtmlDocument) =

        let extractEpisodeNum txt = 
            let result = Regex.Match(txt,"LPB\s(\d+)\s.*")
            if result.Success then 
                Success ({Id= (result.Groups.[1].Value|> int); Name = result.Groups.[0].Value})
            else 
                Error txt

        let results = loader("https://laplanetebleue.com/emissions")

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
   

    let GetPlaylistInternal (loader: string -> HtmlDocument) (episodeNbr: int) =

        // let episodeNbr = episode.Id

        let doc = loader("https://laplanetebleue.com/emission-"+ (episodeNbr |> string))
        let songs = 
            // each song is in its own table
            doc.CssSelect("table")
            |> List.map (fun n ->
                // the artist name and title are in <a> and <i> elements respectively
                // it is important to select them as a child of p, otherwise it will match other tables on the page 
                let a = n.CssSelect("p a") 
                let i = n.CssSelect("p i")
                (a, i)) |> List.filter (fun (a,i) -> 
                    // skip the tables where we didn't find either of those elements
                    not <| Seq.isEmpty a && not <| Seq.isEmpty i
                ) |> List.map (fun (a, i) -> 
                    let artist = (a |> Seq.head).InnerText()  
                    let title = (i |> Seq.head).InnerText()
                    { Title = title; Artist = artist}
                )
        {Tracks= songs;Name= episodeNbr}

    let loadDocument (url:string) =
         HtmlDocument.Load(url)

    let GetPlaylist = GetPlaylistInternal loadDocument 

    let GetEpisodeList = GetEpisodeListInternal loadDocument