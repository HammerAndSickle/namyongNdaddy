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
        int idx;                //datas에서의 webitems 리스트에서 해당 webitem 객체는 몇 번째에 해당하는 것이가.

        BitmapImage img;
        Brush imgBr;             //웹에서 건져온 이미지
        Rectangle imgrec;           //이미지 rectangle

        //이들은 imgrec의 가로세로가 될 것이다.
        int width;
        int height;

        int xPos;                   //떨어지기 시작한 x 위치.
        int yPos;                   //떨어지기 시작한 y 위치.

        ILwin.ShowScreen showscreen;        //showscreen 레퍼런스

        bool isFalling;             //떨어지는 중이라면 true. 떨어지고 나서야 마우스 핸들러를 사용 가능하도록 하는 것이다.

        //생성자는 rectangle을 만들어 추가만을 수행 한다. show screen 참조 정도는 필요하다.
        public WebItem(ILwin.ShowScreen showscreen)
        {
            this.showscreen = showscreen;
            this.isFalling = true;

            showscreen.getMWinReference().Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                {
                    this.imgrec = new System.Windows.Shapes.Rectangle();
                    this.imgrec.Visibility = Visibility.Hidden;         //처음엔 숨겨놓는다
                    this.imgrec.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    this.imgrec.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    this.imgrec.Width = this.imgrec.Height = 120;     //이미지의 너비다.
                    this.imgrec.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));

                    this.imgrec.StrokeThickness = 2;
                    showscreen.sp.Children.Add(this.imgrec);
                }));

            this.imgrec.MouseRightButtonUp += new System.Windows.Input.MouseButtonEventHandler(itemclick);
        }

        //string imgurl에서 다운받아와 이미지를 얻어낸다.
        //그리고 flyingbox의 x 위치, y 위치가 생성 위치에 영향을 준다.
        public static void addDatas(ILwin.ShowScreen showscreen, WebItem thisitem, string imgurl, int box_posX, int box_posY)
        {
            //download image from url via internet
            thisitem.xPos = box_posX; thisitem.yPos = box_posY;

            //image(BitmapImage)를 URL로부터 얻어온다.
            thisitem.img = HTMLhandler.downloadImageFromURL(imgurl);
            thisitem.imgBr = new ImageBrush(thisitem.img);
            
            //********freezable 문제가 발생. 이미지 리소스에 freeze를 시행해 주자!
            thisitem.img.Freeze();
            thisitem.imgBr.Freeze();
            

            //이제 이미지를 연결하고 화면에 나타나도록 하여라
            showscreen.getMWinReference().Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                {
                    thisitem.imgrec.Width = thisitem.img.Width;
                    thisitem.imgrec.Height = thisitem.img.Height;
                    thisitem.imgrec.Margin = new Thickness(thisitem.xPos, thisitem.yPos, thisitem.imgrec.Margin.Right, thisitem.imgrec.Margin.Bottom);
                    thisitem.imgrec.Fill = thisitem.imgBr;
                    thisitem.imgrec.Visibility = Visibility.Visible;         //이제 표시한다.
                }));


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
        public static void fallingItem(ILwin.ShowScreen showscreen, WebItem thisitem, bool isFinal)
        {
            int item_y = 0;


            while (true)
            {
                //현재 webitem의 margin top 값을 가져온다
                showscreen.getMWinReference().Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                    {
                        item_y = (int)thisitem.imgrec.Margin.Top;
                    }));

                Thread.Sleep(50);

                if (item_y > 300)
                {
                    break;
                }

                else
                {
                    showscreen.getMWinReference().Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                        {
                            thisitem.imgrec.Margin = new Thickness(thisitem.imgrec.Margin.Left, thisitem.imgrec.Margin.Top + 8,
                                thisitem.imgrec.Margin.Right, thisitem.imgrec.Margin.Bottom);

                        }));
                }

            }


            //마지막 webitem이라면 다 떨어질때까지 더 webitem을 호출하지 못하도록 블록을 시킨 걸 해제한다.
            if (isFinal == true)
            {
                showscreen.doingWebitem = false;

                //박스 이미지를 일반 이미지로 바꾼다.
                flyingBox.changeImg(showscreen.getFlyingboxReference(), showscreen.getMWinReference(), false);
            }

            thisitem.isFalling = false;
                
                
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

        //마우스 핸들러.
        private void itemclick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //떨어지는 중인 아이템은 제거하지 못하도록 막는다.
            if (this.isFalling == true) return;

            this.imgrec.Visibility = Visibility.Hidden;
            showscreen.getMWinReference().getTextboxReference().printMSG(showscreen.getMWinReference().responseMsgs, "Web image를 제거함");

            //진짜로 리스트에서 해당 webitem을 빼도록 한다.
            for(int i = 0; i < showscreen.getMWinReference().getDatasReference().webItems.Count; i++)
            {
                if(showscreen.getMWinReference().getDatasReference().webItems.ElementAt(i) == this)
                {
                    showscreen.getMWinReference().getDatasReference().webItems.RemoveAt(i);
                }
            }

            this.imgBr = null;
            this.img = null;
            this.imgrec = null;
            
        }
    }
}
