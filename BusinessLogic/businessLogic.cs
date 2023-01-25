using bookParser.repository;

namespace bookParser.Logic{
    public class BLogic : IBLogic
    {
        private readonly IRepository _dbRepo;
        public BLogic(IRepository dbRepo){
            _dbRepo = dbRepo;
        }

        public void readAllBooksByDomain(string domain){
            throw new NotImplementedException(); 
        }
        public void logicTest(){
            _dbRepo.addTest();
        }
    }
}