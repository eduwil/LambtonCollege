using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Bson;
using MongoDB.Driver;
using webAPIMongoDB.Models;

namespace webAPIMongoDB.Services
{
    public class users
    {
        private MongoDbConnection conMongoDb = new MongoDbConnection();

        private MongoClient cliMongoDb;
        private IMongoDatabase datMongoDb;
        private IMongoCollection<UsersModel> colUsers;

        private const string cntColName = "users";

        /// <summary>
        /// create a new users object
        /// </summary>
        public users()
        {
            cliMongoDb = new MongoClient(conMongoDb.ConnectionString);
            datMongoDb = cliMongoDb.GetDatabase(conMongoDb.DatabaseName);
            colUsers = datMongoDb.GetCollection<UsersModel>(cntColName);
        }

        /// <summary>
        /// Get users list
        /// </summary>
        /// <returns></returns>
        public List<UsersModel> GetList()
        {
            return colUsers.Find("{}").ToList();
        }

        /// <summary>
        /// Get user information
        /// </summary>
        /// <param name="pNickname">User nickname</param>
        /// <returns></returns>
        public List<UsersModel> GetInfo(string pNickname)
        {
          //string filter = "{nickname:" + pNickname + "}"; //si el filtro se define de esta forma se tiene que enviar el parametro (en la URL) entre comillas
          FilterDefinition<UsersModel> filter = Builders<UsersModel>.Filter.Eq("nickname", pNickname);
          return colUsers.Find(filter).ToList();
        }

        /// <summary>
        /// Get user information
        /// </summary>
        /// <param name="pNickname">User nickname</param>
        /// <returns></returns>
        public async Task<List<UsersModel>> GetAsyncInfo(string pNickname)
        {
            FilterDefinition<UsersModel> filter = Builders<UsersModel>.Filter.Eq("nickname", pNickname);
            return await colUsers.FindAsync(filter).Result.ToListAsync();
        }

        /// <summary>
        /// Get user information by its id
        /// </summary>
        /// <param name="pId">User id</param>
        /// <returns></returns>
        public List<UsersModel> GetInfoById(string pId)
        {
            ObjectId userId = new ObjectId(pId);
            FilterDefinition<UsersModel> filter = Builders<UsersModel>.Filter.Eq("_id", userId);
            return colUsers.Find(filter).ToList();
        }

        /// <summary>
        /// Add a user
        /// </summary>
        /// <param name="pUser">User object</param>
        public void Add(UsersModel pUser)
        {
            colUsers.InsertOne(pUser);
        }

        /// <summary>
        /// Replace whole user information
        /// </summary>
        /// <param name="pUser">User object</param>
        public void Replace(UsersModel pUser)
        {
            ObjectId userId = new ObjectId(pUser.id);
            FilterDefinition<UsersModel> filter = Builders<UsersModel>.Filter.Eq("_id", userId);
            colUsers.ReplaceOne(filter, pUser);
        }

        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="pId">User Id</param>
        public void Delete(string pId) 
        {
            ObjectId userId = new ObjectId(pId);
            FilterDefinition<UsersModel> filter = Builders<UsersModel>.Filter.Eq("_id", userId);
            colUsers.DeleteOne(filter);
        }

    }
}
