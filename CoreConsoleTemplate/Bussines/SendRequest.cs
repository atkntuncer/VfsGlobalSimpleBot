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
            Browser browser = await OpenBrowser();

            bool result = await GetContent(browser);

            while (!result)
            {
                count++;
                Console.WriteLine(count);
                await File.WriteAllTextAsync(@"C:\Users\Atakan\Documents\IP-rate limits.txt", count.ToString() + Environment.NewLine);
                Thread.Sleep(5 *60 * 1000);
                result = await GetContent(browser);
                Thread.Sleep(10000);
            }
            return true;
        }

        private async Task<Browser> OpenBrowser()
        {
            Browser browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                IgnoreHTTPSErrors = true,
                Headless = false,
                Timeout = 0,
                ExecutablePath = "C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe",
                SlowMo = 10
            });

            return browser;
        }

        private async Task<bool> GetContent(Browser browser)
        {
            // Create a new page and go to Bing Maps
            Page page = await browser.NewPageAsync();
            //await page.SetUserAgentAsync("Chrome/73.0.3683.75");
            await page.GoToAsync("https://visa.vfsglobal.com/tur/tr/pol/login");
            Thread.Sleep(7000);

            await page.SetCacheEnabledAsync(false);

            await page.WaitForSelectorAsync("input[formcontrolname='username']");
            await page.TypeAsync("input[formcontrolname='username']", "****@hotmail.com");
            await page.TypeAsync("input[formcontrolname='password']", "****");
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
            if (result.Contains("bulunmamaktadır"))
            {
                await page.CloseAsync();
                return false;
            }
            await page.CloseAsync();
            return true;
        }
    }
}
