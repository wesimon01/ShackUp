using ShackUp.Data.Interfaces;
using ShackUp.Models.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ShackUp.Data.ADO
{
    public class AccountRepositoryADO : IAccountRepository
    {
        private static string cxnStr = Settings.GetConnectionString();

        public IEnumerable<ContactRequestItem> GetContacts(string userId)
        {
            var contacts = new List<ContactRequestItem>();

            using (var cxn = new SqlConnection(cxnStr))
            using (var cmd = new SqlCommand("ListingsSelectContacts", (SqlConnection)cxn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);
                cxn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        contacts.Add(PopulateContactRequestItemFromDataReader(dr));
                    }
                }
            }
            return contacts;
        }

        public IEnumerable<FavoriteItem> GetFavorites(string userId)
        {
            var favorites = new List<FavoriteItem>();

            using (var cxn = new SqlConnection(cxnStr))
            using (var cmd = new SqlCommand("ListingsSelectFavorites", (SqlConnection)cxn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);
                cxn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        favorites.Add(PopulateFavoriteItemFromDataReader(dr));
                    }
                }
            }
            return favorites;
        }

        public IEnumerable<ListingItem> GetListings(string userId)
        {
            var listings = new List<ListingItem>();

            using (var cxn = new SqlConnection(cxnStr))
            using (var cmd = new SqlCommand("ListingsSelectByUser", (SqlConnection)cxn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);
                cxn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        listings.Add(PopulateListingItemFromDataReader(dr));
                    }
                }
            }
            return listings;
        }

        public void AddFavorite(string userId, int listingId)
        {
            using (var cxn = new SqlConnection(cxnStr))
            using (var cmd = new SqlCommand("FavoritesInsert", (SqlConnection)cxn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@ListingId", listingId);

                cxn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void RemoveFavorite(string userId, int listingId)
        {
            using (var cxn = new SqlConnection(cxnStr))
            using (var cmd = new SqlCommand("FavoritesDelete", (SqlConnection)cxn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@ListingId", listingId);

                cxn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void AddContact(string userId, int listingId)
        {
            using (var cxn = new SqlConnection(cxnStr))
            using (var cmd = new SqlCommand("ContactsInsert", (SqlConnection)cxn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@ListingId", listingId);

                cxn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void RemoveContact(string userId, int listingId)
        {
            using (var cxn = new SqlConnection(cxnStr))
            using (var cmd = new SqlCommand("ContactsDelete", (SqlConnection)cxn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@ListingId", listingId);

                cxn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private FavoriteItem PopulateFavoriteItemFromDataReader(SqlDataReader dr)
        {
            var favoriteItem = new FavoriteItem();

            favoriteItem.ListingId = (int)dr["ListingId"];
            favoriteItem.UserId = dr["UserId"].ToString();
            favoriteItem.StateId = dr["StateId"].ToString();
            favoriteItem.BathroomTypeId = (int)dr["BathroomTypeId"];
            favoriteItem.BathroomTypeName = dr["BathroomTypeName"].ToString();
            favoriteItem.City = dr["City"].ToString();
            favoriteItem.Rate = (decimal)dr["Rate"];
            favoriteItem.SquareFootage = (decimal)dr["SquareFootage"];
            favoriteItem.HasElectric = (bool)dr["HasElectric"];
            favoriteItem.HasHeat = (bool)dr["HasHeat"];

            return favoriteItem;
        }

        private ContactRequestItem PopulateContactRequestItemFromDataReader(SqlDataReader dr)
        {
            var contactRequestItem = new ContactRequestItem();

            contactRequestItem.ListingId = (int)dr["ListingId"];
            contactRequestItem.Email = dr["Email"].ToString();
            contactRequestItem.UserId = dr["UserId"].ToString();
            contactRequestItem.Nickname = dr["Nickname"].ToString();
            contactRequestItem.City = dr["City"].ToString();
            contactRequestItem.StateId = dr["StateId"].ToString();           
            contactRequestItem.Rate = (decimal)dr["Rate"];
            
            return contactRequestItem;
        }

        private ListingItem PopulateListingItemFromDataReader(SqlDataReader dr)
        {
            var listingItem = new ListingItem();

            listingItem.ListingId = (int)dr["ListingId"];
            listingItem.UserId = dr["UserId"].ToString();
            listingItem.Nickname = dr["Nickname"].ToString();
            listingItem.City = dr["City"].ToString();
            listingItem.StateId = dr["StateId"].ToString();
            listingItem.Rate = (decimal)dr["Rate"];
            listingItem.BathroomTypeId = (int)dr["BathroomTypeId"];
            listingItem.BathroomTypeName = dr["BathroomTypeName"].ToString();
            listingItem.SquareFootage = (decimal)dr["SquareFootage"];
            listingItem.HasElectric = (bool)dr["HasElectric"];
            listingItem.HasHeat = (bool)dr["HasHeat"];

            if (dr["ImageFileName"] != DBNull.Value)
                listingItem.ImageFileName = dr["ImageFileName"].ToString();

            return listingItem;
        }

        public bool IsFavorite(string userId, int listingId)
        {
            using (var cxn = new SqlConnection(cxnStr))
            using (var cmd = new SqlCommand("FavoritesSelect", (SqlConnection)cxn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@ListingId", listingId);
                cxn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    return dr.Read();
                }
            }
        }

        public bool IsContact(string userId, int listingId)
        {
            using (var cxn = new SqlConnection(cxnStr))
            using (var cmd = new SqlCommand("ContactsSelect", (SqlConnection)cxn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@ListingId", listingId);
                cxn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    return dr.Read();
                }
            }
        }
    }
}
