﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using HtmlAgilityPack;

namespace ILwin
{
    static class HTMLhandler
    {
        static public void getHTML()
        {
            //HTML agility pack을 사용.
            HtmlWeb hw = new HtmlWeb();
            string urladdr = "https://www.google.co.kr/search?newwindow=1&hl=ko&site=webhp&q=%EB%82%A0%EC%94%A8";
            string filepath = @"bfile.txt";

            HtmlDocument doc = hw.Load(urladdr);
            HtmlNode entire = doc.DocumentNode;

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

            System.IO.File.AppendAllText(filepath, ires_first_tr.InnerHtml, Encoding.Default);
            System.IO.File.AppendAllText(filepath, "\n\n\n===========\n\n\n", Encoding.Default);
            
            string todayVal = ires_first_img.Attributes["title"].Value;
            string todayTem = ires_first_span.InnerText;

            System.IO.File.AppendAllText(filepath, todayVal + "\n", Encoding.Default);
            System.IO.File.AppendAllText(filepath, todayTem + "\n", Encoding.Default);

            //HtmlNodeCollection ires_tables = ires_node.ChildNodes;

            

            /*foreach (HtmlNode link in ires_tables)
            {
               System.IO.File.AppendAllText(filepath, link.InnerHtml, Encoding.Default);
            }*/

            //System.IO.File.AppendAllText(filepath, ires_node.InnerHtml, Encoding.Default);

            /*
            foreach (HtmlNode link in ires_node)
            {

                System.IO.File.AppendAllText(filepath, link.InnerHtml, Encoding.Default);
                System.IO.File.AppendAllText(filepath, "===========\n", Encoding.Default);
                
            }*/


        }

        static public void getHTML2(string url, string filepath)
        {
            
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

            if (resp.StatusCode == HttpStatusCode.OK)
            {
                Stream received = resp.GetResponseStream();
                StreamReader readStream = null;

                if (resp.CharacterSet == null)
                {
                    readStream = new StreamReader(received);
                }
                else
                {
                    readStream = new StreamReader(received, Encoding.GetEncoding(resp.CharacterSet));
                }

                string data = readStream.ReadToEnd();

                System.IO.File.WriteAllText(filepath, data, Encoding.Default);

                resp.Close();
                readStream.Close();
            }
        }
    }
}
