﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ZeroBrowser.Crawler.Common.Interfaces;

namespace ZeroBrowser.Crawler.Api.HostedService
{
    public class ParallelCrawlerHostedService : IHostedService
    {
        private readonly ILogger _logger;
        private int _maxNumOfParallelOperations = 10;
        private int _executorsCount = 2;
        private IUrlChannel _urlChannel { get; }
        private IConfiguration _configuration;
        private readonly IHeadlessBrowserService _headlessBrowserService;
        private readonly IBackgroundUrlQueue _backgroundUrlQueue;

        public ParallelCrawlerHostedService(IUrlChannel urlChannel,
                                            ILoggerFactory loggerFactory,
                                            IHeadlessBrowserService headlessBrowserService,
                                            IConfiguration configuration,
                                            IBackgroundUrlQueue backgroundUrlQueue)
        {
            _urlChannel = urlChannel;
            _logger = loggerFactory.CreateLogger<QueuedHostedService>();
            _configuration = configuration;
            _headlessBrowserService = headlessBrowserService;
            _backgroundUrlQueue = backgroundUrlQueue;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"***** entered consumer.{Environment.NewLine}");

            for (var i = 0; i < _executorsCount; i++)
            {
                var executorTask = new Task(
                    async () =>
                    {
                        while (!cancellationToken.IsCancellationRequested)
                        {
                            await foreach (var url in await _urlChannel.Read())
                            {
                                var urls = await _headlessBrowserService.GetUrls(url, 0);

                                foreach (var newUrl in urls)
                                {
                                    _logger.LogInformation($"***** new url found {url}.{Environment.NewLine}");
                                    _backgroundUrlQueue.QueueUrlItem(newUrl.Url);
                                }
                            }
                        }
                    }, cancellationToken);

                executorTask.Start();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {

            return Task.CompletedTask;
        }
    }
}