﻿namespace BookLibraryExample.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using BookLibraryExample.Models;
    using BookLibraryExample.Services;
    using Typin.Commands;
    using Typin.Commands.Attributes;
    using Typin.Console;
    using Typin.Models.Attributes;

    [Command("book remove", Description = "Remove a book from the library.")]
    public class BookRemoveCommand : ICommand
    {
        private readonly LibraryService _libraryService;
        private readonly IConsole _console;

        [Parameter(0, Name = "title", Description = "Book title.")]
        public string Title { get; init; } = "";

        public BookRemoveCommand(LibraryService libraryService, IConsole console)
        {
            _libraryService = libraryService;
            _console = console;
        }

        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            Book? book = _libraryService.GetBook(Title);

            _ = book ?? throw new Exception("Book not found.");

            _libraryService.RemoveBook(book);

            _console.Output.WriteLine($"Book {Title} removed.");

            return default;
        }
    }
}