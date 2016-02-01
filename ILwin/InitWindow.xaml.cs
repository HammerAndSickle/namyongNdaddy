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
using System.Windows.Shapes;
using System.Threading;
using System.Windows.Threading;
using System.ComponentModel;
using System.Timers;

namespace ILwin
{
    /// <summary>
    /// InitWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 


    //mainwindow로 전달할 모든 정보들.
    public class paraPackage
    {
        //필요한 정보를 담는 datas 클래스
        public Datas datas;

        //about에 필요한 이미지
        public BitmapImage aboutcontentImg;
        public BitmapImage[] okButton;
        public Brush aboutcontentBr;
        public Brush[] okButtonBr;

        //flying 박스 이미지들
        //[0] left normal, [1] right normal
        //[2] left open, [3] right open
        public BitmapImage[] boxImgs;
        public Brush[] boxBr;


        //말풍선
        //[0] 왼쪽으로 이동중인 스프라이트의 말풍선(오른쪽에 뜸)
        //[1] 오른쪽으로 이동중인 스프라이트의 말풍선(왼쪽에 뜸)
        public BitmapImage[] balloonImgs;
        public Brush[] balloonBr;

        //바 버튼들의 위치
        public Rect ICON_RECT = new Rect(4, 2, 13, 14);
        public Rect MINI_RECT = new Rect(760, 2, 14, 14);
        public Rect X_RECT = new Rect(780, 2, 14, 14);

        public BitmapImage mainBarImg;
        public BitmapImage mainContent;
        public BitmapImage[] iconImg;
        public BitmapImage[] miniImg;
        public BitmapImage[] xImg;
        public BitmapImage bottomImg;
        public Brush[] iconBr;
        public Brush[] miniBr;
        public Brush[] xBr;
        public Brush marinBarBr;
        public Brush contentBr;
        public Brush bottomBr;


        //레이아웃 버튼들 - request
        public Rect BUTTONS_RECT = new Rect(20, 500, 338, 174);
        public Rect BUTTON1_RECT = new Rect(20, 10, 300, 50);
        public Rect BUTTON2_RECT = new Rect(20, 90, 300, 50);
        public BitmapImage req_recImg;
        public BitmapImage button1Img;
        public BitmapImage button2Img;
        public BitmapImage requestSndImg;
        public BitmapImage requestSndImgClicked;
        public Brush req_recBr;
        public Brush button1Br;
        public Brush button2Br;
        public Brush requestSndBr;
        public Brush requestSndBrClicked;


        //레이아웃 버튼들 - response
        public BitmapImage resp_recImg;
        public Brush resp_recBr;



    }

    public class  EventClass : INotifyPropertyChanged
    {
        //이 변수가 바뀔 것이다.
        public String leftLoading;

        public EventClass()
        {
            leftLoading = "3";
        }

        //변수 감지 set은 여기에..
        public string LeftLoading
        {
            get { return leftLoading; }
            set
            {
                leftLoading = value;
                INotifyPropertyChanged(LeftLoading);
            }
        }

        //이벤트
        public event PropertyChangedEventHandler PropertyChanged;


