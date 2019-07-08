using System;
using System.Web.Mvc;
using Geev.Auditing;
using Geev.Domain.Uow;
using Geev.Extensions;
using Geev.Runtime.Validation;

namespace Geev.Web.Mvc.Controllers
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
