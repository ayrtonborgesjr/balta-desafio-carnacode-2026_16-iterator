using Playlist.Console.Models;

namespace Playlist.Tests.Models;

public class SongTests
{
    [Fact]
    public void Constructor_WithValidParameters_ShouldCreateSong()
    {
        // Arrange & Act
        var song = new Song("Bohemian Rhapsody", "Queen", "Rock", 354, 1975);

        // Assert
        Assert.Equal("Bohemian Rhapsody", song.Title);
        Assert.Equal("Queen", song.Artist);
        Assert.Equal("Rock", song.Genre);
        Assert.Equal(354, song.DurationSeconds);
        Assert.Equal(1975, song.Year);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithInvalidTitle_ShouldThrowArgumentException(string title)
    {
        // Arrange & Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => 
            new Song(title, "Queen", "Rock", 354, 1975));
        Assert.Equal("Title cannot be empty.", exception.Message);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithInvalidArtist_ShouldThrowArgumentException(string artist)
    {
        // Arrange & Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => 
            new Song("Bohemian Rhapsody", artist, "Rock", 354, 1975));
        Assert.Equal("Artist cannot be empty.", exception.Message);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithInvalidGenre_ShouldThrowArgumentException(string genre)
    {
        // Arrange & Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => 
            new Song("Bohemian Rhapsody", "Queen", genre, 354, 1975));
        Assert.Equal("Genre cannot be empty.", exception.Message);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Constructor_WithInvalidDuration_ShouldThrowArgumentException(int duration)
    {
        // Arrange & Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => 
            new Song("Bohemian Rhapsody", "Queen", "Rock", duration, 1975));
        Assert.Equal("Duration must be greater than zero.", exception.Message);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-1975)]
    public void Constructor_WithInvalidYear_ShouldThrowArgumentException(int year)
    {
        // Arrange & Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => 
            new Song("Bohemian Rhapsody", "Queen", "Rock", 354, year));
        Assert.Equal("Invalid year.", exception.Message);
    }

    [Theory]
    [InlineData(354, "05:54")]
    [InlineData(183, "03:03")]
    [InlineData(60, "01:00")]
    [InlineData(59, "00:59")]
    [InlineData(3661, "61:01")]
    public void GetFormattedDuration_ShouldReturnCorrectFormat(int seconds, string expected)
    {
        // Arrange
        var song = new Song("Test", "Artist", "Genre", seconds, 2020);

        // Act
        var result = song.GetFormattedDuration();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToString_ShouldReturnFormattedString()
    {
        // Arrange
        var song = new Song("Bohemian Rhapsody", "Queen", "Rock", 354, 1975);

        // Act
        var result = song.ToString();

        // Assert
        Assert.Equal("Bohemian Rhapsody - Queen (Rock, 1975) [05:54]", result);
    }

    [Fact]
    public void Song_Properties_ShouldBeReadOnly()
    {
        // Arrange
        var song = new Song("Test", "Artist", "Genre", 180, 2020);

        // Assert - Properties should not have setters (get only)
        Assert.False(song.GetType().GetProperty("Title")!.CanWrite);
        Assert.False(song.GetType().GetProperty("Artist")!.CanWrite);
        Assert.False(song.GetType().GetProperty("Genre")!.CanWrite);
        Assert.False(song.GetType().GetProperty("DurationSeconds")!.CanWrite);
        Assert.False(song.GetType().GetProperty("Year")!.CanWrite);
    }
}

