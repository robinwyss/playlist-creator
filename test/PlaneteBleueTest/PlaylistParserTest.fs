namespace PlaneteBleueTest

open System
open NUnit.Framework
open PlaneteBleue

[<TestFixture>]
type TestClass () =

    [<SetUp>]
    member this.Setup () =
        ()

    [<Test>]
    member this.TestEpisodeParser950Name () =
        let getPlaylist = Scraper.GetPlaylistInternal MockLoader.load
        let playlist950 = getPlaylist 950
        Assert.That( playlist950.Name, Is.EqualTo( 950 ) ) 

    [<Test>]
    member this.TestEpisodeParser950FirstTrack () =
        let getPlaylist = Scraper.GetPlaylistInternal MockLoader.load
        let playlist950 = getPlaylist 950
        Assert.That( (playlist950.Tracks |> Seq.head).Artist, Is.EqualTo( "Mark Isham" ) ) 
        Assert.That( (playlist950.Tracks |> Seq.head).Title, Is.EqualTo( "Do You Like Puzzles?" ) )

    [<Test>]
    member this.TestEpisodeParser950LastTrack () =
        let getPlaylist = Scraper.GetPlaylistInternal MockLoader.load
        let playlist950 = getPlaylist 950
        Assert.That( (playlist950.Tracks |> Seq.last).Artist, Is.EqualTo( "Steve Turre" ) ) 
        Assert.That( (playlist950.Tracks |> Seq.last).Title, Is.EqualTo( "Beautiful India" ) ) 
        
