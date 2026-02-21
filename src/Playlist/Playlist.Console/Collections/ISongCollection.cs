using Playlist.Console.Iterators;
using Playlist.Console.Models;

namespace Playlist.Console.Collections;

public interface ISongCollection
{
    ISongIterator CreateSequentialIterator();
    ISongIterator CreateShuffleIterator();
    ISongIterator CreateFilteredIterator(Func<Song, bool> predicate);
}