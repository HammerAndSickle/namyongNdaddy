using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Threading;
using System.Windows.Threading;
using System.IO;

namespace ILwin
{
    //박스에게 이미지를 요청했을 때, 그 이미지 하나하나가 이 WebItem 객체이다.
    public class WebItem
    {
        BitmapImage img;
        Brush imgBr;             //웹에서 건져온 이미지
        Rectangle imgrec;           //이미지 rectangle

        //이들은 imgrec의 가로세로가 될 것이다.
        int width;
        int height;

        int xPos;                   //떨어지기 시작한 x 위치.
        int yPos;                   //떨어지기 시작한 y 위치.

        ILwin.ShowScreen showscreen;        //showscreen 레퍼런스

        //생성자는 rectangle을 만들어 추가만을 수행 한다. show screen 참조 정도는 필요하다.
        public WebItem(ILwin.ShowScreen showscreen)
        {
            this.showscreen = showscreen;

            showscreen.getMWinReference().Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                {
                    this.imgrec = new System.Windows.Shapes.Rectangle();
                    this.imgrec.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    this.imgrec.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    this.imgrec.Width = this.imgrec.Height = 120;     //이미지의 너비다.
                    this.imgrec.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));

                    this.imgrec.StrokeThickness = 2;
                    showscreen.sp.Children.Add(this.imgrec);
                }));
            
        }

        //string imgurl에서 다운받아와 이미지를 얻어낸다.
        //그리고 flyingbox의 x 위치, y 위치가 생성 위치에 영향을 준다.
        public static void addDatas(WebItem thisitem, string imgurl, int box_posX, int box_posY)
        {
            //download image from url via internet
            thisitem.xPos = box_posX; thisitem.yPos = box_posY;

            //image(BitmapImage)를 URL로부터 얻어온다.
            thisitem.img = HTMLhandler.downloadImageFromURL(imgurl);

            thisitem.imgBr = new ImageBrush(thisitem.img);


            //************************************//
            System.Drawing.Bitmap abit;
            using(MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(thisitem.img));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                abit = new System.Drawing.Bitmap(bitmap);
            }

            abit.Save("temimg.png");

        }

        

        //이제 그 이미지를 화면에 표시하고, 아래로 추락시키는 스레드를 연결시킨다.
        public void showItem()
        {

                
                showscreen.getMWinReference().Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                {
                    this.imgrec.Margin = new Thickness(xPos, yPos, 0, 0);
                    this.imgrec.Fill = this.imgBr;
                    
            }));
            
            
                
                
                

                
                
        }

        public void deleteItem()
        {

        }

        public static void fallingItem(Rectangle imgrec, ILwin.MainWindow mWin)
        {
            Thread.Sleep(200);
            imgrec.Margin = new Thickness(imgrec.Margin.Left, imgrec.Margin.Top + 3, imgrec.Margin.Right, imgrec.Margin.Bottom);

            mWin.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            {
                
            }));
        }
    }
}
