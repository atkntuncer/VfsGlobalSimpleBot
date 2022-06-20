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
        public async Task CheckAppointment()
        {
            int count = 0;
            int randomSleepNumber = 0;

            var bot = new TelegramBotClient("5529977162:AAFIytAOZczhzRhiMFCAv3Vm0jh5_yumObs");
            while (true)
            {
                var rng = new Random();
                randomSleepNumber = rng.Next(12250, 14250);

                Browser browser = await OpenBrowser();
                try
                {
                    count++;
                    await GetContent(browser, count, bot);
                    Thread.Sleep(randomSleepNumber * 60);
                }
                catch (Exception ex)
                {
                    await bot.SendTextMessageAsync("-612527851", $"Id: {_config.Id} - Hata alındı! -- {ex.Message}");
                    await browser.CloseAsync();
                    Thread.Sleep(60 * randomSleepNumber);
                }
            }
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
                ExecutablePath = _config.BrowserExe,
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
            Thread.Sleep(7000 * _config.Multiplier);

            await page.SetCacheEnabledAsync(false);

            await page.WaitForSelectorAsync("input[formcontrolname='username']");

            await page.WaitForSelectorAsync("button[id='onetrust-reject-all-handler']");
            await page.ClickAsync("button[id='onetrust-reject-all-handler']");

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
            Thread.Sleep(10000 * _config.Multiplier);

            await page.Keyboard.PressAsync("Tab");
            await page.Keyboard.PressAsync("Enter");
            Thread.Sleep(4000 * _config.Multiplier);

            await page.ClickAsync("mat-select[id='mat-select-0']");
            Thread.Sleep(500 * _config.Multiplier);
            await page.ClickAsync("mat-option[id=mat-option-2]");
            Thread.Sleep(4000 * _config.Multiplier);

            await page.ClickAsync("mat-select[id='mat-select-2']");
            Thread.Sleep(500 * _config.Multiplier);
            await page.ClickAsync("mat-option[id=mat-option-6]");
            Thread.Sleep(4000 * _config.Multiplier);

            await page.ClickAsync("mat-select[id='mat-select-4']");
            Thread.Sleep(500 * _config.Multiplier);
            await page.ClickAsync("mat-option[id=mat-option-9]");
            Thread.Sleep(5000 * _config.Multiplier);

            var content = await page.GetContentAsync();
            var parser = new HtmlParser();
            var document = await parser.ParseDocumentAsync(content);
            var result = document.QuerySelector(".alert.alert-info.border-0.rounded-0").InnerHtml;

            await page.WaitForSelectorAsync(".alert.alert-info.border-0.rounded-0");

            if (!result.Contains("erken") && !result.Contains("bulunmamaktadır"))
            {
                throw new Exception("Sonuç alınamadı");
            }

            if (!result.Contains("erken"))
            {
                await browser.CloseAsync();
                return false;
            }

            await bot.SendTextMessageAsync("-612527851", $"Id: {_config.Id} -" + result);
            await browser.CloseAsync();
            return true;
        }
    }
}
