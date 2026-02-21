using Playlist.Console.Iterators;
using Playlist.Console.Models;

namespace Playlist.Console.Collections;

public class Playlist : ISongCollection
{
    private readonly List<Song> _songs = new();

    public string Name { get; }

    public Playlist(string name)
    {
        Name = name;
    }

    public void AddSong(Song song)
    {
        _songs.Add(song);
    }

    public ISongIterator CreateSequentialIterator()
        => new SequentialIterator(_songs);

    public ISongIterator CreateShuffleIterator()
        => new ShuffleIterator(_songs);

    public ISongIterator CreateFilteredIterator(Func<Song, bool> predicate)
        => new FilteredIterator(_songs, predicate);
}