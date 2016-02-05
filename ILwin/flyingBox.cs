using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Threading;
using System.Windows.Threading;

namespace ILwin
{
    public class flyingBox
    {
        //flying 박스 이미지들
        //[0] left normal, [1] right normal
        //[2] left open, [3] right open
        private Brush[] boxBr;

        //바라보는 방향.
        public const int LEFT = 0;
        public const int RIGHT = 1;

        //현재 상자가 어느 위치(왼쪽 위 꼭지점을 기준으로)에 있는가.
        public int dir;         //바라보는 방향(LEFT or RIGHT)
        public int xpos;       //상자는 계속 좌우로 움직일 것이고 말이다.
        public int ypos;       //높이

        //신호를 받으면 상자가 물건을 떨어뜨린다.
        public int signal;

        //사용중인 스레드
        Thread thrmove;

        //rectangle. 이미지가 여기에 들어가서 움직인다.
        public System.Windows.Shapes.Rectangle boximg;

        //다루려는 screen 
        public ILwin.ShowScreen screen;

        public flyingBox(Brush[] boxBr, ILwin.ShowScreen screen)
        {
            Random rnd = new Random();

            this.boxBr = boxBr;
            this.screen = screen;

            //위치 선정
            

            dir = rnd.Next(0, 2);
            ypos = rnd.Next(0, 100);
            xpos = rnd.Next(0, 700);
            //xpos = 500; ypos = 20; dir = 0;

            //생성
            boximg = new System.Windows.Shapes.Rectangle();
            boximg.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            boximg.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            boximg.Width = boximg.Height = 120;     //이미지의 너비다.
            boximg.Fill = boxBr[dir];
            boximg.Margin = new Thickness(xpos, ypos, 0, 0);
            screen.sp.Children.Add(boximg);


        }

        public void startmove()
        {
            thrmove = new Thread(() => startmoving(this, screen.getMWinReference()));
            thrmove.Start();
        }

        //박스가 날아다니기 시작할 것이다.
        public static void startmoving(flyingBox flyingbox, ILwin.MainWindow thisWin)
        {
            int boxLoc_x = 0;


            while (true)
            {
                //현재 margin left 값을 가져온다
            thisWin.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                    {
                        boxLoc_x = (int)flyingbox.boximg.Margin.Left;
                    }));

                Thread.Sleep(50);

                if (flyingbox.dir == LEFT)
                {
                    //왼쪽 벽에 다다르면
                    if (boxLoc_x <= 0)
                    {
                        //방향은 오른쪽으로 바뀌고, 그림도 바뀌어야지.
                        flyingbox.dir = RIGHT;
                        thisWin.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                        {
                            flyingbox.boximg.Fill = flyingbox.boxBr[1];
                        }));
                    }

                    //그렇지 않다면 계속 왼쪽으로 이동
                    else
                    {

                        thisWin.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                        {
                            flyingbox.boximg.Margin = new Thickness(flyingbox.boximg.Margin.Left - 2, flyingbox.boximg.Margin.Top,
                                flyingbox.boximg.Margin.Right, flyingbox.boximg.Margin.Bottom);
                        }));
                    }
                }



                else if (flyingbox.dir == RIGHT)
                {
                    //왼쪽 벽에 다다르면
                    if (boxLoc_x >= 640)
                    {
                        flyingbox.dir = LEFT;
                        thisWin.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                        {
                            flyingbox.boximg.Fill = flyingbox.boxBr[0];
                        }));
                    }

                    //그렇지 않다면 계속 왼쪽으로 이동
                    else
                    {

                        thisWin.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                        {
                            flyingbox.boximg.Margin = new Thickness(flyingbox.boximg.Margin.Left + 2, flyingbox.boximg.Margin.Top,
                                flyingbox.boximg.Margin.Right, flyingbox.boximg.Margin.Bottom);

                        }));
                    }
                }

                /*
                //왼쪽을 보고있었다
                if (flyingbox.dir == LEFT)
                {
                    thisWin.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                        {
                            while (flyingbox.boximg.Margin.Left >= 0)
                            {
                                flyingbox.boximg.Margin = new Thickness(flyingbox.boximg.Margin.Left - 3, flyingbox.boximg.Margin.Top,
                                    flyingbox.boximg.Margin.Right, flyingbox.boximg.Margin.Bottom);
                            }

                            ILtextBox.printMSG(thisWin.responseMsgs, "margin : " + flyingbox.boximg.Margin + ", dir : " + flyingbox.dir);
                        }));

                    flyingbox.dir = RIGHT;

                }

                else
                {
                    thisWin.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                        {
                            while (flyingbox.boximg.Margin.Left <= 800)
                            {
                                flyingbox.boximg.Margin = new Thickness(flyingbox.boximg.Margin.Left + 3, flyingbox.boximg.Margin.Top,
                                    flyingbox.boximg.Margin.Right, flyingbox.boximg.Margin.Bottom);
                            }

                            ILtextBox.printMSG(thisWin.responseMsgs, "margin : " + flyingbox.boximg.Margin + ", dir : " + flyingbox.dir);
                        }));

                    flyingbox.dir = LEFT;
                }*/

            }
        }
    }
}
