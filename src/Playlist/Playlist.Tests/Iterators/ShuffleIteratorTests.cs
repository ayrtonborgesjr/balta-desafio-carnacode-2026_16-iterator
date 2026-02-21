using Playlist.Console.Iterators;
using Playlist.Console.Models;

namespace Playlist.Tests.Iterators;

public class ShuffleIteratorTests
{
    [Fact]
    public void Constructor_WithSongs_ShouldCreateIterator()
    {
        // Arrange
        var songs = new List<Song>
        {
            new Song("Song 1", "Artist 1", "Pop", 180, 2020)
        };

        // Act
        var iterator = new ShuffleIterator(songs);

        // Assert
        Assert.NotNull(iterator);
    }

    [Fact]
    public void HasNext_WithSongs_ShouldReturnTrue()
    {
        // Arrange
        var songs = new List<Song>
        {
            new Song("Song 1", "Artist 1", "Pop", 180, 2020)
        };
        var iterator = new ShuffleIterator(songs);

        // Act & Assert
        Assert.True(iterator.HasNext());
    }

    [Fact]
    public void HasNext_EmptyList_ShouldReturnFalse()
    {
        // Arrange
        var songs = new List<Song>();
        var iterator = new ShuffleIterator(songs);

        // Act & Assert
        Assert.False(iterator.HasNext());
    }

    [Fact]
    public void Iterator_ShouldContainAllSongs()
    {
        // Arrange
        var song1 = new Song("Song 1", "Artist 1", "Pop", 180, 2020);
        var song2 = new Song("Song 2", "Artist 2", "Rock", 200, 2021);
        var song3 = new Song("Song 3", "Artist 3", "Jazz", 150, 2019);
        
        var songs = new List<Song> { song1, song2, song3 };
        var iterator = new ShuffleIterator(songs);

        // Act
        var iteratedSongs = new List<Song>();
        while (iterator.HasNext())
        {
            iteratedSongs.Add(iterator.Next());
        }

        // Assert - All songs should be present
        Assert.Equal(3, iteratedSongs.Count);
        Assert.Contains(song1, iteratedSongs);
        Assert.Contains(song2, iteratedSongs);
        Assert.Contains(song3, iteratedSongs);
    }

    [Fact]
    public void HasNext_AfterIteratingAll_ShouldReturnFalse()
    {
        // Arrange
        var song1 = new Song("Song 1", "Artist 1", "Pop", 180, 2020);
        var song2 = new Song("Song 2", "Artist 2", "Rock", 200, 2021);
        
        var songs = new List<Song> { song1, song2 };
        var iterator = new ShuffleIterator(songs);

        // Act
        iterator.Next();
        iterator.Next();

        // Assert
        Assert.False(iterator.HasNext());
    }

    [Fact]
    public void Reset_ShouldResetIteratorToBeginning()
    {
        // Arrange
        var song1 = new Song("Song 1", "Artist 1", "Pop", 180, 2020);
        var song2 = new Song("Song 2", "Artist 2", "Rock", 200, 2021);
        
        var songs = new List<Song> { song1, song2 };
        var iterator = new ShuffleIterator(songs);

        // Act
        iterator.Next();
        iterator.Next();
        Assert.False(iterator.HasNext());
        
        iterator.Reset();

        // Assert
        Assert.True(iterator.HasNext());
    }

    [Fact]
    public void ShuffleIterator_ShouldImplementISongIterator()
    {
        // Arrange
        var songs = new List<Song>();
        var iterator = new ShuffleIterator(songs);

        // Assert
        Assert.IsAssignableFrom<ISongIterator>(iterator);
    }

    [Fact]
    public void Reset_ShouldNotChangeShuffleOrder()
    {
        // Arrange
        var song1 = new Song("Song 1", "Artist 1", "Pop", 180, 2020);
        var song2 = new Song("Song 2", "Artist 2", "Rock", 200, 2021);
        var song3 = new Song("Song 3", "Artist 3", "Jazz", 150, 2019);
        
        var songs = new List<Song> { song1, song2, song3 };
        var iterator = new ShuffleIterator(songs);

        // Act - First iteration
        var firstIteration = new List<Song>();
        while (iterator.HasNext())
        {
            firstIteration.Add(iterator.Next());
        }

        iterator.Reset();

        // Second iteration
        var secondIteration = new List<Song>();
        while (iterator.HasNext())
        {
            secondIteration.Add(iterator.Next());
        }

        // Assert - Order should be the same after reset
        Assert.Equal(firstIteration, secondIteration);
    }

    [Fact]
    public void SingleSong_ShouldReturnSameSong()
    {
        // Arrange
        var song = new Song("Song 1", "Artist 1", "Pop", 180, 2020);
        var songs = new List<Song> { song };
        var iterator = new ShuffleIterator(songs);

        // Act
        var result = iterator.Next();

        // Assert
        Assert.Equal(song, result);
        Assert.False(iterator.HasNext());
    }

    [Fact]
    public void Iterator_WithManySongs_ShouldShuffleStatistically()
    {
        // Arrange - Create enough songs to make it very unlikely to get same order
        var songs = new List<Song>();
        for (int i = 0; i < 10; i++)
        {
            songs.Add(new Song($"Song {i}", $"Artist {i}", "Pop", 180, 2020));
        }

        // Act - Create multiple iterators and collect first songs
        var firstSongs = new List<Song>();
        for (int i = 0; i < 20; i++)
        {
            var iterator = new ShuffleIterator(songs);
            firstSongs.Add(iterator.Next());
        }

        // Assert - We should have some variation (not all same first song)
        var distinctFirstSongs = firstSongs.Distinct().Count();
        Assert.True(distinctFirstSongs > 1, "Shuffle should produce different orders");
    }
}

