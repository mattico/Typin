﻿namespace Typin.Directives
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Features.Input;

    /// <summary>
    /// When preview mode is specified (using the [preview] directive), the app will short-circuit by printing consumed command line arguments as they were parsed.
    /// This is useful when troubleshooting issues related to command routing and argument binding.
    /// </summary>
    [Directive(BuiltInDirectives.Preview, Description = "The app will short-circuit by printing consumed command line arguments as they were parsed.")]
    public sealed class PreviewDirective : IDirective //TODO: add directive hadnler
    {
        private readonly IConsole _console;

        /// <summary>
        /// Initializes a new instance of <see cref="PreviewDirective"/>.
        /// </summary>
        public PreviewDirective(IConsole console)
        {
            _console = console;
        }

        /// <inheritdoc/>
        public ValueTask ExecuteAsync(CliContext args, StepDelegate next, IInvokablePipeline<CliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            WriteCommandLineInput(_console, args.Input.Parsed ?? throw new NullReferenceException("Input not set."));
            args.Output.ExitCode ??= ExitCode.Success;

            return default;
        }

        private static void WriteCommandLineInput(IConsole console, ParsedInput input)
        {
            // Directives
            foreach (DirectiveInput directive in input.Directives)
            {
                console.Output.WithForegroundColor(ConsoleColor.White, (output) => output.Write(directive.ToString()));

                console.Output.Write(' ');
            }

            // Command name
            if (!string.IsNullOrWhiteSpace(input.CommandName))
            {
                console.Output.WithForegroundColor(ConsoleColor.Cyan, (output) => output.Write(input.CommandName));

                console.Output.Write(' ');
            }

            // Parameters
            foreach (ParameterInput parameter in input.Parameters)
            {
                console.Output.Write('<');

                console.Output.WithForegroundColor(ConsoleColor.White, (output) => output.Write(parameter));

                console.Output.Write('>');
                console.Output.Write(' ');
            }

            // Options
            foreach (OptionInput option in input.Options)
            {
                console.Output.Write('(');

                console.Output.WithForegroundColor(ConsoleColor.White, (output) =>
                {
                    // Alias
                    output.Write(option.GetRawAlias());

                    // Values
                    if (option.Values.Any())
                    {
                        output.Write(' ');
                        output.Write(option.GetRawValues());
                    }
                });

                console.Output.Write(')');
                console.Output.Write(' ');
            }

            console.Output.WriteLine();
        }
    }
}
