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

namespace ILwin
{
    /// <summary>
    /// BoardWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class BoardWindow : Window
    {
        Brush bodyimg;

        //이건 윈도우가 켜져있는지, 아닌지를 판별할 datas
        Datas datas;

        //OK 버튼
        Brush OKB;
        Brush OKBClicked;

        //뉴스를 크롤링해오는 스레드
        Thread newsthr;

        public BoardWindow(Brush bodyimg, Brush OKB, Brush OKBClicked, Datas datas)
        {
            InitializeComponent();

            this.datas = datas;

            this.Title = "게시판";
            this.bodyimg = bodyimg;
            this.Background = bodyimg;

            this.texts.IsReadOnly = true;
            var brush = new SolidColorBrush(Color.FromArgb(255, (byte)216, (byte)255, (byte)250));
            this.texts.Background = brush;
            this.texts.FontSize = 15;
            this.texts.Text = "Crawling articles.....";

            this.OKB = OKB;
            this.OKBClicked = OKBClicked;
            this.okbut.Background = OKB;

            //처음에 기사를 일단 가져온다
            updateArticle();
        }

        //기사를 가져올 것이다.
        public void updateArticle()
        {
            newsthr = new Thread(() => HTMLhandler.getNews(this, this.texts));
            newsthr.Start();
        }

        //드래그 부분을 드래그하면 창이 옮겨간다.
        private void board_dragging(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void okenter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.okbut.Background = this.OKBClicked;
        }
        private void okleave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.okbut.Background = this.OKB;
        }
        private void okdown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Close();
        }

        //닫길 때는, board 창이 닫겼다는 걸 bool로 표시
        void boardClosing(object sender, CancelEventArgs e)
        {
            this.datas.isBoardWinOn = false;
        }
    }
}
