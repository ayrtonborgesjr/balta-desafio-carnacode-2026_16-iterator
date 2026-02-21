using Playlist.Console.Iterators;
using Playlist.Console.Models;

namespace Playlist.Tests.Iterators;

public class FilteredIteratorTests
{
    [Fact]
    public void Constructor_WithSongsAndPredicate_ShouldCreateIterator()
    {
        // Arrange
        var songs = new List<Song>
        {
            new Song("Song 1", "Artist 1", "Pop", 180, 2020)
        };

        // Act
        var iterator = new FilteredIterator(songs, s => s.Genre == "Pop");

        // Assert
        Assert.NotNull(iterator);
    }

    [Fact]
    public void HasNext_WithMatchingSongs_ShouldReturnTrue()
    {
        // Arrange
        var songs = new List<Song>
        {
            new Song("Song 1", "Artist 1", "Pop", 180, 2020)
        };
        var iterator = new FilteredIterator(songs, s => s.Genre == "Pop");

        // Act & Assert
        Assert.True(iterator.HasNext());
    }

    [Fact]
    public void HasNext_WithNoMatchingSongs_ShouldReturnFalse()
    {
        // Arrange
        var songs = new List<Song>
        {
            new Song("Song 1", "Artist 1", "Pop", 180, 2020)
        };
        var iterator = new FilteredIterator(songs, s => s.Genre == "Rock");

        // Act & Assert
        Assert.False(iterator.HasNext());
    }

    [Fact]
    public void HasNext_EmptyList_ShouldReturnFalse()
    {
        // Arrange
        var songs = new List<Song>();
        var iterator = new FilteredIterator(songs, s => true);

        // Act & Assert
        Assert.False(iterator.HasNext());
    }

    [Fact]
    public void Next_ShouldReturnOnlyFilteredSongs()
    {
        // Arrange
        var rockSong1 = new Song("Rock Song 1", "Artist 1", "Rock", 180, 2020);
        var popSong = new Song("Pop Song", "Artist 2", "Pop", 200, 2021);
        var rockSong2 = new Song("Rock Song 2", "Artist 3", "Rock", 150, 2019);
        
        var songs = new List<Song> { rockSong1, popSong, rockSong2 };
        var iterator = new FilteredIterator(songs, s => s.Genre == "Rock");

        // Act
        var filteredSongs = new List<Song>();
        while (iterator.HasNext())
        {
            filteredSongs.Add(iterator.Next());
        }

        // Assert
        Assert.Equal(2, filteredSongs.Count);
        Assert.Contains(rockSong1, filteredSongs);
        Assert.Contains(rockSong2, filteredSongs);
        Assert.DoesNotContain(popSong, filteredSongs);
    }

    [Fact]
    public void Filter_ByYear_ShouldReturnCorrectSongs()
    {
        // Arrange
        var oldSong1 = new Song("Old Song 1", "Artist 1", "Rock", 180, 1975);
        var newSong = new Song("New Song", "Artist 2", "Pop", 200, 2021);
        var oldSong2 = new Song("Old Song 2", "Artist 3", "Jazz", 150, 1979);
        
        var songs = new List<Song> { oldSong1, newSong, oldSong2 };
        var iterator = new FilteredIterator(songs, s => s.Year < 1980);

        // Act
        var filteredSongs = new List<Song>();
        while (iterator.HasNext())
        {
            filteredSongs.Add(iterator.Next());
        }

        // Assert
        Assert.Equal(2, filteredSongs.Count);
        Assert.Contains(oldSong1, filteredSongs);
        Assert.Contains(oldSong2, filteredSongs);
        Assert.DoesNotContain(newSong, filteredSongs);
    }

    [Fact]
    public void Filter_ByArtist_ShouldReturnCorrectSongs()
    {
        // Arrange
        var song1 = new Song("Song 1", "Queen", "Rock", 180, 1975);
        var song2 = new Song("Song 2", "Beatles", "Pop", 200, 1970);
        var song3 = new Song("Song 3", "Queen", "Rock", 150, 1980);
        
        var songs = new List<Song> { song1, song2, song3 };
        var iterator = new FilteredIterator(songs, s => s.Artist == "Queen");

        // Act
        var filteredSongs = new List<Song>();
        while (iterator.HasNext())
        {
            filteredSongs.Add(iterator.Next());
        }

        // Assert
        Assert.Equal(2, filteredSongs.Count);
        Assert.Contains(song1, filteredSongs);
        Assert.Contains(song3, filteredSongs);
        Assert.DoesNotContain(song2, filteredSongs);
    }

