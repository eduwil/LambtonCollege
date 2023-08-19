using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webAPIMongoDB.Models;
using webAPIMongoDB.Services;

namespace webAPIMongoDB.Controllers
{

    [ApiController]
    //[Route("api/users")]
    [Route("api/{controller}/{action}")]      // Para invocar --> https://localhost:7296/api/UsersCtrl/GetList

    public class UsersCtrl : ControllerBase
    {
        private users usersTmp;

        [HttpGet]
        public List<UsersModel> GetList()
        {
            usersTmp = new users();
            var listUsrs = usersTmp.GetList();
            return listUsrs;
        }
        
        [HttpGet("{nickname}")]
        public List<UsersModel> GetInfo(string nickname)
        {
            usersTmp = new users();
            var listUsrs = usersTmp.GetInfo(nickname);
            return listUsrs;
        }

        [HttpGet("{nickname}")]
        public async Task<ActionResult<List<UsersModel>>> GetAsyncInfo(string nickname)
        {
            usersTmp = new users();
            var listUsrs = await usersTmp.GetAsyncInfo(nickname);
            return listUsrs;
        }

        [HttpGet("{id}")]
        public List<UsersModel> GetInfoById(string id)
        {
            usersTmp = new users();
            var listUsrs = usersTmp.GetInfoById(id);
            return listUsrs;
        }

        [HttpPost]
        public void Add([FromBody] UsersModel pUser)
        {
            usersTmp = new users();
            usersTmp.Add(pUser);
        }

        [HttpPut]
        public void Replace([FromBody] UsersModel pUser)
        {
            usersTmp = new users();
            usersTmp.Replace(pUser);
        }

        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            usersTmp = new users();
            usersTmp.Delete(id);
        }


    }
}
