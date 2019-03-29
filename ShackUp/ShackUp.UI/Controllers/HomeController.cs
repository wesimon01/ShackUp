using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShackUp.BLL;
using ShackUp.Data.Factory;
using ShackUp.Data.Interfaces;
using ShackUp.Models.Queries;


namespace ShackUp.UI.Controllers
{
    public class HomeController : Controller
    {
        private IShackUpService _svc;
        private IListingsRepository listingRepo;

        public HomeController()
        {
            _svc = ShackUpServiceFactory.GetService();
            listingRepo = ListingRepositoryFactory.GetRepository(); //use if no service layer
        }

        [HttpGet] // GET: /Home/Index
        public ActionResult Index()
        {
            /*
            var repo = ListingsRepositoryFactory.GetRepository();
            var model = repo.ListingGetRecent();
            */

            var model = _svc.ListingGetRecent();

            return View(model);
        }       
    }
}