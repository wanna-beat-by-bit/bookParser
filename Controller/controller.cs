using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using bookParser.repository;

namespace bookParser.Controllers{
    [ApiController]
    [Route("api/testing")]
    public class TestController : ControllerBase, IHandlerLogic
    {
        private IRepository _dbRepo;

        public TestController(IRepository dbRepo){
            _dbRepo = dbRepo;
        }
        
        [HttpPost]
        public IActionResult Post([FromBody] object? data)
        {
            //var testData = new { message = "Hello from post" };
            Console.WriteLine("HEELOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO");
            return Ok(data);
        }

        [HttpGet("lol")]
        public IActionResult GetSome(){
            var testData = new[]{
                new {id = 1, name = "jeff"}
            };
            return Ok(new {testData});
        }
    }
}