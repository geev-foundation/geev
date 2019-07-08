using Geev.Domain.Uow;
using Geev.UI;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GeevAspNetCoreDemo.Pages
{
    [UnitOfWork(IsDisabled = true)]
    public class UowFilterPageDemo2 : PageModel
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public UowFilterPageDemo2(IUnitOfWorkManager unitOfWorkManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
        }

        public void OnGet()
        {
            if (_unitOfWorkManager.Current == null)
            {
                throw new UserFriendlyException("Current UnitOfWork is null");
            }
        }

        public void OnPost()
        {
            if (_unitOfWorkManager.Current == null)
            {
                throw new UserFriendlyException("Current UnitOfWork is null");
            }
        }
    }
}