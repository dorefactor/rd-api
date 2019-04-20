﻿using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;
using NUnit.Framework;
using RegularApi.Dao;
using RegularApi.Dao.Model;

namespace RegularApi.Tests.Dao
{
    public class ApplicationDaoTest : DaoTestBase
    {
        [SetUp]
        public void SetUp()
        {
            CreateTestServer();
        }

        [Test]
        public void TestDaoIsLoaded()
        {
            var dao = GetDao<IApplicationDao>();

            dao.Should().NotBeNull();
        }
        
        [Test]
        public async Task TestGetApplications()
        {
            var application = await CreateApplication("super-application-2k");
            
            var dao = GetDao<IApplicationDao>();
            var apps = await dao.GetApplicationsAsync();

            await DeleteApplication(application.Id);

            apps.Should().NotBeNull();            

            apps.Should().ContainEquivalentOf(application);
        }

        [Test]
        public async Task TestGetNonExistingApplication()
        {
            var dao = GetDao<IApplicationDao>();
            var appHolder = await dao.GetApplicationByNameAsync("non-existing-app");
            
            appHolder.Should<Application>().NotBeNull();
            appHolder.IsNone.Should().BeTrue();
        }

        [Test]
        public async Task TestGetApplicationByName()
        {
            var appName = "aka-aka-app";

            var application = await CreateApplication(appName);
            
            var dao = GetDao<IApplicationDao>();
            var appHolder = await dao.GetApplicationByNameAsync(appName);

            await DeleteApplication(application.Id);
            
            appHolder.Should<Application>().NotBeNull();
            appHolder.IsSome.Should().BeTrue();

            var result = appHolder.AsEnumerable().First();
            result.Should().BeEquivalentTo(application);
        }

        // ------------------------------------------------------------------------------------

        private async Task<Application> CreateApplication(string name)
        {
            var application = new Application
            {
                Name = name,
                Description = "blah, blah, blah"
            };

            var collection = GetCollection<Application>("applications");
            await collection.InsertOneAsync(application);
            return application;
        }

        private async Task<long> DeleteApplication(ObjectId id)
        {
            var collection = GetCollection<Application>("applications");
            var filter = new FilterDefinitionBuilder<Application>()
                .Where(app => id.Equals(app.Id));

            var deleteResult = await collection.DeleteOneAsync(filter);

            return deleteResult.DeletedCount;
        }
    }
}