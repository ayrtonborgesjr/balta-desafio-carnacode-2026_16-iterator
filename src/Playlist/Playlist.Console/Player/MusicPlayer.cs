using Playlist.Console.Iterators;

namespace Playlist.Console.Player;

public class MusicPlayer
{
    public void Play(string title, ISongIterator iterator)
    {
        System.Console.WriteLine($"\n=== {title} ===");

        int count = 1;

        while (iterator.HasNext())
        {
            System.Console.WriteLine($"{count++}. {iterator.Next()}");
        }
    }
}