    [Fact]
    public void Filter_ByDuration_ShouldReturnCorrectSongs()
    {
        // Arrange
        var shortSong = new Song("Short Song", "Artist 1", "Pop", 120, 2020);
        var mediumSong = new Song("Medium Song", "Artist 2", "Rock", 250, 2021);
        var longSong = new Song("Long Song", "Artist 3", "Jazz", 400, 2019);
        
        var songs = new List<Song> { shortSong, mediumSong, longSong };
        var iterator = new FilteredIterator(songs, s => s.DurationSeconds > 200);

        // Act
        var filteredSongs = new List<Song>();
        while (iterator.HasNext())
        {
            filteredSongs.Add(iterator.Next());
        }

        // Assert
        Assert.Equal(2, filteredSongs.Count);
        Assert.Contains(mediumSong, filteredSongs);
        Assert.Contains(longSong, filteredSongs);
        Assert.DoesNotContain(shortSong, filteredSongs);
    }

    [Fact]
    public void Filter_WithComplexPredicate_ShouldReturnCorrectSongs()
    {
        // Arrange
        var song1 = new Song("Song 1", "Artist 1", "Rock", 180, 1975);
        var song2 = new Song("Song 2", "Artist 2", "Rock", 400, 1980);
        var song3 = new Song("Song 3", "Artist 3", "Pop", 200, 1975);
        var song4 = new Song("Song 4", "Artist 4", "Rock", 250, 1970);
        
        var songs = new List<Song> { song1, song2, song3, song4 };
        // Filter: Rock songs from before 1980 with duration less than 300 seconds
        var iterator = new FilteredIterator(songs, 
            s => s.Genre == "Rock" && s.Year < 1980 && s.DurationSeconds < 300);

        // Act
        var filteredSongs = new List<Song>();
        while (iterator.HasNext())
        {
            filteredSongs.Add(iterator.Next());
        }

        // Assert
        Assert.Equal(2, filteredSongs.Count);
        Assert.Contains(song1, filteredSongs);
        Assert.Contains(song4, filteredSongs);
    }

    [Fact]
    public void HasNext_AfterIteratingAll_ShouldReturnFalse()
    {
        // Arrange
        var song1 = new Song("Song 1", "Artist 1", "Pop", 180, 2020);
        var song2 = new Song("Song 2", "Artist 2", "Pop", 200, 2021);
        
        var songs = new List<Song> { song1, song2 };
        var iterator = new FilteredIterator(songs, s => s.Genre == "Pop");

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
        var song2 = new Song("Song 2", "Artist 2", "Pop", 200, 2021);
        
        var songs = new List<Song> { song1, song2 };
        var iterator = new FilteredIterator(songs, s => s.Genre == "Pop");

        // Act
        var firstSong = iterator.Next();
        var secondSong = iterator.Next();
        iterator.Reset();

        // Assert
        Assert.True(iterator.HasNext());
        Assert.Equal(song1, iterator.Next());
    }

    [Fact]
    public void FilteredIterator_ShouldImplementISongIterator()
    {
        // Arrange
        var songs = new List<Song>();
        var iterator = new FilteredIterator(songs, s => true);

        // Assert
        Assert.IsAssignableFrom<ISongIterator>(iterator);
    }

    [Fact]
    public void Filter_WithTruePredicate_ShouldReturnAllSongs()
    {
        // Arrange
        var song1 = new Song("Song 1", "Artist 1", "Pop", 180, 2020);
        var song2 = new Song("Song 2", "Artist 2", "Rock", 200, 2021);
        var song3 = new Song("Song 3", "Artist 3", "Jazz", 150, 2019);
        
        var songs = new List<Song> { song1, song2, song3 };
        var iterator = new FilteredIterator(songs, s => true);

        // Act
        var filteredSongs = new List<Song>();
        while (iterator.HasNext())
        {
            filteredSongs.Add(iterator.Next());
        }

        // Assert
        Assert.Equal(3, filteredSongs.Count);
    }

    [Fact]
    public void Filter_WithFalsePredicate_ShouldReturnNoSongs()
    {
        // Arrange
        var song1 = new Song("Song 1", "Artist 1", "Pop", 180, 2020);
        var song2 = new Song("Song 2", "Artist 2", "Rock", 200, 2021);
        
        var songs = new List<Song> { song1, song2 };
        var iterator = new FilteredIterator(songs, s => false);

        // Act & Assert
        Assert.False(iterator.HasNext());
    }

    [Fact]
    public void Iterator_MultipleIterations_ShouldWorkCorrectly()
    {
        // Arrange
        var song1 = new Song("Song 1", "Artist 1", "Rock", 180, 2020);
        var song2 = new Song("Song 2", "Artist 2", "Pop", 200, 2021);
        var song3 = new Song("Song 3", "Artist 3", "Rock", 150, 2019);
        
        var songs = new List<Song> { song1, song2, song3 };
        var iterator = new FilteredIterator(songs, s => s.Genre == "Rock");

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

