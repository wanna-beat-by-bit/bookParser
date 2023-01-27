
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;

namespace bookParser.Parser
{
    class parserLabirint : IParser
    {
        public static IDocument GetDocument(string url)
        {
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            return context.OpenAsync(url).Result;
        }
        public List<Dictionary<string,string>> parse(){
            List<Dictionary<string,string>> resultArray = new List<Dictionary<string, string>>();

        
            string json = File.ReadAllText("isbns.json");
            if(json == null)
                Environment.Exit(1);

            List<string> isbns = JsonConvert.DeserializeObject<List<string>>(json);
            if(isbns == null){
                Console.WriteLine("File isbns doesn't exit");
                Environment.Exit(1);
            }

            foreach(var isbn in isbns){
                Dictionary<string,string> bookInfo = new Dictionary<string, string>();
                var url = $"https://www.labirint.ru/search/{isbn}/?stype=0";
                var document = GetDocument(url);
                var link = document.QuerySelector("a.product-title-link") as IHtmlAnchorElement;
                var href = link.Href;
                if(href != null)
                    document = GetDocument(href);

                bookInfo.Add("Author",        $"{getAuthor(document)}");
                bookInfo.Add("BookName",      $"{getBookName(document)}");
                bookInfo.Add("Description",   $"{getDescription(document)}");
                bookInfo.Add("Year",          $"{getYear(document)}");
                bookInfo.Add("Image",         $"{getImage(document)}");
                bookInfo.Add("Pages",         $"{getPages(document)}");
                bookInfo.Add("Genre",         $"{getGenre(document)}");
                bookInfo.Add("Isbn",          $"{isbn}");

                resultArray.Add(bookInfo);
                bookInfo = new Dictionary<string, string>();
            }
            return resultArray;
        }
        public string? getBookName(IDocument document){
            var div = document.QuerySelector("div.prodtitle h1");
            if(div != null)
                return div.TextContent;
            return null;
            
        }
        public string? getAuthor(IDocument document){
            var authors = document.QuerySelectorAll("div.authors");// as IHtmlAnchorElement; 
            if(authors != null){
                foreach(var author in authors){
                    var authorName = author.QuerySelector("a");
                    if(authorName != null){
                        return authorName.TextContent;
                    }
                }
            }
            return null;

        }
        public string? getDescription(IDocument document){
            var description = document.QuerySelector("div#smallannotation p");
            if(description == null)
                description = document.QuerySelector("div#product-about p");
            if(description != null)
                return description.TextContent;
            else{
                return null;
            }
        }
        public string? getYear(IDocument document){
            var year = document.QuerySelector("div.publisher");
            if(year != null){
                var input = year.TextContent;
                string[] parts = input.Split(' ');
                string number = parts[3];
                return number;
            }
            return null;
        }
        public string? getImage(IDocument document){
            var image = document.QuerySelector("div#product-image img");
            if (image != null){
                var imageRef = image.GetAttribute("data-src");
                if(imageRef != null)
                    return imageRef;
            }
            return null; 
        }
        public string? getPages(IDocument document){
            var pages = document.QuerySelector("div.pages2");
            if(pages != null){
                var input = pages.TextContent;
                string[] parts = input.Split(' ');
                string number = parts[1];
                return number;
            }
            return null;
        }
        public string? getGenre(IDocument document){
            var genre = document.QuerySelector("span.thermo-item_last a");
            if(genre != null)
                return genre.TextContent;
            return null;
        }
        public string? getTags(){
            throw new NotImplementedException();

        }


    }
}