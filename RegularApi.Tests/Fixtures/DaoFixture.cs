using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using RegularApi.Dao.Model;
using RegularApi.Tests.Dao;

namespace RegularApi.Tests.Fixtures
{
    public class DaoFixture : BaseDaoIT, IDaoFixture
    {
        public async Task<Application> CreateApplication(string name)
        {
            var collection = GetCollection<Application>("applications");

            var application = new Application()
            {
                Name = name
            };

            await collection.InsertOneAsync(application);

            return application;
        }

        public async Task<Application> GetApplicationById(ObjectId id)
        {
            var collection = GetCollection<Application>("applications");

            var filter = new FilterDefinitionBuilder<Application>().Where(application => application.Id.Equals(id));
            var cursor = await collection.FindAsync(filter);

            return await cursor.FirstOrDefaultAsync();
        }
    }
}