using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using ShackUp.Data.Dapper;
using ShackUp.Models.Tables;
using ShackUp.Models.Queries;

namespace ShackUp.Tests.IntegrationTests
{
    [TestFixture]
    public class DapperTests
    {
        [SetUp]
        public void Init()
        {
            using (var cxn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            using (var cmd = new SqlCommand("DbReset", cxn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cxn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        [Test]
        public void CanLoadStates()
        {
            var repo = new StatesRepositoryDapper();
            var states = repo.StateGetAll();

            Assert.AreEqual(3, states.Count);
            Assert.AreEqual("KY", states[0].StateId);
            Assert.AreEqual("Kentucky", states[0].StateName);
        }

        [Test]
        public void CanLoadBathroomTypes()
        {
            var repo = new BathroomTypesRepositoryDapper();
            var bathroomTypes = repo.BathroomTypeGetAll();

            Assert.AreEqual(3, bathroomTypes.Count);
            Assert.AreEqual(1, bathroomTypes[0].BathroomTypeId);
            Assert.AreEqual("Indoor", bathroomTypes[0].BathroomTypeName);
        }

        [Test]
        public void CanLoadListing()
        {
            var repo = new ListingsRepositoryDapper();
            Listing listing = repo.ListingGetById(1);
            Assert.IsNotNull(listing);

            //1, '00000000-0000-0000-0000-000000000000', 'OH', 1, 'Test shack 1', 'Cleveland', 120, 400, 0, 1, 'placeholder.png'
            Assert.AreEqual(1, listing.ListingId);
            Assert.AreEqual(Guid.Empty.ToString(), listing.UserId);
            Assert.AreEqual("OH", listing.StateId);
            Assert.AreEqual(1, listing.BathroomTypeId);
            Assert.AreEqual("Test shack 1", listing.Nickname);
            Assert.AreEqual("Cleveland", listing.City);
            Assert.AreEqual(100M, listing.Rate);
            Assert.AreEqual(400M, listing.SquareFootage);
            Assert.AreEqual(false, listing.HasElectric);
            Assert.AreEqual(true, listing.HasHeat);
            Assert.AreEqual("Description 1", listing.ListingDescription);
            Assert.AreEqual("placeholder.png", listing.ImageFileName);
        }

        [Test]
        public void NotFoundListingReturnsNull()
        {
            var repo = new ListingsRepositoryDapper();
            Listing listing = repo.ListingGetById(10000);
            Assert.IsNull(listing);
        }

        [Test]
        public void CanAddListing()
        {
            var listingToAdd = new Listing();
            var repo = new ListingsRepositoryDapper();

            listingToAdd.UserId = Guid.Empty.ToString();
            listingToAdd.StateId = "OH";
            listingToAdd.BathroomTypeId = 1;
            listingToAdd.Nickname = "Test Shack 1";
            listingToAdd.City = "Columbus";
            listingToAdd.Rate = 50M;
            listingToAdd.SquareFootage = 100M;
            listingToAdd.HasElectric = true;
            listingToAdd.HasHeat = true;
            listingToAdd.ListingDescription = "Test Description";
            listingToAdd.ImageFileName = "placeholder.png";

            repo.ListingInsert(listingToAdd);
            Assert.AreEqual(7, listingToAdd.ListingId);
        }

        [Test]
        public void CanUpdateListing()
        {
            var listingToAdd = new Listing();
            var repo = new ListingsRepositoryDapper();

            listingToAdd.UserId = Guid.Empty.ToString();
            listingToAdd.StateId = "OH";
            listingToAdd.BathroomTypeId = 1;
            listingToAdd.Nickname = "Test Shack 1";
            listingToAdd.City = "Columbus";
            listingToAdd.Rate = 50M;
            listingToAdd.SquareFootage = 100M;
            listingToAdd.HasElectric = true;
            listingToAdd.HasHeat = true;
            listingToAdd.ListingDescription = "Test Description";
            listingToAdd.ImageFileName = "placeholder.png";

            repo.ListingInsert(listingToAdd);

            listingToAdd.StateId = "KY";
            listingToAdd.Nickname = "My Updated Shack";
            listingToAdd.BathroomTypeId = 2;
            listingToAdd.City = "Louisville";
            listingToAdd.Rate = 25M;
            listingToAdd.SquareFootage = 75M;
            listingToAdd.HasElectric = false;
            listingToAdd.HasHeat = false;
            listingToAdd.ListingDescription = "Test Description Updated";
            listingToAdd.ImageFileName = "updated.png";

            repo.ListingUpdate(listingToAdd);
            var updatedListing = repo.ListingGetById(7);

            Assert.AreEqual("KY", updatedListing.StateId);
            Assert.AreEqual("My Updated Shack", updatedListing.Nickname);
            Assert.AreEqual(2, updatedListing.BathroomTypeId);
            Assert.AreEqual("Louisville", updatedListing.City);
            Assert.AreEqual(25M, updatedListing.Rate);
            Assert.AreEqual(75M, updatedListing.SquareFootage);
            Assert.AreEqual(false, updatedListing.HasElectric);
            Assert.AreEqual(false, updatedListing.HasHeat);
            Assert.AreEqual("Test Description Updated", updatedListing.ListingDescription);
            Assert.AreEqual("updated.png", updatedListing.ImageFileName);
        }

        [Test]
        public void CanDeleteListing()
        {
            var listingToAdd = new Listing();
            var repo = new ListingsRepositoryDapper();

            listingToAdd.UserId = Guid.Empty.ToString();
            listingToAdd.StateId = "OH";
            listingToAdd.BathroomTypeId = 1;
            listingToAdd.Nickname = "Test Shack 1";
            listingToAdd.City = "Columbus";
            listingToAdd.Rate = 50M;
            listingToAdd.SquareFootage = 100M;
            listingToAdd.HasElectric = true;
            listingToAdd.HasHeat = true;
            listingToAdd.ListingDescription = "Test Description";
            listingToAdd.ImageFileName = "placeholder.png";

            repo.ListingInsert(listingToAdd);

            var loaded = repo.ListingGetById(7);
            Assert.IsNotNull(loaded);

            repo.ListingDelete(7);
            loaded = repo.ListingGetById(7);

            Assert.IsNull(loaded);
        }

        [Test]
        public void CanLoadRecentListings()
        {
            var repo = new ListingsRepositoryDapper();
            List<ListingShortItem> listings = repo.ListingGetRecent().ToList();

            Assert.AreEqual(5, listings.Count());

            Assert.AreEqual(6, listings[0].ListingId);
            Assert.AreEqual(Guid.Empty.ToString(), listings[0].UserId);
            Assert.AreEqual(150M, listings[0].Rate);
            Assert.AreEqual("Cleveland", listings[0].City);
            Assert.AreEqual("OH", listings[0].StateId);
            Assert.AreEqual("placeholder.png", listings[0].ImageFileName);

        }

        [Test]
        public void CanLoadListingDetails()
        {
            var repo = new ListingsRepositoryDapper();
            ListingItem listingItem = repo.ListingGetDetails(1);
            Assert.IsNotNull(listingItem);

            //1, '00000000-0000-0000-0000-000000000000', 'OH', 1, 'Test shack 1', 'Cleveland', 120, 400, 0, 1, 'placeholder.png'
            Assert.AreEqual(1, listingItem.ListingId);
            Assert.AreEqual(Guid.Empty.ToString(), listingItem.UserId);
            Assert.AreEqual("OH", listingItem.StateId);
            Assert.AreEqual(1, listingItem.BathroomTypeId);
            Assert.AreEqual("Indoor", listingItem.BathroomTypeName);
            Assert.AreEqual("Test shack 1", listingItem.Nickname);
            Assert.AreEqual("Cleveland", listingItem.City);
            Assert.AreEqual(100M, listingItem.Rate);
            Assert.AreEqual(400M, listingItem.SquareFootage);
            Assert.AreEqual(false, listingItem.HasElectric);
            Assert.AreEqual(true, listingItem.HasHeat);
            Assert.AreEqual("Description 1", listingItem.ListingDescription);
            Assert.AreEqual("placeholder.png", listingItem.ImageFileName);
        }

        [Test]
        public void CanLoadFavorites()
        {
            var repo = new AccountRepositoryDapper();
            var favorites = repo.GetFavorites("11111111-1111-1111-1111-111111111111").ToList();

            Assert.AreEqual(2, favorites.Count());
            Assert.AreEqual(1, favorites[0].ListingId);
            Assert.AreEqual("00000000-0000-0000-0000-000000000000", favorites[0].UserId);
            Assert.AreEqual("OH", favorites[0].StateId);
            Assert.AreEqual("Cleveland", favorites[0].City);
            Assert.AreEqual(100M, favorites[0].Rate);
            Assert.AreEqual(400M, favorites[0].SquareFootage);
            Assert.AreEqual(false, favorites[0].HasElectric);
            Assert.AreEqual(true, favorites[0].HasHeat);
            Assert.AreEqual(1, favorites[0].BathroomTypeId);
            Assert.AreEqual("Indoor", favorites[0].BathroomTypeName);
        }

        [Test]
        public void CanLoadContacts()
        {
            var repo = new AccountRepositoryDapper();
            var contacts = repo.GetContacts(Guid.Empty.ToString()).ToList();

            Assert.AreEqual(2, contacts.Count());
            Assert.AreEqual(1, contacts[0].ListingId);
            Assert.AreEqual("11111111-1111-1111-1111-111111111111", contacts[0].UserId);
            Assert.AreEqual("Test shack 1", contacts[0].Nickname);
            Assert.AreEqual("OH", contacts[0].StateId);
            Assert.AreEqual("Cleveland", contacts[0].City);
            Assert.AreEqual(100M, contacts[0].Rate);
            Assert.AreEqual("test2@test.com", contacts[0].Email);
        }

        [Test]
        public void CanLoadListingsForUser()
        {
            var repo = new AccountRepositoryDapper();
            var listings = repo.GetListings(Guid.Empty.ToString()).ToList();

            Assert.AreEqual(6, listings.Count());

            Assert.AreEqual(1, listings[0].ListingId);
            Assert.AreEqual("OH", listings[0].StateId);
            Assert.AreEqual(1, listings[0].BathroomTypeId);
            Assert.AreEqual("Indoor", listings[0].BathroomTypeName);
            Assert.AreEqual("Test shack 1", listings[0].Nickname);
            Assert.AreEqual("Cleveland", listings[0].City);
            Assert.AreEqual(100M, listings[0].Rate);
            Assert.AreEqual(400M, listings[0].SquareFootage);
            Assert.AreEqual(false, listings[0].HasElectric);
            Assert.AreEqual(true, listings[0].HasHeat);
            Assert.AreEqual("placeholder.png", listings[0].ImageFileName);
        }

        [Test]
        public void CanAddAndRemoveFavorites()
        {
            var repo = new AccountRepositoryDapper();
            repo.AddFavorite("11111111-1111-1111-1111-111111111111", 3);
            var favorites = repo.GetFavorites("11111111-1111-1111-1111-111111111111");

            Assert.AreEqual(3, favorites.Count());
            repo.RemoveFavorite("11111111-1111-1111-1111-111111111111", 2);
            favorites = repo.GetFavorites("11111111-1111-1111-1111-111111111111");

            Assert.AreEqual(2, favorites.Count());
        }

        [Test]
        public void CanAddAndRemoveContacts()
        {
            var repo = new AccountRepositoryDapper();
            repo.AddContact("11111111-1111-1111-1111-111111111111", 5);
            var contacts = repo.GetContacts(Guid.Empty.ToString());

            Assert.AreEqual(3, contacts.Count());
            repo.RemoveContact("11111111-1111-1111-1111-111111111111", 3);
            contacts = repo.GetContacts(Guid.Empty.ToString());

            Assert.AreEqual(2, contacts.Count());
        }

        [Test]
        public void CanDetectFavorite()
        {
            var repo = new AccountRepositoryDapper();
            var found = repo.IsFavorite("11111111-1111-1111-1111-111111111111", 2);
            Assert.IsTrue(found);

            found = repo.IsFavorite("11111111-1111-1111-1111-111111111111", 99);
            Assert.IsFalse(found);
        }

        [Test]
        public void CanDetectContact()
        {
            var repo = new AccountRepositoryDapper();
            var found = repo.IsContact("11111111-1111-1111-1111-111111111111", 1);
            Assert.IsTrue(found);

            found = repo.IsContact("11111111-1111-1111-1111-111111111111", 99);
            Assert.IsFalse(found);
        }

        [Test]
        public void CanSearchOnMinRate()
        {
            var repo = new ListingsRepositoryDapper();
            var found = repo.ListingSearch(new ListingSearchParameters { MinRate = 110M });

            Assert.AreEqual(5, found.Count());
        }

        [Test]
        public void CanSearchOnMaxRate()
        {
            var repo = new ListingsRepositoryDapper();
            var found = repo.ListingSearch(new ListingSearchParameters { MaxRate = 110M });

            Assert.AreEqual(2, found.Count());
        }

        [Test]
        public void CanSearchOnRange()
        {
            var repo = new ListingsRepositoryDapper();
            var found = repo.ListingSearch(new ListingSearchParameters { MaxRate = 120M, MinRate = 100M });

            Assert.AreEqual(3, found.Count());
        }

        [Test]
        public void CanSearchOnCity()
        {
            var repo = new ListingsRepositoryDapper();
            var found = repo.ListingSearch(new ListingSearchParameters { City = "Cleveland" });

            Assert.AreEqual(6, found.Count());
        }
    }
}
