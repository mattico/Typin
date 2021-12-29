﻿namespace Typin.Tests.Data.Valid.DefaultCommands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Commands;
    using Typin.Commands.Attributes;
    using Typin.Console;

    [Command(Description = "Default command description")]
    public class DefaultCommand : ICommand
    {
        public const string ExpectedOutputText = nameof(DefaultCommand);

        private readonly IConsole _console;

        public DefaultCommand(IConsole console)
        {
            _console = console;
        }

        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            _console.Output.WriteLine(ExpectedOutputText);

            return default;
        }
    }
}