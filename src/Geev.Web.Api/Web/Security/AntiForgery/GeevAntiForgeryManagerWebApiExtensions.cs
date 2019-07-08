﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Geev.Extensions;
using Geev.WebApi.Extensions;

namespace Geev.Web.Security.AntiForgery
{
    public static class GeevAntiForgeryManagerWebApiExtensions
    {
        public static void SetCookie(this IGeevAntiForgeryManager manager, HttpResponseHeaders headers)
        {
            headers.SetCookie(new Cookie(manager.Configuration.TokenCookieName, manager.GenerateToken()));
        }

        public static bool IsValid(this IGeevAntiForgeryManager manager, HttpRequestHeaders headers)
        {
            var authCookieValue = GetCookieValue(manager.Configuration.AuthorizationCookieName, headers);
            var antiForgeryCookieValue = GetCookieValue(manager.Configuration.TokenCookieName, headers);

            if (antiForgeryCookieValue.IsNullOrEmpty())
            {
                return authCookieValue.IsNullOrEmpty();
            }

            var headerTokenValue = GetHeaderValue(manager, headers);
            if (headerTokenValue.IsNullOrEmpty())
            {
                return false;
            }

            return manager.As<IGeevAntiForgeryValidator>().IsValid(antiForgeryCookieValue, headerTokenValue);
        }

        private static string GetCookieValue(string cookieName, HttpRequestHeaders headers)
        {
            var cookie = headers.GetCookies(cookieName).LastOrDefault();
            if (cookie == null)
            {
                return null;
            }

            return cookie[cookieName].Value;
        }

        private static string GetHeaderValue(IGeevAntiForgeryManager manager, HttpRequestHeaders headers)
        {
            IEnumerable<string> headerValues;
            if (!headers.TryGetValues(manager.Configuration.TokenHeaderName, out headerValues))
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
