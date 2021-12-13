﻿namespace Typin.Tests.Data.Modes.Valid
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Console;

    public class ValidCustomMode : ICliMode
    {
        private readonly ICommandExecutor _commandExecutor;
        private readonly IConsole _console;

        public ValidCustomMode(ICommandExecutor commandExecutor, IConsole console)
        {
            _commandExecutor = commandExecutor;
            _console = console;
        }

        public Task<int> ExecuteAsync(CancellationToken cancellationToken)
        {
            _console.Output.WriteLine(nameof(ValidCustomMode));

            return Task.FromResult(0);
        }
    }
}
