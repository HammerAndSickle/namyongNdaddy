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
        private string presentLoc;      //현재의 위치
        private string presentTem;      //현재의 온도
        private string presentWea;      //현재의 날씨

        //web items는 남용이, 아버지, 박스 등과 달리 화면에 일정 개수만큼(0개도 가능) 존재 가능하니, showScreen이 아닌
        //화면 내 Datas의 일부로서 관리한다.
        public List<WebItem> webItems; //화면에 존재하는 web items 큐
        public int numOfwebitems;       //화면에 존재하는 web items의 개수. 최대는 staticCON.MAX_WEB_ITEMS개이다.

        public Datas()
        {
            webItems = new List<WebItem>();
            numOfwebitems = 0;          //첨엔 0개의 item이 존재한다.
        }

        //web crawling으로 얻어온 사용자의 위치를 set
        public void setLocation(string loc)
        {
            this.presentLoc = loc;
        }

        //web crawling으로 얻어온 사용자의 위치에 해당하는 날씨/기온을 set
        public void setPresentWeathers(string presentTem, string presentWea)
        {
            this.presentTem = presentTem;
            this.presentWea = presentWea;
        }


        public string getLoc()
        {
            return presentLoc;
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
