using Npgsql;

namespace bookParser.repository{
    class Repository : IRepository
    {
        private readonly string _connectionConfig;
        private NpgsqlConnection _connection = new NpgsqlConnection(); //to drop warning -Must contain non-null on init contsructor-

        public Repository(string connectionConfig)
        {
            _connectionConfig = connectionConfig;
        }
        public void OpenConnection()
        {
            _connection = new NpgsqlConnection(_connectionConfig);
            _connection.Open();
        }
        public void CloseConnection()
        {
            _connection.Close();
        }

        public bool TestConnection(){
            try{
                using (var conn = new NpgsqlConnection(_connectionConfig)){
                    conn.Open();
                    return true;
                }
            }
            catch(NpgsqlException){
                return false;
            }
        }

        public void addTest(){
            OpenConnection();
            string cmdText = "INSERT INTO tag(name) values(@name)";
            var query = new NpgsqlCommand(cmdText, _connection);
            query.Parameters.AddWithValue("@name", "Alex");
            query.ExecuteNonQuery();
            CloseConnection();
        }
    }
}