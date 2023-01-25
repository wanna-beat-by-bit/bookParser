
namespace bookParser.repository{
    public interface IRepository{
        void OpenConnection();
        void CloseConnection();
        void addTest();
    }
}