
namespace bookParser.repository{
    public interface IRepository{
        void OpenConnection();
        void CloseConnection();
        void addTest();
        void addBookInfo(Dictionary<string, string> info);
        List<Dictionary<string, string>> getBooksInfo();
    }
}