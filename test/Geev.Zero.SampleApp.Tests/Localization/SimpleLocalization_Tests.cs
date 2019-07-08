﻿using System.Globalization;
using System.Threading;
using Geev.Localization;
using Shouldly;
using Xunit;

namespace Geev.Zero.SampleApp.Tests.Localization
{
    public class SimpleLocalization_Tests : SampleAppTestBase
    {
        [Theory]
        [InlineData("en")]
        [InlineData("en-US")]
        [InlineData("en-GB")]
        public void Test1(string cultureName)
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(cultureName);

            Resolve<ILocalizationManager>()
                .GetString(GeevZeroConsts.LocalizationSourceName, "Identity.UserNotInRole")
                .ShouldBe("User is not in role.");
        }
    }
}
