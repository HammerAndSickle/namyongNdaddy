using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILwin
{
    //이 클래스에는 지금 가지고 있는 여러 정보들이 모여 있다.
    public class Datas
    {
        private string presentTem;      //현재의 온도
        private string presentWea;      //현재의 날씨

        public void setPresentWeathers(string presentTem, string presentWea)
        {
            this.presentTem = presentTem;
            this.presentWea = presentWea;
        }

        public string getTemper()
        {
            return presentTem;
        }

        public string getWeather()
        {
            return presentWea;
        }
    }
}
