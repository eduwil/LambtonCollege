using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using webAPIMongoDB.Models;
using webAPIMongoDB.Services;

namespace webAPIMongoDB.Controllers
{
    [ApiController]
    [Route("api/{controller}/{action}")]

    public class UserBooksCtrl : ControllerBase
    {
        private userbooks usrBooksTmp;

        [HttpGet]
        public List<UserBooksModel> GetList()
        {
            usrBooksTmp = new userbooks();
            var listUsrBooks = usrBooksTmp.GetList();
            return listUsrBooks;
        }

        [HttpGet("{nickname}")]
        public List<UserBooksModel> GetUserBooksList(string nickname)
        {
            usrBooksTmp = new userbooks();
            var listUsrBooks = usrBooksTmp.GetUserBooksList(nickname);
            return listUsrBooks;
        }

        [HttpGet("{isbn}/{state}")]

        public List<UserBooksModel> GetReviews(string isbn, string state)
        {
            usrBooksTmp = new userbooks();
            var listUsrBooks = usrBooksTmp.GetReviews(isbn, state);
            return listUsrBooks;
        }

        [HttpPost]
        public void Add([FromBody] UserBooksModel pUserBook)
        {
            usrBooksTmp = new userbooks();
            usrBooksTmp.Add(pUserBook);
        }

        [HttpPut("{nickname}/{isbn}/{date}/{state}")]
        public void UpdateState(string nickname, string isbn, string date, string state)
        {
            usrBooksTmp = new userbooks();
            usrBooksTmp.UpdateState(nickname, isbn, date, state);
        }

        [HttpPut("{nickname}/{isbn}/{date}/{state}/{rate}/{comment}")]
        public void Rate(string nickname, string isbn, string date, string state, short rate, string comment)
        {
            usrBooksTmp = new userbooks();
            usrBooksTmp.Rate(nickname, isbn, date, state, rate, comment);
        }

        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            usrBooksTmp = new userbooks();
            usrBooksTmp.Delete(id);
        }

    }
}
