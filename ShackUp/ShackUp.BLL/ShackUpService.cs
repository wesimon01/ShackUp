using ShackUp.Data.Factory;
using ShackUp.Data.Interfaces;
using ShackUp.Models.Queries;
using ShackUp.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShackUp.BLL
{
    public class ShackUpService : IShackUpService
    {
        private IStatesRepository statesRepo;
        private IBathroomTypesRepository bathroomTypesRepo;
        private IListingsRepository listingsRepo;
        private IAccountRepository acctRepo;

        public ShackUpService()
        {
            statesRepo = StatesRepositoryFactory.GetRepository();
            bathroomTypesRepo = BathroomTypesRepositoryFactory.GetRepository();
            listingsRepo = ListingRepositoryFactory.GetRepository();
            acctRepo = AccountRepositoryFactory.GetRepository();
        }

        public void AddContact(string userId, int listingId)
        {
            try {
                acctRepo.AddContact(userId, listingId);
            }
            catch (Exception ex)
            {
                //log exception
                throw ex;
            }
        }

        public void AddFavorite(string userId, int listingId)
        {
            try {
                acctRepo.AddFavorite(userId, listingId);
            }
            catch (Exception ex)
            {
                //log exception
                throw ex;
            }
        }

        public List<BathroomType> BathroomTypeGetAll()
        {
            try {
                return bathroomTypesRepo.BathroomTypeGetAll();
            }
            catch (Exception ex)
            {
                //log exception
                throw ex;
            }
        }

        public IEnumerable<ContactRequestItem> GetContacts(string userId)
        {
            try {
                return acctRepo.GetContacts(userId);
            }
            catch (Exception ex)
            {
                //log exception
                throw ex;
            }
        }

        public IEnumerable<FavoriteItem> GetFavorites(string userId)
        {
            try {
                return acctRepo.GetFavorites(userId);
            }
            catch (Exception ex)
            {
                //log exception
                throw ex;
            }
        }

        public IEnumerable<ListingItem> GetListings(string userId)
        {
            try {
                return acctRepo.GetListings(userId);
            }
            catch (Exception ex)
            {
                //log exception
                throw ex;
            }
        }

        public bool IsContact(string userId, int listingId)
        {
            try {
                return acctRepo.IsContact(userId, listingId);
            }
            catch (Exception ex)
            {
                //log exception
                throw ex;
            }
        }

        public bool IsFavorite(string userId, int listingId)
        {
            try {
                return acctRepo.IsFavorite(userId, listingId);
            }
            catch (Exception ex)
            {
                //log exception
                throw ex;
            }
        }

        public void ListingDelete(int listingId)
        {
            try {
                listingsRepo.ListingDelete(listingId);
            }
            catch (Exception ex)
            {
                //log exception
                throw ex;
            }
        }

        public Listing ListingGetById(int listingId)
        {
            try {
                return listingsRepo.ListingGetById(listingId);
            }
            catch (Exception ex)
            {
                //log exception
                throw ex;
            }
        }

        public ListingItem ListingGetDetails(int listingId)
        {
            try {
                return listingsRepo.ListingGetDetails(listingId);
            }
            catch (Exception ex)
            {
                //log exception
                throw ex;
            }
        }

        public IEnumerable<ListingShortItem> ListingGetRecent()
        {
            try {
                return listingsRepo.ListingGetRecent();
            }
            catch (Exception ex)
            {
                //log exception
                throw ex;
            }
        }

        public void ListingInsert(Listing listing)
        {
            try {
                listingsRepo.ListingInsert(listing);
            }
            catch (Exception ex)
            {
                //log exception
                throw ex;
            }
        }

        public IEnumerable<ListingShortItem> ListingSearch(ListingSearchParameters parameters)
        {
            try {
                return listingsRepo.ListingSearch(parameters);
            }
            catch (Exception ex)
            {
                //log exception
                throw ex;
            }
        }

        public void ListingUpdate(Listing listing)
        {
            try {
                listingsRepo.ListingUpdate(listing);
            }
            catch (Exception ex)
            {
                //log exception
            }
        }

        public void RemoveContact(string userId, int listingId)
        {
            try {
                acctRepo.RemoveContact(userId, listingId);
            }
            catch (Exception ex)
            {
                //log exception
            }
        }

        public void RemoveFavorite(string userId, int listingId)
        {
            try {
                acctRepo.RemoveFavorite(userId, listingId);
            }
            catch (Exception ex)
            {
                //log exception
                throw ex;
            }
        }

        public List<State> StateGetAll()
        {
            try {
                return statesRepo.StateGetAll();
            }
            catch (Exception ex)
            {
                //log exception
                throw ex;
            }          
        }
    }
}
