using FluentAssertions;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using UM_TestAutomation.LoggedInPageObjects;
using System;
using UM_TestAutomation.NotLoggedInPageObjects;
using UM_TestAutomation.Helpers;
namespace UM_TestAutomation.StepDefinitions
{
    [Binding]
    // [Parallelizable]
    public class Conduit_NotLoggedInHomePageSteps
    {
        #region Private variables
        private static IWebDriver _driver = WebDriverCore.DriverInstance.Driver;
        private static Conduit_NotLoggedInHomePage _notlogged_homepage = new Conduit_NotLoggedInHomePage(_driver);
        private static Conduit_LoggedInHomePage _logged_homepage;
        #endregion

        [Given(@"user is on the Conduit Angularjs home page")]
        public void UserIsOnTheConduitAngularjsHomePage()
        {
            _notlogged_homepage.Open();
            _notlogged_homepage.WaitForFooterNavBar();
        }

        [StepDefinition(@"user is not logged in")]
        public void UserIsNotLoggedIn()
        {
            var loggedin = false;
            try
            {
                loggedin = (bool)FeatureContext.Current["LoggedIn"];
            }
            catch(Exception e)
            {

            }
            if (loggedin)
            {
                (_logged_homepage ?? new Conduit_LoggedInHomePage(_driver)).LogUserOut();
                if (FeatureContext.Current.ContainsKey("LoggedIn"))
                    FeatureContext.Current.Remove("LoggedIn");
                FeatureContext.Current.Add("LoggedIn", false);
            }

            _notlogged_homepage.WaitForFooterNavBar();
            _notlogged_homepage.IsNavigationLinkDisplayed("Sign In").Should().BeTrue();
            _notlogged_homepage.IsNavigationLinkDisplayed("Sign Up").Should().BeTrue();
        }

        [StepDefinition(@"the home page is shown")]
        public void TheHomePageIsShown()
        {
            _notlogged_homepage.WaitForFooterNavBar();
            _notlogged_homepage.NotLoggedInHomePageIsDisplayed().Should().BeTrue();
            _notlogged_homepage.GetBannerText().Should().Contain("A place to share your knowledge.");
        }

        [When(@"the user signs up with credentials")]
        public void WhenTheUserSignsUpWithCredentials(Table p_Table)
        {
            var credentials = p_Table.CreateInstance<UserCredentials>();
            var signup = _notlogged_homepage.ClickSignUpLink() as Conduit_SignUpPage;
            var signedin = false;
            // Store user credentials in scenario context for later use
            if (ScenarioContext.Current.ContainsKey("Credentials"))
                ScenarioContext.Current.Remove("Credentials");
            ScenarioContext.Current.Add("Credentials", credentials);

            if (FeatureContext.Current.ContainsKey("LoggedIn"))
                FeatureContext.Current.Remove("LoggedIn");
            FeatureContext.Current.Add("LoggedIn", signedin);

            if (signup != null)
            {
                signedin = signup.SignUp_WithCredentials(credentials);
                _logged_homepage = new Conduit_LoggedInHomePage(_driver);
                _logged_homepage.WaitForFooterNavBar();
                if (FeatureContext.Current.ContainsKey("LoggedIn"))
                    FeatureContext.Current.Remove("LoggedIn");
                FeatureContext.Current.Add("LoggedIn", signedin);
            }
            else
                signup.Should().BeOfType<Conduit_SignUpPage>();
        }

        [Then(@"the user is logged in")]
        [Then(@"the user is automatically logged in")]
        public void TheUserIsLoggedIn()
        {
            var signedin = (bool)FeatureContext.Current["LoggedIn"];
            signedin.Should().BeTrue();
            _logged_homepage.IsNavigationLinkDisplayed("Home");
            _logged_homepage.IsNavigationLinkDisplayed("New Article");
            _logged_homepage.IsNavigationLinkDisplayed("Settings");
            _logged_homepage.GetActiveFeedLink().Text.Equals("Your feeds");
        }

        [StepDefinition(@"the users name is displayed")]
        public void TheUsersNameIsDisplayed()
        {
            UserCredentials credentials = null;

            if (ScenarioContext.Current.ContainsKey("Credentials"))
                credentials = (UserCredentials)ScenarioContext.Current["Credentials"];

            _logged_homepage.IsNavigationLinkDisplayed(credentials.UserName);
        }

