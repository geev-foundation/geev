using System;
using Geev.Auditing;
using Geev.Domain.Uow;
using Geev.Extensions;
using Geev.Runtime.Validation;
using Microsoft.AspNetCore.Mvc;

namespace Geev.AspNetCore.Mvc.Controllers
{
    public class GeevAppViewController : GeevController
    {
        [DisableAuditing]
        [DisableValidation]
        [UnitOfWork(IsDisabled = true)]
        public ActionResult Load(string viewUrl)
        {
            if (viewUrl.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(viewUrl));
            }

            return View(viewUrl.EnsureStartsWith('~'));
        }
    }
}
