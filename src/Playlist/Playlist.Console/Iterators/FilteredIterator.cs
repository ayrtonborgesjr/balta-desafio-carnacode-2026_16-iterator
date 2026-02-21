using Playlist.Console.Models;

namespace Playlist.Console.Iterators;

public class FilteredIterator : ISongIterator
{
    private readonly List<Song> _filtered;
    private int _position = 0;

    public FilteredIterator(List<Song> songs, Func<Song, bool> predicate)
    {
        _filtered = songs.Where(predicate).ToList();
    }

    public bool HasNext()
        => _position < _filtered.Count;

    public Song Next()
        => _filtered[_position++];

    public void Reset()
        => _position = 0;
}