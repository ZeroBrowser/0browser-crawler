using System;
using System.Threading.Tasks;
using Xunit;
using ZeroBrowser.Crawler.Core;

namespace ZeroBrowser.Crawler.Test
{
    public class CrawlerTest
    {
        [Fact]
        public async Task CheckSeedUrl_Null_ThowException_Test()
        {
            //Given            
            var headlessBrowserUrl = "http://test";

            //When
            var crawler = new Core.Crawler(null, headlessBrowserUrl);
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => crawler.Crawl());

            Assert.Equal("The SeedUrls field is required. (Parameter 'SeedUrls')", ex.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task CheckSeedUrl_Empty_ThowException_Test(string seedUrl)
        {
            //Given            
            var headlessBrowserUrl = "http://test";

            //When
            var crawler = new Core.Crawler(new[] { seedUrl }, headlessBrowserUrl);
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => crawler.Crawl());

            Assert.Equal("Null/Empty (Parameter 'SeedUrls')", ex.Message);
        }


        [Theory]
        [InlineData("http://www..com", "Invalid uri")]
        [InlineData("httpp://www.", "Invalid scheme")]
        [InlineData("httpp://", "Invalid scheme")]
        [InlineData(".com", "Invalid uri")]
        [InlineData("abc", "Invalid uri")]
        [InlineData("ftp://url", "Invalid scheme")]
        [InlineData("wss://url", "Invalid scheme")]
        public async Task CheckSeedUrl_ValidURL_ThowException_Test(string seedUrl, string expectedError)
        {
            //Given            
            var headlessBrowserUrl = "http://test";

            //When
            var crawler = new Core.Crawler(new[] { seedUrl }, headlessBrowserUrl);
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => crawler.Crawl());

            Assert.Equal($"{expectedError} (Parameter 'SeedUrls')", ex.Message);
        }


        [Fact]
        public async Task CheckHeadlessBrowserUrl_Null_ThowException_Test()
        {
            //Given            
            var seedUrls = new[] { "http://test" };

            //When
            var crawler = new Core.Crawler(seedUrls, null);
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => crawler.Crawl());

            //Then
            Assert.Equal("The HeadlessBrowserUrl field is required. (Parameter 'HeadlessBrowserUrl')", ex.Message);
        }

    }
}