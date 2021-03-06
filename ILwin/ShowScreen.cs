﻿using System;
using System.Threading;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Windows;


namespace ILwin
{
    public class ShowScreen
    {
        //bigRectangle은 앞 mainWindow에서 그린 xaml의 rectangle을 그대로 참조하는 것.
        public System.Windows.Shapes.Rectangle bigRectangle;
        //bigRectangle 안의 logoRectangle이 바로 출력 화면이 될 것이다.
        public System.Windows.Shapes.Rectangle logoRectangle;
        //bigRectangle 안의 winRectangle이 바로 출력 화면이 될 것이다.
        public System.Windows.Shapes.Rectangle winRectangle;
        

        public Rect bigRect;
        public Rect logoRect;
        public Rect winRect;
        public Grid sp;             //위 rectangle들이 들어있는 grid (showscreengrid)

        private ILwin.MainWindow MWin;      //mainwindow의 레퍼런스
        private ILwin.paraPackage packs;      //이미지 리소스가 담긴 packs
        
        //-Sign 객체
        private ILwin.Sign sign;
        //-flying box 객체
        private ILwin.flyingBox flyingbox;
        //-namyong 객체
        private ILwin.Namyong namyong;
        //-daddy 객체
        private ILwin.Daddy daddy;
        //-Board 객체
        private ILwin.Board board;
        //-Mall 객체
        private ILwin.Mall mall; 

        //위치 선정을 위한 random
        private Random rnd;

        public bool doingWebitem;           //webitem을 떨어뜨리는 중이라면 true. 왜냐면, box images 명령어를 사용하여 webitem을 떨구는 중엔 블록 시키기 위해서이다.

        public ShowScreen(Grid ScreenGrid, Rect winRect, ILwin.MainWindow winref, ILwin.paraPackage packs)
        {
            MWin = winref;
            this.packs = packs;
            this.doingWebitem = false;
            this.rnd = new Random();

            this.bigRect = winRect;
            this.winRect = new Rect(winRect.X, winRect.Y + 20, winRect.Width, winRect.Height - 20);
            this.logoRect = new Rect(winRect.X, winRect.Y, winRect.Width, 20);

            //winRectangle을 만들 것이다.
            winRectangle = new System.Windows.Shapes.Rectangle();
            winRectangle.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            winRectangle.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            winRectangle.Margin = new Thickness(0, 20, 0, 0);
            winRectangle.Width = ScreenGrid.Width;        //가로는 bigrectangle과 같다.
            winRectangle.Height = ScreenGrid.Height - 20; //세로는 좀 더 작어.
            sp = ScreenGrid;
            sp.Children.Add(winRectangle);                  //부모 grid에 추가한다.

            //logoRectangle을 만들 것이다.
            logoRectangle = new System.Windows.Shapes.Rectangle();
            logoRectangle.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            logoRectangle.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            logoRectangle.Margin = new Thickness(0, 0, 0, 0);
            logoRectangle.Width = ScreenGrid.Width;        //가로는 bigrectangle과 같다.
            logoRectangle.Height = 21;                      //세로는 딱 20픽셀이어야 하지만, 두께가 1픽셀이니 감안한다.
            sp.Children.Add(logoRectangle);                  //부모 grid에 추가한다.
         
            
            drawScreen();
            
        }

        public void drawScreen()
        {
      
            //winrect
            winRectangle.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(194, 194, 194));
            winRectangle.StrokeThickness = 1;

            //logorect
            //그 중, 이미지를 rectangle에 채우는 과정(이건 정적으로 만든 게 아닌, 동적으로 만든 rectangle이라 xaml을 다루기 힘들다.
            BitmapImage logoImg = new BitmapImage();
            logoImg.BeginInit();
            logoImg.UriSource = new Uri(Constants.REL_PATH + "screenlogo.png", UriKind.Relative);
            logoImg.EndInit();
            System.Windows.Media.Brush logoBr = new ImageBrush(logoImg);

            //이미지 연결
            logoRectangle.Fill = logoBr;
             
