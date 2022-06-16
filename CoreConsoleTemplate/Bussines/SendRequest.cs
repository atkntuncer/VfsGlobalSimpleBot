using AngleSharp.Html.Parser;
using PuppeteerSharp;
using PuppeteerSharp.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CoreConsoleTemplate.Bussines
{
    class SendRequest : ISendRequest
    {
        public async Task<bool> CheckAppointment()
        {
            int count = 0;
            int randomSleepNumber = 0;
            int retryCount = 0;
            Browser browser = await OpenBrowser();
            bool result = await GetContent(browser, count);
            while (retryCount != 3)
            {
                try
                {
                    while (!result)
                    {
                        count++;
                        Console.WriteLine(count);
                        await File.WriteAllTextAsync(@"C:\Users\Atakan\Documents\IP-rate limits.txt", count.ToString() + Environment.NewLine);
                        var rng = new Random();
                        randomSleepNumber = rng.Next(13000, 15000);
                        Thread.Sleep(randomSleepNumber * 60);
                        result = await GetContent(browser, count);
                        Thread.Sleep(10000);
                    }
                }
                catch (Exception)
                {
                    retryCount++;
                    if (retryCount == 3)
                    {
                        // send error sms
                    }
                   var pages=  browser.PagesAsync().Result;
                    pages[1].CloseAsync().Wait();
                    Thread.Sleep(60 * 1000);
                }
            }

            return result;
        }

        private async Task<Browser> OpenBrowser()
        {
            string[] args ={/*"--disable-web-security",*/
       "--disable-features=BlockInsecurePrivateNetworkRequests"
       /*"--disable-site-isolation-trials" */};
            Browser browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                IgnoreHTTPSErrors = true,
                Headless = false,
                Timeout = 0,
                ExecutablePath = "C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe",
                SlowMo = 10,
                Args = args
            });

            return browser;
        }

        private async Task<bool> GetContent(Browser browser, int count)
        {
            Page page = await browser.NewPageAsync();
            //await page.SetUserAgentAsync("Chrome/73.0.3683.75");
            await page.GoToAsync("https://visa.vfsglobal.com/tur/tr/pol/login");
            Thread.Sleep(7000);

            await page.SetCacheEnabledAsync(false);

            await page.WaitForSelectorAsync("input[formcontrolname='username']");
            if (count == 0)
            {
                await page.ClickAsync("button[id='onetrust-reject-all-handler']");

            }
            if (count % 2 == 0)
            {
                await page.TypeAsync("input[formcontrolname='username']", "vfstest@hotmail.com");
            }
            else
            {
                await page.TypeAsync("input[formcontrolname='username']", "testvfs@hotmail.com");
            }
            await page.TypeAsync("input[formcontrolname='password']", "Test1234!");
            await page.Keyboard.PressAsync("Tab");
            await page.Keyboard.PressAsync("Tab");
            await page.Keyboard.PressAsync("Enter");
            Thread.Sleep(7000);

            await page.Keyboard.PressAsync("Tab");
            await page.Keyboard.PressAsync("Enter");
            Thread.Sleep(4000);

            await page.ClickAsync("mat-select[id='mat-select-0']");
            Thread.Sleep(1000);
            await page.ClickAsync("mat-option[id=mat-option-2]");
            Thread.Sleep(4000);

            await page.ClickAsync("mat-select[id='mat-select-2']");
            Thread.Sleep(1000);
            await page.ClickAsync("mat-option[id=mat-option-6]");
            Thread.Sleep(4000);

            await page.ClickAsync("mat-select[id='mat-select-4']");
            Thread.Sleep(1000);
            await page.ClickAsync("mat-option[id=mat-option-9]");
            Thread.Sleep(5000);

            var content = await page.GetContentAsync();
            var parser = new HtmlParser();
            var document = await parser.ParseDocumentAsync(content);
            var result = document.QuerySelector(".alert.alert-info.border-0.rounded-0").InnerHtml;
            if (!result.Contains("En erken"))
            {
                await page.CloseAsync();
                return false;
            }
            await page.CloseAsync();
            return true;
        }
    }
}