        [StepDefinition(@"the user logs in with credentials")]
        public void TheUserlogsInWithCredentials(Table p_Table)
        {
            var credentials = p_Table.CreateInstance<UserCredentials>();
            var signin = _notlogged_homepage.ClickSignInLink() as Conduit_SignInPage;
            var signedin = false;
            // Store user credentials in scenario context for later use
            if (ScenarioContext.Current.ContainsKey("Credentials"))
                ScenarioContext.Current.Remove("Credentials");
            ScenarioContext.Current.Add("Credentials", credentials);

            if (FeatureContext.Current.ContainsKey("LoggedIn"))
                FeatureContext.Current.Remove("LoggedIn");
            FeatureContext.Current.Add("LoggedIn", signedin);

            if (signin != null)
            {
                signedin = signin.SignIn_WithCredentials(credentials);
                _logged_homepage = new Conduit_LoggedInHomePage(_driver);
                _logged_homepage.WaitForFooterNavBar();
                if (FeatureContext.Current.ContainsKey("LoggedIn"))
                    FeatureContext.Current.Remove("LoggedIn");
                FeatureContext.Current.Add("LoggedIn", signedin);
            }
            else
                signin.Should().BeOfType<Conduit_SignUpPage>();

        }

        [Then(@"the global feeds and popular tags are displayed")]
        public void ThenTheGlobalFeedsAndPopularTagsAreDisplayed()
        {
            _notlogged_homepage.WaitForFooterNavBar();
            _notlogged_homepage.GlobalFeedsAreDisplayed().Should().BeTrue();
            _notlogged_homepage.PopularTagsAreDisplayed().Should().BeTrue();
        }

        [When(@"a user tries to like an article")]
        public void WhenAUserTriesToLikeAnArticle()
        {
            // Do this for a random article
            // Get the number of articles displayed
            var feedCount = _notlogged_homepage.GetGlobalFeedCount();
            feedCount.Should().BeGreaterThan(0, $"Should have at least {_notlogged_homepage.GlobalFeedsArticlesListLimit()} articles but none found");

            // Choose a random article
            var rand = new Random();
            var offset = rand.Next(1, feedCount) - 1;

            var landingPage = _notlogged_homepage.LikeGlobalFeedArticleClicked(offset);
            // Save the landing page in ScenarioContext
            if (ScenarioContext.Current.ContainsKey("LandingPage"))
                ScenarioContext.Current.Remove("LandingPage");
            ScenarioContext.Current.Add("LandingPage", landingPage);
        }

        [Then(@"the user is directed to the Sign up area")]
        public void ThenTheUserIsDirectedToTheSignUpArea()
        {
            try
            {
                var landingPage = ScenarioContext.Current["LandingPage"] as Conduit_SignUpPage;

                if (landingPage != null)
                {
                    _driver.Title.Should().BeEquivalentTo(landingPage.PageTitle);
                    landingPage.SignUp_PageIsDisplayed().Should().BeTrue();
                }
            }
            catch(Exception)
            {
                HelperMethods.CreateSoftAssertion("Failed to land on Sign Up when liking an article");
            }
        }

        [StepDefinition(@"the user tries to sign up with '(.*)' that already exists")]
        public void UserTriesToSignUpCredentialsThatAlreadyExists(string p_Credential, Table p_Table)
        {
            var credentials = p_Table.CreateInstance<UserCredentials>();
            var signup = _notlogged_homepage.ClickSignUpLink() as Conduit_SignUpPage;
            var signedin = false;
            // Store user credentials in scenario context for later use
            if (ScenarioContext.Current.ContainsKey("Credentials"))
                ScenarioContext.Current.Remove("Credentials");
            ScenarioContext.Current.Add("Credentials", credentials);

            if (FeatureContext.Current.ContainsKey("LoggedIn"))
                FeatureContext.Current.Remove("LoggedIn");
            FeatureContext.Current.Add("LoggedIn", signedin);

            if (signup != null)
            {
                var cred = p_Credential.Contains("email") ? "email " + credentials.Email : "name " + credentials.UserName;
                signup.SignUp_WithCredentials(credentials).Should().BeFalse($"User{cred} should already exist.");
                if (ScenarioContext.Current.ContainsKey("ValidationErrors"))
                    ScenarioContext.Current.Remove("ValidationErrors");
                ScenarioContext.Current.Add("ValidationErrors", signup.GetErrorText());
            }
            else
                signup.Should().BeOfType<Conduit_SignUpPage>();
        }

        [StepDefinition(@"a validation error is displayed")]
        public void ValidationErrorIsDisplayed(Table p_Table)
        {
            var expectedErrors = p_Table.CreateInstance<ValidationErrors>();
            var actualErrors = (string)ScenarioContext.Current["ValidationErrors"];
            try
            {
                actualErrors.Should().ContainAny(expectedErrors.ValidationError);
            }
            catch(Exception)
            {
                HelperMethods.CreateSoftAssertion($"Expected Validaton error to be {expectedErrors}, actual was {actualErrors}");
            }
        }
    }
}

