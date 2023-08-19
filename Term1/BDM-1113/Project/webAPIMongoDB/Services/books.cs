using MongoDB.Bson;
using MongoDB.Driver;
using System.Net.NetworkInformation;
using webAPIMongoDB.Models;

namespace webAPIMongoDB.Services
{
    public class books
    {
        private MongoDbConnection conMongoDb = new MongoDbConnection();

        private MongoClient cliMongoDb;
        private IMongoDatabase datMongoDb;
        private IMongoCollection<BooksModel> colBooks;

        private const string cntColName = "books";

        /// <summary>
        /// Create a new books object
        /// </summary>
        public books()
        {
            cliMongoDb = new MongoClient(conMongoDb.ConnectionString);
            datMongoDb = cliMongoDb.GetDatabase(conMongoDb.DatabaseName);
            colBooks = datMongoDb.GetCollection<BooksModel>(cntColName);
        }

        /// <summary>
        /// Get books list
        /// </summary>
        /// <returns></returns>
        public List<BooksModel> GetList()
        {
            return colBooks.Find("{}").ToList();
        }

        /// <summary>
        /// Get the list of recommended books for the user
        /// </summary>
        /// <param name="pInterests">Interests of the user</param>
        /// <param name="pExclude">Book list user has already related to</param>
        /// <returns></returns>

        public List<BooksRateModel> GetRecommendedList(List<string> pInterests, List<string> pExclude)
        {
            /* https://www.mongodb.com/developer/languages/csharp/joining-collections-mongodb-dotnet-core-aggregation-pipeline/
               https://gist.github.com/peters/66abc9be2f6334e9d68603697b29d74f
             
            {
                "$lookup", new BsonDocument{
                        { "from", "movies" },
                        { "localField", "items" },
                        { "foreignField", "_id" },
                        { "as", "movies" }}
            }
    
            */

            FilterDefinition<BooksModel> filter = Builders<BooksModel>.Filter.In("genre", pInterests);
            filter &= Builders<BooksModel>.Filter.Nin("ISBN", pExclude);

            return colBooks.Aggregate().Match(filter).Lookup("user_books", "ISBN", "ISBN", "bookRate").As<BooksRateModel>().ToList();
        }



/// <summary>
/// Get book information
/// </summary>
/// <param name="pISBN">ISBN</param>
/// <returns></returns>
public List<BooksModel> GetInfo(string pISBN) 
{
FilterDefinition<BooksModel> filter = Builders<BooksModel>.Filter.Eq("ISBN", pISBN);
return colBooks.Find(filter).ToList();
}

/// <summary>
/// Get book information by its id
/// </summary>
/// <param name="pId">Book id</param>
/// <returns></returns>
public List<BooksModel> GetInfoById(string pId)
{
ObjectId bookId = new ObjectId(pId);
FilterDefinition<BooksModel> filter = Builders<BooksModel>.Filter.Eq("_id", bookId);
return colBooks.Find(filter).ToList();
}

/// <summary>
/// Add a book
/// </summary>
/// <param name="pBook">Book object</param>
public void Add(BooksModel pBook)
{
colBooks.InsertOne(pBook);
}

/// <summary>
/// Replace whole book information
/// </summary>
/// <param name="pBook">Book object</param>
public void Replace(BooksModel pBook)
{
ObjectId bookId = new ObjectId(pBook.id);
FilterDefinition<BooksModel> filter = Builders<BooksModel>.Filter.Eq("_id", bookId);
colBooks.ReplaceOne(filter, pBook);
}

/// <summary>
/// Delete a book
/// </summary>
/// <param name="pId">Book Id</param>
public void Delete(string pId)
{
ObjectId bookId = new ObjectId(pId);
FilterDefinition<BooksModel> filter = Builders<BooksModel>.Filter.Eq("_id", bookId);
colBooks.DeleteOne(filter);
}


}
}
