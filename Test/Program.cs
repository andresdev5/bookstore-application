// See https://aka.ms/new-console-template for more information
using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, World!");

var repo = RepositoryFactory.CreateRepository();
var authors = repo.GetAll<Author>();

foreach (var author in authors)
{
    Console.WriteLine($"{author.Firstname} {author.Lastname}");
}

var books = repo.GetAllWith<Book>(["Author", "Genres"]);

foreach (var book in books)
{
    Console.WriteLine($"{book.Id} | {book.Title} by {book.Author?.Firstname} {book.Author?.Lastname}");

    Console.WriteLine($"total genres: {book.Genres.Count}");

    foreach (var genre in book.Genres)
    {
        Console.WriteLine($"  {genre.Name}");
    }
}