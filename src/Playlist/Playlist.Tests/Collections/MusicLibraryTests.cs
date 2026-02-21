using Playlist.Console.Collections;
using Playlist.Console.Models;

namespace Playlist.Tests.Collections;

public class MusicLibraryTests
{
    [Fact]
    public void Constructor_ShouldCreateEmptyLibrary()
    {
        // Arrange & Act
        var library = new MusicLibrary();

        // Assert
        Assert.NotNull(library);
    }

    [Fact]
    public void AddSong_ShouldAddSongToLibrary()
    {
        // Arrange
        var library = new MusicLibrary();
        var song = new Song("Test Song", "Test Artist", "Pop", 180, 2020);

        // Act
        library.AddSong(song);
        var iterator = library.CreateSequentialIterator();

        // Assert
        Assert.True(iterator.HasNext());
        Assert.Equal(song, iterator.Next());
    }

    [Fact]
    public void AddSong_WithMultipleSongs_ShouldAddAllSongs()
    {
        // Arrange
        var library = new MusicLibrary();
        var song1 = new Song("Song 1", "Artist 1", "Pop", 180, 2020);
        var song2 = new Song("Song 2", "Artist 2", "Rock", 200, 2021);
        var song3 = new Song("Song 3", "Artist 3", "Jazz", 150, 2019);

        // Act
        library.AddSong(song1);
        library.AddSong(song2);
        library.AddSong(song3);
        var iterator = library.CreateSequentialIterator();

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
    public void AddSong_WithSameGenre_ShouldGroupByGenre()
    {
        // Arrange
        var library = new MusicLibrary();
        var rockSong1 = new Song("Rock Song 1", "Artist 1", "Rock", 180, 2020);
        var rockSong2 = new Song("Rock Song 2", "Artist 2", "Rock", 200, 2021);
        var popSong = new Song("Pop Song", "Artist 3", "Pop", 150, 2019);

        // Act
        library.AddSong(rockSong1);
        library.AddSong(rockSong2);
        library.AddSong(popSong);
        var genreIterator = library.CreateGenreIterator("Rock");

        // Assert
        var rockSongs = new List<Song>();
        while (genreIterator.HasNext())
        {
            rockSongs.Add(genreIterator.Next());
        }

        Assert.Equal(2, rockSongs.Count);
        Assert.Contains(rockSong1, rockSongs);
        Assert.Contains(rockSong2, rockSongs);
        Assert.DoesNotContain(popSong, rockSongs);
    }

    [Fact]
    public void AddSong_WithSameArtist_ShouldGroupByArtist()
    {
        // Arrange
        var library = new MusicLibrary();
        var queenSong1 = new Song("Bohemian Rhapsody", "Queen", "Rock", 354, 1975);
        var queenSong2 = new Song("We Will Rock You", "Queen", "Rock", 122, 1977);
        var beatlesSong = new Song("Hey Jude", "Beatles", "Pop", 431, 1968);

        // Act
        library.AddSong(queenSong1);
        library.AddSong(queenSong2);
        library.AddSong(beatlesSong);
        var artistIterator = library.CreateArtistIterator("Queen");

        // Assert
        var queenSongs = new List<Song>();
        while (artistIterator.HasNext())
        {
            queenSongs.Add(artistIterator.Next());
        }

        Assert.Equal(2, queenSongs.Count);
        Assert.Contains(queenSong1, queenSongs);
        Assert.Contains(queenSong2, queenSongs);
        Assert.DoesNotContain(beatlesSong, queenSongs);
    }

    [Fact]
    public void CreateSequentialIterator_EmptyLibrary_ShouldReturnEmptyIterator()
    {
        // Arrange
        var library = new MusicLibrary();

        // Act
        var iterator = library.CreateSequentialIterator();

        // Assert
        Assert.False(iterator.HasNext());
    }

    [Fact]
    public void CreateSequentialIterator_ShouldReturnAllSongs()
    {
        // Arrange
        var library = new MusicLibrary();
        var song1 = new Song("Song 1", "Artist 1", "Pop", 180, 2020);
        var song2 = new Song("Song 2", "Artist 2", "Rock", 200, 2021);
        var song3 = new Song("Song 3", "Artist 3", "Jazz", 150, 2019);

        library.AddSong(song1);
        library.AddSong(song2);
        library.AddSong(song3);

        // Act
        var iterator = library.CreateSequentialIterator();

        // Assert
        var songs = new List<Song>();
        while (iterator.HasNext())
        {
            songs.Add(iterator.Next());
        }

        Assert.Equal(3, songs.Count);
    }

    [Fact]
    public void CreateSequentialIterator_ShouldReturnDistinctSongs()
    {
        // Arrange
        var library = new MusicLibrary();
        var song = new Song("Test Song", "Test Artist", "Pop", 180, 2020);

        // Act - Add the same song reference twice (shouldn't duplicate)
        library.AddSong(song);
        var iterator = library.CreateSequentialIterator();

        // Assert
        var songs = new List<Song>();
        while (iterator.HasNext())
        {
            songs.Add(iterator.Next());
        }

        Assert.Single(songs);
    }

    [Fact]
    public void CreateFilteredIterator_WithGenreFilter_ShouldReturnFilteredSongs()
    {
        // Arrange
        var library = new MusicLibrary();
        var rockSong1 = new Song("Rock Song 1", "Artist 1", "Rock", 180, 2020);
        var popSong = new Song("Pop Song", "Artist 2", "Pop", 200, 2021);
        var rockSong2 = new Song("Rock Song 2", "Artist 3", "Rock", 150, 2019);

        library.AddSong(rockSong1);
        library.AddSong(popSong);
        library.AddSong(rockSong2);

        // Act
        var iterator = library.CreateFilteredIterator(s => s.Genre == "Rock");

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
    public void CreateFilteredIterator_WithYearFilter_ShouldReturnFilteredSongs()
    {
        // Arrange
        var library = new MusicLibrary();
        var oldSong1 = new Song("Old Song 1", "Artist 1", "Rock", 180, 1975);
        var newSong = new Song("New Song", "Artist 2", "Pop", 200, 2021);
        var oldSong2 = new Song("Old Song 2", "Artist 3", "Jazz", 150, 1979);

        library.AddSong(oldSong1);
        library.AddSong(newSong);
        library.AddSong(oldSong2);

        // Act
        var iterator = library.CreateFilteredIterator(s => s.Year < 1980);

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
    public void CreateFilteredIterator_WithComplexPredicate_ShouldReturnFilteredSongs()
    {
        // Arrange
        var library = new MusicLibrary();
        var song1 = new Song("Song 1", "Artist 1", "Rock", 180, 1975);
        var song2 = new Song("Song 2", "Artist 2", "Rock", 400, 1980);
        var song3 = new Song("Song 3", "Artist 3", "Pop", 200, 1975);
        var song4 = new Song("Song 4", "Artist 4", "Rock", 250, 1970);

        library.AddSong(song1);
        library.AddSong(song2);
        library.AddSong(song3);
        library.AddSong(song4);

        // Act - Rock songs from before 1980 with duration less than 300 seconds
        var iterator = library.CreateFilteredIterator(
            s => s.Genre == "Rock" && s.Year < 1980 && s.DurationSeconds < 300);

        // Assert
        var songs = new List<Song>();
        while (iterator.HasNext())
        {
            songs.Add(iterator.Next());
        }

        Assert.Equal(2, songs.Count);
        Assert.Contains(song1, songs);
        Assert.Contains(song4, songs);
    }

    [Fact]
    public void CreateFilteredIterator_WithNoMatches_ShouldReturnEmptyIterator()
    {
        // Arrange
        var library = new MusicLibrary();
        var song1 = new Song("Song 1", "Artist 1", "Rock", 180, 2020);
        var song2 = new Song("Song 2", "Artist 2", "Pop", 200, 2021);

        library.AddSong(song1);
        library.AddSong(song2);

        // Act
        var iterator = library.CreateFilteredIterator(s => s.Genre == "Jazz");

        // Assert
        Assert.False(iterator.HasNext());
    }

    [Fact]
    public void CreateGenreIterator_WithExistingGenre_ShouldReturnSongsOfThatGenre()
    {
        // Arrange
        var library = new MusicLibrary();
        var rockSong1 = new Song("Rock Song 1", "Artist 1", "Rock", 180, 2020);
        var rockSong2 = new Song("Rock Song 2", "Artist 2", "Rock", 200, 2021);
        var popSong = new Song("Pop Song", "Artist 3", "Pop", 150, 2019);

        library.AddSong(rockSong1);
        library.AddSong(rockSong2);
        library.AddSong(popSong);

        // Act
        var iterator = library.CreateGenreIterator("Rock");

        // Assert
        var songs = new List<Song>();
        while (iterator.HasNext())
        {
            songs.Add(iterator.Next());
        }

        Assert.Equal(2, songs.Count);
        Assert.Contains(rockSong1, songs);
        Assert.Contains(rockSong2, songs);
    }

    [Fact]
    public void CreateGenreIterator_WithNonExistingGenre_ShouldReturnEmptyIterator()
    {
        // Arrange
        var library = new MusicLibrary();
        var song = new Song("Test Song", "Test Artist", "Pop", 180, 2020);
        library.AddSong(song);

        // Act
        var iterator = library.CreateGenreIterator("Jazz");

        // Assert
        Assert.False(iterator.HasNext());
    }

    [Fact]
    public void CreateGenreIterator_EmptyLibrary_ShouldReturnEmptyIterator()
    {
        // Arrange
        var library = new MusicLibrary();

        // Act
        var iterator = library.CreateGenreIterator("Rock");

        // Assert
        Assert.False(iterator.HasNext());
    }

    [Fact]
    public void CreateArtistIterator_WithExistingArtist_ShouldReturnSongsOfThatArtist()
    {
        // Arrange
        var library = new MusicLibrary();
        var queenSong1 = new Song("Bohemian Rhapsody", "Queen", "Rock", 354, 1975);
        var queenSong2 = new Song("We Will Rock You", "Queen", "Rock", 122, 1977);
        var beatlesSong = new Song("Hey Jude", "Beatles", "Pop", 431, 1968);

        library.AddSong(queenSong1);
        library.AddSong(queenSong2);
        library.AddSong(beatlesSong);

        // Act
        var iterator = library.CreateArtistIterator("Queen");

        // Assert
        var songs = new List<Song>();
        while (iterator.HasNext())
        {
            songs.Add(iterator.Next());
        }

        Assert.Equal(2, songs.Count);
        Assert.Contains(queenSong1, songs);
        Assert.Contains(queenSong2, songs);
    }

    [Fact]
    public void CreateArtistIterator_WithNonExistingArtist_ShouldReturnEmptyIterator()
    {
        // Arrange
        var library = new MusicLibrary();
        var song = new Song("Test Song", "Test Artist", "Pop", 180, 2020);
        library.AddSong(song);

        // Act
        var iterator = library.CreateArtistIterator("Non-Existing Artist");

        // Assert
        Assert.False(iterator.HasNext());
    }

    [Fact]
    public void CreateArtistIterator_EmptyLibrary_ShouldReturnEmptyIterator()
    {
        // Arrange
        var library = new MusicLibrary();

        // Act
        var iterator = library.CreateArtistIterator("Queen");

        // Assert
        Assert.False(iterator.HasNext());
    }

    [Fact]
    public void MusicLibrary_ShouldImplementISongCollection()
    {
        // Arrange
        var library = new MusicLibrary();

        // Assert
        Assert.IsAssignableFrom<ISongCollection>(library);
    }

    [Fact]
    public void AddSong_WithMultipleGenres_ShouldOrganizeCorrectly()
    {
        // Arrange
        var library = new MusicLibrary();
        var rockSong = new Song("Rock Song", "Artist 1", "Rock", 180, 2020);
        var popSong = new Song("Pop Song", "Artist 2", "Pop", 200, 2021);
        var jazzSong = new Song("Jazz Song", "Artist 3", "Jazz", 150, 2019);

        // Act
        library.AddSong(rockSong);
        library.AddSong(popSong);
        library.AddSong(jazzSong);

        // Assert
        var rockIterator = library.CreateGenreIterator("Rock");
        var popIterator = library.CreateGenreIterator("Pop");
        var jazzIterator = library.CreateGenreIterator("Jazz");

        Assert.True(rockIterator.HasNext());
        Assert.Equal(rockSong, rockIterator.Next());

        Assert.True(popIterator.HasNext());
        Assert.Equal(popSong, popIterator.Next());

        Assert.True(jazzIterator.HasNext());
        Assert.Equal(jazzSong, jazzIterator.Next());
    }

    [Fact]
    public void AddSong_WithMultipleArtists_ShouldOrganizeCorrectly()
    {
        // Arrange
        var library = new MusicLibrary();
        var queenSong = new Song("Bohemian Rhapsody", "Queen", "Rock", 354, 1975);
        var beatlesSong = new Song("Hey Jude", "Beatles", "Pop", 431, 1968);
        var pinkFloydSong = new Song("Comfortably Numb", "Pink Floyd", "Rock", 382, 1979);

        // Act
        library.AddSong(queenSong);
        library.AddSong(beatlesSong);
        library.AddSong(pinkFloydSong);

        // Assert
        var queenIterator = library.CreateArtistIterator("Queen");
        var beatlesIterator = library.CreateArtistIterator("Beatles");
        var pinkFloydIterator = library.CreateArtistIterator("Pink Floyd");

        Assert.True(queenIterator.HasNext());
        Assert.Equal(queenSong, queenIterator.Next());

        Assert.True(beatlesIterator.HasNext());
        Assert.Equal(beatlesSong, beatlesIterator.Next());

        Assert.True(pinkFloydIterator.HasNext());
        Assert.Equal(pinkFloydSong, pinkFloydIterator.Next());
    }

    [Fact]
    public void MusicLibrary_WithLargeDataset_ShouldHandleEfficiently()
    {
        // Arrange
        var library = new MusicLibrary();
        var genres = new[] { "Rock", "Pop", "Jazz", "Classical", "Electronic" };
        var artists = new[] { "Artist A", "Artist B", "Artist C", "Artist D", "Artist E" };

        // Act - Add 100 songs
        for (int i = 0; i < 100; i++)
        {
            var song = new Song(
                $"Song {i}",
                artists[i % artists.Length],
                genres[i % genres.Length],
                180 + (i % 300),
                2000 + (i % 26)
            );
            library.AddSong(song);
        }

        // Assert
        var allSongsIterator = library.CreateSequentialIterator();
        var songs = new List<Song>();
        while (allSongsIterator.HasNext())
        {
            songs.Add(allSongsIterator.Next());
        }

        Assert.Equal(100, songs.Count);

        // Verify genre organization
        var rockIterator = library.CreateGenreIterator("Rock");
        var rockSongs = new List<Song>();
        while (rockIterator.HasNext())
        {
            rockSongs.Add(rockIterator.Next());
        }
        Assert.Equal(20, rockSongs.Count); // 100 / 5 genres = 20 per genre

        // Verify artist organization
        var artistAIterator = library.CreateArtistIterator("Artist A");
        var artistASongs = new List<Song>();
        while (artistAIterator.HasNext())
        {
            artistASongs.Add(artistAIterator.Next());
        }
        Assert.Equal(20, artistASongs.Count); // 100 / 5 artists = 20 per artist
    }

    [Fact]
    public void CreateFilteredIterator_EmptyLibrary_ShouldReturnEmptyIterator()
    {
        // Arrange
        var library = new MusicLibrary();

        // Act
        var iterator = library.CreateFilteredIterator(_ => true);

        // Assert
        Assert.False(iterator.HasNext());
    }

    [Fact]
    public void MusicLibrary_DifferentIterators_ShouldWorkIndependently()
    {
        // Arrange
        var library = new MusicLibrary();
        var song1 = new Song("Song 1", "Queen", "Rock", 180, 2020);
        var song2 = new Song("Song 2", "Queen", "Rock", 200, 2021);
        var song3 = new Song("Song 3", "Beatles", "Pop", 150, 2019);

        library.AddSong(song1);
        library.AddSong(song2);
        library.AddSong(song3);

        // Act
        var genreIterator = library.CreateGenreIterator("Rock");
        var artistIterator = library.CreateArtistIterator("Queen");
        var filteredIterator = library.CreateFilteredIterator(s => s.Year > 2019);

        // Assert - All iterators work independently
        var genreSongs = new List<Song>();
        while (genreIterator.HasNext())
        {
            genreSongs.Add(genreIterator.Next());
        }
        Assert.Equal(2, genreSongs.Count);

        var artistSongs = new List<Song>();
        while (artistIterator.HasNext())
        {
            artistSongs.Add(artistIterator.Next());
        }
        Assert.Equal(2, artistSongs.Count);

        var filteredSongs = new List<Song>();
        while (filteredIterator.HasNext())
        {
            filteredSongs.Add(filteredIterator.Next());
        }
        Assert.Equal(2, filteredSongs.Count);
    }
}

