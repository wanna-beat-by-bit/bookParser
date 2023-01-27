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
        public void addBookInfo(Dictionary<string, string> info){
            OpenConnection();

            //adding to book
            string cmdText = "insert into book(name, year, description, isbn, pages, imagePath) values(@name, @year, @description, @isbn, @pages, @imagePath)";
            var query = new NpgsqlCommand(cmdText, _connection);
            query.Parameters.AddWithValue("@name", info["BookName"]);
            query.Parameters.AddWithValue("@year", int.Parse(info["Year"]));
            query.Parameters.AddWithValue("@description", info["Description"]);
            query.Parameters.AddWithValue("@isbn", info["Isbn"]);
            query.Parameters.AddWithValue("@pages", int.Parse(info["Pages"]));
            query.Parameters.AddWithValue("@imagePath", info["Image"]);
            query.ExecuteNonQuery();

            //adding to genre
            cmdText = "insert into genre(name) values(@name)";
            query = new NpgsqlCommand(cmdText, _connection);
            query.Parameters.AddWithValue("@name", info["Genre"]);
            query.ExecuteNonQuery();

            //adding to author
            cmdText = "insert into author(name) values(@name)";
            query = new NpgsqlCommand(cmdText, _connection);
            query.Parameters.AddWithValue("@name", info["Author"]);
            query.ExecuteNonQuery();

            //adding to bookToGenre
            cmdText = "select id from book where name = @name";
            query = new NpgsqlCommand(cmdText, _connection);
            query.Parameters.AddWithValue("@name", info["BookName"]);
            var bookId = (int) query.ExecuteScalar();

            cmdText = "select id from genre where name = @name";
            query = new NpgsqlCommand(cmdText, _connection);
            query.Parameters.AddWithValue("@name", info["Genre"]);
            var genreId = (int) query.ExecuteScalar();

            cmdText = "insert into bookToGenre(bookId, genreId) values(@bookId, @genreId)";
            query = new NpgsqlCommand(cmdText, _connection);
            query.Parameters.AddWithValue("@bookId", bookId);
            query.Parameters.AddWithValue("@genreId", genreId);
            query.ExecuteNonQuery();

            //adding to bookToGenre
            cmdText = "select id from book where name = @name";
            query = new NpgsqlCommand(cmdText, _connection);
            query.Parameters.AddWithValue("@name", info["BookName"]);
            bookId = (int) query.ExecuteScalar();

            cmdText = "select id from author where name = @name";
            query = new NpgsqlCommand(cmdText, _connection);
            query.Parameters.AddWithValue("@name", info["Author"]);
            var authorId = (int) query.ExecuteScalar();

            cmdText = "insert into bookToAuthor(bookId, authorId) values(@bookId, @authorId)";
            query = new NpgsqlCommand(cmdText, _connection);
            query.Parameters.AddWithValue("@bookId", bookId);
            query.Parameters.AddWithValue("@authorId", authorId);
            query.ExecuteNonQuery();
            CloseConnection();
        }
        public List<Dictionary<string, string>> getBooksInfo(){
            List<Dictionary<string,string>> resultList = new List<Dictionary<string, string>>();
            Dictionary<string,string> temp = new Dictionary<string, string>();
            OpenConnection();
            var cmdText = @"select book.name as book, author.name as author, book.year, book.description, book.isbn,
                        book.pages, book.imagePath, genre.name
                            from bookToAuthor 
                                join book on bookToAuthor.bookId = book.id 
                                join author on author.id = bookToAuthor.id
                                join bookToGenre on bookToGenre.bookId = book.id
                                join genre on genre.id = bookToGenre.genreId; ";
            var query = new NpgsqlCommand(cmdText, _connection);
            var reader = query.ExecuteReader();
            while(reader.Read()){
                temp.Add("BookName",    $"{reader.GetString(0)}");
                temp.Add("Author",      $"{reader.GetString(1)}");
                temp.Add("Year",        $"{reader.GetInt32(2)}");
                temp.Add("Description", $"{reader.GetString(3)}");
                temp.Add("Isbn",        $"{reader.GetString(4)}");
                temp.Add("Pages",       $"{reader.GetInt32(5)}");
                temp.Add("ImagePath",   $"{reader.GetString(6)}");
                temp.Add("Genre",       $"{reader.GetString(7)}");

                resultList.Add(temp);
                temp = new Dictionary<string, string>();
            }
            
            CloseConnection();
            return resultList;
        }
    }
}