namespace WebApp

module HttpHandlers =

    open Microsoft.AspNetCore.Http
    open FSharp.Control.Tasks.V2.ContextInsensitive
    open Giraffe
    open WebApp.Models
    open PlaneteBleue

    let handleGetPing =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            task {
                let response = {
                    Text = "Pong!"
                }
                return! json response next ctx
            }
    
    let handlePlaneteBleueEpisodes =
        fun (next : HttpFunc) (ctx: HttpContext) ->
            task { 

                let response = Scraper.GetEpisodeList()
                return! json response next ctx
            }

    let handlePlaneteBleuePlaylist (number:int) =
        fun (next : HttpFunc) (ctx: HttpContext) ->
            task { 
                let response = Scraper.GetPlaylist number
                return! json response next ctx
            }