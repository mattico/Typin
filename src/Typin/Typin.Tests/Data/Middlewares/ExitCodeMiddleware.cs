﻿namespace Typin.Tests.Data.Middlewares
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Console;

    public sealed class ExitCodeMiddleware : IMiddleware
    {
        public const string ExpectedOutput = "Command finished succesfully.";

        public async Task HandleAsync(ICliContext context, CommandPipelineHandlerDelegate next, CancellationToken cancellationToken)
        {
            await next();

            bool isInteractive = false;// context.ModeSwitcher.Current == CliModes.Interactive;
            int? exitCode = context.ExitCode;

            if (context.ExitCode == 0)
                context.Console.WithForegroundColor(ConsoleColor.White, () =>
                    context.Console.Output.WriteLine($"{context.Metadata.ExecutableName}: {ExpectedOutput}."));
            else
                context.Console.WithForegroundColor(ConsoleColor.White, () =>
                    context.Console.Output.WriteLine($"{context.Metadata.ExecutableName}: Command finished with exit code ({exitCode})."));
        }
    }
}
