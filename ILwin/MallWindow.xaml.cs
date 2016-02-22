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
    /// MallWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MallWindow : Window
    {
        Brush bodyimg;

        //이건 윈도우가 켜져있는지, 아닌지를 판별할 datas
        Datas datas;

        //목록들을 크롤링해오는 스레드
        Thread itemthr;

        public MallWindow(Brush bodyimg, Datas datas)
        {
            InitializeComponent();

            this.Title = "찾는 물건 시세가?";
            this.bodyimg = bodyimg;
            this.Background = bodyimg;
            this.datas = datas;

            //텍스트박스 배경색
            var brushDark1 = new SolidColorBrush(Color.FromArgb(255, (byte)30, (byte)30, (byte)30));        //짙은회색
            var brushDark2 = new SolidColorBrush(Color.FromArgb(255, (byte)40, (byte)39, (byte)37));        //덜짙은회색

            this.queryitem.Background = brushDark1;        //검색어
            this.queryitem.Focus();
            
            this.status.Background = brushDark1;        //상태
            this.status.Text = "검색어를 입력하쇼";
            this.status.IsReadOnly = true;

            this.texts.Background = brushDark2;         //내용물
            this.texts.IsReadOnly = true;
            this.texts.FontSize = 15;
            this.texts.Text = "Crawling item prices.....";

            //처음에 기사를 일단 가져온다
            updateItems();
        }

        //기사를 가져올 것이다.
        public void updateItems()
        {
            
        }

        //닫길 때는, about 창이 닫겼다는 걸 bool로 표시
        void mallClosing(object sender, CancelEventArgs e)
        {
            this.datas.isMallWinOn = false;
        }
    }
}
