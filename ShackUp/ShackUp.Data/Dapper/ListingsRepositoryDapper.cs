using Dapper;
using ShackUp.Data.Interfaces;
using ShackUp.Models.Queries;
using ShackUp.Models.Tables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ShackUp.Data.Dapper
{
    public class ListingsRepositoryDapper : IListingsRepository
    {
        private static string cxnStr = Settings.GetConnectionString();

        public void ListingDelete(int listingId)
        {
            using (var cxn = new SqlConnection(cxnStr))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ListingId", listingId);

                cxn.Execute("ListingsDelete", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public Listing ListingGetById(int listingId)
        {
            Listing listing = null;

            using (var cxn = new SqlConnection(cxnStr))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ListingId", listingId);

                listing = cxn.Query<Listing>("ListingsSelect", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }            
            return listing;
        }

        public ListingItem ListingGetDetails(int listingId)
        {
            ListingItem listingItem = null;

            using (var cxn = new SqlConnection(cxnStr))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ListingId", listingId);
                listingItem = cxn.Query<ListingItem>("ListingsSelectDetails", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            return listingItem;
        }

        public IEnumerable<ListingShortItem> ListingGetRecent()
        {
            var listings = new List<ListingShortItem>();

            using (var cxn = new SqlConnection(cxnStr))
            {
                listings = cxn.Query<ListingShortItem>("ListingsSelectRecent", commandType: CommandType.StoredProcedure).ToList();
            }
            return listings;
        }

        public void ListingInsert(Listing listing)
        {
            using (var cxn = new SqlConnection(cxnStr))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", listing.UserId);
                parameters.Add("@StateId", listing.StateId);
                parameters.Add("@BathroomTypeId", listing.BathroomTypeId);
                parameters.Add("@Nickname", listing.Nickname);
                parameters.Add("@City", listing.City);
                parameters.Add("@Rate", listing.Rate);
                parameters.Add("@SquareFootage", listing.SquareFootage);
                parameters.Add("@HasElectric", listing.HasElectric);
                parameters.Add("@HasHeat", listing.HasHeat);
                parameters.Add("@ListingDescription", listing.ListingDescription);
                
                if (string.IsNullOrEmpty(listing.ImageFileName))
                    parameters.Add("@ImageFileName", DBNull.Value);
                else
                    parameters.Add("@ImageFileName", listing.ImageFileName);

                parameters.Add("@ListingId", listing.ListingId, DbType.Int32, ParameterDirection.Output);

                cxn.Execute("ListingsInsert", parameters, commandType: CommandType.StoredProcedure);
                listing.ListingId = parameters.Get<int>("@ListingId");
            }           
        }

        public IEnumerable<ListingShortItem> ListingSearch(ListingSearchParameters parameters)
        {
            var listings = new List<ListingShortItem>();

            using (var cxn = new SqlConnection(cxnStr))
            {
                string query = "select top 12 ListingId, UserId, StateId, City, Rate, ImageFileName from Listings where 1 = 1 ";
                var dymprms = new DynamicParameters();

                if (parameters.MinRate.HasValue)
                {
                    query += $"and Rate >= @MinRate ";
                    dymprms.Add("@MinRate", parameters.MinRate.Value);
                }
                if (parameters.MaxRate.HasValue)
                {
                    query += $"and Rate <= @MaxRate ";
                    dymprms.Add("@MaxRate", parameters.MaxRate.Value);
                }
                if (!string.IsNullOrEmpty(parameters.City))
                {
                    query += $"and City like @City ";
                    dymprms.Add("@City", parameters.City + "%");
                }
                if (!string.IsNullOrEmpty(parameters.StateId))
                {
                    query += $"and StateId like @StateId ";
                    dymprms.Add("@StateId", parameters.StateId);
                }
                query += "order by CreatedDate desc";

                listings = cxn.Query<ListingShortItem>(query, dymprms, commandType: CommandType.Text).ToList();
            }
            return listings;
        }

        public void ListingUpdate(Listing listing)
        {
            using (var cxn = new SqlConnection(cxnStr))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ListingId", listing.ListingId);
                parameters.Add("@UserId", listing.UserId);
                parameters.Add("@StateId", listing.StateId);
                parameters.Add("@BathroomTypeId", listing.BathroomTypeId);
                parameters.Add("@Nickname", listing.Nickname);
                parameters.Add("@City", listing.City);
                parameters.Add("@Rate", listing.Rate);
                parameters.Add("@SquareFootage", listing.SquareFootage);
                parameters.Add("@HasElectric", listing.HasElectric);
                parameters.Add("@HasHeat", listing.HasHeat);
                parameters.Add("@ListingDescription", listing.ListingDescription);
                parameters.Add("@ImageFileName", listing.ImageFileName);

                cxn.Execute("ListingsUpdate", parameters, commandType: CommandType.StoredProcedure);

            }           
        }
    }
}
