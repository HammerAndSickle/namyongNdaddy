using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Controls;
using System.Threading;
using System.Windows.Threading;
using System.Windows.Input;

namespace ILwin
{
    class Mall
    {
        //게시판 스프라이트 이미지
        Brush img;
        //게시판 창에 쓰이는 몸통 이미지
        Brush bodyimg;

        //게시판 스프라이트 grid
        Grid imgrec;

        //스프라이트의 텍스트
        TextBox mallText;

        //다루려는 screen 
        public ILwin.ShowScreen screen;

        //텍스트를 계속 바뀌게 하는 스레드
        Thread textchangeTh;

        //이건 윈도우가 켜져있는지, 아닌지를 판별할 datas
        Datas datas;


        //mall 윈도우
        private Window mallWin;

    //rectangle을 생성한다. 초기에 호출될 함수.
        //w와 h는 원래의 bitmapimage의 width, height
        public Mall(Brush img, Brush bodyimg, double w, double h, int xpos, int ypos, ILwin.ShowScreen showscreen, Datas datas)
        {
            Random rnd = new Random();

            this.img = img;
            this.bodyimg = bodyimg;
            this.screen = showscreen;
            this.datas = datas;

            //생성
            imgrec = new Grid();
            imgrec.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            imgrec.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            imgrec.Width = w; imgrec.Height = h;
            imgrec.Background = this.img;
            imgrec.Margin = new Thickness(xpos, ypos, 0, 0);
            screen.sp.Children.Add(imgrec);

            //마우스 핸들러 연결
            imgrec.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(mall_up);
            


            makeText();

            System.Diagnostics.Debug.WriteLine("board 완성");
        }

        public void makeText()
        {
            mallText = new System.Windows.Controls.TextBox();
            mallText.Width = 25;            mallText.Height = 15;
            mallText.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            mallText.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            mallText.Margin = new Thickness(1, 5, 0, 0);
            mallText.BorderThickness = new Thickness(0.0);

            mallText.Background = Brushes.White;
            mallText.IsReadOnly = true;
            mallText.FontSize = 11;
            mallText.FontWeight = FontWeights.Bold;
            mallText.Foreground = Brushes.Black;
            mallText.Text = "";
            mallText.Cursor = Cursors.Arrow;
            imgrec.Children.Add(mallText);


            //스레드 실행
            textchangeTh = new Thread(() => changeColor(screen.getMWinReference(), mallText));
            textchangeTh.Start();
            
        }

        //텍스트를 계속 변하게 만드는 스레드이다.
        public static void changeColor(ILwin.MainWindow mWin, TextBox thetxt)
        {
    

            while (true)
            {
                mWin.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                    {
                        thetxt.Text = "물건";
                        thetxt.Margin = new Thickness(1, 5, 0, 0);
                    }));

                    Thread.Sleep(1000);

                    mWin.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                    {
                        thetxt.Text = "검색";
                        thetxt.Margin = new Thickness(53, 5, 0, 0);
                    }));

                    Thread.Sleep(1000);
                }


               
        }

        //클릭 시 게시판을 열자
        private void mall_up(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(datas.isMallWinOn)
            {
                return;
            }

            mallWin = new MallWindow(bodyimg, datas);
            datas.isMallWinOn = true;
            mallWin.Show();
        }

    }
}
