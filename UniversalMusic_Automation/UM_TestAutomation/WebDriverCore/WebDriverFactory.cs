// Author : I.S.Panesar
// Date July 2018
// Description:
//          Web driver factory returns an instance of a Webdriver object based on enum
//
#region Usings
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using UM_TestAutomation.Helpers;
using System;
#endregion

namespace UM_TestAutomation.WebDriverCore
{
    public static class WebDriverFactory
    {
        #region Public methods
        // Get the driver supported and
        // - maximise the browser window
        // - set an implicit wait on browser
        public static IWebDriver GetWebDriver(BrowserEnum p_Browser)
        {
            var driver = GetDriver(p_Browser);

            // Implicit wait of up to 10 seconds
            //  driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
            driver.Manage().Window.Maximize();
            return driver;
        }
        #endregion

        #region Private methods
        private static IWebDriver GetDriver(BrowserEnum p_Browser)
        {
            IWebDriver driver = null;
            switch (p_Browser)
            {
                case BrowserEnum.IE:
                    driver = new InternetExplorerDriver(HelperMethods.AssemblyDirectory + @"\Drivers\IE64\");
                    break;
                case BrowserEnum.Firefox:
                    FirefoxDriverService service =
                        FirefoxDriverService.CreateDefaultService(HelperMethods.AssemblyDirectory + @"\Drivers\GECKO");
                    service.FirefoxBinaryPath = @"C:\Program Files (x86)\Mozilla Firefox\firefox.exe";
                    driver = new FirefoxDriver(service);
                    break;
                case BrowserEnum.Chrome:
                    driver = new ChromeDriver();//HelperMethods.AssemblyDirectory + @"\Drivers\CHROME");
                    break;
                default:
                    driver = new ChromeDriver();//HelperMethods.AssemblyDirectory + @"\Drivers\CHROME");
                    break;

            }

            return driver;
        }
        #endregion
    }
}
