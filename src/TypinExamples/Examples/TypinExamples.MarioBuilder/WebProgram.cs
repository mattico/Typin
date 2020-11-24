﻿namespace TypinExamples.MarioBuilder
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Directives;
    using Typin.Modes;
    using TypinExamples.Infrastructure.TypinWeb.Configuration;
    using TypinExamples.TypinWeb.Extensions;

    public static class WebProgram
    {
        public static async Task<int> WebMain(WebCliConfiguration configuration, string commandLine, IReadOnlyDictionary<string, string> environmentVariables)
        {
            return await new CliApplicationBuilder().AddCommandsFromThisAssembly()
                                                    .AddDirective<PreviewDirective>()
                                                    .UseInteractiveMode()
                                                    .UseWebExample(configuration)
                                                    .Build()
                                                    .RunAsync(commandLine, environmentVariables, true);
        }
    }
}
