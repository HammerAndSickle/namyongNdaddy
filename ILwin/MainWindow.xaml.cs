using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;




namespace ILwin
{


    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        //필요한 정보를 담는 datas 클래스
        private Datas datas;

        //about에 필요한 이미지
        /*private BitmapImage aboutcontentImg;
        private BitmapImage[] okButton;
        private Brush aboutcontentBr;
        private Brush[] okButtonBr;



        //바 버튼들의 위치
        public Rect ICON_RECT = new Rect(4, 2, 13, 14);
        public Rect MINI_RECT = new Rect(760, 2, 14, 14);
        public Rect X_RECT = new Rect(780, 2, 14, 14);

        private BitmapImage mainBarImg;
        private BitmapImage mainContent;
        private BitmapImage[] iconImg;
        private BitmapImage[] miniImg;
        private BitmapImage[] xImg;

        private bool iconclicked = false;
        private bool miniclicked = false;
        private bool xclicked = false;

        private Brush[] iconBr;
        private Brush[] miniBr;
        private Brush[] xBr;

        //레이아웃 버튼들 - request
        public Rect BUTTONS_RECT = new Rect(20, 500, 338, 174);
        public Rect BUTTON1_RECT = new Rect(20, 10, 300, 50);
        public Rect BUTTON2_RECT = new Rect(20, 90, 300, 50);
        private BitmapImage req_recImg;
        private BitmapImage button1Img;
        private BitmapImage button2Img;
        private BitmapImage requestSndImg;
        private BitmapImage requestSndImgClicked;
        private Brush req_recBr;
        private Brush button1Br;
        private Brush button2Br;
        private Brush requestSndBr;
        private Brush requestSndBrClicked;

        //레이아웃 버튼들 - response
        private BitmapImage resp_recImg;
        private Brush resp_recBr;*/

        //바 버튼들의 위치
        public Rect ICON_RECT = new Rect(4, 2, 13, 14);
        public Rect MINI_RECT = new Rect(760, 2, 14, 14);
        public Rect X_RECT = new Rect(780, 2, 14, 14);

        //packs에서 가져올 Brush들
        private Brush[] iconBr;
        private Brush[] miniBr;
        private Brush[] xBr;
        private Brush req_recBr;
        private Brush button1Br;
        private Brush button2Br;
        private Brush requestSndBr;
        private Brush requestSndBrClicked;
        private Brush resp_recBr;
        private Brush marinBarBr;
        private Brush contentBr;
        private Brush aboutcontentBr;
        private Brush[] okButtonBr;

        //Datas나 이미지들이 담긴 packs.
        private ILwin.paraPackage packs;

        private ILwin.ShowScreen screen;
        public Rect SCREEN_RECT = new Rect(20, 30, 760, 470);

        public MainWindow(ILwin.paraPackage packs)
        {
            InitializeComponent();

            //init에서 만든 정보들을 모두 받아온다.
            this.packs = packs;
            datas = packs.datas;
            iconBr = packs.iconBr;
            miniBr = packs.miniBr;
            xBr = packs.xBr;
            req_recBr = packs.req_recBr;
            button1Br = packs.button1Br;
            button2Br = packs.button2Br;
            requestSndBr = packs.requestSndBr;
            requestSndBrClicked = packs.requestSndBrClicked;
            resp_recBr = packs.resp_recBr;
            aboutcontentBr = packs.aboutcontentBr;
            okButtonBr = packs.okButtonBr;
            marinBarBr = packs.marinBarBr;
            contentBr = packs.contentBr;
            

            //메인 윈도우 바를 만들기 위해 호출.
            createBar();

            //윈도우와 버튼들을 만들기 위해 호출.
            createLayout();


            //윈도우를 만든다.
            screen = new ShowScreen(this.animationRec, SCREEN_RECT, this);
        }

