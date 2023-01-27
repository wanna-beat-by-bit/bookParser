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
        public TestController(IRepository dbRepo, IBLogic bLogic, IParser parser, parserIgraslov parserIgraslov){
            _dbRepo = dbRepo;
            _bLogic = bLogic;
            _parser = parser;
            _parserIgraslov = parserIgraslov;
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

        [HttpPost("addAllBookInfo")]
        public IActionResult addAllBookInfo(){
            _bLogic.addAllBookInfo();
            return Ok();
        }

        [HttpGet("getBooksInfo")]
        public IActionResult getBooksInfo(){
            var json = JsonConvert.SerializeObject(_dbRepo.getBooksInfo(), Formatting.Indented);
            return Ok(json);
        }
    }
}