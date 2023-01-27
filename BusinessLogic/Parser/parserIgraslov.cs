
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using System;

namespace bookParser.Parser{
    class parserIgraslov 
    {
        
        public static IDocument GetDocument(string url)
        {
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            return context.OpenAsync(url).Result;
        }
        public bool isFileOnLocal(){
            FileInfo file = new FileInfo("pages/igraslov.html");
            if(file.Exists)
                return true;
            else
                return false;
        }
        public IDocument handleDocument(){
            if(!isFileOnLocal()){
                var url = "https://igraslov.store/shop/?products-per-page=all";
                IDocument document = GetDocument(url);
                return document;
            }
            else{
                string filePath = "pages/igraslov.html";
                string html = File.ReadAllText(filePath);
                var parser = new HtmlParser();
                IDocument document = parser.ParseDocument(html);
                return document;
            }

        }
        public List<string> parse(int max){
            var document = handleDocument();
            var links = document.QuerySelectorAll("a");
            var linksWihtoutClass = links.Where(link => !link.HasAttribute("class"));
            int maximum = max;
            List<string> isbns = new List<string> {};
            foreach(var link in linksWihtoutClass){
                var href = link.GetAttribute("href");
                if((href != null && href.Contains("/product/") && maximum > 0)){
                    isbns.Add(getIsbn(href));
                    maximum--;
                }
                if(maximum == 0){
                    break;
                }
            }
            return isbns;
        }
        public static string getIsbn(string url){
            var document = GetDocument(url);
            var trTags = document.QuerySelectorAll("tr");
            AngleSharp.Dom.IElement? result;
            foreach (var tr in trTags)
            {
                var th = tr.QuerySelector("th");
                if(th != null){
                    if(th.TextContent == "ISBN/ISSN"){
                        var td = tr.QuerySelector("td");
                        if(td != null){
                            result = td.QuerySelector("p");
                            if(result != null)
                                return result.TextContent;
                        }
                    }
                }
            }
            return "empty";
        }
    }
}