        public void createLayout()
        {
            //매개변수로 받아온 packs에 있는 이미지들을 모두 불러들인다.

            /*req_recImg = new BitmapImage();
            button1Img = new BitmapImage();
            button2Img = new BitmapImage();
            requestSndImg = new BitmapImage();
            requestSndImgClicked = new BitmapImage();
            resp_recImg = new BitmapImage();

            req_recImg.BeginInit();
            req_recImg.UriSource = new Uri(Constants.REL_PATH + "interactrec.png", UriKind.Relative);
            req_recImg.EndInit();

            button1Img.BeginInit();
            button1Img.UriSource = new Uri(Constants.REL_PATH + "1button.bmp", UriKind.Relative);
            button1Img.EndInit();

            button2Img.BeginInit();
            button2Img.UriSource = new Uri(Constants.REL_PATH + "2button.bmp", UriKind.Relative);
            button2Img.EndInit();

            requestSndImg.BeginInit();
            requestSndImg.UriSource = new Uri(Constants.REL_PATH + "reqSend.bmp", UriKind.Relative);
            requestSndImg.EndInit();

            requestSndImgClicked.BeginInit();
            requestSndImgClicked.UriSource = new Uri(Constants.REL_PATH + "reqSendClicked.bmp", UriKind.Relative);
            requestSndImgClicked.EndInit();

            resp_recImg.BeginInit();
            resp_recImg.UriSource = new Uri(Constants.REL_PATH + "interactrec2.png", UriKind.Relative);
            resp_recImg.EndInit();

            req_recBr = new ImageBrush(req_recImg);
            button1Br = new ImageBrush(button1Img);
            button2Br = new ImageBrush(button2Img);
            requestSndBr = new ImageBrush(requestSndImg);
            requestSndBrClicked = new ImageBrush(requestSndImgClicked);
            resp_recBr = new ImageBrush(resp_recImg);
             */

            this.buttons.Background = req_recBr;
            this.button1.Background = button1Br;
            this.button2.Background = button2Br;
            this.requestSend.Background = requestSndBr;
            this.requestMachine.FontSize = 11;
            this.requestMachine.KeyDown += new KeyEventHandler(tb_KeyDown);
            this.responses.Background = resp_recBr;
            this.responseMsgs.FontSize = 11;
            

            ILtextBox.printMSG(this.responseMsgs, "Namyong N Daddy started");
            ILtextBox.printMSG(this.responseMsgs, "온도 : " + datas.getTemper() + ", 날씨 : " + datas.getWeather());
            
            System.Diagnostics.Debug.WriteLine("메시지aaaddddddddd");

            
            //screen = new ShowScreen(this.animationRec);
            

        }

        public void createBar()
        {
            
            //메뉴바
            /*mainBarImg = new BitmapImage();
            mainBarImg.BeginInit();
            mainBarImg.UriSource = new Uri(Constants.REL_PATH + "mainbar.bmp", UriKind.Relative);
            //bi.UriSource = new Uri("D:\\igongwin\\ILwin\\ILwin\\rscs\\winbar\\mainbar.bmp", UriKind.Absolute);
            mainBarImg.EndInit();

            //몸통
            mainContent = new BitmapImage();
            mainContent.BeginInit();
            mainContent.UriSource = new Uri(Constants.REL_PATH + "maincontent.png", UriKind.Relative);
            mainContent.EndInit();

            //버튼들을 만든다. [0]은 노말, [1]은 마우스오버 때.
            iconImg = new BitmapImage[2];
            iconImg[0] = new BitmapImage();
            iconImg[0].BeginInit();
            iconImg[0].UriSource = new Uri(Constants.REL_PATH + "ilicon.bmp", UriKind.Relative);
            iconImg[0].EndInit();
            iconImg[1] = new BitmapImage();
            iconImg[1].BeginInit();
            iconImg[1].UriSource = new Uri(Constants.REL_PATH + "iliconOver.bmp", UriKind.Relative);
            iconImg[1].EndInit();


            
            miniImg = new BitmapImage[2];
            miniImg[0] = new BitmapImage();
            miniImg[0].BeginInit();
            miniImg[0].UriSource = new Uri(Constants.REL_PATH + "minibutton.bmp", UriKind.Relative);
            miniImg[0].EndInit();
            miniImg[1] = new BitmapImage();
            miniImg[1].BeginInit();
            miniImg[1].UriSource = new Uri(Constants.REL_PATH + "minibuttonOver.bmp", UriKind.Relative);
            miniImg[1].EndInit();

            xImg = new BitmapImage[2];
            xImg[0] = new BitmapImage();
            xImg[0].BeginInit();
            xImg[0].UriSource = new Uri(Constants.REL_PATH + "xbutton.bmp", UriKind.Relative);
            xImg[0].EndInit();
            xImg[1] = new BitmapImage();
            xImg[1].BeginInit();
            xImg[1].UriSource = new Uri(Constants.REL_PATH + "xbuttonOver.bmp", UriKind.Relative);
            xImg[1].EndInit();


            //******************ABOUT 창
            aboutcontentImg = new BitmapImage();
            aboutcontentImg.BeginInit();
            aboutcontentImg.UriSource = new Uri(Constants.REL_PATH_ABOUT + "about.png", UriKind.Relative);
            aboutcontentImg.EndInit();

            aboutcontentBr = new ImageBrush(aboutcontentImg);

            okButton = new BitmapImage[2];
            okButton[0] = new BitmapImage();
            okButton[0].BeginInit();
            okButton[0].UriSource = new Uri(Constants.REL_PATH_ABOUT + "aboutOK.bmp", UriKind.Relative);
            okButton[0].EndInit();
            okButton[1] = new BitmapImage();
            okButton[1].BeginInit();
            okButton[1].UriSource = new Uri(Constants.REL_PATH_ABOUT + "aboutOKClicked.bmp", UriKind.Relative);
            okButton[1].EndInit();


            //브러쉬 데이터 타입으로 만든다.

            Brush marinBarBr = new ImageBrush(mainBarImg);
            Brush contentBr = new ImageBrush(mainContent);
            iconBr = new Brush[2];
            miniBr = new Brush[2];
            xBr = new Brush[2];
            okButtonBr = new Brush[2];

            for (int i = 0; i < 2; i++ )
            {
                iconBr[i] = new ImageBrush(iconImg[i]);
                miniBr[i] = new ImageBrush(miniImg[i]);
                xBr[i] = new ImageBrush(xImg[i]);
                okButtonBr[i] = new ImageBrush(okButton[i]);
            }*/

            //바에 적용
            this.MainBar.Background = marinBarBr;

            //몸통에 적용
            this.Content.Background = contentBr;

            //버튼에 적용
            this.icon.Background = iconBr[0];
            this.minimi.Background = miniBr[0];
            this.xbutton.Background = xBr[0];



            xbutton.MouseUp += new MouseButtonEventHandler(x_up);
        }




