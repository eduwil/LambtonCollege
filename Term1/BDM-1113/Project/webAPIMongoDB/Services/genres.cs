using MongoDB.Bson;
using MongoDB.Driver;
using webAPIMongoDB.Models;

namespace webAPIMongoDB.Services
{
    public class genres
    {
        private MongoDbConnection conMongoDb = new MongoDbConnection();

        private MongoClient cliMongoDb;
        private IMongoDatabase datMongoDb;
        private IMongoCollection<GenresModel> colGenres;

        private const string cntColName = "genres";

        /// <summary>
        /// Create a new genres object
        /// </summary>
        public genres()
        {
            cliMongoDb = new MongoClient(conMongoDb.ConnectionString);
            datMongoDb = cliMongoDb.GetDatabase(conMongoDb.DatabaseName);
            colGenres = datMongoDb.GetCollection<GenresModel>(cntColName);
        }

        /// <summary>
        /// Get genres list
        /// </summary>
        /// <returns></returns>
        public List<GenresModel> GetList()
        {
            SortDefinition<GenresModel> sortDefinition = Builders<GenresModel>.Sort.Ascending("name");
            return colGenres.Find("{}").Sort(sortDefinition).ToList();
        }

        /// <summary>
        /// Get genres list
        /// </summary>
        /// <returns></returns>
        public async Task<List<GenresModel>> GetAsyncList()
        {
            return await colGenres.FindAsync("{}").Result.ToListAsync();
        }

        /// <summary>
        /// Add a genre
        /// </summary>
        /// <param name="pGenre">Genre object</param>
        public void Add(GenresModel pGenre)
        {
            colGenres.InsertOne(pGenre);
        }

        /// <summary>
        /// Replace whole genre information
        /// </summary>
        /// <param name="pGenre">Genre object</param>
        public void Replace(GenresModel pGenre)
        {
            ObjectId genreId = new ObjectId(pGenre.id);
            FilterDefinition<GenresModel> filter = Builders<GenresModel>.Filter.Eq("_id", genreId);
            colGenres.ReplaceOne(filter, pGenre);
        }

        /// <summary>
        /// Delete a genre
        /// </summary>
        /// <param name="pId">Genre Id</param>
        public void Delete(string pId)
        {
            ObjectId genreId = new ObjectId(pId);
            FilterDefinition<GenresModel> filter = Builders<GenresModel>.Filter.Eq("_id", genreId);
            colGenres.DeleteOne(filter);
        }


    }
}
