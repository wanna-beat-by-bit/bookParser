using bookParser.repository;
using bookParser.Parser;

namespace bookParser.Logic{
    public class BLogic : IBLogic
    {
        private readonly IRepository _dbRepo;
        private readonly IParser _parser;
        public BLogic(IRepository dbRepo, IParser parser){
            _dbRepo = dbRepo;
            _parser = parser;
        }

        public void addAllBookInfo(){
            List<Dictionary<string, string>> booksInfo = _parser.parse();
            foreach(var info in booksInfo){
                _dbRepo.addBookInfo(info);
            }
        }
    }
}