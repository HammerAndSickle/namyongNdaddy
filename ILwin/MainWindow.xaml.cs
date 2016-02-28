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


        //바 버튼들의 위치
        public Rect ICON_RECT = new Rect(4, 2, 13, 14);
        public Rect MINI_RECT = new Rect(760, 2, 14, 14);
        public Rect X_RECT = new Rect(780, 2, 14, 14);

        public Rect BUTTONS_RECT = new Rect(20, 500, 338, 174);
        public Rect BUTTON1_RECT = new Rect(20, 10, 300, 50);
        public Rect BUTTON2_RECT = new Rect(20, 90, 300, 50);

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
        private Brush bottomBr;
        private Brush[] boxBr;

        //Datas나 이미지들이 담긴 packs.
        private ILwin.paraPackage packs;

        //show screen
        private ILwin.ShowScreen screen;
        private ILwin.ILtextBox textbox;
        public Rect SCREEN_RECT = new Rect(20, 30, 760, 470);

        //윈도우 데이터
        private Window aboutWin;
        private Window helpWin;

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
            bottomBr = packs.bottomBr;
            boxBr = packs.boxBr;

            //텍스트박스는 response에 위치한 그곳이다.
            textbox = new ILtextBox();


            //메인 윈도우 바를 만들기 위해 호출.
            createBar();

            //윈도우와 버튼들을 만들기 위해 호출.
            createLayout();

            //윈도우를 만든다.
            screen = new ShowScreen(showScreenGrid, SCREEN_RECT, this, packs);

            //텍스트박스는 response에 위치한 그곳이다.
            textbox.addShowScreenReference(screen);

            this.requestMachine.Focus();

            
        }

        public void createLayout()
        {
            //매개변수로 받아온 packs에 있는 이미지들을 모두 불러들인다.

            this.buttons.Background = req_recBr;
            this.button1.Background = button1Br;
            this.button2.Background = button2Br;
            this.requestSend.Background = requestSndBr;
            this.requestMachine.FontSize = 11;
            this.requestMachine.KeyDown += new KeyEventHandler(tb_KeyDown);
            this.responses.Background = resp_recBr;
            this.responseMsgs.FontSize = 11;

            //첫 메시지들을 출력
            printInitMsgs();

        }

        public void printInitMsgs()
        {
            DateTime currTime = DateTime.Now;
            string currTimeStr = currTime.ToString("yyyy") + "/" + currTime.ToString("MM") + "/" + currTime.ToString("dd") + " " + currTime.ToString("HH:mm:ss");

            textbox.printMSG(this.responseMsgs, "Namyong N Daddy started");
            textbox.printMSG(this.responseMsgs, "현 로그인 시간 : " + currTimeStr);
            textbox.printMSG(this.responseMsgs, "위치 : " + datas.getLoc());
            textbox.printMSG(this.responseMsgs, "온도 : " + datas.getTemper() + ", 날씨 : " + datas.getWeather());
            textbox.printMSG(this.responseMsgs, "램 사용가능량 : " + datas.getUsableRAM());
        }

        public void createBar()
        {
            

            //바에 적용
            this.MainBar.Background = marinBarBr;

            //몸통에 적용
            this.Content.Background = contentBr;
            this.etcbar.Background = bottomBr;

            //버튼에 적용
            this.icon.Background = iconBr[0];
            this.minimi.Background = miniBr[0];
            this.xbutton.Background = xBr[0];

            xbutton.MouseUp += new MouseButtonEventHandler(x_up);
        }


        //datas 참조가 필요할 때 반환하게 한다
        public Datas getDatasReference()
        {
            return datas;
        }

        //textbox 참조가 필요할 때 반환하게 한다.
        public ILtextBox getTextboxReference()
        {
            return textbox;
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
            openAboutWIndow();
        }
        public void openAboutWIndow()          //아이콘을 누르거나 about 입력 시 실행
        {
            //이미 aboutWindow가 켜져있으면 다시 키지 않도록 한다.
            if (datas.isAboutWinOn) return;

            aboutWin = new AboutWindow(aboutcontentBr, okButtonBr[0], okButtonBr[1], datas);
            aboutWin.Show();
            datas.isAboutWinOn = true;
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
            exitNamyongNDaddy();
        }
        public void exitNamyongNDaddy()         //x버튼 누르거나 exit 입력 시 실행
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
            textbox.handleRequest(this.responseMsgs, this.requestMachine.Text, datas);
            this.requestMachine.Text = "";
        }

        //엔터키를 누르면 request input에 있던 명령어를 프로그램으로 보내는 것이다.
        private void tb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                //enter key is down
                textbox.handleRequest(this.responseMsgs, this.requestMachine.Text, datas);
                this.requestMachine.Text = "";
            }

            
        }

        //텍스트박스에서 위/아래 버튼이 눌러지면 이전의 명령어들을 훑어보는 것.
        //previewKeyDown으로 수행해야 한다. Control 안에서 일어나는 이벤트이기 때문이다. 
        private void textBoxKeydown(object sender, KeyEventArgs e)
        {
            //위 버튼을 누르면 이전에 사용한 명령어를 탐색
            if (e.Key == Key.Up)
            {
                //up key is down
                if(textbox.currentLine > 0) textbox.currentLine--;

                //아무 문자열도 없다면?
                if (textbox.lines <= 0) return;

                this.requestMachine.Text = textbox.textboxlines.ElementAt(textbox.currentLine);
            }

            //아래 버튼을 누르면 이전에 사용한 명령어를 탐색
            else if (e.Key == Key.Down)
            {
                //down key is down
                if (textbox.currentLine < textbox.lines-1) textbox.currentLine++;

                //이미 끝 문자열이라면?
                if (textbox.currentLine >= textbox.lines) return;

                this.requestMachine.Text = textbox.textboxlines.ElementAt(textbox.currentLine);
            }
        }

        //reload 버튼
        private void reload_mousemove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.button1.Background = this.packs.button1CBr;
        }
        private void reload_mouseleave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.button1.Background = this.packs.button1Br;
        }
        private void reload_click(object sender, System.Windows.Input.MouseEventArgs e)
        {
            reloadScreen();
        }

        

        //help 버튼
        private void help_mousemove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.button2.Background = this.packs.button2CBr;
        }
        private void help_mouseleave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.button2.Background = this.packs.button2Br;
        }
        private void help_click(object sender, System.Windows.Input.MouseEventArgs e)
        {
            OpenHelpWindow();

        }
        public void OpenHelpWindow()                //help 버튼을 누르거나 help 입력시 실행
        {
            //이미 helpwindow가 켜져있으면 또 켜지지 않게하라.
            if (this.datas.isHelpWinOn)
                return;

            helpWin = new HelpWindow(this.packs.helpbodyBr, datas);
            helpWin.Show();
            datas.isHelpWinOn = true;
        }


        //마우스 x, y가 rect 안에 있는가?
        public bool isInThere(int x, int y, Rect rect)
        {
            //x와 y가 rect 안에 있냐
            if ((x >= rect.TopLeft.X && y >= rect.TopLeft.Y) && (x <= rect.BottomRight.X && y <= rect.BottomRight.Y))
                return true;

            else return false;
        }




        //RELOAD======================
        public void reloadScreen()          //reload 버튼 클릭 혹은 reload 입력시 실행. 화면을 갱신하는 함수.
        {
            //---여러 객체들을 다 지운다. 거기에 달려있는 스레드들도 제거
            screen.getBoard().deleteBoard();
            screen.getNamyong().deleteNamyong();
            screen.getDaddy().deleteDaddy();
            screen.getFlyingboxReference().deleteBOX();
            screen.getMall().deleteMall();

            
            screen.sp.Children.Clear();
            showScreenGrid.Background = packs.refreshBr;

            //---날씨/장소/시간 정보를 다시 가져온다.
            HTMLhandler.getWeatherFromHTML(packs.datas);



            //윈도우를 만든다.
            screen = new ShowScreen(showScreenGrid, SCREEN_RECT, this, packs);

            //텍스트박스를 재생성
            textbox.textboxlines = new List<string>();
            textbox.lines = textbox.currentLine = 0;
            this.responseMsgs.Text = "";

            printInitMsgs();

            //텍스트박스는 response에 위치한 그곳이다.
            textbox.addShowScreenReference(screen);

            this.requestMachine.Focus();

        }
    }

    



}
