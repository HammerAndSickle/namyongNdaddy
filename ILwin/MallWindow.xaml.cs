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

        public Thickness originalMargin; //status의 margin (처음)
        public Thickness duringMargin; //status의 margin (검색 중)

        //현재 검색 중이라면(즉, 검색 스레드가 돌아가는 중이라면) 처리해야 한다
        public bool isSearching;
        


        public MallWindow(Brush bodyimg, Datas datas)
        {
            InitializeComponent();

            originalMargin = new Thickness(110, 145, 0, 0);
            duringMargin = new Thickness(140, 145, 0, 0);
            isSearching = false;

            this.Title = "찾는 물건 시세가?";
            this.bodyimg = bodyimg;
            this.Background = bodyimg;
            this.datas = datas;

            this.okbutton.Background = Brushes.Transparent;
            this.okbutton.Cursor = Cursors.Hand;

            //처음엔 입력을 기다린다.
            readyForInput();

            updateItems();
        }

        //사용자의 입력을 기다리는 상태일 때.
        public void readyForInput()
        {
            //텍스트박스 배경색
            var brushDark1 = new SolidColorBrush(Color.FromArgb(255, (byte)30, (byte)30, (byte)30));        //짙은회색
            var brushDark2 = new SolidColorBrush(Color.FromArgb(255, (byte)40, (byte)39, (byte)37));        //덜짙은회색

            this.status.Background = brushDark1;        //상태
            this.status.Margin = this.originalMargin;
            this.status.Text = "검색어를 입력하쇼";
            this.status.IsReadOnly = true;

            this.texts.Background = brushDark2;         //내용물
            this.texts.IsReadOnly = true;
            this.texts.FontSize = 13;
            this.texts.Text = "상품 목록이 이곳에 표시됩니다.";
            this.texts.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;

            this.queryitem.Background = brushDark1;        //검색어
            this.queryitem.Focus();
        }
        
        public void updateItems()
        {
            
        }

        //검색 버튼 누름
        private void searchItems(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //이미 탐색 중이라면 중지시킨다.
            if(isSearching)
            {
                isSearching = false;
                itemthr.Abort();
            }

            //그럼 quertyitem의 내용물을 가져와서 검색을 시도한다.

            this.status.Margin = this.duringMargin;
            this.status.Text = "검색 중..";



            List<string> priceslist = new List<string>();
            string query = this.queryitem.Text;

            //스레드를 돌린다.
            isSearching = true;
            itemthr = new Thread(() => HTMLhandler.getPrices(priceslist, query, this));
            itemthr.Start();
        }



        //닫길 때는, about 창이 닫겼다는 걸 bool로 표시
        void mallClosing(object sender, CancelEventArgs e)
        {
            this.datas.isMallWinOn = false;
        }


    }
}
