using Playlist.Console.Iterators;
using Playlist.Console.Models;

namespace Playlist.Tests.Iterators;

public class SequentialIteratorTests
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
        var iterator = new SequentialIterator(songs);

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
        var iterator = new SequentialIterator(songs);

        // Act & Assert
        Assert.True(iterator.HasNext());
    }

    [Fact]
    public void HasNext_EmptyList_ShouldReturnFalse()
    {
        // Arrange
        var songs = new List<Song>();
        var iterator = new SequentialIterator(songs);

        // Act & Assert
        Assert.False(iterator.HasNext());
    }

    [Fact]
    public void Next_ShouldReturnSongsInOrder()
    {
        // Arrange
        var song1 = new Song("Song 1", "Artist 1", "Pop", 180, 2020);
        var song2 = new Song("Song 2", "Artist 2", "Rock", 200, 2021);
        var song3 = new Song("Song 3", "Artist 3", "Jazz", 150, 2019);
        
        var songs = new List<Song> { song1, song2, song3 };
        var iterator = new SequentialIterator(songs);

        // Act & Assert
        Assert.Equal(song1, iterator.Next());
        Assert.Equal(song2, iterator.Next());
        Assert.Equal(song3, iterator.Next());
    }

    [Fact]
    public void HasNext_AfterIteratingAll_ShouldReturnFalse()
    {
        // Arrange
        var song1 = new Song("Song 1", "Artist 1", "Pop", 180, 2020);
        var song2 = new Song("Song 2", "Artist 2", "Rock", 200, 2021);
        
        var songs = new List<Song> { song1, song2 };
        var iterator = new SequentialIterator(songs);

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
        var iterator = new SequentialIterator(songs);

        // Act
        var firstSong = iterator.Next();
        var secondSong = iterator.Next();
        iterator.Reset();

        // Assert
        Assert.True(iterator.HasNext());
        Assert.Equal(song1, iterator.Next());
    }

    [Fact]
    public void SequentialIterator_ShouldImplementISongIterator()
    {
        // Arrange
        var songs = new List<Song>();
        var iterator = new SequentialIterator(songs);

        // Assert
        Assert.IsAssignableFrom<ISongIterator>(iterator);
    }

    [Fact]
    public void Iterator_MultipleIterations_ShouldWorkCorrectly()
    {
        // Arrange
        var song1 = new Song("Song 1", "Artist 1", "Pop", 180, 2020);
        var song2 = new Song("Song 2", "Artist 2", "Rock", 200, 2021);
        
        var songs = new List<Song> { song1, song2 };
        var iterator = new SequentialIterator(songs);

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

        // Assert
        Assert.Equal(firstIteration, secondIteration);
    }
}

