using Playlist.Console.Iterators;
using Playlist.Console.Models;

namespace Playlist.Console.Collections;

public class MusicLibrary : ISongCollection
{
    private readonly Dictionary<string, List<Song>> _songsByGenre = new();
    private readonly Dictionary<string, List<Song>> _songsByArtist = new();

    public void AddSong(Song song)
    {
        if (!_songsByGenre.ContainsKey(song.Genre))
            _songsByGenre[song.Genre] = new List<Song>();

        if (!_songsByArtist.ContainsKey(song.Artist))
            _songsByArtist[song.Artist] = new List<Song>();

        _songsByGenre[song.Genre].Add(song);
        _songsByArtist[song.Artist].Add(song);
    }

    // 🔹 Itera sobre TODAS as músicas
    public ISongIterator CreateSequentialIterator()
    {
        var allSongs = _songsByGenre
            .SelectMany(g => g.Value)
            .Distinct()
            .ToList();

        return new SequentialIterator(allSongs);
    }

    // 🔹 Iteração filtrada genérica
    public ISongIterator CreateFilteredIterator(Func<Song, bool> predicate)
    {
        var allSongs = _songsByGenre
            .SelectMany(g => g.Value)
            .Distinct()
            .Where(predicate)
            .ToList();

        return new FilteredIterator(allSongs, s => true);
    }

    // 🔹 Iterator especializado por gênero
    public ISongIterator CreateGenreIterator(string genre)
    {
        if (!_songsByGenre.ContainsKey(genre))
            return new SequentialIterator(new List<Song>());

        return new SequentialIterator(_songsByGenre[genre]);
    }

    // 🔹 Iterator especializado por artista
    public ISongIterator CreateArtistIterator(string artist)
    {
        if (!_songsByArtist.ContainsKey(artist))
            return new SequentialIterator(new List<Song>());

        return new SequentialIterator(_songsByArtist[artist]);
    }
}