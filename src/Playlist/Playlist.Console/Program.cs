using Playlist.Console.Collections;
using Playlist.Console.Models;
using Playlist.Console.Player;

var library = new MusicLibrary();

library.AddSong(new Song("Bohemian Rhapsody", "Queen", "Rock", 354, 1975));
library.AddSong(new Song("Imagine", "John Lennon", "Pop", 183, 1971));
library.AddSong(new Song("Smells Like Teen Spirit", "Nirvana", "Rock", 301, 1991));
library.AddSong(new Song("Billie Jean", "Michael Jackson", "Pop", 294, 1982));

var player = new MusicPlayer();

// 🔹 Todas as músicas da biblioteca
player.Play("Biblioteca - Todas",
    library.CreateSequentialIterator());

// 🔹 Apenas Rock
player.Play("Biblioteca - Rock",
    library.CreateGenreIterator("Rock"));

// 🔹 Apenas Queen
player.Play("Biblioteca - Queen",
    library.CreateArtistIterator("Queen"));