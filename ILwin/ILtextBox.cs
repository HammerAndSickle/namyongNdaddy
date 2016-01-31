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
            if (cmdreq.Equals("")) return;
                
            msgbox.Text += ("received \"" + cmdreq + "\"\n");
            msgbox.ScrollToEnd();
        }

        static public void printMSG(TextBox msgbox, string msg)
        {
            if (msg.Equals("")) return;

            msgbox.Text += (msg + "\n");
            msgbox.ScrollToEnd();
        }
    }
}
