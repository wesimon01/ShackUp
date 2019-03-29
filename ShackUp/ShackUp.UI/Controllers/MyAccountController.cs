using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using ShackUp.Data.Factory;
using ShackUp.UI.Models;
using ShackUp.UI.Utilities;
using ShackUp.BLL;
using ShackUp.Data.Interfaces;

namespace ShackUp.UI.Controllers
{
    [Authorize]
    public class MyAccountController : Controller
    {
        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;
        private IShackUpService _svc;
        private IAccountRepository acctRepo;

        public MyAccountController()
        {
            _svc = ShackUpServiceFactory.GetService();
            acctRepo = AccountRepositoryFactory.GetRepository(); //use if no service layer
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
       
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        [HttpGet] // GET: /MyAccount/Index
        public ActionResult Index()
        {
            var userId = Util.GetUserId(this);
            
            /*
            var repo = AccountRepositoryFactory.GetRepository();
            var model = repo.GetListings(userId);
            */

            var model = _svc.GetListings(userId);

            return View(model);
        }

        [HttpGet] // GET: /MyAccount/Favorites
        public ActionResult Favorites()
        {
            var userId = Util.GetUserId(this);

            /*
            var repo = AccountRepositoryFactory.GetRepository();
            var model = repo.GetFavorites(userId);
            */

            var model = _svc.GetFavorites(userId);

            return View(model);
        }

        [HttpPost] // POST: /MyAccount/DeleteFavorite
        public ActionResult DeleteFavorite(int listingId)
        {
            var userId = Util.GetUserId(this);

            //var repo = AccountRepositoryFactory.GetRepository();
            //var model = repo.RemoveFavorite(userId, listingId);

            _svc.RemoveFavorite(userId, listingId);

            return RedirectToAction("Favorites");
        }

        [HttpGet] // GET: /MyAccount/Contacts
        public ActionResult Contacts()
        {
            var userId = Util.GetUserId(this);            
            var model = _svc.GetContacts(userId);

            return View(model);
        }

        [HttpGet] // GET: /MyAccount/UpdateAccount
        public ActionResult UpdateAccount()
        {
            var model = new UpdateAccountViewModel();
            model.States = new SelectList(_svc.StateGetAll(), "StateId", "StateId");
            model.EmailAddress = User.Identity.Name;

            return View(model);
        }

        [HttpPost] // POST: /MyAccount/UpdateAccount
        public ActionResult UpdateAccount(UpdateAccountViewModel model)
        {
            var currentUser = UserManager.FindByEmail(User.Identity.Name);
            currentUser.UserName = model.EmailAddress;
            currentUser.Email = model.EmailAddress;
            currentUser.StateId = model.StateId;

            UserManager.Update(currentUser);

            SignInManager.AuthenticationManager.SignOut();

            return RedirectToAction("UpdateAccount");
        }

    }
}