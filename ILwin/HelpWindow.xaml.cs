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
using System.ComponentModel;

namespace ILwin
{
    /// <summary>
    /// HelpWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class HelpWindow : Window
    {
         Brush bodyimg;

         //OK 버튼
         Brush OKB;
         Brush OKBClicked;

        //이건 윈도우가 켜져있는지, 아닌지를 판별할 datas
        Datas datas;

        public HelpWindow(Brush bodyimg, Brush OKB, Brush OKBClicked, Datas datas)
        {
            InitializeComponent();

            this.OKB = OKB;
            this.OKBClicked = OKBClicked;
            this.okbut.Background = OKB;

            this.Title = "도움말";
            this.datas = datas;
            this.bodyimg = bodyimg;
            this.mainGrid.Background = bodyimg;
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
        //닫길 때는, help 창이 닫겼다는 걸 bool로 표시
        void helpClosing(object sender, CancelEventArgs e)
        {
            this.datas.isHelpWinOn = false;
        }
    }
}
