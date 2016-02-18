﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILwin
{
    static class Constants
    {
        //init 리소스 경로
        public const String REL_PATH_INIT = "rscs\\init\\";

        //이미지 경로
        public const String ABS_PATH = "D:\\igongwin\\ILwin\\ILwin\\rscs\\winbar\\";
        public const String REL_PATH = "rscs\\winbar\\";

        //ABOUT 이미지 경로
        public const String ABS_PATH_ABOUT = "D:\\igongwin\\ILwin\\ILwin\\rscs\\about\\";
        public const String REL_PATH_ABOUT = "rscs\\about\\";

        //버튼 이미지 경로
        public const String REL_PATH_SPRITE = "rscs\\sprite\\";

        //다른 윈도우 이미지 경로
        public const String REL_OTEHR_WIN_PATH = "rscs\\otherwin\\";

        //스프라이트 이미지 경로


        //배경 이미지
        public const String REL_PATH_BG2_SUNNY = "rscs\\bg2\\sunny\\";
        public const String REL_PATH_BG2_DAWN = "rscs\\bg2\\dawn\\";
        public const String REL_PATH_BG2_NIGHT = "rscs\\bg2\\night\\";

        //각 시간대마다 배경 이미지 개수
        public const int SUNNY_IMGS = 10;
        public const int DAWN_IMGS = 10;
        public const int NIGHT_IMGS = 10;

        //화면 에 존재 가능한 web items 개수
        public const int MAX_WEB_ITEMS = 5;

        //남용이 이미지 size
        public const int NAYONG_WIDTH = 140;
        public const int NAYONG_HEIGHT = 233;
        //아버지 이미지 size
        public const int DADDY_WIDTH = 160;
        public const int DADDY_HEIGHT = 233;
        //말풍선 이미지 size
        public const int BALLOON_WIDTH = 95;
        public const int BALLOON_HEIGHT = 69;
        //남용&아버지와 말풍선 거리
        public const int DADDY_BALLOON_RIGHT_DIST = 95;
        public const int DADDY_BALLOON_LEFT_DIST = 180;
        public const int NAMYONG_BALLOON_RIGHT_DIST = 50;
        public const int NAMYONG_BALLOON_LEFT_DIST = 100;

        //이미지 speed
        public const int NAMYONG_SPEED = 80;
        public const int DADDY_SPEED = 100;

        //남용/아버지 = balloon에서 식별용으로 쓰임
        public const int IS_NAMYONG = 0;
        public const int IS_DADDY = 1;
    }

}
