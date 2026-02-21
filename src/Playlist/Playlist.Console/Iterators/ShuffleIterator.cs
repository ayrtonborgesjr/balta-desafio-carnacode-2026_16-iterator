using Playlist.Console.Models;

namespace Playlist.Console.Iterators;

public class ShuffleIterator : ISongIterator
{
    private readonly List<Song> _shuffled;
    private int _position = 0;

    public ShuffleIterator(List<Song> songs)
    {
        var random = new Random();
        _shuffled = songs.OrderBy(x => random.Next()).ToList();
    }

    public bool HasNext()
        => _position < _shuffled.Count;

    public Song Next()
        => _shuffled[_position++];

    public void Reset()
        => _position = 0;
}