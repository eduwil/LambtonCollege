using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using webAPIMongoDB.Models;
using webAPIMongoDB.Services;

namespace webAPIMongoDB.Controllers
{
    [ApiController]
    [Route("api/{controller}/{action}")]

    public class BooksCtrl : ControllerBase
    {
        private books booksTmp;

        [HttpGet]
        public List<BooksModel> GetList()
        {
            booksTmp = new books();
            var listBooks = booksTmp.GetList();
            return listBooks;
        }

        [HttpGet("{nickname}")]
        public List<BooksRateModel> GetRecommendedList(string nickname)
        {
            List<BooksRateModel> listBooks = new();

            // get user information (interests)
            users usersTmp = new users();
            List<UsersModel> lstUsers = usersTmp.GetInfo(nickname);

            // get books related to the user
            userbooks usrBooksTmp = new userbooks();
            List<MongoDB.Bson.BsonDocument> lstUserBooks = usrBooksTmp.GetUserBookListISBN(nickname);
            List<string> lstISBNUser = new();

            foreach (var document in lstUserBooks)
            {
                 lstISBNUser.Add(document["ISBN"].ToString());
            }

            if (lstUsers.Count > 0) {
                UsersModel userTmp = lstUsers[0];
                booksTmp = new books();
                listBooks = booksTmp.GetRecommendedList(userTmp.interests, lstISBNUser);
            }

            return listBooks;
        }

        [HttpGet("{isbn}")]
        public List<BooksModel> GetInfo(string isbn) 
        {
            booksTmp = new books();
            var listBooks = booksTmp.GetInfo(isbn);
            return listBooks;
        }

        [HttpGet("{id}")]
        public List<BooksModel> GetInfoById(string id) 
        {
            booksTmp = new books();
            var listBooks = booksTmp.GetInfoById(id);
            return listBooks;
        }

        [HttpPost]
        public void Add([FromBody] BooksModel pBook)
        {
            booksTmp = new books();
            booksTmp.Add(pBook);
        }

        [HttpPut]
        public void Replace([FromBody] BooksModel pBook)
        {
            booksTmp = new books();
            booksTmp.Replace(pBook);
        }

        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            booksTmp = new books();
            booksTmp.Delete(id);
        }

    }
}