            //배경을 임의로 선택.
            //현재 시간에 따라 선택되어야 한다.
            winRectangle.Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri(selectBG(), UriKind.Relative))
            };

            //generateSprite(); 개발 테스트용
            generateSign();
            generateBox();
            generateMall();
            generateBoard();

            //남용이와 아버지는, 더 아래에 있는 사람이 더 앞으로 보이도록 해야 하므로 난수를 잘 다루어야 한다.
            int yPosNamyong = rnd.Next(180, 271);
            int yPosDaddy = rnd.Next(180, 271);

            //더 큰 숫자를 Ypos(Y 위치)로 갖는 사람이 늦게 생성되어야 한다. 그래야 앞으로 보인다,
            if(yPosNamyong < yPosDaddy)
            {
                generateNamyong(yPosNamyong);
                generateDaddy(yPosDaddy);
            }

            else
            {
                generateDaddy(yPosDaddy);
                generateNamyong(yPosNamyong);
            }
            
            
        }

        public string selectBG()
        {
            //초기값으로는 sunny를 해두자..
            string filename = Constants.REL_PATH_BG2_SUNNY + "sunny";

            //현재 시간을 받아온 후.
            DateTime currTime = DateTime.Now;
            string currHour = currTime.ToString("HH");
            int currHourInt = Convert.ToInt32(currHour);        //시간을 정수 값으로 얻어옴

            //난수 만들자
            Random rd = new Random();
            int imgnum = 0;

            //새벽은 2AM~8AM
            if (currHourInt >= 2 && currHourInt <= 8)
            {
                filename = Constants.REL_PATH_BG2_DAWN + "dawn";
                imgnum = rd.Next(0, Constants.DAWN_IMGS);
            }

            //낮은 9AM~5PM
            else if (currHourInt >= 9 && currHourInt <= 17)
            {
                filename = Constants.REL_PATH_BG2_SUNNY + "sunny";
                imgnum = rd.Next(0, Constants.SUNNY_IMGS);
            }

            //밤은 6PM~1AM
            else if (currHourInt >= 18 || currHourInt <= 1)
            {
                filename = Constants.REL_PATH_BG2_NIGHT + "night";
                imgnum = rd.Next(0, Constants.NIGHT_IMGS);
            }

            filename += imgnum + ".png";

            return filename;
        }

        //테스트용 스프라이트 생성
        public void generateSprite()
        {
            //Image를 가져와(FromFile로) Bitmap으로 타입캐스띵 해주면 될 거이다.
            BitmapImage pictogram = new BitmapImage(new Uri(Constants.REL_PATH_SPRITE + "tman1.png", UriKind.Relative));


            //개발 중 테스트용으로 픽토그램을 움직이게 해 본 코드
            //image가 들어갈 rectangle이 만들어져야 한다.
            System.Windows.Shapes.Rectangle imgRec = new System.Windows.Shapes.Rectangle();
            imgRec.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            imgRec.Margin = new Thickness(0, 250, 0, 0);
            imgRec.Width = pictogram.Width;
            imgRec.Height = pictogram.Height;
            imgRec.Fill = new ImageBrush(pictogram);

            sp.Children.Add(imgRec);

            
            Thread thr = new Thread(() => working(MWin, imgRec));
            thr.Start();
            //ThreadStart th = new ThreadStart(working);
             
            
        }

        //namyong을 생성한다. namyong은 한 개 존재하므로 datas에 포함시키지 않는다.
        public void generateNamyong(int yPos)
        {
            namyong = new Namyong(packs.namyongBr, this,  rnd.Next(0, 701), yPos, rnd.Next(0, 2));
            namyong.startmove();
        }

        //daddy을 생성한다. daddy은 한 개 존재하므로 datas에 포함시키지 않는다.
        public void generateDaddy(int yPos)
        {
            daddy = new Daddy(packs.daddyBr, this,  rnd.Next(0, 701), yPos, rnd.Next(0, 2));
            daddy.startmove();
        }

        //flying box를 생성한다. flying box는 한 개 존재하므로 datas에 포함시키지 않는다.
        public void generateBox()
        {
            //flying box를 만든다.
            flyingbox = new flyingBox(packs.boxBr, this, rnd.Next(0, 701), rnd.Next(0, 71), rnd.Next(0, 2));
            flyingbox.startmove();


        }

        //board icon을 생성한다. board는 한 개 존재하므로 datas에 포함시키지 않는다.
        public void generateBoard()
        {
            //board를 만든다.
            board = new Board(packs.boardBr, packs.boardbodyBr, packs.boardImg.Width, packs.boardImg.Height, rnd.Next(0, 701), rnd.Next(300, 401), this, MWin.getDatasReference());
            
        }

        //mall icon을 생성한다. mall는 한 개 존재하므로 datas에 포함시키지 않는다.
        public void generateMall()
        {
            //mall를 만든다.
            mall = new Mall(packs.mallBr, packs.mallbodyBr, packs.mallImg.Width, packs.mallImg.Height, rnd.Next(0, 701), rnd.Next(300, 401), this, MWin.getDatasReference());

        }

        //sign icon을 생성한다. sign는 한 개 존재하므로 datas에 포함시키지 않는다.
        public void generateSign()
        {
            //sign을 만든다
            sign = new Sign(rnd.Next(0, 701), rnd.Next(140, 170), this, MWin.getDatasReference());
        }

        //webImage를 생성한다. 단, 더 생성 가능한지만 확인하고, 가능하다면 스레드를 돌린다.
        public void generateWebImage(string query, int num)
        {
            if(MWin.getDatasReference().webItems.Count + num > Constants.MAX_WEB_ITEMS )
            {
                MWin.getTextboxReference().printMSG(MWin.responseMsgs, "최대 Web Item 개수를 넘는다. 안돼");
                return;
            }

            if(doingWebitem == true)
            {
                MWin.getTextboxReference().printMSG(MWin.responseMsgs, "지금은 Web Item을 떨어뜨리는 중이다..");
                return;
            }

            //떨어뜨리는 도중에는 doingwebitem을 true로 하여 중복되지 않게 하자.
            doingWebitem = true;

            //이미지들을 생성하기 전, 현존하는 webitems의 수를 가져온다.
            int startCount = MWin.getDatasReference().webItems.Count;
            MWin.getTextboxReference().printMSG(MWin.responseMsgs, "startCount : " + startCount);

            //num 개수만큼 webitem을 추가한 뒤, startCount와 num을 이용해 그것들을 참조해나갈 것이다.
            for (int i = 0; i < num; i++)
            {
                WebItem wb = new WebItem(this, rnd.Next(350, 371));
                MWin.getDatasReference().webItems.Add(wb);
                MWin.getTextboxReference().printMSG(MWin.responseMsgs, "added " + i + " webitem");
            }

            //만일 3개의 이미지를 요청했고, startCount가 2라면, 현재 2개의 item이 존재하며, webimagelist (2) (3) (4)를 만드는 것이다.
            Thread thr = new Thread(() => generateWebImageThread(MWin.getDatasReference(), flyingbox, query, startCount, num, this));
            thr.Start();
            
        }

        //webImage를 생성한다. 하나의 query에 대해 주어진 개수만큼 이미지를 가져올 것이다.
        public static void generateWebImageThread(Datas datas, flyingBox flyingbox, string query, int start, int num, ILwin.ShowScreen showscreen)
        {

            List<string> urls = new List<string>();

            //string 리스트에 url을 필요한 개수만큼 받아온다.
            HTMLhandler.getImages(urls, num, query);

            //*************위 urlgetThread가 모두 끝날 때까지 기다린다. 그리고 나서 webitems를 추가하라.

            //박스 이미지를 변경. 열린 박스로 만든다
            flyingBox.changeImg(flyingbox, showscreen.getMWinReference(), true);

            for(int i = 0; i < num; i++)
            {
                //현재 플라잉 박스의 위치
                int flyingbox_x = 0;
                int flyingbox_y = 0;

                //마지막 web item인지를 확인
                bool isFinal = false;
                if (i == (num - 1)) isFinal = true;


                showscreen.getMWinReference().Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                {
                    flyingbox_x = (int)flyingbox.boximg.Margin.Left;
                    flyingbox_y = (int)flyingbox.boximg.Margin.Top;
                }));

                //가져온 string 리스트의 url을 하나하나 넣어 webItem를 생성한다.
                WebItem.addDatas(showscreen, datas.webItems.ElementAt(start + i), urls[i], flyingbox_x, flyingbox_y);
                WebItem.fallingItem(showscreen, datas.webItems.ElementAt(start + i), isFinal);
            }
        }

        //개발 중 테스트용으로 픽토그램을 움직이게 해 보았다. 
        public static void working(ILwin.MainWindow mWin, System.Windows.Shapes.Rectangle imgRec)
        {
            while (true)
            {

                for (int i = 0; i < 15; i++)
                {
                    mWin.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                    {
                        imgRec.Margin = new Thickness(imgRec.Margin.Left + 10, imgRec.Margin.Top,
                        imgRec.Margin.Right, imgRec.Margin.Bottom);
                    }));


                    Thread.Sleep(50);
                    //winRectangle.UpdateLayout()                
                }

                for (int i = 0; i < 15; i++)
                {
                    mWin.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                    {
                        imgRec.Margin = new Thickness(imgRec.Margin.Left - 10, imgRec.Margin.Top,
                        imgRec.Margin.Right, imgRec.Margin.Bottom);
                    }));


                    Thread.Sleep(50);
                }
            }

        }


        public ILwin.MainWindow getMWinReference()
        {
            return MWin;
        }

        public flyingBox getFlyingboxReference()
        {
            return flyingbox;
        }

        public Daddy getDaddy()
        {
            return daddy;
        }

        public Namyong getNamyong()
        {
            return namyong;
        }

        public Mall getMall()
        {
            return mall;
        }

        public Board getBoard()
        {
            return board;
        }
    }
}
