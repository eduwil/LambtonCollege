using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webAPIMongoDB.Models;
using webAPIMongoDB.Services;

namespace webAPIMongoDB.Controllers
{
   
    [ApiController]
    [Route("api/{controller}/{action}")]

    public class GenresCtrl : ControllerBase
    {
        private genres genresTmp;

        [HttpGet]
        public List<GenresModel> GetList()
        {
            genresTmp = new genres();
            var listGenres = genresTmp.GetList();
            return listGenres;
        }

        [HttpGet]
        public async Task<ActionResult<List<GenresModel>>> GetAsyncList()
        {
            genresTmp = new genres();
            var listGenres = await genresTmp.GetAsyncList();
            return listGenres;
        }

        [HttpPost]
        public void Add([FromBody] GenresModel pGenre)
        {
            genresTmp = new genres();
            genresTmp.Add(pGenre);
        }

        [HttpPut]
        public void Replace([FromBody] GenresModel pGenre)
        {
            genresTmp = new genres();
            genresTmp.Replace(pGenre);
        }

        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            genresTmp = new genres();
            genresTmp.Delete(id);
        }

    }
}
