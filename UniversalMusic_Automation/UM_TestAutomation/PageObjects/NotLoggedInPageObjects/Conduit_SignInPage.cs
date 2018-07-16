// Author : I.S.Panesar
// Date July 2018
// Description:
//      Conduit Sign In page object, 
//

#region Usings
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Configuration;
using UM_TestAutomation.Helpers;
using UM_TestAutomation.InterfacesAbstracts.Navigation;
using UM_TestAutomation.StepDefinitions;

#endregion

namespace UM_TestAutomation.NotLoggedInPageObjects
{
    public class Conduit_SignInPage : Conduit_NotLoggedInNavigationBar
    {
        #region Private variables
        private Uri _conduit_signin_uri;
        private static IWebDriver _driver;
        private static string PAGETITLE = "Sign in — Conduit";
        #endregion

       #region Page Constructors
        public Conduit_SignInPage(IWebDriver p_Driver) : base(p_Driver)
        {
            _driver = p_Driver;
            PageFactory.InitElements(_driver, this);
            _conduit_signin_uri = new Uri(new Uri(ConfigurationManager.AppSettings["base_url"]),"login");
        }

        #endregion

        #region Page web elements
       // Conduit Sign In page header
        [FindsBy(How = How.TagName, Using = "h1")]
        private IWebElement _signin_page_header { get; set; }

        // Need an account link
        [FindsBy(How = How.XPath, Using = ".//a[contains(text(),'Need an account')]")]
        private IWebElement _signin_needaccount { get; set; }

        // Input Email
        [FindsBy(How = How.XPath, Using = ".//input[@placeholder='Email']")]
        private IWebElement _signin_input_email { get; set; }

        // Input Password
        [FindsBy(How = How.XPath, Using = ".//input[@placeholder='Password']")]
        private IWebElement _signin_input_password { get; set; }

        // Submit button
        [FindsBy(How = How.XPath, Using = ".//button[contains(text(), 'Sign in')]")]
        private IWebElement _signin_button_submit { get; set; }

        // Sign In Page errors
        [FindsBy(How = How.ClassName, Using = "error-messages")]
        private IWebElement _signin_error_messages { get; set; }

        #endregion

        #region Page Methods
        /// <summary>
        ///  Navigates to the conduit home page when not loggedin
        /// </summary>
        public void Open()
        {
            _driver.Navigate().GoToUrl(_conduit_signin_uri.AbsoluteUri);
        }

        /// <summary>
        /// PageTitle Property
        /// </summary>
        public string PageTitle
        {
            get { return PAGETITLE; }
        }

        /// <summary>
        /// Enter Email
        /// </summary>
        public void SignIn_EnterEmail(string p_Email)
        {
            HelperMethods.JavaScriptScrollToElement(_signin_input_email);
            _signin_input_email.Clear();
            _signin_input_email.SendKeys(p_Email);
        }

        /// <summary>
        /// Enter Password
        /// </summary>
        public void SignIn_EnterPassword(string p_Password)
        {
            HelperMethods.JavaScriptScrollToElement(_signin_input_password);
            _signin_input_password.Clear();
            _signin_input_password.SendKeys(p_Password);
        }

        /// <summary>
        /// Click Sign In button
        /// </summary>
        public void SignIn_ClickSignIn()
        {
            HelperMethods.JavaScriptScrollToElement(_signin_button_submit);
            _signin_button_submit.Click();
        }

        /// <summary>
        /// Signin with credentials
        /// </summary>
        /// <param name="p_Credentials"></param>
        public bool SignIn_WithCredentials(UserCredentials p_Credentials)
        {
            try
            {
                SignIn_EnterEmail(p_Credentials.Email);
                SignIn_EnterPassword(p_Credentials.Password);
                SignIn_ClickSignIn();
                return _signin_error_messages.Displayed ? false : true;
            }
            catch (NoSuchElementException)
            {
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets all the error texts
        /// </summary>
        /// <returns>All errors shown</returns>
        public string GetErrorText()
        {
            return _signin_error_messages.Text;
        }

        /// <summary>
        /// Click on the Home link
        /// </summary>
        /// <returns>Homepage object</returns>
        public override Conduit_NotLoggedInNavigationBar ClickHomeLink()
        {
            _navbar_items_nologin[0].Click();
            return new Conduit_NotLoggedInHomePage(_driver);
        }

        /// <summary>
        /// Click on the Signin link
        /// </summary>
        /// <returns>SignIn page object</returns>
        public override Conduit_NotLoggedInNavigationBar ClickSignInLink()
        {
            _navbar_items_nologin[1].Click();
            return this;
        }

        /// <summary>
        /// Click on the SignUp link
        /// </summary>
        /// <returns>SignUp page object</returns>
        public override Conduit_NotLoggedInNavigationBar ClickSignUpLink()
        {
            _navbar_items_nologin[2].Click();
            return new Conduit_SignUpPage(_driver);
        }

        #endregion
    }
}

