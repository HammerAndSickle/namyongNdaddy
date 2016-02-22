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
    public class Daddy : talking
    {
        //아버지 이미지들
        //[0] left_1, [1] left_2
        //[2] right_1, [3] right open
        private Brush[] daddyBr;

        //바라보는 방향.
        public const int LEFT = 0;
        public const int RIGHT = 1;

        //속도(thread sleep term)
        public int speedTerm;

        //현재 아버지가 어느 위치(왼쪽 위 꼭지점을 기준으로)에 있는가.
        public int dir;         //바라보는 방향(LEFT or RIGHT)
        public int xpos;       //아버지는 계속 좌우로 움직일 것
        public int ypos;       //높이
        public int status;      //0과 1 중 하나, 그림 상태를 바뀌게 하는 데 쓴다.

        //사용중인 스레드
        Thread thrmove;

        //rectangle. 이미지가 여기에 들어가서 움직인다.
        public System.Windows.Shapes.Rectangle daddyRec;

        //다루려는 screen 
        public ILwin.ShowScreen screen;

        //가지고 있는 말풍선
        public Balloon balloon;



        public Daddy(Brush[] imgs, ILwin.ShowScreen screen, int xpos, int ypos, int dir)
        {
            Random rnd = new Random();

            daddyBr = imgs;
            this.screen = screen;

            this.dir = dir;
            this.ypos = ypos;
            this.xpos = xpos;

            this.speedTerm = Constants.DADDY_SPEED;
            
            balloon = new Balloon(screen);
            balloon.showBalloon(Constants.IS_DADDY, dir, xpos, ypos);

            //dir = rnd.Next(0, 2);
            //ypos = rnd.Next(180, 270);
            //xpos = rnd.Next(0, 700);
            //xpos = 500; ypos = 20; dir = 0;

            //생성
            daddyRec = new System.Windows.Shapes.Rectangle();
            daddyRec.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            daddyRec.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            daddyRec.Width = Constants.DADDY_WIDTH;
            daddyRec.Height = Constants.DADDY_HEIGHT;
            daddyRec.Fill = daddyBr[dir];
            daddyRec.Margin = new Thickness(xpos, ypos, 0, 0);
            screen.sp.Children.Add(daddyRec);
        }


        public void startmove()
        {
            thrmove = new Thread(() => startmoving(this, screen.getMWinReference()));
            thrmove.Start();
        }

        //박스가 날아다니기 시작할 것이다.
        public static void startmoving(Daddy daddy, ILwin.MainWindow thisWin)
        {
            int daddyLoc_x = 0;
            daddy.status = 0;


            while (true)
            {
                //현재 margin left 값을 가져온다
                thisWin.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                {
                    daddyLoc_x = (int)daddy.daddyRec.Margin.Left;
                }));

                Thread.Sleep(daddy.speedTerm);

                daddy.status = (daddy.status == 1) ? 0 : 1;

                thisWin.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                {
                    daddy.daddyRec.Fill = daddy.daddyBr[(2 * daddy.dir) + daddy.status];
                }));

                if (daddy.dir == LEFT)
                {
                    //왼쪽 벽에 다다르면
                    if (daddyLoc_x <= 0)
                    {
                        //방향은 오른쪽으로 바뀌고, 그림도 바뀌어야지.
                        daddy.dir = RIGHT;

                        //말풍선도 오른쪽으로 바뀌고, 말풍선 위치도 바뀐다.
                        thisWin.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                        {
                            daddy.balloon.rec.Background = daddy.balloon.ballBr[RIGHT];
                            daddy.balloon.rec.Margin = new Thickness(daddy.daddyRec.Margin.Left - Constants.DADDY_BALLOON_RIGHT_DIST, daddy.daddyRec.Margin.Top, 0, 0);
                        }));
                    }

                    //그렇지 않다면 계속 왼쪽으로 이동
                    else
                    {

                        thisWin.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                        {
                            daddy.daddyRec.Margin = new Thickness(daddy.daddyRec.Margin.Left - 2, daddy.daddyRec.Margin.Top,
                                daddy.daddyRec.Margin.Right, daddy.daddyRec.Margin.Bottom);

                            daddy.balloon.rec.Margin = new Thickness(daddy.balloon.rec.Margin.Left - 2, daddy.balloon.rec.Margin.Top,
                                daddy.balloon.rec.Margin.Right, daddy.balloon.rec.Margin.Bottom);
                        }));
                    }
                }



                else if (daddy.dir == RIGHT)
                {
                    //왼쪽 벽에 다다르면
                    if (daddyLoc_x >= 640)
                    {
                        daddy.dir = LEFT;

                        //말풍선도 왼쪽으로 바뀌고, 말풍선 위치도 바뀐다.
                        thisWin.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                        {
                            daddy.balloon.rec.Background = daddy.balloon.ballBr[LEFT];
                            daddy.balloon.rec.Margin = new Thickness(daddy.daddyRec.Margin.Left + Constants.DADDY_BALLOON_LEFT_DIST, daddy.daddyRec.Margin.Top, 0, 0);
                        }));
                    }

                    //그렇지 않다면 계속 왼쪽으로 이동
                    else
                    {

                        thisWin.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                        {
                            daddy.daddyRec.Margin = new Thickness(daddy.daddyRec.Margin.Left + 2, daddy.daddyRec.Margin.Top,
                                daddy.daddyRec.Margin.Right, daddy.daddyRec.Margin.Bottom);

                            daddy.balloon.rec.Margin = new Thickness(daddy.balloon.rec.Margin.Left + 2, daddy.balloon.rec.Margin.Top,
                                daddy.balloon.rec.Margin.Right, daddy.balloon.rec.Margin.Bottom);

                        }));
                    }
                }


            }


        }
        //이동 함수 끝


        //sayHello
        public void sayHello()
        {
            balloon.setMSG("그래");
        }

        //sayTime
        public void sayTime()
        {
            DateTime currTime = DateTime.Now;
            string currTimeStr = "오늘은 " + currTime.ToString("yyyy") + "월 " + currTime.ToString("MM") + "일 "
                + currTime.ToString("dd") + "일이구, 지금 시간은 " + currTime.ToString("HH") + "시" + currTime.ToString("mm") + "분이여.";
            balloon.setMSG(currTimeStr);
        }

        //sayQuick. level 1~level 5에 따라 속도도 다르다.
        public void sayQuick(int level)
        {
            //속도는 1~5만 가능하다.
            if (level < 1 || level > 5)
            {
                if (level > 5)
                    balloon.setMSG("느금마가 그렇게 빨리 달려봐라!");

                else if (level < 1)
                    balloon.setMSG("마! 걷지 말란 말이가?");

                return;
            }

            //속도가 조건에 맞을 경우.
            switch (level)
            {
                case 1:
                    this.speedTerm = (int)(Constants.DADDY_SPEED * 4);
                    balloon.setMSG("1단계로 걷는데이. 단디 보래이");
                    break;
                case 2:
                    this.speedTerm = (int)(Constants.DADDY_SPEED * 2);
                    balloon.setMSG("2단계로 걷는다. 봉제산 등산하는 아줌마 속도다.");
                    break;
                case 3:
                    this.speedTerm = (Constants.DADDY_SPEED);
                    balloon.setMSG("3단계로 걷는다. 이게 내한테 적합한 속도야.");
                    break;
                case 4:
                    this.speedTerm = (int)(Constants.DADDY_SPEED * 0.5);
                    balloon.setMSG("4단계로 뛰라 이 말인가? 임마가요 지금 중년학대하노?");
                    break;
                case 5:
                    this.speedTerm = (int)(Constants.DADDY_SPEED * 0.25);
                    balloon.setMSG("내보고 5단계로 뛰라고? 헉.. 뭔데? 니.. 직이삔다..");
                    break;
                default:
                    this.speedTerm = (Constants.DADDY_SPEED);
                    balloon.setMSG("뭔가 이상한데?");
                    break;
            }


        }


    }
}