        private void INotifyPropertyChanged(string leftloadings)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(leftloadings));
            }

          
        }
    }

    public partial class InitWindow : Window
    {
        //이벤트 핸들 클래스
        EventClass eventclass = new EventClass();

        //mainwindow에 전달할 것들
        public paraPackage packs;

        //init 윈도우의 리소스들
        public BitmapImage initImg;
        public Brush initBr;
        public BitmapImage initImgfin;
        public Brush initfinBr;

        //3가지가 모두 준비되면 main window로 들어간다.
        //이건 3으로 초기화되며, 0이 되면 트리거 작동하는 식이다.
        DispatcherTimer dispatcherTimer;

        public InitWindow()
        {
            InitializeComponent();

            packs = new paraPackage();


            eventclass.PropertyChanged += new PropertyChangedEventHandler(eventclass_propertcyChanged);

           

            
            //leftLoading = 3;

            //윈도우 가운데에 위치하도록 함.
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);

            //로딩 화면을 만든다.
            initImg = new BitmapImage();
            initImgfin = new BitmapImage();

            initImg.BeginInit();
            initImg.UriSource = new Uri(Constants.REL_PATH_INIT + "il_loading1.png", UriKind.Relative);
            initImg.EndInit();
            initBr = new ImageBrush(initImg);
            this.initwin.Background = initBr;
            initImgfin.BeginInit();
            initImgfin.UriSource = new Uri(Constants.REL_PATH_INIT + "il_loadingfinished.png", UriKind.Relative);
            initImgfin.EndInit();
            initfinBr = new ImageBrush(initImgfin);


            //이 객체에 계속해서 정보를 담을 것이다.
            packs.datas = new Datas();

            //모든 준비가 다 되었는지를 체크하는 스레드

            //Thread preparing = new Thread(() => checking(this));
            //preparing.Start();

            //1번째. 이미지 loading
            Thread thr1 = new Thread(() => initImages(this));
            thr1.Start();

            //2번째. web 데이터 loading
            Thread thr2 = new Thread(() => initWeather(this));
            thr2.Start();

            //3번째. wmi 데이터 loading
            Thread thr3 = new Thread(() => initWMI(this));
            thr3.Start();

            this.msgs.Text = "thread all fired";
        }

        //이벤트 트리거 감지 함수
        //이 함수는 static 아닌가보다. 폼에 접근 가능하네.
        void eventclass_propertcyChanged(object sender, PropertyChangedEventArgs e)
        {
            //0 이하가 되면 로딩 끝. 시작!
            if(Convert.ToInt32(e.PropertyName) <= 0)
            {
                this.initwin.Background = this.initfinBr;

                dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
                //타이머 함수도 스레드 다루듯이 이렇게 넣을 수 있다. 유의!
                dispatcherTimer.Tick += (s, args) => goto_mainwin(this);
                dispatcherTimer.Interval = new TimeSpan(0, 0, 2);
                dispatcherTimer.Start();

            }
   
        }

        //타이머 함수. 로딩이 끝나면 잠시 후에 이걸 실행시킬 것이다.
        static void goto_mainwin(ILwin.InitWindow thiswin)
        {
            thiswin.dispatcherTimer.Stop();
            Window mainWin = new MainWindow(thiswin.packs);
            mainWin.Show();
            thiswin.Close();
            
        }

        //이미지들을 읽어들인다.
        public static void initImages(ILwin.InitWindow thiswin)
        {
            thiswin.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                {

                    //main window에 쓰이는 이미지들을 할당한다.
                    thiswin.packs.req_recImg = new BitmapImage();
                    thiswin.packs.button1Img = new BitmapImage();
                    thiswin.packs.button2Img = new BitmapImage();
                    thiswin.packs.requestSndImg = new BitmapImage();
                    thiswin.packs.requestSndImgClicked = new BitmapImage();
                    thiswin.packs.resp_recImg = new BitmapImage();
                    thiswin.packs.bottomImg = new BitmapImage();

                    //main window에 쓰이는 이미지들을 디스크에서 읽어들인다.
                    thiswin.packs.req_recImg.BeginInit();
                    thiswin.packs.req_recImg.UriSource = new Uri(Constants.REL_PATH + "interactrec.png", UriKind.Relative);
                    thiswin.packs.req_recImg.EndInit();

                    thiswin.packs.button1Img.BeginInit();
                    thiswin.packs.button1Img.UriSource = new Uri(Constants.REL_PATH + "1button.bmp", UriKind.Relative);
                    thiswin.packs.button1Img.EndInit();

                    thiswin.packs.button2Img.BeginInit();
                    thiswin.packs.button2Img.UriSource = new Uri(Constants.REL_PATH + "2button.bmp", UriKind.Relative);
                    thiswin.packs.button2Img.EndInit();

                    thiswin.packs.requestSndImg.BeginInit();
                    thiswin.packs.requestSndImg.UriSource = new Uri(Constants.REL_PATH + "reqSend.bmp", UriKind.Relative);
                    thiswin.packs.requestSndImg.EndInit();

                    thiswin.packs.requestSndImgClicked.BeginInit();
                    thiswin.packs.requestSndImgClicked.UriSource = new Uri(Constants.REL_PATH + "reqSendClicked.bmp", UriKind.Relative);
                    thiswin.packs.requestSndImgClicked.EndInit();

                    thiswin.packs.resp_recImg.BeginInit();
                    thiswin.packs.resp_recImg.UriSource = new Uri(Constants.REL_PATH + "interactrec2.png", UriKind.Relative);
                    thiswin.packs.resp_recImg.EndInit();

                    thiswin.packs.bottomImg.BeginInit();
                    thiswin.packs.bottomImg.UriSource = new Uri(Constants.REL_PATH + "bottom.bmp", UriKind.Relative);
                    thiswin.packs.bottomImg.EndInit();


                    //main window에 쓰이는 이미지들을 브러쉬화 하여 background에 적용할 준비를 모두 마친다.
                    thiswin.packs.req_recBr = new ImageBrush(thiswin.packs.req_recImg);
                    thiswin.packs.button1Br = new ImageBrush(thiswin.packs.button1Img);
                    thiswin.packs.button2Br = new ImageBrush(thiswin.packs.button2Img);
                    thiswin.packs.requestSndBr = new ImageBrush(thiswin.packs.requestSndImg);
                    thiswin.packs.requestSndBrClicked = new ImageBrush(thiswin.packs.requestSndImgClicked);
                    thiswin.packs.resp_recBr = new ImageBrush(thiswin.packs.resp_recImg);
                    thiswin.packs.bottomBr = new ImageBrush(thiswin.packs.bottomImg);


                    //flying box 이미지들
                    thiswin.packs.boxImgs = new BitmapImage[4];
                    thiswin.packs.boxBr = new Brush[4];

                    for (int i = 0; i < 4; i++ )
                    {
                        thiswin.packs.boxImgs[i] = new BitmapImage();
                    }

                        thiswin.packs.boxImgs[0].BeginInit();
                    thiswin.packs.boxImgs[0].UriSource = new Uri(Constants.REL_PATH_SPRITE + "n_leftbox.png", UriKind.Relative);
                    thiswin.packs.boxImgs[0].EndInit();
                    thiswin.packs.boxImgs[1].BeginInit();
                    thiswin.packs.boxImgs[1].UriSource = new Uri(Constants.REL_PATH_SPRITE + "n_rightbox.png", UriKind.Relative);
                    thiswin.packs.boxImgs[1].EndInit();
                    thiswin.packs.boxImgs[2].BeginInit();
                    thiswin.packs.boxImgs[2].UriSource = new Uri(Constants.REL_PATH_SPRITE + "o_leftbox.png", UriKind.Relative);
                    thiswin.packs.boxImgs[2].EndInit();
                    thiswin.packs.boxImgs[3].BeginInit();
                    thiswin.packs.boxImgs[3].UriSource = new Uri(Constants.REL_PATH_SPRITE + "o_rightbox.png", UriKind.Relative);
                    thiswin.packs.boxImgs[3].EndInit();

                    for (int i = 0; i < 4; i++)
                        thiswin.packs.boxBr[i] = new ImageBrush(thiswin.packs.boxImgs[i]);

                    //balloons 이미지들
                    thiswin.packs.balloonImgs = new BitmapImage[2];
                    thiswin.packs.balloonBr = new Brush[2];

                    for (int i = 0; i < 2; i++)
                        thiswin.packs.balloonImgs[i] = new BitmapImage();

                    thiswin.packs.balloonImgs[0].BeginInit();
                    thiswin.packs.balloonImgs[0].UriSource = new Uri(Constants.REL_PATH_SPRITE + "trans_leftbal.png", UriKind.Relative);
                    thiswin.packs.balloonImgs[0].EndInit();
                    thiswin.packs.balloonImgs[1].BeginInit();
                    thiswin.packs.balloonImgs[1].UriSource = new Uri(Constants.REL_PATH_SPRITE + "trans_rightbal.png", UriKind.Relative);
                    thiswin.packs.balloonImgs[1].EndInit();

                    for (int i = 0; i < 2; i++)
                        thiswin.packs.balloonBr[i] = new ImageBrush(thiswin.packs.balloonImgs[i]);
                    

                    //바 이미지들

                   


                    //메뉴바
                    thiswin.packs.mainBarImg = new BitmapImage();
                    thiswin.packs.mainBarImg.BeginInit();
                    thiswin.packs.mainBarImg.UriSource = new Uri(Constants.REL_PATH + "mainbar.bmp", UriKind.Relative);
                    //bi.UriSource = new Uri("D:\\igongwin\\ILwin\\ILwin\\rscs\\winbar\\mainbar.bmp", UriKind.Absolute);
                    thiswin.packs.mainBarImg.EndInit();

                    //몸통
                    thiswin.packs.mainContent = new BitmapImage();
                    thiswin.packs.mainContent.BeginInit();
                    thiswin.packs.mainContent.UriSource = new Uri(Constants.REL_PATH + "maincontent.png", UriKind.Relative);
                    thiswin.packs.mainContent.EndInit();

                    //버튼들을 만든다. [0]은 노말, [1]은 마우스오버 때.
                    thiswin.packs.iconImg = new BitmapImage[2];
                    thiswin.packs.iconImg[0] = new BitmapImage();
                    thiswin.packs.iconImg[0].BeginInit();
                    thiswin.packs.iconImg[0].UriSource = new Uri(Constants.REL_PATH + "ilicon.bmp", UriKind.Relative);
                    thiswin.packs.iconImg[0].EndInit();
                    thiswin.packs.iconImg[1] = new BitmapImage();
                    thiswin.packs.iconImg[1].BeginInit();
                    thiswin.packs.iconImg[1].UriSource = new Uri(Constants.REL_PATH + "iliconOver.bmp", UriKind.Relative);
                    thiswin.packs.iconImg[1].EndInit();



                    thiswin.packs.miniImg = new BitmapImage[2];
                    thiswin.packs.miniImg[0] = new BitmapImage();
                    thiswin.packs.miniImg[0].BeginInit();
                    thiswin.packs.miniImg[0].UriSource = new Uri(Constants.REL_PATH + "minibutton.bmp", UriKind.Relative);
                    thiswin.packs.miniImg[0].EndInit();
                    thiswin.packs.miniImg[1] = new BitmapImage();
                    thiswin.packs.miniImg[1].BeginInit();
                    thiswin.packs.miniImg[1].UriSource = new Uri(Constants.REL_PATH + "minibuttonOver.bmp", UriKind.Relative);
                    thiswin.packs.miniImg[1].EndInit();

                    thiswin.packs.xImg = new BitmapImage[2];
                    thiswin.packs.xImg[0] = new BitmapImage();
                    thiswin.packs.xImg[0].BeginInit();
                    thiswin.packs.xImg[0].UriSource = new Uri(Constants.REL_PATH + "xbutton.bmp", UriKind.Relative);
                    thiswin.packs.xImg[0].EndInit();
                    thiswin.packs.xImg[1] = new BitmapImage();
                    thiswin.packs.xImg[1].BeginInit();
                    thiswin.packs.xImg[1].UriSource = new Uri(Constants.REL_PATH + "xbuttonOver.bmp", UriKind.Relative);
                    thiswin.packs.xImg[1].EndInit();


                    //******************ABOUT 창
                    thiswin.packs.aboutcontentImg = new BitmapImage();
                    thiswin.packs.aboutcontentImg.BeginInit();
                    thiswin.packs.aboutcontentImg.UriSource = new Uri(Constants.REL_PATH_ABOUT + "about.png", UriKind.Relative);
                    thiswin.packs.aboutcontentImg.EndInit();

                    thiswin.packs.aboutcontentBr = new ImageBrush(thiswin.packs.aboutcontentImg);

                    thiswin.packs.okButton = new BitmapImage[2];
                    thiswin.packs.okButton[0] = new BitmapImage();
                    thiswin.packs.okButton[0].BeginInit();
                    thiswin.packs.okButton[0].UriSource = new Uri(Constants.REL_PATH_ABOUT + "aboutOK.bmp", UriKind.Relative);
                    thiswin.packs.okButton[0].EndInit();
                    thiswin.packs.okButton[1] = new BitmapImage();
                    thiswin.packs.okButton[1].BeginInit();
                    thiswin.packs.okButton[1].UriSource = new Uri(Constants.REL_PATH_ABOUT + "aboutOKClicked.bmp", UriKind.Relative);
                    thiswin.packs.okButton[1].EndInit();

                    thiswin.packs.marinBarBr = new ImageBrush(thiswin.packs.mainBarImg);
                    thiswin.packs.contentBr = new ImageBrush(thiswin.packs.mainContent);
                    thiswin.packs.iconBr = new Brush[2];
                    thiswin.packs.miniBr = new Brush[2];
                    thiswin.packs.xBr = new Brush[2];
                    thiswin.packs.okButtonBr = new Brush[2];

                    for (int i = 0; i < 2; i++)
                    {
                        thiswin.packs.iconBr[i] = new ImageBrush(thiswin.packs.iconImg[i]);
                        thiswin.packs.miniBr[i] = new ImageBrush(thiswin.packs.miniImg[i]);
                        thiswin.packs.xBr[i] = new ImageBrush(thiswin.packs.xImg[i]);
                        thiswin.packs.okButtonBr[i] = new ImageBrush(thiswin.packs.okButton[i]);
                    }



                    //**이벤트 클래스의 leftLoading에 직접 접근하는 게 아니라, 그 set 함수에 접근해라!! 안 그러면 이벤트 안떠
                    thiswin.eventclass.LeftLoading = Convert.ToString(Convert.ToInt32(thiswin.eventclass.leftLoading) - 1);
                    
                    thiswin.images.Text = (3 - (Convert.ToInt32(thiswin.eventclass.leftLoading))) + ". Image loading Complete.";
                }));

        }

        //웹 크롤링으로 일단은 기온과 날씨를 얻어온다.
        public static void initWeather(ILwin.InitWindow thiswin)
        {

            HTMLhandler.getWeatherFromHTML(thiswin.packs.datas);

            thiswin.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                    {

                        //**이벤트 클래스의 leftLoading에 직접 접근하는 게 아니라, 그 set 함수에 접근해라!! 안 그러면 이벤트 안떠
                        thiswin.eventclass.LeftLoading = Convert.ToString(Convert.ToInt32(thiswin.eventclass.leftLoading) - 1);
                        thiswin.webs.Text = (3 - (Convert.ToInt32(thiswin.eventclass.leftLoading))) + ". Web data loading Complete.";
                    }));

        }

        //WMI 정보를 얻는다.
        public static void initWMI(ILwin.InitWindow thiswin)
        {
            thiswin.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                {
                    //**이벤트 클래스의 leftLoading에 직접 접근하는 게 아니라, 그 set 함수에 접근해라!! 안 그러면 이벤트 안떠
                    thiswin.eventclass.LeftLoading = Convert.ToString(Convert.ToInt32(thiswin.eventclass.leftLoading) - 1);
                    thiswin.wmis.Text = (3 - (Convert.ToInt32(thiswin.eventclass.leftLoading))) + ". WMI data loading Complete.";

                }));
        }


    }
}
