﻿using System;
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
    class Namyong
    {
        //남용이 이미지들
        //[0] left_1, [1] left_2
        //[2] right_1, [3] right open
        private Brush[] namyongBr;

        //바라보는 방향.
        public const int LEFT = 0;
        public const int RIGHT = 1;

        //속도(thread sleep term)
        public int speedTerm;

        //현재 남용이가 어느 위치(왼쪽 위 꼭지점을 기준으로)에 있는가.
        public int dir;         //바라보는 방향(LEFT or RIGHT)
        public int xpos;       //남용이는 계속 좌우로 움직일 것
        public int ypos;       //높이
        public int status;      //0과 1 중 하나, 그림 상태를 바뀌게 하는 데 쓴다.

        //사용중인 스레드
        Thread thrmove;

        //rectangle. 이미지가 여기에 들어가서 움직인다.
        public System.Windows.Shapes.Rectangle namyongRec;

        //다루려는 screen 
        public ILwin.ShowScreen screen;



        public Namyong(Brush[] imgs, ILwin.ShowScreen screen, int xpos, int ypos, int dir)
        {
            Random rnd = new Random();

            namyongBr = imgs;
            this.screen = screen;

            this.dir = dir;
            this.ypos = ypos;
            this.xpos = xpos;

            speedTerm = Constants.NAMYONG_SPEED;

            //dir = rnd.Next(0, 2);
            //ypos = rnd.Next(180, 270);
            //xpos = rnd.Next(0, 700);
            //xpos = 500; ypos = 20; dir = 0;

            //생성
            namyongRec = new System.Windows.Shapes.Rectangle();
            namyongRec.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            namyongRec.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            namyongRec.Width = Constants.NAYONG_WIDTH;
            namyongRec.Height = Constants.NAYONG_HEIGHT;
            namyongRec.Fill = namyongBr[dir];
            namyongRec.Margin = new Thickness(xpos, ypos, 0, 0);
            screen.sp.Children.Add(namyongRec);
        }


        public void startmove()
        {
            thrmove = new Thread(() => startmoving(this, screen.getMWinReference()));
            thrmove.Start();
        }

        //박스가 날아다니기 시작할 것이다.
        public static void startmoving(Namyong namyong, ILwin.MainWindow thisWin)
        {
            int namyongLoc_x = 0;
            namyong.status = 0;


            while (true)
            {
                //현재 margin left 값을 가져온다
                thisWin.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                {
                    namyongLoc_x = (int)namyong.namyongRec.Margin.Left;
                }));

                Thread.Sleep(namyong.speedTerm);

                namyong.status = (namyong.status == 1) ? 0 : 1;

                thisWin.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                {
                    namyong.namyongRec.Fill = namyong.namyongBr[(2*namyong.dir) + namyong.status];
                }));

                if (namyong.dir == LEFT)
                {
                    //왼쪽 벽에 다다르면
                    if (namyongLoc_x <= 0)
                    {
                        //방향은 오른쪽으로 바뀌고, 그림도 바뀌어야지.
                        namyong.dir = RIGHT;
                    }

                    //그렇지 않다면 계속 왼쪽으로 이동
                    else
                    {

                        thisWin.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                        {
                            namyong.namyongRec.Margin = new Thickness(namyong.namyongRec.Margin.Left - 2, namyong.namyongRec.Margin.Top,
                                namyong.namyongRec.Margin.Right, namyong.namyongRec.Margin.Bottom);
                        }));
                    }
                }



                else if (namyong.dir == RIGHT)
                {
                    //왼쪽 벽에 다다르면
                    if (namyongLoc_x >= 640)
                    {
                        namyong.dir = LEFT;

                    }

                    //그렇지 않다면 계속 왼쪽으로 이동
                    else
                    {

                        thisWin.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                        {
                            namyong.namyongRec.Margin = new Thickness(namyong.namyongRec.Margin.Left + 2, namyong.namyongRec.Margin.Top,
                                namyong.namyongRec.Margin.Right, namyong.namyongRec.Margin.Bottom);

                        }));
                    }
                }


            }


        }
    }
}
