﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using System.Windows.Controls;


namespace ILwin
{
    public class ILtextBox
    {
        ShowScreen showscreen;      //showscreen의 레퍼런스

        //생성 후, showscreen 레퍼런스를 넣는다.
        public void addShowScreenReference(ShowScreen showscreen)
        {
            this.showscreen = showscreen;
        }


        public void handleRequest(TextBox msgbox, string cmdreq, Datas datas)
        {
            //공백 기준으로 나누어보자.
            string[] ssize = cmdreq.Split(new char[0]);
            int len = ssize.Length;

            //아무것도 없다.
            if (ssize[0].Equals("")) return;

            //help를 입력받음
            else if (ssize[0].Equals("help")) cmdreq = printHelp();

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

            else
            {
                cmdreq = "received \"" + cmdreq + "\"";
            }

            msgbox.Text += cmdreq + "\n";

            msgbox.ScrollToEnd();
        }

        public void printMSG(TextBox msgbox, string msg)
        {
            if (msg.Equals("")) return;

            msgbox.Text += (msg + "\n");
            msgbox.ScrollToEnd();
        }

        public string printHelp()
        {
            return "========[HELP]========\n" + "hello : 남용이와 아버지에게 인사\n"
                + "hello n : 남용이에게 인사\n" + "hello d : 아버지에게 인사\n" +
                "help : 커맨드 도움\n" + "talk n [word] : 남용이에게 말 걸기\n" + "talk d [word] : 아버지에게 말 걸기\n" +
                "box images  [num] [word] : 박스에게 num개(max : 3)의 이미지 요구";

        }

        //날씨와 온도 업데이트
    }
}