        //창 부분의 마우스 핸들러를 위함.
        private void entire_dragging(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Window win = sender as Window;

            int cX, cY;
            cX = (int)e.GetPosition(win).X; cY = (int)e.GetPosition(win).Y;

            if( isInThere(cX, cY, ICON_RECT) || isInThere(cX, cY, MINI_RECT) || isInThere(cX, cY, X_RECT))
                return;

			else this.DragMove();
        	// TODO: 여기에 구현된 이벤트 처리기를 추가하십시오.
        }

        //아이콘의 핸들러들
        private void icon_mousemove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.icon.Background = iconBr[1];

        }
        private void icon_mouseleave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.icon.Background = iconBr[0];
        }
        private void icon_up(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Window aboutWin = new AboutWindow(aboutcontentBr, okButtonBr[0], okButtonBr[1]);
            aboutWin.Show();
        }
        private void icon_clicked(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        //최소화 버튼의 핸들러들
        private void mini_mousemove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.minimi.Background = miniBr[1];
        }
        private void mini_mouseleave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.minimi.Background = miniBr[0];
        }
        private void mini_clicked(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        }
        private void mini_up(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
             this.WindowState = WindowState.Minimized;
        }

        //x 버튼의 핸들러들
        private void x_mousemove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.xbutton.Background = xBr[1];
        }
        private void x_mouseleave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.xbutton.Background = xBr[0];
        }
        private void x_up(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Environment.Exit(0);
            System.Diagnostics.Process.GetCurrentProcess().Kill();
            this.Close();
        }

        //requestSnd
        private void sndenter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.requestSend.Background = this.requestSndBrClicked;
        }
        private void sndleave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.requestSend.Background = this.requestSndBr;
        }
        private void sndclick(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ILtextBox.handleRequest(this.responseMsgs, this.requestMachine.Text);
            this.requestMachine.Text = "";
        }
        private void tb_KeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.Key == Key.Return)
            {
                //enter key is down
                ILtextBox.handleRequest(this.responseMsgs, this.requestMachine.Text);
                this.requestMachine.Text = "";
            }
        }


        //마우스 x, y가 rect 안에 있는가?
        public bool isInThere(int x, int y, Rect rect)
        {
            //x와 y가 rect 안에 있냐
            if ((x >= rect.TopLeft.X && y >= rect.TopLeft.Y) && (x <= rect.BottomRight.X && y <= rect.BottomRight.Y))
                return true;

            else return false;
        }
    
    }

    



}
