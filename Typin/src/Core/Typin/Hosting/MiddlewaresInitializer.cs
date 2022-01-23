﻿namespace Typin.Hosting
{
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin.Directives.Pipeline;
    using Typin.Pipeline;

    /// <summary>
    /// Initializes middlewares.
    /// </summary>
    internal sealed class MiddlewaresInitializer : IPipelineInitializer
    {
        /// <inheritdoc/>
        public ValueTask RegisterAsync(IPipelineCollection pipelines, CancellationToken cancellationToken)
        {
            _ = PipelineBuilder.Create<CliContext>()
                 .Lifetime(InvokablePipelineLifetime.Singleton)
                 .Add<TypinExceptionsHandler>()
                 .Add<InitializeTokenizer>()
                 .Add<TokenizeInput>()
                 .Add<InitializeBinder>()
                 .Add<InitializeDirectives>()
                 .Add<BindInput>()
                 .Add<HandleDirectives>()
                 .Build().TryAddTo(pipelines);

            return default;
        }
    }
}
