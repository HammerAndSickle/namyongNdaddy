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
    public class Namyong : talking
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
        Thread thrmove; //움직이는 스레드
        Thread thrtalk; //대화 스레드
        Thread thrjump; //점프 스레드
        Thread thrRAM;  //RAM 계산 스레드
        bool isGettingKeyword;      //지금 키워드 가져오는 스레드가 실행중인가?
        bool isJumping;             //지금 점프 중인가?
        bool isComputingRAM;        //지금 RAM 계산 중인가?

        //rectangle. 이미지가 여기에 들어가서 움직인다.
        public System.Windows.Shapes.Rectangle namyongRec;

        //다루려는 screen 
        public ILwin.ShowScreen screen;

        //가지고 있는 말풍선
        public Balloon balloon;





        public Namyong(Brush[] imgs, ILwin.ShowScreen screen, int xpos, int ypos, int dir)
        {
            Random rnd = new Random();

            namyongBr = imgs;
            this.screen = screen;

            this.dir = dir;
            this.ypos = ypos;
            this.xpos = xpos;

            speedTerm = Constants.NAMYONG_SPEED;

            balloon = new Balloon(Constants.IS_NAMYONG, screen);
            balloon.showBalloon(dir, xpos, ypos);

            //생성
            namyongRec = new System.Windows.Shapes.Rectangle();
            namyongRec.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            namyongRec.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            namyongRec.Width = Constants.NAYONG_WIDTH;
            namyongRec.Height = Constants.NAYONG_HEIGHT;
            namyongRec.Fill = namyongBr[dir];
            namyongRec.Margin = new Thickness(xpos, ypos, 0, 0);
            screen.sp.Children.Add(namyongRec);

            isGettingKeyword = false;
            isJumping = false;
            isComputingRAM = false;
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

                        //말풍선도 오른쪽으로 바뀌고, 말풍선 위치도 바뀐다.
                        thisWin.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                        {
                            namyong.balloon.rec.Background = namyong.balloon.ballBr[RIGHT];
                            namyong.balloon.rec.Margin = new Thickness(namyong.namyongRec.Margin.Left - Constants.NAMYONG_BALLOON_RIGHT_DIST, namyong.namyongRec.Margin.Top, 0, 0);
                        }));
                    }

                    //그렇지 않다면 계속 왼쪽으로 이동
                    else
                    {

                        thisWin.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                        {
                            namyong.namyongRec.Margin = new Thickness(namyong.namyongRec.Margin.Left - 2, namyong.namyongRec.Margin.Top,
                                namyong.namyongRec.Margin.Right, namyong.namyongRec.Margin.Bottom);

                            namyong.balloon.rec.Margin = new Thickness(namyong.balloon.rec.Margin.Left - 2, namyong.namyongRec.Margin.Top,
                                0, 0);
                        }));
                    }
                }



                else if (namyong.dir == RIGHT)
                {
                    //왼쪽 벽에 다다르면
                    if (namyongLoc_x >= 640)
                    {
                        namyong.dir = LEFT;

                        //말풍선도 왼쪽으로 바뀌고, 말풍선 위치도 바뀐다.
                        thisWin.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                        {
                            namyong.balloon.rec.Background = namyong.balloon.ballBr[LEFT];
                            namyong.balloon.rec.Margin = new Thickness(namyong.namyongRec.Margin.Left + Constants.NAMYONG_BALLOON_LEFT_DIST, namyong.namyongRec.Margin.Top, 0, 0);
                        }));
                    }

                    //그렇지 않다면 계속 왼쪽으로 이동
                    else
                    {

                        thisWin.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                        {
                            namyong.namyongRec.Margin = new Thickness(namyong.namyongRec.Margin.Left + 2, namyong.namyongRec.Margin.Top,
                                namyong.namyongRec.Margin.Right, namyong.namyongRec.Margin.Bottom);

                            namyong.balloon.rec.Margin = new Thickness(namyong.balloon.rec.Margin.Left + 2, namyong.namyongRec.Margin.Top,
                                0, 0);

                        }));
                    }
                }


            }


        }
        //이동 함수 끝

        //점프 함수
        public void startjump()
        {
            //이미 점프중이라면 더 점프하지 않는다.
            if (isJumping)
            {
                thrjump.Abort();
                isJumping = false;
            }


            balloon.setMSG("으아아아~");

            thrjump = new Thread(() => startjumping(this, screen.getMWinReference()));
            isJumping = true;
            thrjump.Start();
        }

        //박스가 날아다니기 시작할 것이다.
        public static void startjumping(Namyong namyong, ILwin.MainWindow thisWin)
        {
            //점프를 수행한다.

            int jumpY;

            for (jumpY = 0; jumpY < 50; jumpY++)
            {
                thisWin.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                {
                    namyong.namyongRec.Margin = new Thickness(namyong.namyongRec.Margin.Left, (namyong.ypos - jumpY * 3), 0, 0);
                }));

                Thread.Sleep(5);
            }

            Thread.Sleep(600);

            for (; jumpY > 0; jumpY--)
            {
                thisWin.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                {
                    namyong.namyongRec.Margin = new Thickness(namyong.namyongRec.Margin.Left, (namyong.ypos - jumpY * 3), 0, 0);
                }));

                Thread.Sleep(5);
            }


            //점프가 끝났음을 알린다.
            namyong.isJumping = false;

        }



        //sayHello
        public void sayHello()
        {
            balloon.setMSG("안녕하쇼");
        }
        
        //sayTime
        public void sayTime()
        {
            DateTime currTime = DateTime.Now;
            string currTimeStr = "날짜 : " + currTime.ToString("yyyy") + "/" + currTime.ToString("MM") + "/"
                + currTime.ToString("dd") + ", 시간 : " + currTime.ToString("HH:mm") + "요.";
            balloon.setMSG(currTimeStr);
        }

        //sayKeyword
        public void sayKeyword(Balloon balloon)
        {
            if (isGettingKeyword)
            {
                thrtalk.Abort();
                isGettingKeyword = false;
            }

            isGettingKeyword = true;
            thrtalk = new Thread(() => sayKeywordThr(this, balloon));
            thrtalk.Start();
        }


        //sayKeyword에서 실행되는 스레드.
        public static void sayKeywordThr(Namyong namyong, Balloon balloon)
        {
            //가져올 검색어들. 2개 정도만 가져온다.
            List<string> keywords = new List<string>();

            Thread thrkeyword = new Thread(() => HTMLhandler.getHotKeyword(Constants.IS_NAMYONG, keywords));
            thrkeyword.Start();

            //기다리자
            thrkeyword.Join();

            namyong.isGettingKeyword = false;

            Balloon.setMSGsub(namyong.screen.getMWinReference(), balloon, "요새 뜨는 키워드 : " + keywords.ElementAt(0) + ", " + keywords.ElementAt(1) + " 입니다.");
        }

        //sayQuick. level 1~level 5에 따라 속도도 다르다.
        public void sayQuick(int level)
        {
            //속도는 1~5만 가능하다.
            if(level < 1 || level > 5)
            {
                if(level > 5)
                    balloon.setMSG("그렇게 빨리 못뜁니다.");

                else if(level < 1)
                    balloon.setMSG("그럼 아예 걷지 말란 거잖아요.");

                return;
            }

            //속도가 조건에 맞을 경우.
            switch(level)
            {
                case 1:
                    this.speedTerm = (int)(Constants.NAMYONG_SPEED * 4);
                    balloon.setMSG("1단계로 걷습니다. 많이 느려요.");
                    break;
                case 2:
                    this.speedTerm = (int)(Constants.NAMYONG_SPEED * 2);
                    balloon.setMSG("2단계로 걷습니다. 쪼매 느려요.");
                    break;
                case 3:
                    this.speedTerm = (Constants.NAMYONG_SPEED);
                    balloon.setMSG("3단계로 걷습니다. 이게 내 평소 걸음속도에요.");
                    break;
                case 4:
                    this.speedTerm = (int)(Constants.NAMYONG_SPEED * 0.5);
                    balloon.setMSG("4단계로 뜁니다. 좀 빠른데 헉헉..");
                    break;
                case 5:
                    this.speedTerm = (int)(Constants.NAMYONG_SPEED * 0.25);
                    balloon.setMSG("5단계로 뛰라고? 이건 미친짓이야..");
                    break;
                default:
                    this.speedTerm = (Constants.NAMYONG_SPEED);
                    balloon.setMSG("뭔가 이상한데?");
                    break;
            }

            
        }

        //자기를 소개
        public void introduce()
        {
            balloon.setMSG("저는 안남용입니다");
        }

        //컴퓨터에 대해서.
        public void sayaboutCom()
        {
            //이미 계산 중이라면 끝낸다.
            if(isComputingRAM)
            {
                thrRAM.Abort();
                isComputingRAM = false;
            }

            //아니라면 진행
            isComputingRAM = true;
            balloon.setMSG("잠시만요.. RAM 정보 계산중이에요..");
            thrRAM = new Thread(() => sayaboutComThread(screen, this));
            thrRAM.Start();
        }

        //위 sayaboutCom에서 작동할 스레드
        public static void sayaboutComThread(ShowScreen screen, Namyong namyong)
        {
            //WMIhandler에 있는 RAM 계산 함수를 작동시켜서 정보를 얻고, 그 스레드를 기다리자
            Thread thrRAMWMI = new Thread(() => WMIhandler.initialRAMdata(screen.getMWinReference().getDatasReference()));
            thrRAMWMI.Start();

            thrRAMWMI.Join();
            screen.getMWinReference().Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            {
                namyong.balloon.setMSG("현재 사용가능한 RAM 용량은 " + screen.getMWinReference().getDatasReference().getUsableRAM() + "에요.");
                namyong.isComputingRAM = false;
            }));
        }







        //relod 시 호출되는 delete 함수
        public void deleteNamyong()
        {
            //사용중인 스레드 종료
            thrmove.Abort();
            if(isJumping) thrjump.Abort();
            if(isGettingKeyword) thrtalk.Abort();
            if(isComputingRAM) thrRAM.Abort();

            thrmove = thrjump = thrtalk = thrRAM = null;

            balloon.deleteBalloon();



        }
    }
}
