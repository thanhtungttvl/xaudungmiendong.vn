using Microsoft.AspNetCore.Components;
using System.Collections.Specialized;
using System.Web;

namespace MudThemeLibrary.Helpers
{
    public static class NavigationManagerHelper
    {
        /// <summary>
        /// Gets the section part of the documentation page
        /// Ex: /components/button;  "components" is the section
        /// </summary>
        public static string GetSection(this NavigationManager navMan)
        {
            // get the absolute path with out the base path
            var currentUri = navMan.Uri.Remove(0, navMan.BaseUri.Length - 1);
            var firstElement = currentUri
                .Split("/", StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault();
            return firstElement!;
        }

        /// <summary>
        /// Gets the link of the component on the documentation page
        /// Ex: api/button; "button" is the component link, and "api" is the section
        /// </summary>
        public static string GetComponentLink(this NavigationManager navMan)
        {
            // get the absolute path with out the base path
            var currentUri = navMan.Uri.Remove(0, navMan.BaseUri.Length - 1);
            var secondElement = currentUri
                .Split("/", StringSplitOptions.RemoveEmptyEntries)
                .ElementAtOrDefault(1);
            return secondElement!;
        }

        /// <summary>
        /// Determines if the current page is the base page
        /// </summary>
        public static bool IsHomePage(this NavigationManager navMan)
        {
            return navMan.Uri == navMan.BaseUri;
        }

        public static string GetPathname(this NavigationManager navMan, bool isQuery = false)
        {
            var currentUri = navMan.Uri.Remove(0, navMan.BaseUri.Length - 1);
            if (isQuery is false)
            {
                if (currentUri.IndexOf('?') is not -1)
                {
                    currentUri = currentUri.Remove(currentUri.IndexOf('?'), currentUri.Length - currentUri.IndexOf('?'));
                }

            }
            return currentUri;
        }

        public static NameValueCollection QueryStringUrl(this NavigationManager navigationManager)
        {
            return HttpUtility.ParseQueryString(new Uri(navigationManager.Uri).Query);
        }

        public static string? QueryStringUrl(this NavigationManager navigationManager, string key)
        {
            return navigationManager.QueryStringUrl()[key];
        }
    }
}
