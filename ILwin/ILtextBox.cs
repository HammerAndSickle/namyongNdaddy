using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using System.Windows.Controls;


namespace ILwin
{
    public class ILtextBox
    {
        public ShowScreen showscreen;      //showscreen의 레퍼런스
        public List<string> textboxlines;    //텍스트박스의 문장들
        public int lines;                 //현재 텍스트 박스에 몇 문장이 존재?
        public int currentLine;            //현재 텍스트 박스의 문장 index

        public ILtextBox()
        {
            currentLine = lines = 0;
            textboxlines = new List<string>();
        }

        //생성 후, showscreen 레퍼런스를 넣는다.
        public void addShowScreenReference(ShowScreen showscreen)
        {
            if (this.showscreen != null)
                this.showscreen = null;

            this.showscreen = showscreen;
        }


        public void handleRequest(TextBox msgbox, string cmdreq, Datas datas)
        {
            //텍스트박스 라인에 추가한다.
            textboxlines.Add(cmdreq);
            lines++;
            currentLine = lines;

            //공백 기준으로 나누어보자.
            string[] ssize = cmdreq.Split(new char[0]);
            int len = ssize.Length;

            //아무것도 없다.
            if (ssize[0].Equals("")) return;

            //hello를 입력받음
            else if(ssize[0].Equals("hello"))
            {
                //그냥 hello는 둘 다에게 인사
                if (len == 1)
                {
                    showscreen.getNamyong().sayHello();
                    showscreen.getDaddy().sayHello();
                }

                 //hello n은 남용이에게 인사
                else if(len == 2 && ssize[1].Equals("n"))
                    showscreen.getNamyong().sayHello();

                 //hello d는 아버지에게 인사
                else if (len == 2 && ssize[1].Equals("d"))
                    showscreen.getDaddy().sayHello();

            }

            //time를 입력받음
            else if (ssize[0].Equals("time"))
            {
                //그냥 time는 둘 다에게 묻기
                if (len == 1)
                {
                    showscreen.getNamyong().sayTime();
                    showscreen.getDaddy().sayTime();
                }

                 //time n은 남용이에게 묻기
                else if (len == 2 && ssize[1].Equals("n"))
                    showscreen.getNamyong().sayTime();

                 //time d는 아버지에게 묻기
                else if (len == 2 && ssize[1].Equals("d"))
                    showscreen.getDaddy().sayTime();

            }

            //jump를 입력받음
            else if (ssize[0].Equals("jump"))
            {
                //그냥 jump는 둘 다 점프
                if (len == 1)
                {
                    showscreen.getNamyong().startjump();
                    showscreen.getDaddy().startjump();
                }

                 //jump n은 남용이 점프
                else if (len == 2 && ssize[1].Equals("n"))
                    showscreen.getNamyong().startjump();

                 //jump d는 아버지 점프
                else if (len == 2 && ssize[1].Equals("d"))
                    showscreen.getDaddy().startjump();

            }

            //speed를 입력받음
            else if (ssize[0].Equals("speed"))
            {
                //그냥 speed [n]는 둘 다의 속도 변경
                if (len == 2)
                {
                    //speed [숫자] 일 경우. 이젠 단어만 추출하면 된다.
                    int num;

                    //숫자가 들어있는지 확인하며, 맞다면 num에 집어넣고 그 레벨만큼으로 둘의 속도를 설정
                    if (int.TryParse(ssize[1], out num))
                    {
                        showscreen.getNamyong().sayQuick(num);
                        showscreen.getDaddy().sayQuick(num);
                    }
                }

                 //speed n은 남용이 속도 변경
                else if (len == 3 && ssize[1].Equals("n"))
                {
                    //speed [숫자] 일 경우. 이젠 단어만 추출하면 된다.
                    int num;

                    //숫자가 들어있는지 확인하며, 맞다면 num에 집어넣고 그 레벨만큼으로 둘의 속도를 설정
                    if (int.TryParse(ssize[2], out num))
                        showscreen.getNamyong().sayQuick(num);
                }

                 //speed d는 아버지 속도 변경
                else if (len == 3 && ssize[1].Equals("d"))
                {
                    //speed [숫자] 일 경우. 이젠 단어만 추출하면 된다.
                    int num;

                    //숫자가 들어있는지 확인하며, 맞다면 num에 집어넣고 그 레벨만큼으로 둘의 속도를 설정
                    if (int.TryParse(ssize[2], out num))
                        showscreen.getDaddy().sayQuick(num);
                }


            }

            //box를 처음에 입력받음
            else if (ssize[0].Equals("box"))
            {
                //box images일 경우, webitem을 떨어뜨릴 준비를 한다.
                if(ssize[1].Equals("images"))
                {
                    //box images [숫자] 일 경우. 이젠 단어만 추출하면 된다.
                    int num;

                    //숫자가 들어있는지 확인하며, 맞다면 num에 집어넣고 webimage 떨구기를 시행한다
                    if(int.TryParse(ssize[2], out num) == true)
                    {
                        //공백으로 split 하였지만, box images [aaa bbb ccc]는 가능하도록, 그 다음부턴 공백도 단어로 인정해야 한다
                        string queryImage = "";
                        for (int i = 3; i < ssize.Length; i++)
                        {
                            queryImage += ssize[i];

                            //끝이 아니라면 공백 문자를 그대로 사용
                            if (i != ssize.Length) queryImage += " ";

                        }

                        //스레드로 돌린다.
                        showscreen.generateWebImage(queryImage, num);

                        //HTMLhandler.getImages(datas, queryImage);
                        cmdreq = "박스에게 [" + queryImage + "] 이미지 " + num + "개 를 요청함!";
                    }
                }
            }

            //talk를 처음에 입력받음
            else if(ssize[0].Equals("talk"))
            {
                //그냥 talk는 둘 다에게 말걸기
                if (len == 1)
                {
                    showscreen.getNamyong().sayKeyword(showscreen.getNamyong().balloon);
                    showscreen.getDaddy().sayKeyword( showscreen.getDaddy().balloon);
                }

                 //talk n은 남용이에게 말걸기
                else if (len == 2 && ssize[1].Equals("n"))
                    showscreen.getNamyong().sayKeyword(showscreen.getNamyong().balloon);
                    

                 //time d는 아버지에게 말걸기
                else if (len == 2 && ssize[1].Equals("d"))
                    showscreen.getDaddy().sayKeyword(showscreen.getDaddy().balloon);

            }

            //introduce를 처음에 입력받음
            else if (ssize[0].Equals("introduce"))
            {
                //그냥 introduce는 둘 다에게 말걸기
                if (len == 1)
                {
                    showscreen.getNamyong().introduce();
                    showscreen.getDaddy().introduce();
                }

                 //introduce n은 남용이에게 말걸기
                else if (len == 2 && ssize[1].Equals("n"))
                    showscreen.getNamyong().introduce();


                 //introduce d는 아버지에게 말걸기
                else if (len == 2 && ssize[1].Equals("d"))
                    showscreen.getDaddy().introduce();

            }

            //computer를 처음에 입력받음
            else if (ssize[0].Equals("computer"))
            {
                //그냥 computer는 둘 다에게 말걸기
                if (len == 1)
                {
                    showscreen.getNamyong().sayaboutCom();
                    showscreen.getDaddy().sayaboutCom();
                }

                 //computer n은 남용이에게 말걸기
                else if (len == 2 && ssize[1].Equals("n"))
                    showscreen.getNamyong().sayaboutCom();


                 //computer d는 아버지에게 말걸기
                else if (len == 2 && ssize[1].Equals("d"))
                    showscreen.getDaddy().sayaboutCom();

            }

            //board 입력
            else if(ssize[0].Equals("board") && len == 1)
            {
                showscreen.getBoard().openBoardWindow();
            }

            //product 입력
            else if (ssize[0].Equals("product") && len == 1)
            {
                showscreen.getMall().openMallWindow();
            }

            //help 입력
            else if(ssize[0].Equals("help") && len == 1)
            {
                showscreen.getMWinReference().OpenHelpWindow();
            }

            //about 입력
            else if (ssize[0].Equals("about") && len == 1)
            {
                showscreen.getMWinReference().openAboutWIndow();
            }

            //exit 입력
            else if (ssize[0].Equals("exit") && len == 1)
            {
                showscreen.getMWinReference().exitNamyongNDaddy();
            }

            //reload 입력
            else if (ssize[0].Equals("reload") && len == 1)
            {
                showscreen.getMWinReference().reloadScreen();
            }

            else
            {
                cmdreq = "received \"" + cmdreq + "\"";
            }

            //msgbox.Text += cmdreq + "\n";
            
            

            msgbox.ScrollToEnd();
        }

        public void printMSG(TextBox msgbox, string msg)
        {
            if (msg.Equals("")) return;

            msgbox.Text += (msg + "\n");
            msgbox.ScrollToEnd();
        }

      
    }
}
