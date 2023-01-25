namespace bookParser.Models{
    public class Book{
        public int Id {get; set;}
        public string? Name {get; set;}
        public int Year {get; set;}
        public string? Description {get; set;}
        public string? Isbn {get; set;}
        public int Pages {get; set;}
        public string? ImagePath {get; set;}
    }
}