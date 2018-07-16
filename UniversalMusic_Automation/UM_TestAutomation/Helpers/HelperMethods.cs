// Author : I.S.Panesar
// Date July 2018
// Description:
//      Helper methods

#region Usings
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using TechTalk.SpecFlow;
using Excel = Microsoft.Office.Interop.Excel;
using System.Globalization;
#endregion

namespace UM_TestAutomation.Helpers
{
    public static class HelperMethods
    {
        #region Private variables
        private static IWebDriver _driver = WebDriverCore.DriverInstance.Driver;
        private static List<string> _softassertlist = null;
        #endregion

        #region Helpers
        /// <summary>
        /// Add the error message to the assertions list
        /// </summary>
        /// <param name="p_AssertMsg"></param>
        public static void CreateSoftAssertion(string p_AssertMsg)
        {
            if (null == _softassertlist)
                _softassertlist = new List<string>();

            if (!string.IsNullOrEmpty(p_AssertMsg))
                _softassertlist.Add(p_AssertMsg);
        }

        /// <summary>
        /// Verify the assertions and clear the soft assertions list down
        /// </summary>
        public static void VerifySoftAssertions()
        {
            if (null == _softassertlist || 0 == _softassertlist.Count)
                return;
            var salcount = _softassertlist.Count();
            Console.WriteLine("\n============= Soft Assertion Error List Start =============\n");
            Console.WriteLine(string.Format("Soft Assertion Error List contains {0} error{1}:\n", salcount, salcount > 1 ? "s" : ""));
            salcount = 1;
            foreach (string saitem in _softassertlist)
            {
                Console.WriteLine($"{salcount} {saitem}");
                salcount++;
            }

            Console.WriteLine("============= Soft Assertion Error List End =============\n");
            _softassertlist.Clear();
            Assert.Fail("Soft Assertions were encountered during test runs. Please refer to the test output/report for further details");
        }

        /// <summary>
        /// Wait for element to be visible by element locator
        /// </summary>
        /// <param name="p_By"></param>
        /// <param name="p_Timeout"></param>
        public static void WaitForElementToExist(By p_By, TimeSpan p_Timeout)
        {
            new WebDriverWait(_driver, p_Timeout).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(p_By)); Thread.Sleep(1500);  // Using this just for now
        }

        /// <summary>
        /// Get property returns the path of the bin folder for the currently executing assembly
        /// </summary>
        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        /// <summary>
        /// JavaScript function to scroll to element using using its YOffset
        /// </summary>
        /// <param name="p_YOffset"></param>
        public static void JavasciptscrollDown(int p_YOffset)
        {
            ((IJavaScriptExecutor)_driver).ExecuteScript("scrollBy(0," + p_YOffset + ");");
        }

        /// <summary>
        /// JavaScript function to scroll to element 
        /// </summary>
        /// <param name="p_Element"></param>
        public static void JavaScriptScrollToElement(IWebElement p_Element)
        {
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", p_Element);
            Thread.Sleep(1000);
        }

        /// <summary>
        /// JavaScript function to scroll to element using its XPath
        /// </summary>
        /// <param name="p_XPath"></param>
        public static void JavaScriptScrollToElementWithXPath(string p_XPath)
        {
            var element = _driver.FindElement(By.XPath(p_XPath));
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
            Thread.Sleep(1000);
        }

        /// <summary>
        /// Function to take a screenshot when error occurs
        /// </summary>
        public static void TakeSceenShot()
        {            
            try
            {
                var screenShotDriver = _driver as ITakesScreenshot;
                if (screenShotDriver == null)
                    return;
                var screenShot = screenShotDriver.GetScreenshot();
                var fileName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(ScenarioContext.Current.ScenarioInfo.Title).Replace(" ",string.Empty);

                var path = $"{AssemblyDirectory}\\..\\..\\ScreenShots\\{fileName}.jpg";
                screenShot.SaveAsFile(path, ScreenshotImageFormat.Jpeg);
            }
            catch (Exception exception)
            {
                CreateSoftAssertion($"Failed to take screen shot error : {exception.Message}");
            }
        }

        /// <summary>
        /// Function to take a screenshot for each step
        /// </summary>
        public static void TakeStepSceenShot()
        {
            try
            {
                var screenShotDriver = _driver as ITakesScreenshot;
                if (screenShotDriver == null)
                    return;
                var screenShot = screenShotDriver.GetScreenshot();
                var scenarioTitle = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(ScenarioContext.Current.ScenarioInfo.Title).Replace(" ", string.Empty);
                var testStep = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(ScenarioStepContext.Current.StepInfo.Text).Replace(" ", string.Empty);
                var fileName = $"{scenarioTitle}_{testStep}";

                var path = $"{AssemblyDirectory}\\..\\..\\ScreenShots\\{fileName}.jpg";
                screenShot.SaveAsFile(path, ScreenshotImageFormat.Jpeg);
            }
            catch (Exception exception)
            {
                CreateSoftAssertion($"Failed to take screen shot error : {exception.Message}");
            }
        }


        /// <summary>
        /// Simple function, if duplicate key occurs then it is ignored
        /// Returns a Dictionary<string, string>
        /// </summary>
        /// <param name="p_ExcelFile"></param>
        /// <param name="p_Sheet"></param>
        /// <param name="p_IgnoreFirstRow"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetExcelFileDic(string p_ExcelFile, string p_Sheet, string p_TestData, bool p_IgnoreFirstRow)
        {
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(AssemblyDirectory + "\\..\\..\\DataFiles\\" + p_ExcelFile);
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[p_Sheet];
            Excel.Range xlRange = xlWorksheet.UsedRange;

            int rowCount = xlRange.Rows.Count;
            int colCount = xlRange.Columns.Count;

            // Determine the test data column
            int colOffset;
            if (int.TryParse(p_TestData.Split('_')[1], out colOffset))
                colOffset++;
            else
                colOffset = 2;

            var excelDic = new Dictionary<string, string>();
            // Process the rows and columns as in the file
            for (int i = 1; i <= rowCount; i++)
            {
                if (p_IgnoreFirstRow && i == 1)
                    continue;

                var col1 = xlRange.Cells[i, 1].Value2.ToString();
                var col2 = xlRange.Cells[i, colOffset].Value2.ToString();

                if (excelDic.ContainsKey(col1))
                    continue;
                else
                    excelDic.Add(col1, col2);
            }

            //cleanup
            GC.Collect();
            GC.WaitForPendingFinalizers();
            xlWorkbook.Close();
            xlApp.Quit();
            Marshal.ReleaseComObject(xlRange);
            Marshal.ReleaseComObject(xlWorksheet);
            Marshal.ReleaseComObject(xlWorkbook);

            Marshal.ReleaseComObject(xlApp);
            return excelDic;
        }
    }
    #endregion
}