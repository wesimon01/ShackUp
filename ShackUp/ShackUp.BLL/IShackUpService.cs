using ShackUp.Models.Queries;
using ShackUp.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShackUp.BLL
{
    public interface IShackUpService
    {
        List<State> StateGetAll();
        List<BathroomType> BathroomTypeGetAll();
        Listing ListingGetById(int listingId);
        ListingItem ListingGetDetails(int listingId);
        void ListingInsert(Listing listing);
        void ListingUpdate(Listing listing);
        void ListingDelete(int listingId);
        IEnumerable<ListingShortItem> ListingGetRecent();
        IEnumerable<ListingShortItem> ListingSearch(ListingSearchParameters parameters);
        IEnumerable<FavoriteItem> GetFavorites(string userId);
        IEnumerable<ContactRequestItem> GetContacts(string userId);
        IEnumerable<ListingItem> GetListings(string userId);
        void AddFavorite(string userId, int listingId);
        void RemoveFavorite(string userId, int listingId);
        void AddContact(string userId, int listingId);
        void RemoveContact(string userId, int listingId);
        bool IsFavorite(string userId, int listingId);
        bool IsContact(string userId, int listingId);
    }
}
