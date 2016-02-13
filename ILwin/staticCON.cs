using System;
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

    }

}
