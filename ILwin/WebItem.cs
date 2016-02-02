using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ILwin
{
    //박스에게 이미지를 요청했을 때, 그 이미지 하나하나가 이 WebItem 객체이다.
    public class WebItem
    {
        BitmapImage img;
        ImageBrush imgBr;             //웹에서 건져온 이미지
        Rectangle imgrec;           //이미지 rectangle

        //이들은 imgrec의 가로세로가 될 것이다.
        int width;
        int height;

        int xPos;                   //떨어지기 시작한 x 위치.
        int yPos;                   //떨어지기 시작한 y 위치.

        //string imgurl에서 다운받아와 이미지를 얻어낸다. 생성자에서 하는 것이다. 
        //그리고 flyingbox의 x 위치, y 위치가 생성 위치에 영향을 준다.
        public WebItem(string imgurl, int box_posX, int box_posY)
        {
            //download image from url via internet
            this.xPos = box_posX; this.yPos = box_posY;
        }

        //이제 그 이미지를 화면에 표시하고, 아래로 추락시키는 스레드를 연결시킨다.
        public void showItem()
        {

        }

        public static void fallingItem()
        {

        }
    }
}
