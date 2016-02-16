using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Threading.Tasks;

namespace ILwin
{
    //말풍선이다.
    class Balloon
    {
        int xPos;
        int yPos;
        string text;                        //말풍선안의 텍스트
        Rectangle rec;                      //말풍선이 담긴 rectangle
        ILwin.ShowScreen screen;            //showscreen이 있어야 말풍선을 넣는다.

        public Balloon(int xPos, int yPos)
        {
            this.xPos = xPos; this.yPos = yPos;
            text = "텍스트";
        }

        public void setPosition(int xPos, int yPos)
        {
            this.xPos = xPos;
            this.yPos = yPos;
        }
    }
}
