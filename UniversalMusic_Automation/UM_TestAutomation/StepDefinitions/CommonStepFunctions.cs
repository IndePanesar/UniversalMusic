// Author : I.S.Panesar
// Date July 2018
// Description:
//      Conduit page common functions seperated out from step definitions

using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace UM_TestAutomation.StepDefinitions
{
    public static class CommonStepFunctions
    {
        public static string GetValueFromExcelDicByKey(string p_Key)
        {
            var contextKey = "ExcelDicData";
            var excelDic = (Dictionary<string, string>)ScenarioContext.Current[contextKey];
            string keyValue;
            if (excelDic.TryGetValue(p_Key, out keyValue))
                return keyValue;
            else
                Helpers.HelperMethods.CreateSoftAssertion($"Failed to extract a value for key {p_Key} code from excel dictionary.");
            return string.Empty;
        }
    }
}
