using Playlist.Console.Iterators;
using Playlist.Console.Models;
using Playlist.Console.Player;
using System.IO;

namespace Playlist.Tests.Player;

public class MusicPlayerTests
{
    [Fact]
    public void Play_WithSongs_ShouldOutputCorrectly()
    {
        // Arrange
        var player = new MusicPlayer();
        var songs = new List<Song>
        {
            new Song("Bohemian Rhapsody", "Queen", "Rock", 354, 1975),
            new Song("Imagine", "John Lennon", "Pop", 183, 1971)
        };
        var iterator = new SequentialIterator(songs);

        // Capture console output
        var originalOutput = System.Console.Out;
        using var stringWriter = new StringWriter();
        System.Console.SetOut(stringWriter);

        // Act
        player.Play("Test Playlist", iterator);

        // Restore console output
        System.Console.SetOut(originalOutput);
        var output = stringWriter.ToString();

        // Assert
        Assert.Contains("=== Test Playlist ===", output);
        Assert.Contains("1. Bohemian Rhapsody - Queen (Rock, 1975) [05:54]", output);
        Assert.Contains("2. Imagine - John Lennon (Pop, 1971) [03:03]", output);
    }

    [Fact]
    public void Play_WithEmptyIterator_ShouldOutputTitleOnly()
    {
        // Arrange
        var player = new MusicPlayer();
        var songs = new List<Song>();
        var iterator = new SequentialIterator(songs);

        // Capture console output
        var originalOutput = System.Console.Out;
        using var stringWriter = new StringWriter();
        System.Console.SetOut(stringWriter);

        // Act
        player.Play("Empty Playlist", iterator);

        // Restore console output
        System.Console.SetOut(originalOutput);
        var output = stringWriter.ToString();

        // Assert
        Assert.Contains("=== Empty Playlist ===", output);
        // Should not contain any song numbers
        Assert.DoesNotContain("1.", output);
    }

    [Fact]
    public void Play_WithSingleSong_ShouldOutputCorrectly()
    {
        // Arrange
        var player = new MusicPlayer();
        var songs = new List<Song>
        {
            new Song("Test Song", "Test Artist", "Pop", 180, 2020)
        };
        var iterator = new SequentialIterator(songs);

        // Capture console output
        var originalOutput = System.Console.Out;
        using var stringWriter = new StringWriter();
        System.Console.SetOut(stringWriter);

        // Act
        player.Play("Single Song", iterator);

        // Restore console output
        System.Console.SetOut(originalOutput);
        var output = stringWriter.ToString();

        // Assert
        Assert.Contains("=== Single Song ===", output);
        Assert.Contains("1. Test Song - Test Artist (Pop, 2020) [03:00]", output);
        Assert.DoesNotContain("2.", output);
    }

    [Fact]
    public void Play_WithFilteredIterator_ShouldOutputOnlyFilteredSongs()
    {
        // Arrange
        var player = new MusicPlayer();
        var songs = new List<Song>
        {
            new Song("Rock Song 1", "Artist 1", "Rock", 180, 2020),
            new Song("Pop Song", "Artist 2", "Pop", 200, 2021),
            new Song("Rock Song 2", "Artist 3", "Rock", 150, 2019)
        };
        var iterator = new FilteredIterator(songs, s => s.Genre == "Rock");

        // Capture console output
        var originalOutput = System.Console.Out;
        using var stringWriter = new StringWriter();
        System.Console.SetOut(stringWriter);

        // Act
        player.Play("Rock Only", iterator);

        // Restore console output
        System.Console.SetOut(originalOutput);
        var output = stringWriter.ToString();

        // Assert
        Assert.Contains("=== Rock Only ===", output);
        Assert.Contains("Rock Song 1", output);
        Assert.Contains("Rock Song 2", output);
        Assert.DoesNotContain("Pop Song", output);
    }

    [Fact]
    public void Play_WithMultipleSongs_ShouldNumberCorrectly()
    {
        // Arrange
        var player = new MusicPlayer();
        var songs = new List<Song>();
        for (int i = 1; i <= 5; i++)
        {
            songs.Add(new Song($"Song {i}", $"Artist {i}", "Pop", 180, 2020));
        }
        var iterator = new SequentialIterator(songs);

        // Capture console output
        var originalOutput = System.Console.Out;
        using var stringWriter = new StringWriter();
        System.Console.SetOut(stringWriter);

        // Act
        player.Play("Multiple Songs", iterator);

        // Restore console output
        System.Console.SetOut(originalOutput);
        var output = stringWriter.ToString();

        // Assert
        Assert.Contains("1. Song 1", output);
        Assert.Contains("2. Song 2", output);
        Assert.Contains("3. Song 3", output);
        Assert.Contains("4. Song 4", output);
        Assert.Contains("5. Song 5", output);
    }

    [Fact]
    public void Play_WithShuffleIterator_ShouldOutputAllSongs()
    {
        // Arrange
        var player = new MusicPlayer();
        var songs = new List<Song>
        {
            new Song("Song A", "Artist A", "Rock", 180, 2020),
            new Song("Song B", "Artist B", "Pop", 200, 2021),
            new Song("Song C", "Artist C", "Jazz", 150, 2019)
        };
        var iterator = new ShuffleIterator(songs);

        // Capture console output
        var originalOutput = System.Console.Out;
        using var stringWriter = new StringWriter();
        System.Console.SetOut(stringWriter);

        // Act
        player.Play("Shuffled", iterator);

        // Restore console output
        System.Console.SetOut(originalOutput);
        var output = stringWriter.ToString();

        // Assert
        Assert.Contains("=== Shuffled ===", output);
        Assert.Contains("Song A", output);
        Assert.Contains("Song B", output);
        Assert.Contains("Song C", output);
    }

    [Fact]
    public void Play_MultipleTimes_ShouldWorkIndependently()
    {
        // Arrange
        var player = new MusicPlayer();
        var songs = new List<Song>
        {
            new Song("Test Song", "Test Artist", "Pop", 180, 2020)
        };

        // Capture console output
        var originalOutput = System.Console.Out;
        using var stringWriter = new StringWriter();
        System.Console.SetOut(stringWriter);

        // Act
        player.Play("First Play", new SequentialIterator(songs));
        player.Play("Second Play", new SequentialIterator(songs));

        // Restore console output
        System.Console.SetOut(originalOutput);
        var output = stringWriter.ToString();

        // Assert
        Assert.Contains("=== First Play ===", output);
        Assert.Contains("=== Second Play ===", output);
    }
}

