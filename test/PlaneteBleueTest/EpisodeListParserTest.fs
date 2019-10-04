namespace PlaneteBleueTest

open System
open NUnit.Framework
open PlaneteBleue

[<TestFixture>]
type EpisodeListTest () =


    [<Test>]
    member this.TestEpisodeList() =
        let episodeList = Scraper.GetEpisodeListInternal MockLoader.load
        Assert.That( episodeList |> Seq.length, Is.EqualTo( 1977 ) ) 
        Assert.That( (episodeList |> Seq.head ).Name, Is.EqualTo( "LPB 970 du dimanche 1 septembre 2019" ) ) 
        Assert.That( (episodeList |> Seq.last ).Name, Is.EqualTo( "LPB 1 du dimanche 8 janvier 1995" ) )
        