// Author : I.S.Panesar
// Date July 2018
// Description:
//          Singleton returns an instance of the webdriver.
//
using OpenQA.Selenium;
using System;
using System.Configuration;

namespace UM_TestAutomation.WebDriverCore
{
    // Singleton gets a single instance of the webdriver
    public static class DriverInstance
    {
        private static IWebDriver _driver;
        public static IWebDriver Driver
        {
            get
            {
                if (null == _driver)
                {
                    var browser = ConfigurationManager.AppSettings["Browser"];
                    _driver = WebDriverFactory.GetWebDriver((BrowserEnum)Enum.Parse(typeof(BrowserEnum), browser, true));
                }
                return _driver;
            }
        }
    }
}
