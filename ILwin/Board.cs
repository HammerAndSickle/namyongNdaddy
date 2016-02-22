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
    class Board
    {
        //게시판 스프라이트 이미지
        Brush img;
        //게시판 창에 쓰이는 몸통 이미지
        Brush bodyimg;

        //게시판 스프라이트 grid
        Grid imgrec;

        //스프라이트의 텍스트
        TextBox boardText;

        //다루려는 screen 
        public ILwin.ShowScreen screen;

        //색깔을 계속 변하게 하는 스레드
        Thread colorchangeTh;


        //board 윈도우
        private Window boardWin;

        //이건 윈도우가 켜져있는지, 아닌지를 판별할 datas
        Datas datas;

     
        

        //rectangle을 생성한다. 초기에 호출될 함수.
        //w와 h는 원래의 bitmapimage의 width, height
        public Board(Brush img, Brush bodyimg, double w, double h, int xpos, int ypos, ILwin.ShowScreen showscreen, Datas datas)
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
            //imgrec.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(board_up);
            


            makeText();

            System.Diagnostics.Debug.WriteLine("board 완성");
        }

        public void makeText()
        {
            boardText = new System.Windows.Controls.TextBox();
            boardText.Width = 44;            boardText.Height = 13;
            boardText.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            boardText.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            boardText.Margin = new Thickness(7, 10, 0, 0);
            boardText.BorderThickness = new Thickness(0.0);

            boardText.Background = Brushes.Black;
            boardText.IsReadOnly = true;
            boardText.FontSize = 12;
            boardText.FontWeight = FontWeights.Bold;
            boardText.Foreground = Brushes.Yellow;
            boardText.Text = "BOARD";
            boardText.Cursor = Cursors.Arrow;
            imgrec.Children.Add(boardText);

            //그냥 mouseleftbutton 으로는 안된다. textBox는 preview 핸들러를 달아서 버블링을 다뤄주고 어쩌고 저쩌고 해줘야한다.
            boardText.PreviewMouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(board_up);

            //스레드 실행
            colorchangeTh = new Thread(() => changeColor(screen.getMWinReference(), boardText));
            colorchangeTh.Start();
            
        }

        //색을 계속 변하게 만드는 스레드이다.
        public static void changeColor(ILwin.MainWindow mWin, TextBox thetxt)
        {
            //여기서부터 시작
            int[] rgbarr = new int[3]{255, 0, 0};
            int time_term = 5;

            //올릴 색상의 idx, 낮출 색상의 idx. 한 칸씩 나아갈거다
            int upidx = 1; int downidx = 0;

            while (true)
            {
                //처음에는 rgbarr[upidx]를 255까지 올린다.
                while(rgbarr[upidx] < 255)
                {
                    rgbarr[upidx]++;

                    mWin.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                    {
                        var brush = new SolidColorBrush(Color.FromArgb(255, (byte)rgbarr[0], (byte)rgbarr[1], (byte)rgbarr[2]));
                        thetxt.Foreground = brush;
                    }));

                    Thread.Sleep(time_term);
                }


                if (upidx == 2) upidx = 0;
                else upidx++;

                //그 담에는 rgbarr[downidx]를 0까지 내린다.
                while (rgbarr[downidx] > 0)
                {
                    rgbarr[downidx]--;

                    mWin.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                    {
                        var brush = new SolidColorBrush(Color.FromArgb(255, (byte)rgbarr[0], (byte)rgbarr[1], (byte)rgbarr[2]));
                        thetxt.Foreground = brush;
                    }));

                    Thread.Sleep(time_term);
                }

                if (downidx == 2) downidx = 0;
                else downidx++;

                

               
            }
        }

        //클릭 시 게시판을 열자
        private void board_up(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(datas.isBoardWinOn)
            {
                return;
            }

            boardWin = new BoardWindow(bodyimg, datas);
            datas.isBoardWinOn = true;
            boardWin.Show();
        }

    }
}
