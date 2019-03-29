using ShackUp.Data.Interfaces;
using ShackUp.Models.Queries;
using ShackUp.Models.Tables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ShackUp.Data.ADO
{
    public class ListingsRepositoryADO : IListingsRepository
    {
        private static string cxnStr = Settings.GetConnectionString();

        public Listing ListingGetById(int listingId)
        {
            Listing listing = null;

            using (var cxn = new SqlConnection(cxnStr))           
            using (var cmd = new SqlCommand("ListingsSelect", (SqlConnection)cxn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ListingId", listingId);
                cxn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        listing = PopulateListingFromDataReader(dr);
                    }
                }
            }           
            return listing;
        }

        public IEnumerable<ListingShortItem> ListingGetRecent()
        {
            var listings = new List<ListingShortItem>();

            using (var cxn = new SqlConnection(cxnStr))
            using (var cmd = new SqlCommand("ListingsSelectRecent", (SqlConnection)cxn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cxn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        listings.Add(PopulateListingShortItemFromDataReader(dr));
                    }
                }
            }
            return listings;
        }

        public ListingItem ListingGetDetails(int listingId)
        {
            ListingItem listingItem = null;

            using (var cxn = new SqlConnection(cxnStr))
            using (var cmd = new SqlCommand("ListingsSelectDetails", (SqlConnection)cxn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ListingId", listingId);
                cxn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        listingItem = PopulateListingItemFromDataReader(dr);
                    }
                }
            }
            return listingItem;
        }

        public void ListingInsert(Listing listing)
        {
            using (var cxn = new SqlConnection(cxnStr))
            using (var cmd = new SqlCommand("ListingsInsert", (SqlConnection)cxn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", listing.UserId);
                cmd.Parameters.AddWithValue("@StateId", listing.StateId);
                cmd.Parameters.AddWithValue("@BathroomTypeId", listing.BathroomTypeId);
                cmd.Parameters.AddWithValue("@Nickname", listing.Nickname);
                cmd.Parameters.AddWithValue("@City", listing.City);
                cmd.Parameters.AddWithValue("@Rate", listing.Rate);
                cmd.Parameters.AddWithValue("@SquareFootage", listing.SquareFootage);
                cmd.Parameters.AddWithValue("@HasElectric", listing.HasElectric);
                cmd.Parameters.AddWithValue("@HasHeat", listing.HasHeat);
                cmd.Parameters.AddWithValue("@ListingDescription", listing.ListingDescription);

                if (string.IsNullOrEmpty(listing.ImageFileName))                
                    cmd.Parameters.AddWithValue("@ImageFileName", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@ImageFileName", listing.ImageFileName);

                cmd.Parameters.Add("@ListingId", SqlDbType.Int);
                cmd.Parameters["@ListingId"].Direction = ParameterDirection.Output;

                cxn.Open();
                cmd.ExecuteNonQuery();
                listing.ListingId = (int)cmd.Parameters["@ListingId"].Value;
            }
        }

        public void ListingUpdate(Listing listing)
        {
            using (var cxn = new SqlConnection(cxnStr))
            using (var cmd = new SqlCommand("ListingsUpdate", (SqlConnection)cxn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ListingId", listing.ListingId);
                cmd.Parameters.AddWithValue("@UserId", listing.UserId);
                cmd.Parameters.AddWithValue("@StateId", listing.StateId);
                cmd.Parameters.AddWithValue("@BathroomTypeId", listing.BathroomTypeId);
                cmd.Parameters.AddWithValue("@Nickname", listing.Nickname);
                cmd.Parameters.AddWithValue("@City", listing.City);
                cmd.Parameters.AddWithValue("@Rate", listing.Rate);
                cmd.Parameters.AddWithValue("@SquareFootage", listing.SquareFootage);
                cmd.Parameters.AddWithValue("@HasElectric", listing.HasElectric);
                cmd.Parameters.AddWithValue("@HasHeat", listing.HasHeat);
                cmd.Parameters.AddWithValue("@ListingDescription", listing.ListingDescription);
                cmd.Parameters.AddWithValue("@ImageFileName", listing.ImageFileName);

                cxn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void ListingDelete(int listingId)
        {
            using (var cxn = new SqlConnection(cxnStr))
            using (var cmd = new SqlCommand("ListingsDelete", (SqlConnection)cxn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ListingId", listingId);

                cxn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public IEnumerable<ListingShortItem> ListingSearch(ListingSearchParameters parameters)
        {
            var listings = new List<ListingShortItem>();

            using (var cxn = new SqlConnection(cxnStr))
            using (var cmd = new SqlCommand())
            {
                cmd.Connection = (SqlConnection)cxn;
                string query = "select top 12 ListingId, UserId, StateId, City, Rate, ImageFileName from Listings where 1 = 1";

                if (parameters.MinRate.HasValue)
                {
                    query += $"and Rate >= @MinRate ";
                    cmd.Parameters.AddWithValue("@MinRate", parameters.MinRate.Value);
                }
                if (parameters.MaxRate.HasValue)
                {
                    query += $"and Rate <= @MaxRate ";
                    cmd.Parameters.AddWithValue("@MaxRate", parameters.MaxRate.Value);
                }
                if (!string.IsNullOrEmpty(parameters.City))
                {
                    query += $"and City like @City ";
                    cmd.Parameters.AddWithValue("@City", parameters.City + '%');
                }
                if (!string.IsNullOrEmpty(parameters.StateId))
                {
                    query += $"and StateId like @StateId ";
                    cmd.Parameters.AddWithValue("@StateId", parameters.StateId);
                }
                query += "order by CreatedDate desc";
                cmd.CommandText = query;
                cxn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        listings.Add(PopulateListingShortItemFromDataReader(dr));
                    }
                }
            }
            return listings;
        }

        private Listing PopulateListingFromDataReader(SqlDataReader dr)
        {
            var listing = new Listing();

            listing.ListingId = (int)dr["ListingId"];
            listing.UserId = dr["UserId"].ToString();
            listing.StateId = dr["StateId"].ToString();
            listing.BathroomTypeId = (int)dr["BathroomTypeId"];
            listing.Nickname = dr["Nickname"].ToString();
            listing.City = dr["City"].ToString();
            listing.Rate = (decimal)dr["Rate"];
            listing.SquareFootage = (decimal)dr["SquareFootage"];
            listing.HasElectric = (bool)dr["HasElectric"];
            listing.HasHeat = (bool)dr["HasHeat"];

            if (dr["ListingDescription"] != DBNull.Value)
                listing.ListingDescription = dr["ListingDescription"].ToString();

            if (dr["ImageFileName"] != DBNull.Value)
                listing.ImageFileName = dr["ImageFileName"].ToString();

            return listing;
        }

        private ListingShortItem PopulateListingShortItemFromDataReader(SqlDataReader dr)
        {
            var listingShortItem = new ListingShortItem();

            listingShortItem.ListingId = (int)dr["ListingId"];
            listingShortItem.UserId = dr["UserId"].ToString();
            listingShortItem.StateId = dr["StateId"].ToString();
            listingShortItem.City = dr["City"].ToString();
            listingShortItem.Rate = (decimal)dr["Rate"];
            
            if (dr["ImageFileName"] != DBNull.Value)
                listingShortItem.ImageFileName = dr["ImageFileName"].ToString();

            return listingShortItem;
        }

        private ListingItem PopulateListingItemFromDataReader(SqlDataReader dr)
        {
            var listingItem = new ListingItem();

            listingItem.ListingId = (int)dr["ListingId"];
            listingItem.UserId = dr["UserId"].ToString();
            listingItem.StateId = dr["StateId"].ToString();
            listingItem.BathroomTypeId = (int)dr["BathroomTypeId"];
            listingItem.BathroomTypeName = dr["BathroomTypeName"].ToString();
            listingItem.Nickname = dr["Nickname"].ToString();
            listingItem.City = dr["City"].ToString();
            listingItem.Rate = (decimal)dr["Rate"];
            listingItem.SquareFootage = (decimal)dr["SquareFootage"];
            listingItem.HasElectric = (bool)dr["HasElectric"];
            listingItem.HasHeat = (bool)dr["HasHeat"];

            if (dr["ListingDescription"] != DBNull.Value)
                listingItem.ListingDescription = dr["ListingDescription"].ToString();

            if (dr["ImageFileName"] != DBNull.Value)
                listingItem.ImageFileName = dr["ImageFileName"].ToString();

            return listingItem;
        }

       
    }
}
