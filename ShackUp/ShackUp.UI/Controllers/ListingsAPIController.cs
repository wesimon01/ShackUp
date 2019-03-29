using ShackUp.BLL;
using ShackUp.Data.Factory;
using ShackUp.Data.Interfaces;
using ShackUp.Models.Queries;
using System;
using System.Web.Http;

namespace ShackUp.UI.Controllers
{
    public class ListingsAPIController : ApiController
    {
        private IShackUpService _svc;
        private IAccountRepository acctRepo;
        private IListingsRepository listingRepo;

        public ListingsAPIController()
        {
            _svc = ShackUpServiceFactory.GetService();
            acctRepo = AccountRepositoryFactory.GetRepository(); //use if no service layer
            listingRepo = ListingRepositoryFactory.GetRepository(); //use if no service layer
        }

        [Route("api/contact/check/{userId}/{listingId}")]
        [AcceptVerbs("GET")]
        public IHttpActionResult CheckContact(string userId, int listingId)
        {
            try {
                var result = _svc.IsContact(userId, listingId);
                return Ok(result);
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [Route("api/favorite/check/{userId}/{listingId}")]
        [AcceptVerbs("GET")]
        public IHttpActionResult CheckFavorite(string userId, int listingId)
        {           
            try {
                var result = _svc.IsFavorite(userId, listingId);
                return Ok(result);
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [Route("api/contact/add/{userId}/{listingId}")]
        [AcceptVerbs("POST")]
        public IHttpActionResult AddContact(string userId, int listingId)
        {         
            try {
                _svc.AddContact(userId, listingId);
                return Ok();
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [Route("api/contact/remove/{userId}/{listingId}")]
        [AcceptVerbs("DELETE")]
        public IHttpActionResult RemoveContact(string userId, int listingId)
        {          
            try {
                _svc.RemoveContact(userId, listingId);
                return Ok();
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [Route("api/favorite/add/{userId}/{listingId}")]
        [AcceptVerbs("POST")]
        public IHttpActionResult AddFavorite(string userId, int listingId)
        {            
            try {
                _svc.AddFavorite(userId, listingId);
                return Ok();
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [Route("api/favorite/remove/{userId}/{listingId}")]
        [AcceptVerbs("DELETE")]
        public IHttpActionResult RemoveFavorite(string userId, int listingId)
        {            
            try {
                _svc.RemoveFavorite(userId, listingId);
                return Ok();
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [Route("api/listing/search")]
        [AcceptVerbs("GET")]
        public IHttpActionResult Search(decimal? minRate, decimal? maxRate, string city, string stateId)
        {           
            try {
                var parameters = new ListingSearchParameters()
                {
                    MinRate = minRate,
                    MaxRate = maxRate,
                    City = city,
                    StateId = stateId
                };

                var result = _svc.ListingSearch(parameters);
                return Ok(result);
            }

            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

    }
}
