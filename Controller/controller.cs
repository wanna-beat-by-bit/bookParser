using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Npgsql;

using bookParser.repository;
using bookParser.Logic;
using bookParser.Parser;
using Newtonsoft.Json;

namespace bookParser.Controllers{
    [ApiController]
    [Route("api/")]
    public class TestController : ControllerBase
    {
        private readonly IRepository _dbRepo;
        private readonly IBLogic _bLogic;
        private readonly IParser _parser;
        private readonly parserIgraslov _parserIgraslov;
        //private static object? aWord;
        public TestController(IRepository dbRepo, IBLogic bLogic, IParser parser){
            _dbRepo = dbRepo;
            _bLogic = bLogic;
            _parser = parser;
        }
        
        [HttpGet("getAllBookInfo")]
        public IActionResult getAllBookInfo()
        {
            string json = JsonConvert.SerializeObject(_parser.parse(), Formatting.Indented); 
            return Ok(json);
        }

        [HttpGet("getAllowedBooks/{amount}")]
        public IActionResult getAllowedBooks([FromRoute] int amount)
        {
            string json = JsonConvert.SerializeObject(_parserIgraslov.parse(amount), Formatting.Indented);
            System.IO.File.WriteAllText("isbns.json", json);
            return Ok(json);
        }

        //[HttpGet("logicTest")]
        //public IActionResult logicTest(){
        //    _bLogic.logicTest();
        //    return Ok();
        //}

    }
}