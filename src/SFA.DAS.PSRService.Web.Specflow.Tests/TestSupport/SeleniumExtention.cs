using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Configuration;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport
{
    public static class SeleniumExtention
    {
        public static void WaitForLoading(this IWebDriver driver)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(int.Parse(ConfigurationManager.AppSettings["WaitingTimeout"])));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("body:not(.body--loading)")));
        }

        public static IWebElement WaitForElementToBeVisible(this IWebDriver driver, By selector)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(int.Parse(ConfigurationManager.AppSettings["WaitingTimeout"])));
            var reportElement =
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(selector));
            return reportElement;
        }

        public static IWebElement WaitForElementToBeClickable(this IWebDriver driver, By selector)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(int.Parse(ConfigurationManager.AppSettings["WaitingTimeout"])));
            var reportElement =
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(selector));
            return reportElement;
        }
    }
}
