using AngleSharp.Html.Parser;
using CoreConsoleTemplate.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PuppeteerSharp;
using PuppeteerSharp.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;

namespace CoreConsoleTemplate.Bussines
{
    class SendRequest : ISendRequest
    {
        private readonly Configuration _config;

        public SendRequest(IOptions<Configuration> config)
        {
            _config = config.Value;
        }
        public async Task<bool> CheckAppointment()
        {
            int count = 0;
            int randomSleepNumber = 0;
            int retryCount = 0;
            bool result = false;

            var bot = new TelegramBotClient("5529977162:AAFIytAOZczhzRhiMFCAv3Vm0jh5_yumObs");
            Browser browser = await OpenBrowser();
            while (true)
            {
                try
                {
                    result = await GetContent(browser, count, bot);
                    count++;
                    var rng = new Random();
                    randomSleepNumber = rng.Next(12250, 14250);
                    Thread.Sleep(randomSleepNumber * 60);
                }
                catch (Exception ex)
                {
                    retryCount++;
                    await bot.SendTextMessageAsync("-612527851", $"Id: {_config.Id} - Hata alındı deneme : {retryCount} -- {ex.Message}");
                    var pages = browser.PagesAsync().Result;
                    pages[0].CloseAsync().Wait();
                    Thread.Sleep(60 * 1000 * 13);
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

        private async Task<bool> GetContent(Browser browser, int count, TelegramBotClient bot)
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
                await page.TypeAsync("input[formcontrolname='username']", $"{_config.FirstMail}");
            }
            else
            {
                await page.TypeAsync("input[formcontrolname='username']", $"{_config.SecondMail}");
            }
            await page.TypeAsync("input[formcontrolname='password']", $"{_config.Password}");
            await page.Keyboard.PressAsync("Tab");
            await page.Keyboard.PressAsync("Tab");
            await page.Keyboard.PressAsync("Enter");
            Thread.Sleep(7000);

            await page.Keyboard.PressAsync("Tab");
            await page.Keyboard.PressAsync("Enter");
            Thread.Sleep(4000);

            await page.ClickAsync("mat-select[id='mat-select-0']");
            Thread.Sleep(2000);
            await page.ClickAsync("mat-option[id=mat-option-2]");
            Thread.Sleep(4000);

            await page.ClickAsync("mat-select[id='mat-select-2']");
            Thread.Sleep(2000);
            await page.ClickAsync("mat-option[id=mat-option-6]");
            Thread.Sleep(4000);

            await page.ClickAsync("mat-select[id='mat-select-4']");
            Thread.Sleep(2000);
            await page.ClickAsync("mat-option[id=mat-option-9]");
            Thread.Sleep(5000);

            var content = await page.GetContentAsync();
            var parser = new HtmlParser();
            var document = await parser.ParseDocumentAsync(content);
            var result = document.QuerySelector(".alert.alert-info.border-0.rounded-0").InnerHtml;
            if (!result.Contains("erken"))
            {
                await page.CloseAsync();
                return false;
            }
            await bot.SendTextMessageAsync("-612527851", "Id: {_config.Id} -" + result);
            await page.CloseAsync();
            return true;
        }
    }
}
