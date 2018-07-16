// Author : I.S.Panesar
// Date July 2018
// Description:
//      Abstract class for the Navigation bar menu items when user is not logged in. 
//      

#region Usings
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using UM_TestAutomation.Helpers;
#endregion

namespace UM_TestAutomation.InterfacesAbstracts.Navigation
{
    public abstract class Conduit_NotLoggedInNavigationBar
    {
        #region Public variables
        public enum HomePageNavBarEnum
        {
            Home,
            SignIn,
            SignUp
        }
        #endregion

        #region Page Constructors
        public Conduit_NotLoggedInNavigationBar(IWebDriver p_Driver)
        {            
            PageFactory.InitElements(p_Driver, this);
        }
    
        #endregion

        #region Page web elements
        // Main navigation items
        // Navigation bar items
        [FindsBy(How = How.XPath, Using =".//ul[@show-authed='false']/li")]
        protected IList<IWebElement> _navbar_items_nologin { get; set; }

        //// Navigation bar
        //// Conduit brand
        [FindsBy(How = How.ClassName, Using = "navbar-brand")]
        protected IWebElement _conduit_navbar_brand { get; set; }

        // Footer navigation bar
        [FindsBy(How = How.TagName, Using = "app-footer")]
        protected IWebElement _conduit_navbar_footer { get; set; }

        // By locators
        protected By _by_conduit_footer = By.TagName("app-footer");
        #endregion

        #region Page Methods

        /// <summary>
        /// Gets all the navbar menu items
        /// </summary>
        /// <returns>IList<IWebElements></returns>
        public IList<IWebElement> GetAllNavBarMenuItems()
        {
            return _navbar_items_nologin;
        }

        /// <summary>
        /// Click the Navigation Bar Brand icon
        /// </summary>
        public void Click_NavBarBrand()
        {
            _conduit_navbar_brand.Click();
            WaitForFooterNavBar();
        }

        /// <summary>
        /// Wait for the footer navigation bar to be present
        /// </summary>
        public void WaitForFooterNavBar()
        {
            HelperMethods.WaitForElementToExist(_by_conduit_footer, TimeSpan.FromSeconds(5));
        }

        // Clicking on one of the navigation bar links should return instance of page that inherits from this class
        public abstract Conduit_NotLoggedInNavigationBar ClickHomeLink();
        public abstract Conduit_NotLoggedInNavigationBar ClickSignInLink();
        public abstract Conduit_NotLoggedInNavigationBar ClickSignUpLink();        
        #endregion
    }
}

