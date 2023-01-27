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
        //private static object? aWord;
        public TestController(IRepository dbRepo, IBLogic bLogic, IParser parser){
            _dbRepo = dbRepo;
            _bLogic = bLogic;
            _parser = parser;
        }
        
        [HttpGet("addToDomains/{amount}")]
        public IActionResult addToDomains([/*FromBody*/FromRoute] int amount)
        {
            
            return Ok(new {giverNumber = amount});
        }

        [HttpGet("getAllowedBooks/{amount}")]
        public IActionResult readBooks([FromRoute] int amount)
        {
            string json = JsonConvert.SerializeObject(_parser.parse(amount), Formatting.Indented);
            System.IO.File.WriteAllText("isbns.json", json);
            return Ok(json);
        }

        [HttpGet("getAllBooks")]
        public IActionResult getAllBooks(){

            return Ok();
        }

        [HttpGet("logicTest")]
        public IActionResult logicTest(){
            _bLogic.logicTest();
            return Ok();
        }

    }
}