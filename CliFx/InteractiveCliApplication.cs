﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CliFx.Domain;
using CliFx.Domain.Input;

namespace CliFx
{
    /// <summary>
    /// Command line application facade.
    /// </summary>
    public partial class InteractiveCliApplication : CliApplication
    {
        private readonly ConsoleColor _promptForeground;
        private readonly ConsoleColor _commandForeground;

        /// <summary>
        /// Initializes an instance of <see cref="InteractiveCliApplication"/>.
        /// </summary>
        public InteractiveCliApplication(IServiceProvider serviceProvider,
                                         CliContext cliContext,
                                         ConsoleColor promptForeground,
                                         ConsoleColor commandForeground) :
            base(serviceProvider, cliContext)
        {
            _promptForeground = promptForeground;
            _commandForeground = commandForeground;
        }

        /// <inheritdoc/>
        protected override async Task<int> PreExecuteCommand(IReadOnlyList<string> commandLineArguments,
                                                             IReadOnlyDictionary<string, string> environmentVariables,
                                                             RootSchema root)
        {
            var input = CommandInput.Parse(commandLineArguments, root.GetCommandNames());
            CliContext.CurrentInput = input;

            if (input.HasDirective(StandardDirectives.Interactive))
            {
                CliContext.IsInteractiveMode = true;

                // we don't want to run default command for e.g. `[interactive]`
                if (!string.IsNullOrWhiteSpace(input.CommandName))
                    await ExecuteCommand(environmentVariables, root, input);

                await RunInteractivelyAsync(environmentVariables, root);
            }

            return await ExecuteCommand(environmentVariables, root, input);
        }

        /// <inheritdoc/>
        protected override async ValueTask<int?> ProcessHardcodedDirectives(ApplicationConfiguration configuration, CommandInput input)
        {
            if (await base.ProcessHardcodedDirectives(configuration, input) is int exitCode)
                return exitCode;

            //// Scope up
            //if (input.HasDirective(StandardDirectives.ScopeUp))
            //{
            //    string[] splittedScope = CliContext.Scope.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            //    if (splittedScope.Length > 1)
            //        CliContext.Scope = string.Join(" ", splittedScope, 0, splittedScope.Length - 1);
            //    else if (splittedScope.Length == 1)
            //        CliContext.Scope = string.Empty;

            //    return ExitCode.Success;
            //}

            return null;
        }

        private async Task RunInteractivelyAsync(IReadOnlyDictionary<string, string> environmentVariables,
                                                 RootSchema root)
        {
            IConsole console = CliContext.Console;
            string executableName = CliContext.Metadata.ExecutableName;

            //TODO: Add behaviours like in mediatr
            while (true) //TODO maybe add CliContext.Exit and CliContext.Status
            {
                string[] commandLineArguments = GetInput(console, executableName);
                var input = CommandInput.Parse(commandLineArguments, root.GetCommandNames());
                CliContext.CurrentInput = input; //TODO maybe refactor with some clever IDisposable class

                await ExecuteCommand(environmentVariables, root, input);
                console.ResetColor();
            }
        }

        private string[] GetInput(IConsole _console, string executableName)
        {
            string[] arguments;
            string line = string.Empty;
            do
            {
                // Print prompt
                _console.WithForegroundColor(_promptForeground, () =>
                {
                    _console.Output.Write(executableName);
                });

                if (!string.IsNullOrWhiteSpace(CliContext.Scope))
                {
                    _console.WithForegroundColor(ConsoleColor.Cyan, () =>
                    {
                        _console.Output.Write(' ');
                        _console.Output.Write(CliContext.Scope);
                    });
                }

                _console.WithForegroundColor(_promptForeground, () =>
                {
                    _console.Output.Write("> ");
                });

                // Read user input
                _console.WithForegroundColor(_commandForeground, () =>
                {
                    line = _console.Input.ReadLine();
                });

                // handle default directive
                // TODO: fix for `[default] [debug]` etc.
                if (line.StartsWith(StandardDirectives.Default))
                    return Array.Empty<string>();

                if (string.IsNullOrWhiteSpace(CliContext.Scope)) // handle unscoped command input
                {
                    arguments = line.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                    .ToArray();
                }
                else // handle scoped command input
                {
                    var tmp = line.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                  .ToList();

                    int lastDirective = tmp.FindLastIndex(x => x.StartsWith('[') && x.EndsWith(']'));
                    tmp.Insert(lastDirective + 1, CliContext.Scope);

                    arguments = tmp.ToArray();
                }

            } while (string.IsNullOrWhiteSpace(line)); // retry on empty line

            return arguments;
        }
    }
}
