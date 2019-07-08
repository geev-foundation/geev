using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Geev.Auditing;
using Geev.Localization;
using Geev.Runtime.Validation;
using Geev.Zero.SampleApp.Users;
using Geev.Zero.SampleApp.Users.Dto;
using Shouldly;
using Xunit;

namespace Geev.Zero.SampleApp.Tests.Application.Services
{
    public class Validation_Tests : SampleAppTestBase
    {
        private readonly IUserAppService _userAppService;

        public Validation_Tests()
        {
            _userAppService = Resolve<IUserAppService>();
        }


        [Theory]
        [InlineData("en")]
        [InlineData("en-US")]
        [InlineData("en-GB")]
        public void CustomValidationContext_Localize_Test(string cultureName)
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(cultureName);

            var exception = Assert.Throws<GeevValidationException>(() =>
            {
                _userAppService.CustomValidateMethod(new CustomValidateMethodInput());
            });

            exception.ValidationErrors.ShouldNotBeNull();
            exception.ValidationErrors.Count.ShouldBe(1);
            exception.ValidationErrors[0].ErrorMessage.ShouldBe("User is not in role.");
        }
    }
}
