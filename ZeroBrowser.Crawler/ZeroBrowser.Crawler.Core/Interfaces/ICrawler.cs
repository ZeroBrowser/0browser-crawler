﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZeroBrowser.Crawler.Common.Models;

namespace ZeroBrowser.Crawler.Core.Interfaces
{
    public interface ICrawler
    {
        Task Crawl(CrawlerContext crawlerContext);
    }
}
