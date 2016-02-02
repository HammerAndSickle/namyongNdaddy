using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.IO;
using HtmlAgilityPack;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace ILwin
{
    static class HTMLhandler
    {
        //Datas의 날씨 정보들을 업데이트한다.
        static public void getWeatherFromHTML(Datas datas)
        {
            //HTML agility pack을 사용.
            HtmlWeb hw = new HtmlWeb();
            string urladdr = "https://www.google.co.kr/search?newwindow=1&hl=ko&site=webhp&q=%EB%82%A0%EC%94%A8";
            string filepath = @"bfile.txt";

            HtmlDocument doc = hw.Load(urladdr);
            HtmlNode entire = doc.DocumentNode;

            //테스트용 소쓰
            System.IO.File.WriteAllText(@"afile.txt", entire.InnerHtml, Encoding.Default);

            //파일 초기화
            System.IO.File.WriteAllText(filepath, "", Encoding.Default);

            //div 중, id = ires인 걸 찾아낸다.
            // ex) //가 붙으면 doc 전체에서 찾는다. 즉, HtmlNode는 일종의 포인터일 뿐인 듯하다
            HtmlNode ires_node = doc.DocumentNode.SelectSingleNode("//div[@id='ires']");
            // ex) ./이라면, 위 div(id = 'ires')의 자식들(한 단계 아래) 중에서만 찾는다.
            // ex) .//이라면, 위 div(id = 'ires')의 자식들(몽땅) 중에서만 찾는다.
            //selectsinglenode이니 첫 table만을 가져올 것이다. 이제 table을 얻은 셈이다.
            HtmlNode ires_first_table = ires_node.SelectSingleNode(".//table");
            HtmlNode ires_first_tr = ires_first_table.SelectSingleNode(".//tr");

            //ires_first_tr에 의미 있는 정보들이 들어있다.
            //특히, IMG에 들어있는 것을 이용해 그의 attribute에만 접근해도 정보 앵간히 찾는다.
            HtmlNode ires_first_img = ires_first_tr.SelectSingleNode(".//img");
            HtmlNode ires_first_span = ires_first_tr.SelectSingleNode(".//span");

            //img의 title에는 현재의 날씨가 들어있다.
            //span의 text에는 현재의 기온이 들어있다.

            HtmlNode loc_b = ires_node.SelectSingleNode(".//b");

            System.IO.File.AppendAllText(filepath, ires_first_tr.InnerHtml, Encoding.Default);
            System.IO.File.AppendAllText(filepath, "\n\n\n===========\n\n\n", Encoding.Default);
            
            string todayVal = ires_first_img.Attributes["title"].Value;
            string todayTem = ires_first_span.InnerText;
            string currentLoc = loc_b.InnerText;

            datas.setPresentWeathers(todayTem, todayVal);
            datas.setLocation(currentLoc);

            System.IO.File.AppendAllText(filepath, todayVal + "\n", Encoding.Default);
            System.IO.File.AppendAllText(filepath, todayTem + "\n", Encoding.Default);

          


        }

        //getImages는 준 query를 이용해 urls에 요청한 개수만큼의 url을 담는다.
        static public void getImages(List<string> urls, int num, string query)
        {   
            HtmlWeb hw = new HtmlWeb();
            string urladdr = "http://www.google.com/search?q=" + query + "&tbm=isch";
            string filepath = @"cfile.txt";

            HtmlDocument doc = hw.Load(urladdr);
            HtmlNode entire = doc.DocumentNode;

            //테스트용 소쓰
            System.IO.File.WriteAllText(filepath, entire.InnerHtml, Encoding.Default);

        }


        static public BitmapImage getImage(string url)
        {
            /*
            var bytes = await new WebClient().DownloadDataTaskAsync(url);
            var image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.StreamSource = new MemoryStream(bytes);
            image.EndInit();
            RSSImage.Source = image;
             * */
            return null;
        }
    }
}
