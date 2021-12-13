﻿namespace Typin.Features
{
    using System;
    using Typin;

    /// <summary>
    /// <see cref="ICliModeFeature"/> implementation.
    /// </summary>
    internal sealed class CliModeFeature : ICliModeFeature
    {
        /// <inheritdoc/>
        public Type Type { get; }

        /// <inheritdoc/>
        public ICliMode Instance { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="CliModeFeature"/>.
        /// </summary>
        public CliModeFeature(ICliMode instance)
        {
            Type = instance.GetType();
            Instance = instance;
        }
    }
}
