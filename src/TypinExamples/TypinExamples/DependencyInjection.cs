﻿namespace TypinExamples
{
    using System;
    using System.Net.Http;
    using Blazor.Extensions.Storage;
    using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Serilog;
    using TypinExamples.Application.Services.TypinWeb;
    using TypinExamples.Common.Extensions;
    using TypinExamples.Configuration;
    using TypinExamples.Infrastructure.WebWorkers.Core;
    using TypinExamples.Services;
    using TypinExamples.Services.Terminal;
    using TypinExamples.Shared;

    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration, IWebAssemblyHostEnvironment environment)
        {
            services.AddOptions();
            services.AddStorage();

            services.AddWebWorkers()
                    .RegisterCommandHandler<TestCommand, TestCommand.Handler>()
                    .RegisterNotificationHandler<TestNotification, TestNotification.Handler>();

            services.AddLogging(builder => builder.AddSerilog(dispose: true)
                                                  .SetMinimumLevel(environment.IsDevelopment() ? LogLevel.Trace : LogLevel.Information));

            services.AddConfiguration<ApplicationSettings>(configuration)
                    .AddConfiguration<HeaderSettings>(configuration)
                    .AddConfiguration<FooterSettings>(configuration);

            services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(environment.BaseAddress) })
                    .AddScoped<IMarkdownService, MarkdownService>()
                    .AddScoped<ITerminalRepository, TerminalRepository>()
                    .AddScoped<MonacoEditorService>();

            return services;
        }
    }
}
