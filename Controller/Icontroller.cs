using Microsoft.AspNetCore.Mvc;

namespace bookParser.Controllers{
    interface IHandlerLogic{
        public IActionResult getAllBookInfo();
        public IActionResult getAllowedBooks();
    }
}