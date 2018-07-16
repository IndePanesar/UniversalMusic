// Author : I.S.Panesar
// Date July 2018
// Description:
//          SSE Test hooks
//
#region Usings
using OpenQA.Selenium;
using UM_TestAutomation.Helpers;
using TechTalk.SpecFlow;
#endregion

namespace UM_TestAutomation.StepDefinitions
{
    [Binding]
    public class WebDriverTestHooks
    {
        #region Private variables
        private static IWebDriver _driver;
        #endregion

        #region Hooks
        [BeforeScenario("FunctionalTests")]
        public void BeforeScenario()
        {
            _driver = WebDriverCore.DriverInstance.Driver;
        }

        [AfterScenario("FunctionalTests")]
        public void AfterScenario()
        {
            // An error has occurred have take a screen shot and shut browser
            var error = ScenarioContext.Current.TestError;
            if (null != error)
            {
                var test = ScenarioContext.Current.ScenarioInfo.Title;
                HelperMethods.CreateSoftAssertion($"Unhandled error in {test} during test. Error info -\n{error.Message} \n{error.GetType().Name}");
                // Close browser
                if (null != _driver)
                {
                    _driver.Quit();
                    _driver = null;
                }
            }
            HelperMethods.VerifySoftAssertions();
        }

        [AfterStep]
        public void TakeStepScreenShot()
        {
            HelperMethods.TakeStepSceenShot();
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            if (null != _driver)
            {
                _driver.Quit();
                _driver = null;
            }
        }

        #endregion
    }
}