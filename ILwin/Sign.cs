using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ILwin
{
    //표지판 정보.
    class Sign
    {
        Grid imgrec;        //표지판 grid
        Datas datas;        //여기에 날씨 정보가 있을기니
        ShowScreen screen;  //표지판을 추가할 것이다.

        BitmapImage signImg;
        Brush signBr;
        
        public Sign(int xPos, int yPos, ShowScreen showscreen, Datas datas)
        {
            this.datas = datas;
            this.screen = showscreen;

            //생성
            imgrec = new Grid();
            imgrec.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            imgrec.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            imgrec.Width = 95; imgrec.Height = 202;
            imgrec.Margin = new Thickness(xPos, yPos, 0, 0);
            screen.sp.Children.Add(imgrec);

            changeSign();
        }

        //날씨에 맞춰서 표지판 그림을 지정한다.
        public void changeSign()
        {
            string wS = datas.getWeather();         //날씨 문자열 가져온다

            signImg = new BitmapImage();
            signImg.BeginInit();

            if(wS.Contains("맑음"))
            {
                signImg.UriSource = new Uri(Constants.REL_WEATHER_PATH + "w_sunny.png", UriKind.Relative);
            }

            else if (wS.Contains("흐림") || wS.Contains("구름"))
            {
                signImg.UriSource = new Uri(Constants.REL_WEATHER_PATH + "w_cloudy.png", UriKind.Relative);
            }

            else if (wS.Contains("비"))
            {
                signImg.UriSource = new Uri(Constants.REL_WEATHER_PATH + "w_rainy.png", UriKind.Relative);
            }

            else if (wS.Contains("눈"))
            {
                signImg.UriSource = new Uri(Constants.REL_WEATHER_PATH + "w_snowy.png", UriKind.Relative);
            }

            else
            {
                signImg.UriSource = new Uri(Constants.REL_WEATHER_PATH + "w_normal.png", UriKind.Relative);
            }

            signImg.EndInit();
            signBr = new ImageBrush(signImg);

            imgrec.Background = signBr;
        }
    }
}
