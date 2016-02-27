﻿using System;
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

        //CPU, RAM 등의 정보
        private string CPUusage;        //CPU 사용량
        private string usableRAM;       //RAM 사용가능량
        private string myProcessCPU;     //my process CPU 사용량
        public bool computingCPU;        //CPU 사용량을 체크하는 중이라면 true. 아니라면 false

        //지금 켜져있는 윈도우들
        public bool isAboutWinOn;       //AboutWindow가 켜져있는가
        public bool isBoardWinOn;       //boardWindow가 켜져있는가
        public bool isMallWinOn;       //mallWindow가 켜져있는가
        public bool isHelpWinOn;       //helpWindow가 켜져있는가

        public Datas()
        {
            webItems = new List<WebItem>();
            numOfwebitems = 0;          //첨엔 0개의 item이 존재한다.

            computingCPU = false;

            //처음엔 etc 윈도우는 모두 꺼져있다.
            isAboutWinOn = isBoardWinOn = isMallWinOn = isHelpWinOn = false;
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

        //ram 정보
        public void setRAMDatas(string usableRAM)
        {
            this.usableRAM = usableRAM;
        }

        //CPU 정보
        public void setCPUDatas(string CPUusage, string myProcessCPU)
        {
            this.CPUusage = CPUusage;
            this.myProcessCPU = myProcessCPU;
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

        public string getCPUusage()
        {
            return this.CPUusage;
        }

        public string getUsableRAM()
        {
            return this.usableRAM;
        }

        public string getMyProcessCPU()
        {
            return this.myProcessCPU;
        }

    }
}
