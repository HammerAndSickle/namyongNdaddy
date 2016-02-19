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

namespace ILwin
{
    /// <summary>
    /// BoardWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class BoardWindow : Window
    {
        Brush bodyimg;

        //뉴스를 크롤링해오는 스레드
        Thread newsthr;

        public BoardWindow(Brush bodyimg)
        {
            InitializeComponent();

            this.Title = "게시판";
            this.bodyimg = bodyimg;
            this.Background = bodyimg;

            this.texts.IsReadOnly = true;
            var brush = new SolidColorBrush(Color.FromArgb(255, (byte)216, (byte)255, (byte)250));
            this.texts.Background = brush;
            this.texts.FontSize = 15;
            this.texts.Text = "Crawling articles.....";

            //처음에 기사를 일단 가져온다
            updateArticle();
        }

        //기사를 가져올 것이다.
        public void updateArticle()
        {
            newsthr = new Thread(() => HTMLhandler.getNews(this, this.texts));
            newsthr.Start();
        }
    }
}
