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
using System.Net;
using System.IO;

namespace ILwin
{
    /// <summary>
    /// AboutWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class AboutWindow : Window
    {
        Brush OKB;
        Brush OKBClicked;

        public AboutWindow(Brush contentImg, Brush OKB, Brush OKBClicked)
        {
            InitializeComponent();

            //this.okbutton.Background = OKB;
            this.Background = contentImg;


            this.OKB = OKB;
            this.OKBClicked = OKBClicked;
            this.okbut.Background = OKB;

           // testfile();
        }

        public void testfile()
        {
            string path = @"afile.txt";
            string urladdr = "https://www.google.co.kr/search?newwindow=1&hl=ko&site=webhp&q=%EB%82%A0%EC%94%A8";
            string SeoulUrl = "http://www.kma.go.kr/weather/forecast/mid-term-rss3.jsp?stnId=109";


            


        }

        //윗부분을 드래그가능.
        //* 중요한 건, 우리가 Grid에 Background 같은거 안 넣으면 얘는 nontouchable 하다!
        //xaml에서 background="TransParent" 해주던가 해야 한다. null은 안된다 이거다.
        private void about_dragging(object sender, System.Windows.Input.MouseButtonEventArgs e)
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
        private void okup(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            
        }
    }
}
