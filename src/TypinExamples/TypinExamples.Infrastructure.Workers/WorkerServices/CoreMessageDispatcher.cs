﻿namespace TypinExamples.Infrastructure.Workers.WorkerServices
{
    using System.Threading.Tasks;
    using BlazorWorker.WorkerCore;
    using Newtonsoft.Json;
    using TypinExamples.Application.Services;
    using TypinExamples.Domain.Models;

    public class CoreMessageDispatcher : ICoreMessageDispatcher
    {
        private readonly IWorkerMessageService _workerMessageService;

        public CoreMessageDispatcher(IWorkerMessageService workerMessageService)
        {
            _workerMessageService = workerMessageService;
        }

        public async Task DispatchAsync(WorkerMessage model)
        {
            string serializedModel = JsonConvert.SerializeObject(model);
            await _workerMessageService.PostMessageAsync(serializedModel);
        }
    }
}
