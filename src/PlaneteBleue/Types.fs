
namespace PlaneteBleueTypes

type Release = {Id: int; Name: string}

type Track = {Title: string; Artist: string}

type Playlist = {Tracks: Track list; Name: int} 

type ParseResult =
    | Success of Release
    | Error of string