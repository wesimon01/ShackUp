using Dapper;
using ShackUp.Data.Interfaces;
using ShackUp.Models.Queries;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ShackUp.Data.Dapper
{
    public class AccountRepositoryDapper : IAccountRepository
    {
        private static string cxnStr = Settings.GetConnectionString();

        public void AddContact(string userId, int listingId)
        {
            using (var cxn = new SqlConnection(cxnStr))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userId);
                parameters.Add("@ListingId", listingId);

                cxn.Execute("ContactsInsert", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void AddFavorite(string userId, int listingId)
        {
            using (var cxn = new SqlConnection(cxnStr))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userId);
                parameters.Add("@ListingId", listingId);

                cxn.Execute("FavoritesInsert", parameters, commandType: CommandType.StoredProcedure);
            }           
        }

        public IEnumerable<ContactRequestItem> GetContacts(string userId)
        {
            var contacts = new List<ContactRequestItem>();

            using (var cxn = new SqlConnection(cxnStr))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userId);

                contacts = cxn.Query<ContactRequestItem>("ListingsSelectContacts", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
            return contacts;
        }

        public IEnumerable<FavoriteItem> GetFavorites(string userId)
        {
            var favorites = new List<FavoriteItem>();

            using (var cxn = new SqlConnection(cxnStr))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userId);

                favorites = cxn.Query<FavoriteItem>("ListingsSelectFavorites", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
            return favorites;
        }

        public IEnumerable<ListingItem> GetListings(string userId)
        {
            var listings = new List<ListingItem>();

            using (var cxn = new SqlConnection(cxnStr))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userId);

                listings = cxn.Query<ListingItem>("ListingsSelectByUser", parameters, commandType: CommandType.StoredProcedure).ToList();
            }            
            return listings;
        }

        public bool IsContact(string userId, int listingId)
        {
            using (var cxn = new SqlConnection(cxnStr))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userId);
                parameters.Add("@ListingId", listingId);

                return cxn.Query<bool>("ContactsSelect", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        public bool IsFavorite(string userId, int listingId)
        {
            using (var cxn = new SqlConnection(cxnStr))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userId);
                parameters.Add("@ListingId", listingId);

                return cxn.Query<bool>("FavoritesSelect", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        public void RemoveContact(string userId, int listingId)
        {
            using (var cxn = new SqlConnection(cxnStr))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userId);
                parameters.Add("@ListingId", listingId);

                cxn.Execute("ContactsDelete", parameters, commandType: CommandType.StoredProcedure);
            }            
        }

        public void RemoveFavorite(string userId, int listingId)
        {
            using (var cxn = new SqlConnection(cxnStr))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userId);
                parameters.Add("@ListingId", listingId);

                cxn.Execute("FavoritesDelete", parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
