
using AngleSharp;
using AngleSharp.Dom;
using System;

namespace Parser{
    class testParser{
        
        public static IDocument GetDocument(string url)
        {
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            return context.OpenAsync(url).Result;
        }
        public static void Main(string[] args){
            var url = "https://igraslov.store/shop/?products-per-page=all";
            var document = GetDocument(url);
            var links = document.QuerySelectorAll("a");
            var linksWihtoutClass = links.Where(link => !link.HasAttribute("class"));
            int maximum = 5;
            foreach(var link in linksWihtoutClass){
                var href = link.GetAttribute("href");
                if((href != null && href.Contains("/product/") && maximum > 0)){
                    Console.WriteLine(getIsbn(href));
                    maximum--;
                }
                if(maximum < 0){
                    break;
                }
              //if(link.TextContent.Contains("Кэмерон")){
              //    Console.WriteLine("helo");
              //    //Console.WriteLine(link.GetAttribute("href"));
              //}
              

            }
        }
        public static string getIsbn(string url){
            //var url = "https://igraslov.store/product/ellis-b-i-amerikanskij-psihopat-azbuka-myagk/";
            var document = GetDocument(url);
            var trTags = document.QuerySelectorAll("tr");
            AngleSharp.Dom.IElement result;
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