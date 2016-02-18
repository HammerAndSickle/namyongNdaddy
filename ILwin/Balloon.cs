using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Input;

namespace ILwin
{
    //말풍선이다. 기본적으로 namyong이나 daddy가 하나씩 가지고 있으며, 그때그때 감추거나 메세지를 변경하거나 하면서 드러낼 것이다.
    class Balloon
    {

        //바라보는 방향.
        public const int LEFT = 0;
        public const int RIGHT = 1;

        //[0] left, [1] right
        private BitmapImage[] ballImg;
        public Brush[] ballBr;

        private TextBox textbox;                    //그 텍스트박스
        public Grid rec;                      //말풍선이 담긴 rectangle
        
        ILwin.ShowScreen screen;            //showscreen이 있어야 말풍선을 넣는다.

        public Balloon(ILwin.ShowScreen screen)
        {
            ballImg = new BitmapImage[2];
            ballBr = new Brush[2];
            textbox = new TextBox();


            ballImg[0] = new BitmapImage();
            ballImg[0].BeginInit();
            ballImg[0].UriSource = new Uri(Constants.REL_PATH_SPRITE + "trans_leftbal.png", UriKind.Relative);
            ballImg[0].EndInit();

            ballImg[1] = new BitmapImage();
            ballImg[1].BeginInit();
            ballImg[1].UriSource = new Uri(Constants.REL_PATH_SPRITE + "trans_rightbal.png", UriKind.Relative);
            ballImg[1].EndInit();

            ballBr[0] = new ImageBrush(ballImg[0]);
            ballBr[1] = new ImageBrush(ballImg[1]);

            this.screen = screen;

            //텍스트박스
            textbox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            textbox.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            textbox.Margin = new Thickness(6, 6, 0, 0);
            textbox.FontSize = 9;
            textbox.Width = 80;
            textbox.Height = 50;
            textbox.BorderThickness = new Thickness(0.0);
            textbox.Background = Brushes.White;
            textbox.IsReadOnly = true;
            textbox.Cursor = Cursors.Arrow;
        }

        //말풍선 메시지
        public static void setMSG(TextBox textbox, ILwin.ShowScreen screen, string text)
        {
            textbox.Text = text;
        }

        //풍선을 출력, 여기서 매개변수로 들어오는 xPos, yPos는 Namyong/Daddy의 xpos, ypos이다.
        public void showBalloon(int type, int dir, int xPos, int yPos)
        {
            int balloonX = 0;

            switch(type)
            {
                case Constants.IS_NAMYONG:
                    if (dir == LEFT) balloonX = xPos + Constants.NAMYONG_BALLOON_LEFT_DIST;
                    else if (dir == RIGHT) balloonX = xPos - Constants.NAMYONG_BALLOON_RIGHT_DIST;;
                    break;
                case Constants.IS_DADDY:
                    if (dir == LEFT) balloonX = xPos + Constants.DADDY_BALLOON_LEFT_DIST;
                    else if (dir == RIGHT) balloonX = xPos - Constants.DADDY_BALLOON_RIGHT_DIST;
                    break;
            }

            rec = new Grid();
            rec.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            rec.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            rec.Width = Constants.BALLOON_WIDTH;
            rec.Height = Constants.BALLOON_HEIGHT;
            rec.Background = ballBr[dir];
            rec.Margin = new Thickness(balloonX, yPos, 0, 0);
            screen.sp.Children.Add(rec);

            //textbox add
            rec.Children.Add(textbox);
        }

        //textbox 반환
        public TextBox getTextboxReference()
        {
            return textbox;
        }

    }
}
