using Playlist.Console.Models;

namespace Playlist.Console.Iterators;

public class SequentialIterator : ISongIterator
{
    private readonly List<Song> _songs;
    private int _position = 0;

    public SequentialIterator(List<Song> songs)
    {
        _songs = songs;
    }

    public bool HasNext()
        => _position < _songs.Count;

    public Song Next()
        => _songs[_position++];

    public void Reset()
        => _position = 0;
}