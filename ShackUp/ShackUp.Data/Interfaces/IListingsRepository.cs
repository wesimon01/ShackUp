using ShackUp.Models.Queries;
using ShackUp.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShackUp.Data.Interfaces
{
    public interface IListingsRepository
    {
        Listing ListingGetById(int listingId);
        ListingItem ListingGetDetails(int listingId);
        void ListingInsert(Listing listing);
        void ListingUpdate(Listing listing);        
        void ListingDelete(int listingId);
        IEnumerable<ListingShortItem> ListingGetRecent();
        IEnumerable<ListingShortItem> ListingSearch(ListingSearchParameters parameters);
    }
}
