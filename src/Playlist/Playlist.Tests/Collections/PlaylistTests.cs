using Playlist.Console.Collections;
using Playlist.Console.Models;

namespace Playlist.Tests.Collections;

public class PlaylistTests
{
    [Fact]
    public void Constructor_WithValidName_ShouldCreatePlaylist()
    {
        // Arrange & Act
        var playlist = new Console.Collections.Playlist("Minhas Favoritas");

        // Assert
        Assert.Equal("Minhas Favoritas", playlist.Name);
    }

    [Fact]
    public void AddSong_ShouldAddSongToPlaylist()
    {
        // Arrange
        var playlist = new Console.Collections.Playlist("Test Playlist");
        var song = new Song("Test Song", "Test Artist", "Pop", 180, 2020);

        // Act
        playlist.AddSong(song);
        var iterator = playlist.CreateSequentialIterator();

        // Assert
        Assert.True(iterator.HasNext());
        Assert.Equal(song, iterator.Next());
    }

    [Fact]
    public void AddSong_WithMultipleSongs_ShouldAddAllSongs()
    {
        // Arrange
        var playlist = new Console.Collections.Playlist("Test Playlist");
        var song1 = new Song("Song 1", "Artist 1", "Pop", 180, 2020);
        var song2 = new Song("Song 2", "Artist 2", "Rock", 200, 2021);
        var song3 = new Song("Song 3", "Artist 3", "Jazz", 150, 2019);

        // Act
        playlist.AddSong(song1);
        playlist.AddSong(song2);
        playlist.AddSong(song3);
        var iterator = playlist.CreateSequentialIterator();

        // Assert
        var songs = new List<Song>();
        while (iterator.HasNext())
        {
            songs.Add(iterator.Next());
        }

        Assert.Equal(3, songs.Count);
        Assert.Contains(song1, songs);
        Assert.Contains(song2, songs);
        Assert.Contains(song3, songs);
    }

    [Fact]
    public void CreateSequentialIterator_ShouldReturnSequentialIterator()
    {
        // Arrange
        var playlist = new Console.Collections.Playlist("Test Playlist");
        var song1 = new Song("Song 1", "Artist 1", "Pop", 180, 2020);
        var song2 = new Song("Song 2", "Artist 2", "Rock", 200, 2021);
        
        playlist.AddSong(song1);
        playlist.AddSong(song2);

        // Act
        var iterator = playlist.CreateSequentialIterator();

        // Assert
        Assert.NotNull(iterator);
        Assert.True(iterator.HasNext());
        Assert.Equal(song1, iterator.Next());
        Assert.Equal(song2, iterator.Next());
        Assert.False(iterator.HasNext());
    }

    [Fact]
    public void CreateShuffleIterator_ShouldReturnShuffleIterator()
    {
        // Arrange
        var playlist = new Console.Collections.Playlist("Test Playlist");
        var song1 = new Song("Song 1", "Artist 1", "Pop", 180, 2020);
        var song2 = new Song("Song 2", "Artist 2", "Rock", 200, 2021);
        var song3 = new Song("Song 3", "Artist 3", "Jazz", 150, 2019);
        
        playlist.AddSong(song1);
        playlist.AddSong(song2);
        playlist.AddSong(song3);

        // Act
        var iterator = playlist.CreateShuffleIterator();

        // Assert
        Assert.NotNull(iterator);
        
        var songs = new List<Song>();
        while (iterator.HasNext())
        {
            songs.Add(iterator.Next());
        }

        // Should have all songs, but potentially in different order
        Assert.Equal(3, songs.Count);
        Assert.Contains(song1, songs);
        Assert.Contains(song2, songs);
        Assert.Contains(song3, songs);
    }

    [Fact]
    public void CreateFilteredIterator_WithGenreFilter_ShouldReturnOnlyMatchingSongs()
    {
        // Arrange
        var playlist = new Console.Collections.Playlist("Test Playlist");
        var rockSong1 = new Song("Rock Song 1", "Artist 1", "Rock", 180, 2020);
        var popSong = new Song("Pop Song", "Artist 2", "Pop", 200, 2021);
        var rockSong2 = new Song("Rock Song 2", "Artist 3", "Rock", 150, 2019);
        
        playlist.AddSong(rockSong1);
        playlist.AddSong(popSong);
        playlist.AddSong(rockSong2);

        // Act
        var iterator = playlist.CreateFilteredIterator(s => s.Genre == "Rock");

        // Assert
        var songs = new List<Song>();
        while (iterator.HasNext())
        {
            songs.Add(iterator.Next());
        }

        Assert.Equal(2, songs.Count);
        Assert.Contains(rockSong1, songs);
        Assert.Contains(rockSong2, songs);
        Assert.DoesNotContain(popSong, songs);
    }

    [Fact]
    public void CreateFilteredIterator_WithYearFilter_ShouldReturnOnlyMatchingSongs()
    {
        // Arrange
        var playlist = new Console.Collections.Playlist("Test Playlist");
        var oldSong1 = new Song("Old Song 1", "Artist 1", "Rock", 180, 1975);
        var newSong = new Song("New Song", "Artist 2", "Pop", 200, 2021);
        var oldSong2 = new Song("Old Song 2", "Artist 3", "Jazz", 150, 1979);
        
        playlist.AddSong(oldSong1);
        playlist.AddSong(newSong);
        playlist.AddSong(oldSong2);

        // Act
        var iterator = playlist.CreateFilteredIterator(s => s.Year < 1980);

        // Assert
        var songs = new List<Song>();
        while (iterator.HasNext())
        {
            songs.Add(iterator.Next());
        }

        Assert.Equal(2, songs.Count);
        Assert.Contains(oldSong1, songs);
        Assert.Contains(oldSong2, songs);
        Assert.DoesNotContain(newSong, songs);
    }

    [Fact]
    public void CreateFilteredIterator_WithNoMatches_ShouldReturnEmptyIterator()
    {
        // Arrange
        var playlist = new Console.Collections.Playlist("Test Playlist");
        var song1 = new Song("Song 1", "Artist 1", "Rock", 180, 2020);
        var song2 = new Song("Song 2", "Artist 2", "Pop", 200, 2021);
        
        playlist.AddSong(song1);
        playlist.AddSong(song2);

        // Act
        var iterator = playlist.CreateFilteredIterator(s => s.Genre == "Jazz");

        // Assert
        Assert.False(iterator.HasNext());
    }

    [Fact]
    public void Playlist_ShouldImplementISongCollection()
    {
        // Arrange
        var playlist = new Console.Collections.Playlist("Test Playlist");

        // Assert
        Assert.IsAssignableFrom<ISongCollection>(playlist);
    }

    [Fact]
    public void EmptyPlaylist_CreateIterators_ShouldReturnEmptyIterators()
    {
        // Arrange
        var playlist = new Console.Collections.Playlist("Empty Playlist");

        // Act
        var sequentialIterator = playlist.CreateSequentialIterator();
        var shuffleIterator = playlist.CreateShuffleIterator();
        var filteredIterator = playlist.CreateFilteredIterator(s => true);

        // Assert
        Assert.False(sequentialIterator.HasNext());
        Assert.False(shuffleIterator.HasNext());
        Assert.False(filteredIterator.HasNext());
    }
}

