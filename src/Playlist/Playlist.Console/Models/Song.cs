namespace Playlist.Console.Models;

public class Song
{
    public string Title { get; }
    public string Artist { get; }
    public string Genre { get; }
    public int DurationSeconds { get; }
    public int Year { get; }

    public Song(string title, string artist, string genre, int durationSeconds, int year)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty.");

        if (string.IsNullOrWhiteSpace(artist))
            throw new ArgumentException("Artist cannot be empty.");

        if (string.IsNullOrWhiteSpace(genre))
            throw new ArgumentException("Genre cannot be empty.");

        if (durationSeconds <= 0)
            throw new ArgumentException("Duration must be greater than zero.");

        if (year <= 0)
            throw new ArgumentException("Invalid year.");

        Title = title;
        Artist = artist;
        Genre = genre;
        DurationSeconds = durationSeconds;
        Year = year;
    }

    public string GetFormattedDuration()
    {
        var minutes = DurationSeconds / 60;
        var seconds = DurationSeconds % 60;
        return $"{minutes:D2}:{seconds:D2}";
    }

    public override string ToString()
    {
        return $"{Title} - {Artist} ({Genre}, {Year}) [{GetFormattedDuration()}]";
    }
}