using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using System.Windows.Controls;


namespace ILwin
{
    static class ILtextBox
    {
        static public void handleRequest(TextBox msgbox, string cmdreq, Datas datas)
        {
            //공백 기준으로 나누어보자.
            string[] ssize = cmdreq.Split(new char[0]);

            //아무것도 없다.
            if (ssize[0].Equals("")) return;

            //help를 입력받음
            else if (ssize[0].Equals("help")) cmdreq = printHelp();

            //box images를 입력받음
            else if (ssize.Length > 2 && ssize[0].Equals("box") && ssize[1].Equals("images"))
            {
                //공백으로 split 하였지만, box images [aaa bbb ccc]는 가능하도록, 그 다음부턴 공백도 단어로 인정해야 한다
                string queryImage = "";
                for (int i = 2; i < ssize.Length; i++ )
                {
                    queryImage += ssize[i];

                    //끝이 아니라면 공백 문자를 그대로 사용
                    if (i != ssize.Length) queryImage += " ";
                }

                    //HTMLhandler.getImages(datas, queryImage);
                cmdreq = "박스에게 이미지를 요청함!";
            }

            else
            {
                cmdreq = "received \"" + cmdreq + "\"";
            }

            msgbox.Text += cmdreq + "\n";

            msgbox.ScrollToEnd();
        }

        static public void printMSG(TextBox msgbox, string msg)
        {
            if (msg.Equals("")) return;

            msgbox.Text += (msg + "\n");
            msgbox.ScrollToEnd();
        }

        static public string printHelp()
        {
            return "========[HELP]========\n" + "hello : 남용이와 아버지에게 인사\n"
                + "hello n : 남용이에게 인사\n" + "hello d : 아버지에게 인사\n" +
                "help : 커맨드 도움\n" + "talk n [word] : 남용이에게 말 걸기\n" + "talk d [word] : 아버지에게 말 걸기\n" +
                "box images [word] [num] : 박스에게 num개(max : 3)의 이미지 요구";

        }

        //날씨와 온도 업데이트
    }
}
