using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;
using System.Drawing;
using System.Windows;


namespace ILwin
{
    class ShowScreen
    {
        //bigRectangle은 앞 mainWindow에서 그린 xaml의 rectangle을 그대로 참조하는 것.
        public System.Windows.Shapes.Rectangle bigRectangle;
        //bigRectangle 안의 winRectangle이 바로 출력 화면이 될 것이다.
        public System.Windows.Shapes.Rectangle winRectangle;
        
        public Rect bigRect;
        public Rect winRect;

        public ShowScreen(System.Windows.Shapes.Rectangle mainwin, Rect winRect)
        {
            bigRectangle = mainwin;
            this.bigRect = winRect;
            this.winRect = new Rect(winRect.X, winRect.Y + 20, winRect.Width, winRect.Height - 20);

            //winRectangle을 만들 것이다.
            winRectangle = new System.Windows.Shapes.Rectangle();
            winRectangle.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            winRectangle.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            winRectangle.Margin = new Thickness(0, 20, 0, 0);
            winRectangle.Width = bigRectangle.Width;        //가로는 bigrectangle과 같다.
            winRectangle.Height = bigRectangle.Height - 20; //세로는 좀 더 작어.
            Grid sp = (Grid)bigRectangle.Parent;
            sp.Children.Add(winRectangle);                  //부모 grid에 추가한다.


         
            
            drawScreen();
            
        }

        public void drawScreen()
        {
            bigRectangle.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(194, 194, 194));
            bigRectangle.StrokeThickness = 1;
      

            winRectangle.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(194, 194, 194));
            winRectangle.StrokeThickness = 1;



            //winBox.DrawRectangle(grayPen, targetRect);
            
        }
    }
}
