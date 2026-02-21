using Playlist.Console.Models;
using Playlist.Console.Player;

Console.WriteLine("=== Sistema de Playlist (Iterator Pattern) ===");

var playlist = new Playlist.Console.Collections.Playlist("Minhas Favoritas");

playlist.AddSong(new Song("Bohemian Rhapsody", "Queen", "Rock", 354, 1975));
playlist.AddSong(new Song("Imagine", "John Lennon", "Pop", 183, 1971));
playlist.AddSong(new Song("Smells Like Teen Spirit", "Nirvana", "Rock", 301, 1991));
playlist.AddSong(new Song("Billie Jean", "Michael Jackson", "Pop", 294, 1982));
playlist.AddSong(new Song("Hotel California", "Eagles", "Rock", 391, 1976));
playlist.AddSong(new Song("Sweet Child O' Mine", "Guns N' Roses", "Rock", 356, 1987));

var player = new MusicPlayer();

// Iteração Sequencial
player.Play("Tocando Sequencial", 
    playlist.CreateSequentialIterator());

// Iteração Aleatória
player.Play("Tocando Aleatório",
    playlist.CreateShuffleIterator());

// Filtro por gênero
player.Play("Tocando Rock",
    playlist.CreateFilteredIterator(s => s.Genre == "Rock"));

// Antigas
player.Play("Tocando Antigas",
    playlist.CreateFilteredIterator(s => s.Year < 1980));

Console.WriteLine("\n=== BENEFÍCIOS ALCANÇADOS ===");
Console.WriteLine("✓ Estrutura interna encapsulada");
Console.WriteLine("✓ Interface uniforme de iteração");
Console.WriteLine("✓ Fácil adicionar novos iteradores");
Console.WriteLine("✓ Cliente desacoplado da coleção");
Console.WriteLine("✓ Múltiplas iterações independentes");