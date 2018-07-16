// Author : I.S.Panesar
// Date July 2018
// Description:
//      Conduit Home page class, when user is not logged in.
//      

#region Usings
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using UM_TestAutomation.Helpers;
using UM_TestAutomation.InterfacesAbstracts.Navigation;
#endregion

namespace UM_TestAutomation.NotLoggedInPageObjects
{
    public class Conduit_NotLoggedInHomePage : Conduit_NotLoggedInNavigationBar
    {
        #region Private variables
        private Uri _conduit_hp_uri;
        private static IWebDriver _driver;
        private static string PAGETITLE = "Home — Conduit";
        #endregion

        #region Page Constructors
        public Conduit_NotLoggedInHomePage(IWebDriver p_Driver) : base(p_Driver)
        {
            _driver = p_Driver;
            PageFactory.InitElements(_driver, this);
            _conduit_hp_uri = new Uri(ConfigurationManager.AppSettings["base_url"]);
        }
    
        #endregion

        #region Page web elements        
        // Conduit banner
        [FindsBy(How = How.ClassName, Using = "banner")]
        private IWebElement _conduit_banner { get; set; }

        // Global feed
        [FindsBy(How = How.XPath, Using = ".//div[@class='feed-toggle']//li/a[contains(text(),'Global Feed')]")]
        private IWebElement _conduit_global_feed_toggle { get; set; }

        [FindsBy(How = How.TagName, Using = "article-list")]
        private IWebElement _conduit_global_feed_list { get; set; }

        // Global feed articles
        [FindsBy(How = How.XPath, Using = ".//article-list/article-preview")]
        private IList<IWebElement> _conduit_article_previews { get; set; }

        // Popular tag links
        [FindsBy(How = How.XPath, Using = ".//div[@class='sidebar']/p[contains(text(),'Popular Tags')]")]
        private IWebElement _conduit_popular_tag { get; set; }

        [FindsBy(How = How.XPath, Using = ".//div[@class='col-md-3']//div[@class='tag-list']/a")]
        private IList<IWebElement> _conduit_popular_tags_links { get; set; }

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
        /// Home page when user is not logged in is displayed
        /// </summary>
        /// <returns></returns>
        public bool NotLoggedInHomePageIsDisplayed()
        {
            return IsNavigationLinkDisplayed("Sign In") && IsNavigationLinkDisplayed("Sign Up") && IsBannerDisplayed();
        }

        /// <summary>
        /// Specified navigation link is displayed when not logged in
        /// </summary>
        /// <param name="p_NavLink"></param>
        /// <returns></returns>
        public bool IsNavigationLinkDisplayed(string p_NavLink)
        {
            var elements = _navbar_items_nologin.Where(el => (el.Text.Trim().ToLower().Equals(p_NavLink.ToLower()) && el.Displayed)).Select(el=>el);
            return elements.Count() > 0;
        }

        /// <summary>
        /// Banner is displayed when not logged in
        /// </summary>
        /// <returns>Bool</returns>
        public bool IsBannerDisplayed()
        {
            return _conduit_banner.Displayed;
        }

        /// <summary>
        /// Get the Banner text
        /// </summary>
        /// <returns>string banner text</returns>
        public string GetBannerText()
        {
            return _conduit_banner.Text;
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
        /// Global feeds articles list limit
        /// </summary>
        /// <returns>int</returns>
        public int GlobalFeedsArticlesListLimit()
        {
            int limit;
            return int.TryParse(_conduit_global_feed_list.GetAttribute("limit"), out limit) ? limit : 0;
        }

        /// <summary>
        /// Global feeds articles list
        /// </summary>
        /// <returns>IList<IWebElement></returns>
        public IList<IWebElement> GetAllArticleElements()
        {
            var e = _conduit_article_previews;
            return _conduit_article_previews;
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
        /// Like an article in the Global feeds given its offset in list
        /// </summary>
        /// <returns>SignUp page or remain here</returns>
        public Conduit_NotLoggedInNavigationBar LikeGlobalFeedArticleClicked(int p_Offset)
        {
            if (p_Offset > GlobalFeedsArticlesListLimit() - 1)
                return this;
            var article = _conduit_article_previews[p_Offset].FindElement(By.XPath(".//favorite-btn/button"));
            HelperMethods.JavaScriptScrollToElement(article);
            article.Click();
            var landingpage = new Conduit_SignUpPage(_driver);
            landingpage.WaitForFooterNavBar();
            return landingpage;
        }

        /// <summary>
        /// Click on the Home link
        /// </summary>
        /// <returns>Homepage object</returns>
        public override Conduit_NotLoggedInNavigationBar ClickHomeLink()
        {
            _navbar_items_nologin[0].Click();
            WaitForFooterNavBar();
            return this;
        }

        /// <summary>
        /// Click on the Signin link
        /// </summary>
        /// <returns>SignIn page object</returns>
        public override Conduit_NotLoggedInNavigationBar ClickSignInLink()
        {
            _navbar_items_nologin[1].Click();
            var landingpage = new Conduit_SignInPage(_driver);
            landingpage.WaitForFooterNavBar();
            return landingpage;
        }

        /// <summary>
        /// Click on the SignUp link
        /// </summary>
        /// <returns>SignUp page object</returns>
        public override Conduit_NotLoggedInNavigationBar ClickSignUpLink()
        {
            _navbar_items_nologin[2].Click();
            var landingpage = new Conduit_SignUpPage(_driver);
            landingpage.WaitForFooterNavBar();
            return landingpage;
        }
        #endregion
    }
}

