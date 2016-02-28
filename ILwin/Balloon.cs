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
using System.Threading;
using System.Windows.Threading;

namespace ILwin
{
    //말풍선이다. 기본적으로 namyong이나 daddy가 하나씩 가지고 있으며, 그때그때 감추거나 메세지를 변경하거나 하면서 드러낼 것이다.
    public class Balloon
    {

        //말풍선의 소유자. 0은 남용이, 1은 아버지
        public int TALKER;

        //바라보는 방향.
        public const int LEFT = 0;
        public const int RIGHT = 1;

        //[0] left, [1] right
        private BitmapImage[] ballImg;
        public Brush[] ballBr;

        private TextBox textbox;                    //그 텍스트박스
        public Grid rec;                      //말풍선이 담긴 rectangle

        Thread showing;                     //말풍선 출력, 일정시간 후 제거하는 스레드
        bool isShowing;                     //현재 출력 중인가.

        ILwin.ShowScreen screen;            //showscreen이 있어야 말풍선을 넣는다.

        public Balloon(int TALKER, ILwin.ShowScreen screen)
        {
            this.TALKER = TALKER;

            ballImg = new BitmapImage[2];
            ballBr = new Brush[2];
            textbox = new TextBox();
            rec = new Grid();

            isShowing = false;


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
            textbox.FontSize = 10;
            textbox.Width = 110;
            textbox.Height = 70;
            textbox.BorderThickness = new Thickness(0.0);
            textbox.Background = Brushes.White;
            textbox.IsReadOnly = true;
            textbox.TextWrapping = TextWrapping.WrapWithOverflow;
            textbox.Cursor = Cursors.Arrow;

            //일단은 숨김
            rec.Visibility = Visibility.Hidden;
            textbox.Visibility = Visibility.Hidden;
        }

        //메인 스레드에서 호출하는 말풍선 메시지 함수
        public void setMSG(string text)
        {
            //만일 말풍선이 켜져있던 상태라면
            if(isShowing)
            {
                showing.Abort();                    //일단 그 스레드를 종료한다.
                rec.Visibility = Visibility.Hidden;         //그리고 말풍선을 모두 닫아버린다.
                textbox.Visibility = Visibility.Hidden;
                this.textbox.Text = "";
                isShowing = false;
            }

            isShowing = true;
            rec.Visibility = Visibility.Visible;
            textbox.Visibility = Visibility.Visible;
            this.textbox.Text = text;

            //말풍선 내용이 response 창에도 나오도록 하자
            screen.getMWinReference().responseMsgs.Text += ((this.TALKER == Constants.IS_NAMYONG) ? "남용이 : " : "아버지 : ");
            screen.getMWinReference().responseMsgs.Text += text + "\n";
            screen.getMWinReference().responseMsgs.ScrollToEnd();

            showing = new Thread(() => setMSG(this, this.screen.getMWinReference()));
            showing.Start();
        }


        //서브 스레드에서 호출하는 말풍선 메시지 함수
        public static void setMSGsub(ILwin.MainWindow thisWin, Balloon thisballoon, string text)
        {
            //만일 말풍선이 켜져있던 상태라면
            if (thisballoon.isShowing)
            {
                thisballoon.showing.Abort();                    //일단 그 스레드를 종료한다.

                thisWin.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                    {
                        thisballoon.rec.Visibility = Visibility.Hidden;         //그리고 말풍선을 모두 닫아버린다.
                        thisballoon.textbox.Visibility = Visibility.Hidden;
                        thisballoon.textbox.Text = "";
                    }));
                thisballoon.isShowing = false;
            }

            thisballoon.isShowing = true;
            thisWin.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                    {
                        thisballoon.rec.Visibility = Visibility.Visible;
                        thisballoon.textbox.Visibility = Visibility.Visible;
                        thisballoon.textbox.Text = text;

                        //말풍선 내용이 response 창에도 나오도록 하자
                        thisWin.getTextboxReference().printMSG(thisWin.responseMsgs, ((thisballoon.TALKER == Constants.IS_NAMYONG) ? "남용이 : " : "아버지 : ") + text);
                        thisWin.responseMsgs.ScrollToEnd();
                    }));

            thisballoon.showing = new Thread(() => setMSG(thisballoon, thisballoon.screen.getMWinReference()));
            thisballoon.showing.Start();
        }


        //말풍선 메시지 내부의 스레드
        public static void setMSG(Balloon thisballoon, ILwin.MainWindow thisWin)
        {
            //5초 후, 메세지를 닫아버린다.
            Thread.Sleep(5000);

            thisWin.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            {
                thisballoon.textbox.Visibility = Visibility.Hidden;
                thisballoon.rec.Visibility = Visibility.Hidden;
                thisballoon.textbox.Text = "";
            }));

            thisballoon.isShowing = false;
        }

        //풍선을 출력, 여기서 매개변수로 들어오는 xPos, yPos는 Namyong/Daddy의 xpos, ypos이다.
        public void showBalloon(int dir, int xPos, int yPos)
        {
            int balloonX = 0;

            switch(this.TALKER)
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




        //말풍선 제거 함수
        public void deleteBalloon()
        {
            if (isShowing)
                showing.Abort();

            rec.Children.Clear();

            
        }
    }
}
