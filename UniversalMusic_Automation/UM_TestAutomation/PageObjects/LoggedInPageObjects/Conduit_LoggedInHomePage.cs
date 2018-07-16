// Author : I.S.Panesar
// Date July 2018
// Description:
//      Conduit Home page class, when user is logged in.
//      

#region Usings
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using UM_TestAutomation.InterfacesAbstracts.Navigation;
#endregion

namespace UM_TestAutomation.LoggedInPageObjects
{
    public class Conduit_LoggedInHomePage : Conduit_LoggedInNavigationBar
    {
        #region Private variables
        private Uri _conduit_hp_uri;
        private static IWebDriver _driver;
        private static string PAGETITLE = "Home — Conduit";
        #endregion

        #region Page Constructors
        public Conduit_LoggedInHomePage(IWebDriver p_Driver) : base(p_Driver)
        {
            _driver = p_Driver;
            PageFactory.InitElements(_driver, this);
            _conduit_hp_uri = new Uri(ConfigurationManager.AppSettings["base_url"]);
        }

        #endregion

        #region Page web elements
        // Your feed and Global feed toggle
        [FindsBy(How = How.XPath, Using = ".//div[@class='feed-toggle']//li/a[contains(text(),'Your Feed')]")]
        private IWebElement _conduit_your_feed_toggle { get; set; }

        [FindsBy(How = How.XPath, Using = ".//div[@class='feed-toggle']//li/a[contains(text(),'Global Feed')]")]
        private IWebElement _conduit_global_feed_toggle { get; set; }


        // Global feed articles
        [FindsBy(How = How.XPath, Using = ".//article-list/article-preview")]
        private IList<IWebElement> _conduit_article_previews { get; set; }

        // Popular tag links
        [FindsBy(How = How.XPath, Using = ".//div[@class='sidebar']/p[contains(text(),'Popular Tags')]")]
        private IWebElement _conduit_popular_tag { get; set; }

        [FindsBy(How = How.XPath, Using = ".//div[@class='col-md-3']//div[@class='tag-list']/a")]
        private IList<IWebElement> _conduit_popular_tags_links { get; set; }

        // Logout button
        [FindsBy(How = How.ClassName, Using = "btn-outline-danger")]
        private IWebElement _conduit_logout_btn { get; set; }

        // Pagination
        [FindsBy(How = How.XPath, Using = ".//ul[@class='pagination']/li/a")]
        private IList<IWebElement> _conduit_hp_page_links { get; set; }

        #endregion

        #region Page Methods
        /// <summary>
        ///  Navigates to the conduit home page when not loggedin
        /// </summary>
        public void Open()
        {
            _driver.Navigate().GoToUrl(_conduit_hp_uri.AbsoluteUri);
        }

        /// <summary>
        /// PageTitle Property
        /// </summary>
        public string PageTitle
        {
            get { return PAGETITLE; }
        }


        /// <summary>
        /// Specified navigation link is displayed when not logged in
        /// </summary>
        /// <param name="p_NavLink"></param>
        /// <returns></returns>
        public bool IsNavigationLinkDisplayed(string p_NavLink)
        {
            var elements = _navbar_items_loggedin.Where(el => (el.Text.Trim().ToLower().Equals(p_NavLink.ToLower()) && el.Displayed)).Select(el => el);
            return elements.Count() > 0;
        }

        /// <summary>
        /// Global feeds are shown
        /// </summary>
        /// <returns>Boolean</returns>
        public bool GlobalFeedsAreDisplayed()
        {
            return _conduit_global_feed_toggle.Displayed;
        }

        /// <summary>
        /// Your feeds are shown
        /// </summary>
        /// <returns>Boolean</returns>
        public bool YourFeedsAreDisplayed()
        {
            return _conduit_your_feed_toggle.Displayed;
        }

        /// <summary>
        /// Popular tags are shown
        /// </summary>
        /// <returns>Boolean</returns>
        public bool PopularTagsAreDisplayed()
        {
            return _conduit_popular_tag.Displayed;
        }

        /// <summary>
        /// Get number of global articles displayed
        /// </summary>
        /// <returns>int</returns>
        public int GetGlobalFeedCount()
        {
            return _conduit_article_previews.Count;
        }

        /// <summary>
        /// Get number of popular tags displayed
        /// </summary>
        /// <returns>int</returns>
        public int GetPopularTagCount()
        {
            return _conduit_popular_tags_links.Count;
        }

        /// <summary>
        /// Logout logged in user
        /// </summary>
        /// <param name="p_NavLink"></param>
        /// <returns></returns>
        public void LogUserOut()
        {
            _navbar_items_loggedin[2].Click();
            WaitForFooterNavBar();
            _conduit_logout_btn.Click();
            WaitForFooterNavBar();
        }

        /// <summary>
        /// Get the active feed link IWebElement
        /// </summary>
        /// <returns>IWebElement represent the active feed link</returns>
        public IWebElement GetActiveFeedLink()
        {
            var element = _conduit_your_feed_toggle.GetAttribute("class");
            return (element.Contains("active")) ? _conduit_your_feed_toggle : _conduit_global_feed_toggle;
        }

        /// <summary>
        /// Click on the Home link
        /// </summary>
        /// <returns>Home page object</returns>
        public override Conduit_LoggedInNavigationBar ClickHomeLink()
        {
            _navbar_items_loggedin[0].Click();
            WaitForFooterNavBar();
            return this;
        }

        /// <summary>
        /// Click on the New Article link
        /// </summary>
        /// <returns>New Article page object</returns>
        public override Conduit_LoggedInNavigationBar ClickNewArticleLink()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Click on the Settings link
        /// </summary>
        /// <returns>Settings page object</returns>
        public override Conduit_LoggedInNavigationBar ClickSettingsLink()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Click on the Username link
        /// </summary>
        /// <returns>Username page object</returns>
        public override Conduit_LoggedInNavigationBar ClickUsernameLink()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

