using Playlist.Console.Models;

namespace Playlist.Console.Iterators;

public interface ISongIterator
{
    bool HasNext();
    Song Next();
    void Reset();
}