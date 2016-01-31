using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;


namespace ILwin
{
    static class ILtextBox
    {
        static public void handleRequest(TextBox msgbox, string cmdreq)
        {
            //아무것도 없다.
            if (cmdreq.Equals("")) return;

            //help를 입력받음
            else if (cmdreq.Equals("help")) cmdreq = printHelp();

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
