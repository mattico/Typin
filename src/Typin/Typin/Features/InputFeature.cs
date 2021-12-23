﻿namespace Typin.Features
{
    using System.Collections.Generic;
    using Typin;
    using Typin.Features.Input;

    /// <summary>
    /// <see cref="IInputFeature"/> implementation.
    /// </summary>
    internal sealed class InputFeature : IInputFeature
    {
        /// <inheritdoc/>
        public IEnumerable<string> Arguments { get; }

        /// <inheritdoc/>
        public CommandExecutionOptions ExecutionOptions { get; }

        /// <inheritdoc/>
        public ParsedInput? Parsed { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="InputFeature"/>.
        /// </summary>
        public InputFeature(IEnumerable<string> arguments,
                            CommandExecutionOptions executionOptions)
        {
            Arguments = arguments;
            ExecutionOptions = executionOptions;
        }

        /// <inheritdoc/>
        public override string? ToString()
        {
            return base.ToString() +
                " | " +
                $"{nameof(Arguments)} = [\"{string.Join("\", ", Arguments)}\"], " +
                $"{nameof(ExecutionOptions)} = {ExecutionOptions}";
        }
    }
}
