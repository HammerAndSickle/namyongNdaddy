using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Threading;
using System.Net;
using System.IO;
using HtmlAgilityPack;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Text.RegularExpressions;


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
            //string filepath = @"bfile.txt";

            HtmlDocument doc = hw.Load(urladdr);
            HtmlNode entire = doc.DocumentNode;

            //테스트용 소쓰
            System.IO.File.WriteAllText(@"afile.txt", entire.InnerHtml, Encoding.Default);

            //파일 초기화
            //System.IO.File.WriteAllText(filepath, "", Encoding.Default);

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

            //System.IO.File.AppendAllText(filepath, ires_first_tr.InnerHtml, Encoding.Default);
            //System.IO.File.AppendAllText(filepath, "\n\n\n===========\n\n\n", Encoding.Default);
            
            string todayVal = ires_first_img.Attributes["title"].Value;
            string todayTem = ires_first_span.InnerText;
            string currentLoc = loc_b.InnerText;

            datas.setPresentWeathers(todayTem, todayVal);
            datas.setLocation(currentLoc);

            //System.IO.File.AppendAllText(filepath, todayVal + "\n", Encoding.Default);
            //System.IO.File.AppendAllText(filepath, todayTem + "\n", Encoding.Default);

          


        }

        //getImages는 준 query를 이용해 urls에 요청한 개수만큼의 url을 담는다.
        static public void getImages(List<string> urls, int num, string query)
        {   
            HtmlWeb hw = new HtmlWeb();
            string urladdr = "http://www.google.com/search?q=" + query + "&tbm=isch";
            //string filepath = @"cfile.txt";

            HtmlDocument doc = hw.Load(urladdr);
            HtmlNode entire = doc.DocumentNode;


            //테스트용 소쓰
            //System.IO.File.WriteAllText(filepath, entire.InnerHtml, Encoding.Default);

            //필요한 부분만 추출
            //DIV id="center_col"
            HtmlNode center_col_node = doc.DocumentNode.SelectSingleNode("//table[@class='images_table']");

            HtmlNodeCollection images = center_col_node.SelectNodes(".//img");

            //System.IO.File.WriteAllText(filepath, center_col_node.InnerHtml, Encoding.Default);

            //System.IO.File.WriteAllText(@"dfile.txt", images.Count + "개", Encoding.Default);

            //System.IO.File.AppendAllText(@"dfile.txt", "\n\n=====\n\n", Encoding.Default);

            //일단 urls에 담아보자.
            for (int i = 0; i < num; i++ )
            {
                urls.Add(images[i].Attributes["src"].Value);
            }

            /*
            foreach (HtmlNode ls in images)
            {
                System.IO.File.AppendAllText(@"dfile.txt", ls.Attributes["src"].Value + "\n\n", Encoding.Default);
            }
            */

        }

        static public BitmapImage downloadImageFromURL(string url)
        {
            try
            {
                if (string.IsNullOrEmpty(url))
                    return null;

                WebClient wc = new WebClient();
                Byte[] MyData = wc.DownloadData(url);
                wc.Dispose();

                BitmapImage bimgTemp = new BitmapImage();
                bimgTemp.BeginInit();
                bimgTemp.StreamSource = new MemoryStream(MyData);
                bimgTemp.EndInit();

                return bimgTemp;

            }

            catch
            {
                return null;
            }

        }

        //포털 사이트에서 뉴스를 가져온다.  boardWindow 전용
        static public void getNews(ILwin.BoardWindow bWin, TextBox textbox)
        {
            //http://reedfim.tistory.com/31 참고,
            //국내 웹 페이지를 크롤링할 때는 유의, 글자가 깨질 수도 있어서..
            int ARTICLES_TO_CRAWL = 6;      //가져올 기사 개수

            //가져올 기사의 제목&내용들
            List<string> article_titles = new List<string>();
            List<string> article_contents = new List<string>();


            //필요한 자료구조
            WebRequest req;
            HttpWebResponse resp;
            Stream datas;
            HtmlDocument docu;

            //필요한 데이터
            string urladdr = "http://news.naver.com/main/list.nhn?mode=LS2D&mid=shm&sid1=102&sid2=249";
            string testfile = @"newsfile.txt";

            //이제 내용을 크롤링해온다.
            req = WebRequest.Create(urladdr);
            req.Credentials = CredentialCache.DefaultCredentials;
            resp = (HttpWebResponse)req.GetResponse();
            datas = resp.GetResponseStream();
            docu = new HtmlDocument();
            docu.Load(datas, Encoding.Default);

            //이제 모든 document가 HtmlNode에 담겼다.
            HtmlNode entire = docu.DocumentNode;

            //네이버 기사들이 담긴 HtmlNode
            HtmlNode mainnewsNode = docu.DocumentNode.SelectSingleNode("//div[@class='list_body newsflash_body']");

            //인제 그 밑에 있는 DL들을 모두 가져온다.
            HtmlNodeCollection DLs = mainnewsNode.SelectNodes(".//dl");

            System.IO.File.WriteAllText(@"longsrc.txt", entire.InnerHtml, Encoding.Default);
            System.IO.File.WriteAllText(testfile, "", Encoding.Default);

            //while(ARTICLES_TO_CRAWL > 0)
            {
                //DL 아래 : DT는 기사 제목을, DD는 기사 내용을 담는다.
                //DL들을 가져온 후, 각 DL 안에서 selectsinglenode로 DT와 DD를 가져온다.
                
                //일반 DT는 사진 섬네일이 없는 기사로, DT의 A의 text에서 기사 제목 추출
                //일반 DT의 DD는 곧바로 text에서 기사 내용 추출

                //class가 photo인 DT는 사진 섬네일이 달린 기사로, DT의 A의 IMG의 alt에서 기사 제목 추출
                //class가 photo인 DT의 DD는 곧바로 text에서 기사 내용 추출

                //for (int i = 0; i < ARTICLES_TO_CRAWL; i++ )
                foreach(HtmlNode a_DLNode in DLs)
                {
                    //HtmlNode a_DLNode = DLs.ElementAt(i);

                    HtmlNode a_DTNode = a_DLNode.SelectSingleNode(".//dt");
                    HtmlNode a_DDNode = a_DLNode.SelectSingleNode(".//dd");

                    //DT의 class가 "photo"인지 확인, 
                    if (a_DTNode.Attributes["class"] != null)
                    {
                        if (a_DTNode.Attributes["class"].Value.Equals("photo"))
                        {
                            //DTNode 아래의 <a> 아래의 img의 alt가 기사 제목을 가지고 있다.
                            HtmlNode article_a = a_DTNode.SelectSingleNode(".//img");
                            article_titles.Add(article_a.Attributes["alt"].Value);

                            //DDNode의 text가 기사 내용을 가지고 있다.
                            HtmlNode article_text = a_DTNode.SelectSingleNode(".//img");
                            article_contents.Add(a_DDNode.InnerText);
                            
                            /*
                            System.IO.File.AppendAllText(testfile, "<PHOTO!!>\n", Encoding.Default);
                            System.IO.File.AppendAllText(testfile, a_DTNode.InnerHtml, Encoding.Default);
                            System.IO.File.AppendAllText(testfile, "\n\n", Encoding.Default);
                            System.IO.File.AppendAllText(testfile, a_DDNode.InnerHtml, Encoding.Default);
                            System.IO.File.AppendAllText(testfile, "\n\n=================\n\n", Encoding.Default);
                            */
                        }
                    }

                    else
                    {
                        //DTNode 아래의 <a>의 text가 기사 제목을 가지고 있다.
                        HtmlNode article_a = a_DTNode.SelectSingleNode(".//a");
                        article_titles.Add(article_a.InnerText);

                        //DDNode의 text가 기사 내용을 가지고 있다.
                        HtmlNode article_text = a_DTNode.SelectSingleNode(".//img");
                        article_contents.Add(a_DDNode.InnerText);
                            
                        /*
                        System.IO.File.AppendAllText(testfile, a_DTNode.InnerHtml, Encoding.Default);
                        System.IO.File.AppendAllText(testfile, "\n\n", Encoding.Default);
                        System.IO.File.AppendAllText(testfile, a_DDNode.InnerHtml, Encoding.Default);
                        System.IO.File.AppendAllText(testfile, "\n\n=================\n\n", Encoding.Default);
                         */
                    }
                }


            }




                bWin.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                {
                    textbox.Text = "";

                    for (int i = 0; i < ARTICLES_TO_CRAWL; i++)
                    {
                        var output_title = Regex.Replace(article_titles.ElementAt(i), @"\s+", " ", RegexOptions.Singleline);
                        textbox.Text += "<" + output_title + ">\n";
                        var output_content = Regex.Replace(article_contents.ElementAt(i), @"\s+", " ", RegexOptions.Singleline);
                        //textbox.Text += "{" + article_contents.ElementAt(i) + "}\n\n";
                        textbox.Text += "{" + output_content + "}\n\n";

                    }


                }));

        }

        //화재의 검색어 가져온다
        static public void getHotKeyword(int NAMYOUNG_OR_DADDY, List<string> keywordsStore)
        {
            string urladdr = "http://www.naver.com";

            //가져올 검색어들
            List<string> keywords = new List<string>();

            //필요한 자료구조
            HtmlWeb hw = new HtmlWeb();
            HtmlDocument doc = hw.Load(urladdr);
            HtmlNode entire = doc.DocumentNode;

            //System.IO.File.WriteAllText(@"longsrc.txt", entire.InnerHtml, Encoding.Default);
            System.IO.File.WriteAllText(@"longsrc.txt", "", Encoding.Default);

            //실시간 급상승 검색어는 이 노드들에
            HtmlNode OL_Node = entire.SelectSingleNode("//ol[@id='realrank']");
            HtmlNodeCollection LI_Nodes = OL_Node.SelectNodes("./li");

            foreach(HtmlNode node in LI_Nodes)
            {
                HtmlNode A_NODE = node.SelectSingleNode("a");
                keywords.Add( A_NODE.Attributes["title"].Value );
                System.IO.File.AppendAllText(@"longsrc.txt", A_NODE.Attributes["title"].Value + "\n\n", Encoding.Default);
            }
            
            //아버지는 좀 상위에 속한 걸 2개 반환하고, 남용이는 하위에 속한 걸 2개 반환
            Random rnd = new Random();

            //난수의 범위는 아버지냐? 남용이냐? 에 따라 다르지
            int RndStart = 0; int RndEnd = 6;
            switch(NAMYOUNG_OR_DADDY)
            {
                case Constants.IS_DADDY:
                    RndStart = 6; RndEnd = 11;
                    break;
                default:
                    break;
            }

            //두개가 모일 때까지 랜덤을 돌린다
            while(keywordsStore.Count < 2)
            {
                int thisIdx = rnd.Next(RndStart, RndEnd);
                
                //두번째 검색어를 뽑을 때, 그게 이미 뽑은 적 있던 검색어라면 다시.
                if (keywordsStore.Count == 1 && keywordsStore.ElementAt(0).Equals(keywords.ElementAt(thisIdx)))
                    continue;

                else
                    keywordsStore.Add(keywords.ElementAt(thisIdx));
            }

            //이제 각 범위에 알맞게 검색어 두개가 추출되었다.
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
