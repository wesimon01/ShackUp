using ShackUp.Data.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ShackUp.UI.Models;
using ShackUp.Models.Tables;
using System.IO;
using ShackUp.UI.Utilities;
using ShackUp.Models.Queries;
using ShackUp.BLL;
using ShackUp.Data.Interfaces;

namespace ShackUp.UI.Controllers
{
    public class ListingsController : Controller
    {
        private IShackUpService _svc;
        private IListingsRepository listingRepo;
        private IStatesRepository statesRepo;
        private IBathroomTypesRepository bathroomTypesRepo;

        public ListingsController()
        {
            _svc = ShackUpServiceFactory.GetService();
            listingRepo = ListingRepositoryFactory.GetRepository(); //use if no service layer
            statesRepo = StatesRepositoryFactory.GetRepository(); //use if no service layer
            bathroomTypesRepo = BathroomTypesRepositoryFactory.GetRepository(); //use if no service layer
        }

        private void PopulateListingAddDropDowns(ListingAddViewModel model)
        {
            /*
            var statesRepo = StatesRepositoryFactory.GetRepository();
            var bathroomTypesRepo = BathroomTypesRepositoryFactory.GetRepository();
            model.States = new SelectList(statesRepo.StateGetAll(), "StateId", "StateId");
            model.BathroomTypes = new SelectList(bathRoomTypesRepo.BathroomTypeGetAll(), "BathroomTypeId", "BathroomTypeName");
            */

            model.States = new SelectList(_svc.StateGetAll(), "StateId", "StateId");
            model.BathroomTypes = new SelectList(_svc.BathroomTypeGetAll(), "BathroomTypeId", "BathroomTypeName");
        }

        private void PopulateListingEditDropDowns(ListingEditViewModel model)
        {
            /*
            var statesRepo = StatesRepositoryFactory.GetRepository();
            var bathroomTypesRepo = BathroomTypesRepositoryFactory.GetRepository();
            model.States = new SelectList(statesRepo.StateGetAll(), "StateId", "StateId");
            model.BathroomTypes = new SelectList(bathRoomTypesRepo.BathroomTypeGetAll(), "BathroomTypeId", "BathroomTypeName");
            */

            model.States = new SelectList(_svc.StateGetAll(), "StateId", "StateId");
            model.BathroomTypes = new SelectList(_svc.BathroomTypeGetAll(), "BathroomTypeId", "BathroomTypeName");
        }

        [HttpGet] // GET: /Listings/Details
        public ActionResult Details(int id)
        {
            if (Request.IsAuthenticated) {                
                ViewBag.UserId = Util.GetUserId(this);
            }
            /*
            var repo = ListingRepositoryFactory.GetRepository();
            var model = repo.ListingGetDetails(id);
            */

            var model = _svc.ListingGetDetails(id);

            return View(model);
        }

        [HttpGet] // GET: /Listings/Index
        public ActionResult Index()
        {
            /*
            var repo = StatesRepositoryFactory.GetRepository();
            var model = repo.StateGetAll();
            */

            var model = _svc.StateGetAll();

            return View(model);
        }

        [HttpGet]
        [Authorize] // GET: /Listings/Add
        public ActionResult Add()
        {
            var model = new ListingAddViewModel();
            model.Listing = new Listing();
            PopulateListingAddDropDowns(model);
            
            return View(model);
        }

        [HttpPost]
        [Authorize] // POST: /Listings/Add
        public ActionResult Add(ListingAddViewModel model)
        {
            if (ModelState.IsValid)
            {
                try {
                    //var repo = ListingRepositoryFactory.GetRepository();

                    model.Listing.UserId = Util.GetUserId(this);

                    if (model.ImageUpload != null && model.ImageUpload.ContentLength > 0)
                    {
                        string savePath = Server.MapPath("~/Images");
                        string fileName = Path.GetFileNameWithoutExtension(model.ImageUpload.FileName);
                        string extension = Path.GetExtension(model.ImageUpload.FileName);
                        string filePath = Path.Combine(savePath, fileName + extension);
                        int counter = 1;

                        while (System.IO.File.Exists(filePath))
                        {
                            filePath = Path.Combine(savePath, fileName + counter.ToString() + extension);
                            counter++;
                        }
                        model.ImageUpload.SaveAs(filePath);
                        model.Listing.ImageFileName = Path.GetFileName(filePath);
                    }
                    //repo.ListingInsert(model.Listing);

                    _svc.ListingInsert(model.Listing);

                    return RedirectToAction("Edit", new { id = model.Listing.ListingId });
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else {
                PopulateListingAddDropDowns(model);
                return View(model);
            }                                 
        }

        [HttpGet]
        [Authorize] // GET: /Listings/Edit
        public ActionResult Edit(int id)
        {
            //var repo = ListingRepositoryFactory.GetRepository();

            var model = new ListingEditViewModel();

            //model.Listing = repo.ListingGetBy(id);
            model.Listing = _svc.ListingGetById(id);

            if (model.Listing.UserId != Util.GetUserId(this))
                throw new Exception("Attempt to edit a listing you do not own! Shame!");

            PopulateListingEditDropDowns(model);
                       
            return View(model);
        }

        [HttpPost]
        [Authorize] // POST: /Listings/Edit
        public ActionResult Edit(ListingEditViewModel model)           
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //var repo = ListingRepositoryFactory.GetRepository();

                    model.Listing.UserId = Util.GetUserId(this);
                    
                    //var oldListing = repo.ListingGetById(model.Listing.ListingId);
                    var oldListing = _svc.ListingGetById(model.Listing.ListingId);

                    if (model.ImageUpload != null && model.ImageUpload.ContentLength > 0)
                    {
                        string savePath = Server.MapPath("~/Images");
                        string fileName = Path.GetFileNameWithoutExtension(model.ImageUpload.FileName);
                        string extension = Path.GetExtension(model.ImageUpload.FileName);
                        string filePath = Path.Combine(savePath, fileName + extension);
                        int counter = 1;

                        while (System.IO.File.Exists(filePath))
                        {
                            filePath = Path.Combine(savePath, fileName + counter.ToString() + extension);
                            counter++;
                        }
                        model.ImageUpload.SaveAs(filePath);
                        model.Listing.ImageFileName = Path.GetFileName(filePath);

                        var oldPath = Path.Combine(savePath, oldListing.ImageFileName);
                        if (System.IO.File.Exists(oldPath)) {
                            System.IO.File.Delete(oldPath);
                        }
                    }
                    else {
                        // they did not replace the old file, so keep the old file name
                        model.Listing.ImageFileName = oldListing.ImageFileName;
                    }
                    //repo.ListingUpdate(model.Listing);

                    _svc.ListingUpdate(model.Listing);

                    return RedirectToAction("Edit", new { id = model.Listing.ListingId });
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                PopulateListingEditDropDowns(model);
                return View(model);
            }

        }
    }
}