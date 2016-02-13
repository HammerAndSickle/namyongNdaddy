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

namespace ILwin
{
    /// <summary>
    /// BoardWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class BoardWindow : Window
    {
        Brush bodyimg;

        public BoardWindow(Brush bodyimg)
        {
            InitializeComponent();

            this.Title = "게시판";
            this.bodyimg = bodyimg;
            this.Background = bodyimg;

            this.texts.IsReadOnly = true;
            this.texts.Background = Brushes.SkyBlue;
            this.texts.FontSize = 15;
            this.texts.Text = "Crawling articles.....";
        }

        //기사를 가져올 것이다.
        public void updateArticle()
        {

        }
    }
}
