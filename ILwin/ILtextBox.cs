using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using System.Windows.Controls;


namespace ILwin
{
    public class ILtextBox
    {
        ShowScreen showscreen;      //showscreen의 레퍼런스

        //생성 후, showscreen 레퍼런스를 넣는다.
        public void addShowScreenReference(ShowScreen showscreen)
        {
            this.showscreen = showscreen;
        }


        public void handleRequest(TextBox msgbox, string cmdreq, Datas datas)
        {
            //공백 기준으로 나누어보자.
            string[] ssize = cmdreq.Split(new char[0]);

            //아무것도 없다.
            if (ssize[0].Equals("")) return;

            //help를 입력받음
            else if (ssize[0].Equals("help")) cmdreq = printHelp();

            //hello를 입력받은
            else if(ssize[0].Equals("hello"))
            {
                showscreen.getDaddy().balloon.setMSG("그래");
                showscreen.getNamyong().balloon.setMSG("안녕하쇼");
            }

            //box를 처음에 입력받음
            else if (ssize[0].Equals("box"))
            {
                //box images일 경우, webitem을 떨어뜨릴 준비를 한다.
                if(ssize[1].Equals("images"))
                {
                    //box images [숫자] 일 경우. 이젠 단어만 추출하면 된다.
                    int num;

                    //숫자가 들어있는지 확인하며, 맞다면 num에 집어넣고 webimage 떨구기를 시행한다
                    if(int.TryParse(ssize[2], out num) == true)
                    {
                        //공백으로 split 하였지만, box images [aaa bbb ccc]는 가능하도록, 그 다음부턴 공백도 단어로 인정해야 한다
                        string queryImage = "";
                        for (int i = 3; i < ssize.Length; i++)
                        {
                            queryImage += ssize[i];

                            //끝이 아니라면 공백 문자를 그대로 사용
                            if (i != ssize.Length) queryImage += " ";

                        }

                        //스레드로 돌린다.
                        showscreen.generateWebImage(queryImage, num);

                        //HTMLhandler.getImages(datas, queryImage);
                        cmdreq = "박스에게 [" + queryImage + "] 이미지 " + num + "개 를 요청함!";
                    }
                }
            }

            else
            {
                cmdreq = "received \"" + cmdreq + "\"";
            }

            msgbox.Text += cmdreq + "\n";

            msgbox.ScrollToEnd();
        }

        public void printMSG(TextBox msgbox, string msg)
        {
            if (msg.Equals("")) return;

            msgbox.Text += (msg + "\n");
            msgbox.ScrollToEnd();
        }

        public string printHelp()
        {
            return "========[HELP]========\n" + "hello : 남용이와 아버지에게 인사\n"
                + "hello n : 남용이에게 인사\n" + "hello d : 아버지에게 인사\n" +
                "help : 커맨드 도움\n" + "talk n [word] : 남용이에게 말 걸기\n" + "talk d [word] : 아버지에게 말 걸기\n" +
                "box images  [num] [word] : 박스에게 num개(max : 3)의 이미지 요구";

        }

        //날씨와 온도 업데이트
    }
}
