﻿namespace Typin
{
    using System;
    using Typin.Features;
    using Typin.Modes.Features;

    /// <summary>
    /// Encapsulates all CLI-specific information about a command.
    /// </summary>
    public sealed class DefaultCliContext : CliContext
    {
        /// <inheritdoc/>
        public override IFeatureCollection Features { get; }

        /// <inheritdoc/>
        public override ICallInfoFeature Call => Features.Get<ICallInfoFeature>() ??
            throw new InvalidOperationException("Call has not been configured for this application or call.");

        /// <inheritdoc/>
        public override IServiceProvider Services => Features.Get<ICallServicesFeature>()?.CallServices ??
            throw new InvalidOperationException("Call has not been configured for this application or call.");

        /// <inheritdoc/>
        public override ICallLifetimeFeature Lifetime => Features.Get<ICallLifetimeFeature>() ??
            throw new InvalidOperationException("Lifetime has not been configured for this application or call.");

        /// <inheritdoc/>
        public override IInputFeature Input => Features.Get<IInputFeature>() ??
            throw new InvalidOperationException("Input has not been configured for this application or call.");

        /// <inheritdoc/>
        public override ITokenizerFeature Tokenizer => Features.Get<ITokenizerFeature>() ??
            throw new InvalidOperationException("Tokenizer has not been configured for this application or call.");

        /// <inheritdoc/>
        public override IBinderFeature Binder => Features.Get<IBinderFeature>() ??
            throw new InvalidOperationException("Binder has not been configured for this application or call.");

        /// <inheritdoc/>
        public override IOutputFeature Output => Features.Get<IOutputFeature>() ??
            throw new InvalidOperationException("Output has not been configured for this application or call.");

        /// <summary>
        /// Initializes an instance of <see cref="CliContext"/>.
        /// </summary>
        public DefaultCliContext() :
            base()
        {
            Features = new FeatureCollection();
        }

        /// <summary>
        /// Initializes an instance of <see cref="CliContext"/>.
        /// </summary>
        public DefaultCliContext(ICallServicesFeature callServicesFeature,
                                 CliContext? parentCliContext,
                                 ICliModeFeature cliModeFeature,
                                 ICallLifetimeFeature callLifetimeFeature,
                                 IInputFeature inputFeature,
                                 IOutputFeature outputFeature) :
            base()
        {
            ICallInfoFeature callInfoFeature = new CallInfoFeature(Guid.NewGuid(), this, parentCliContext);

            Features = new FeatureCollection
            {
                [typeof(ICallServicesFeature)] = callServicesFeature,
                [typeof(ICallInfoFeature)] = callInfoFeature,
                [typeof(ICliModeFeature)] = cliModeFeature,
                [typeof(ICallLifetimeFeature)] = callLifetimeFeature,
                [typeof(IInputFeature)] = inputFeature,
                [typeof(IOutputFeature)] = outputFeature,
            };
        }
    }
}
