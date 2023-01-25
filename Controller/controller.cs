using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Npgsql;

using bookParser.repository;
using bookParser.Logic;

namespace bookParser.Controllers{
    [ApiController]
    [Route("api/testing")]
    public class TestController : ControllerBase
    {
        private readonly IRepository _dbRepo;
        private readonly IBLogic _bLogic;
        private static object? aWord;
        public TestController(IRepository dbRepo, IBLogic bLogic){
            _dbRepo = dbRepo;
            _bLogic = bLogic;
        }
        
        [HttpPost("addToDomains")]
        public IActionResult addToDomains([FromBody] object data)
        {
            aWord = data; 
            return Ok(new {Status = aWord});
        }

        [HttpPost("readBooks")]
        public IActionResult readBooks([FromBody] object data)
        {
            aWord = data; 
            return Ok(new {Status = aWord});
        }

        [HttpGet("getAllBooks")]
        public IActionResult getAllBooks(){

            return Ok(new {content = aWord});
        }

        [HttpGet("logicTest")]
        public IActionResult logicTest(){
            _bLogic.logicTest();
            return Ok();
        }

    }
}