﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroBrowser.Crawler.Common.Interfaces;
using static ZeroBrowser.Crawler.Core.CrawlerContext;

namespace ZeroBrowser.Crawler.Core
{
    public class SQLiteRepository : IRepository
    {
        private readonly CrawlerContext _crawlerContext;

        public SQLiteRepository(CrawlerContext crawlerContext)
        {
            _crawlerContext = crawlerContext;
        }

        public async Task AddPages(List<string> pagesToCrawl)
        {
            await _crawlerContext.CrawledRecords.AddRangeAsync(pagesToCrawl.Select(createCrawledRecord));
            await _crawlerContext.SaveChangesAsync();
        }

        public async Task<bool> Exist(string url)
        {            
            var record = await getCrawledRecord(url);

            return false;
        }

        public async Task UpdateHttpStatusCode(string url, HttpStatusCode statusCode)
        {
            var record = await getCrawledRecord(url);

            record.HttpStatusCode = statusCode;

            await _crawlerContext.SaveChangesAsync();
        }

        private async Task<CrawledRecord> getCrawledRecord(string url)
        {
            var hashedUrl = url.CreateMD5();

            var record = await _crawlerContext.CrawledRecords.FirstOrDefaultAsync(a => a.HashedUrl == hashedUrl);

            return record;
        }

        private CrawledRecord createCrawledRecord(string url)
        {
            url = url.ToLower();

            return new CrawledRecord
            {
                Url = url,
                CrawlStatus = CrawlStatus.Pending,
                HashedUrl = url.CreateMD5(),
                HealthStatus = HealthStatus.Pending,
                HttpStatusCode = default,
                Inserted = DateTime.UtcNow,
                Updated = DateTime.UtcNow
            };
        }
    }
}
