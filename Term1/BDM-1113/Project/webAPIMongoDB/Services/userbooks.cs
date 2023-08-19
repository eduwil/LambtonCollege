using MongoDB.Bson;
using MongoDB.Driver;
using System.Security.Cryptography;
using System.Xml.Linq;
using webAPIMongoDB.Models;

namespace webAPIMongoDB.Services
{
    public class userbooks
    {
        private MongoDbConnection conMongoDb = new MongoDbConnection();

        private MongoClient cliMongoDb;
        private IMongoDatabase datMongoDb;
        private IMongoCollection<UserBooksModel> colUserBooks;

        private const string cntColName = "user_books";

        /// <summary>
        /// create a new userbooks object
        /// </summary>
        public userbooks() 
        {
            cliMongoDb = new MongoClient(conMongoDb.ConnectionString);
            datMongoDb = cliMongoDb.GetDatabase(conMongoDb.DatabaseName);
            colUserBooks = datMongoDb.GetCollection<UserBooksModel>(cntColName);
        }

        /// <summary>
        /// Get relationship User-Books list
        /// </summary>
        /// <returns></returns>
        public List<UserBooksModel> GetList()
        {
            return colUserBooks.Find("{}").ToList();
        }

        /// <summary>
        /// Get the list of viewed or read books by the user
        /// </summary>
        /// <param name="pNickname">User's nickname</param>
        /// <returns></returns>
        public List<UserBooksModel> GetUserBooksList(string pNickname)
        {
            FilterDefinition<UserBooksModel> filter = Builders<UserBooksModel>.Filter.Eq("nickname", pNickname);
            return colUserBooks.Find(filter).ToList();
        }

        /// <summary>
        /// Get the ISBN list of books related to the user
        /// </summary>
        /// <param name="pNickname">User nickname</param>
        /// <returns></returns>
        public List<MongoDB.Bson.BsonDocument> GetUserBookListISBN(string pNickname)
        {
            FilterDefinition<UserBooksModel> filter = Builders<UserBooksModel>.Filter.Eq("nickname", pNickname);
            ProjectionDefinition<UserBooksModel> projection = Builders<UserBooksModel>.Projection.Include("ISBN").Exclude("_id");
            return colUserBooks.Find(filter).Project(projection).ToList();  
        }

        /// <summary>
        /// Get the reviews of a book
        /// </summary>
        /// <param name="pISBN">Book ISBN</param>
        /// <param name="pState">State of review</param>
        /// <returns></returns>
        public List<UserBooksModel> GetReviews(string pISBN, string pState) 
        {
            FilterDefinition<UserBooksModel> filter = Builders<UserBooksModel>.Filter.Eq("ISBN", pISBN);
            filter &= Builders<UserBooksModel>.Filter.Eq("state", pState);
            return colUserBooks.Find(filter).ToList();
        }

        /// <summary>
        /// Add a relationship between a user and a book
        /// </summary>
        /// <param name="pUserBook">User-Book object</param>
        public void Add(UserBooksModel pUserBook)
        {
            colUserBooks.InsertOne(pUserBook);
        }

        /// <summary>
        /// Update user-book relationship state
        /// </summary>
        /// <param name="nickname">User nickname</param>
        /// <param name="isbn">Book ISBN</param>
        /// <param name="date">Current date (yyyy-MM-dd)</param>
        /// <param name="state">State</param>
        public void UpdateState(string pNickname, string pISBN, string pDate, string pState)
        {
            //https://www.mongodb.com/docs/drivers/csharp/current/fundamentals/builders/
            //The & operator is overloaded. Other overloaded operators include the | operator for “or” and the ! operator for “not”.

            //FilterDefinition<UserBooksModel> filter = Builders<UserBooksModel>.Filter.Eq("nickname", pNickname) & Builders<UserBooksModel>.Filter.Eq("ISBN", pISBN); //AND
            //FilterDefinition<UserBooksModel> filter = Builders<UserBooksModel>.Filter.Eq("nickname", pNickname) | Builders<UserBooksModel>.Filter.Eq("ISBN", pISBN); //OR

            FilterDefinition<UserBooksModel> filter = Builders<UserBooksModel>.Filter.Eq("nickname", pNickname);
            filter &= Builders<UserBooksModel>.Filter.Eq("ISBN", pISBN);

            UpdateDefinition<UserBooksModel> update = Builders<UserBooksModel>.Update.Set("reading_date", pDate).Set("state", pState);
            
            colUserBooks.UpdateOne(filter, update);
        }

        /// <summary>
        /// Rate a book
        /// </summary>
        /// <param name="pNickname">User nickname</param>
        /// <param name="pISBN">Book ISBN</param>
        /// <param name="pDate">Current date (yyyy-MM-dd)</param>
        /// <param name="pState">State</param>
        /// <param name="pRate">Book rate</param>
        /// <param name="pComment">Comments about the book</param>
        public void Rate(string pNickname, string pISBN, string pDate, string pState, short pRate, string pComment) 
        {
            FilterDefinition<UserBooksModel> filter = Builders<UserBooksModel>.Filter.Eq("nickname", pNickname);
            filter &= Builders<UserBooksModel>.Filter.Eq("ISBN", pISBN);

            UpdateDefinition<UserBooksModel> update = Builders<UserBooksModel>.Update.Set("read_date", pDate)
                                                                                     .Set("state", pState)
                                                                                     .Set("rate", pRate)
                                                                                     .Set("comment", pComment);

            colUserBooks.UpdateOne(filter, update);
        }

        /// <summary>
        /// Delete a User-Book relationship
        /// </summary>
        /// <param name="pId">Relation Id</param>
        public void Delete(string pId)
        {
            ObjectId userBookId = new ObjectId(pId);
            FilterDefinition<UserBooksModel> filter = Builders<UserBooksModel>.Filter.Eq("_id", userBookId);
            colUserBooks.DeleteOne(filter);
        }




    }
}
