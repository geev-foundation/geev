﻿using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Helpers;
using Geev.Extensions;

namespace Geev.Web.Security.AntiForgery
{
    public static class GeevAntiForgeryManagerMvcExtensions
    {
        public static void SetCookie(this IGeevAntiForgeryManager manager, HttpContextBase context, IIdentity identity = null)
        {
            if (identity != null)
            {
                context.User = new ClaimsPrincipal(identity);
            }

            context.Response.Cookies.Add(new HttpCookie(manager.Configuration.TokenCookieName, manager.GenerateToken()));
        }

        public static bool IsValid(this IGeevAntiForgeryManager manager, HttpContextBase context)
        {
            var authCookieValue = GetCookieValue(context, manager.Configuration.AuthorizationCookieName);
            var antiForgeryCookieValue = GetCookieValue(context, AntiForgeryConfig.CookieName);

            if (antiForgeryCookieValue.IsNullOrEmpty())
            {
                return authCookieValue.IsNullOrEmpty();
            }

            var formOrHeaderValue = manager.Configuration.GetFormOrHeaderValue(context);
            if (formOrHeaderValue.IsNullOrEmpty())
            {
                return false;
            }

            return manager.As<IGeevAntiForgeryValidator>().IsValid(antiForgeryCookieValue, formOrHeaderValue);
        }

        private static string GetCookieValue(HttpContextBase context, string cookineName)
        {
            var cookie = context.Request.Cookies[cookineName];
            return cookie?.Value;
        }

        private static string GetFormOrHeaderValue(this IGeevAntiForgeryConfiguration configuration, HttpContextBase context)
        {
            var formValue = context.Request.Form["__RequestVerificationToken"];
            if (!formValue.IsNullOrEmpty())
            {
                return formValue;
            }

            var headerValues = context.Request.Headers.GetValues(configuration.TokenHeaderName);
            if (headerValues == null)
            {
                return null;
            }

            var headersArray = headerValues.ToArray();
            if (!headersArray.Any())
            {
                return null;
            }

            return headersArray.Last().Split(", ").Last();
        }
    }
}