using System;
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
        public Grid sp;             //위 rectangle들이 들어있는 grid

        private ILwin.MainWindow MWin;      //mainwindow의 레퍼런스
        private ILwin.paraPackage packs;      //이미지 리소스가 담긴 packs
        //flying box 객체
        private ILwin.flyingBox flyingbox;

        public ShowScreen(System.Windows.Shapes.Rectangle mainwin, Rect winRect, ILwin.MainWindow winref, ILwin.paraPackage packs)
        {
            MWin = winref;
            this.packs = packs;

            bigRectangle = mainwin;
            this.bigRect = winRect;
            this.winRect = new Rect(winRect.X, winRect.Y + 20, winRect.Width, winRect.Height - 20);
            this.logoRect = new Rect(winRect.X, winRect.Y, winRect.Width, 20);

            //winRectangle을 만들 것이다.
            winRectangle = new System.Windows.Shapes.Rectangle();
            winRectangle.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            winRectangle.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            winRectangle.Margin = new Thickness(0, 20, 0, 0);
            winRectangle.Width = bigRectangle.Width;        //가로는 bigrectangle과 같다.
            winRectangle.Height = bigRectangle.Height - 20; //세로는 좀 더 작어.
            sp = (Grid)bigRectangle.Parent;
            sp.Children.Add(winRectangle);                  //부모 grid에 추가한다.

            //logoRectangle을 만들 것이다.
            logoRectangle = new System.Windows.Shapes.Rectangle();
            logoRectangle.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            logoRectangle.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            logoRectangle.Margin = new Thickness(0, 0, 0, 0);
            logoRectangle.Width = bigRectangle.Width;        //가로는 bigrectangle과 같다.
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
            /*logoRectangle.Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri(Constants.REL_PATH + "screenlogo.bmp", UriKind.Relative))
            };*/
            //logoRectangle.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(194, 194, 194));
            //logoRectangle.StrokeThickness = 1;

            //확인용
            System.Diagnostics.Debug.WriteLine("showing");
            System.Diagnostics.Debug.WriteLine("width : " + bigRectangle.Width + ", height : " + bigRectangle.Height);
            System.Diagnostics.Debug.WriteLine("width : " + winRectangle.Width + ", height : " + winRectangle.Height);
            System.Diagnostics.Debug.WriteLine("width : " + logoRectangle.Width + ", height : " + logoRectangle.Height);
            System.Diagnostics.Debug.WriteLine("showing");
            System.Diagnostics.Debug.WriteLine("topleft : " + bigRect.TopLeft + ", bottomright : " + bigRect.BottomRight
                + ", width : " + bigRect.Width + ", height : " + bigRect.Height);
            System.Diagnostics.Debug.WriteLine("topleft : " + winRect.TopLeft + ", bottomright : " + winRect.BottomRight
                + ", width : " + winRect.Width + ", height : " + winRect.Height);
            
            //테스트 출력.
            winRectangle.Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri(Constants.REL_PATH_BG2_SUNNY + "city1.png", UriKind.Relative))
            };

            generateSprite();
            generateBox();

            //winBox.DrawRectangle(grayPen, targetRect);
            
        }

        //테스트용 스프라이트 생성
        public void generateSprite()
        {
            //Image를 가져와(FromFile로) Bitmap으로 타입캐스띵 해주면 될 거이다.
            BitmapImage pictogram = new BitmapImage(new Uri(Constants.REL_PATH_SPRITE + "tman1.png", UriKind.Relative));
            //Bitmap pictogram = (Bitmap)System.Drawing.Image.FromFile(Constants.REL_PATH_SPRITE + "man1.png", true);
            System.Diagnostics.Debug.WriteLine("png width : " + pictogram.Width + ", height : " + pictogram.Height);
            

            //image가 들어갈 rectangle이 만들어져야 한다.
            System.Windows.Shapes.Rectangle imgRec = new System.Windows.Shapes.Rectangle();
            imgRec.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            //imgRec.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            imgRec.Margin = new Thickness(0, 250, 0, 0);
            imgRec.Width = pictogram.Width;
            imgRec.Height = pictogram.Height;
            imgRec.Fill = new ImageBrush(pictogram);

            sp.Children.Add(imgRec);

            //쓰레드를 만들어보자.
            Thread thr = new Thread(() => working(MWin, imgRec));
            thr.Start();
            //ThreadStart th = new ThreadStart(working);
            
        }
        
        //flying box를 생성한다. flying box는 최소 하나 존재하므로 datas에 포함시키지 않는다.
        public void generateBox()
        {
            //flying box를 만든다.
            flyingbox = new flyingBox( packs.boxBr, this);

            flyingbox.startmove();


        }

        //webImage를 생성한다. 단, 더 생성 가능한지만 확인하고, 가능하다면 스레드를 돌린다.
        public void generateWebImage(string query, int num)
        {
            if(MWin.getDatasReference().webItems.Count + num > Constants.MAX_WEB_ITEMS )
            {
                MWin.getTextboxReference().printMSG(MWin.responseMsgs, "최대 Web Item 개수를 넘는다. 안돼");
                return;
            }

            Thread thr = new Thread(() => generateWebImageThread(MWin.getDatasReference(), flyingbox, query, num));
            thr.Start();
        }

        //webImage를 생성한다. 하나의 query에 대해 주어진 개수만큼 이미지를 가져올 것이다.
        public static void generateWebImageThread(Datas datas, flyingBox flyingbox, string query, int num)
        {

            List<string> urls = new List<string>();

            //string 리스트에 url을 필요한 개수만큼 받아온다.
            HTMLhandler.getImages(urls, num, query);
            //Thread urlgetThread = new Thread(() => HTMLhandler.getImages(urls, num, query));

            //*************위 urlgetThread가 모두 끝날 때까지 기다린다. 그리고 나서 webitems를 추가하라.

            for(int i = 0; i < num; i++)
            {
                //가져온 string 리스트의 url을 하나하나 넣어 webItem를 생성한다.
                datas.addWebitems(urls[i], flyingbox.xpos, flyingbox.ypos);
            }
        }

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
                    //winRectangle.UpdateLayout();
                }

                for (int i = 0; i < 15; i++)
                {
                    mWin.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                    {
                        imgRec.Margin = new Thickness(imgRec.Margin.Left - 10, imgRec.Margin.Top,
                        imgRec.Margin.Right, imgRec.Margin.Bottom);
                    }));


                    Thread.Sleep(50);
                    //winRectangle.UpdateLayout();
                }
            }
        }


        public ILwin.MainWindow getMWinReference()
        {
            return MWin;
        }
    }
}
