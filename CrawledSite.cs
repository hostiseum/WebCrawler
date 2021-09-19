﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler
{
    public class CrawledSite
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public bool Crawled { get; set; }
        public string ErrorIfNotCrawled { get; set; }
    }
}
