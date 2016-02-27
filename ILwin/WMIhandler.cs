using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics; 
using System.Threading;


namespace ILwin
{
    public static class WMIhandler
    {
        //첫 실행될 때, 가장 가져오기 편한 Ram 정보를 가져온다. 이건 아버지에게 여쭤보면 된다.
        public static void initialRAMdata(Datas datas)
        {
            PerformanceCounter ram = new PerformanceCounter("Memory", "Available MBytes");
            string usableRAMStr = ram.NextValue().ToString() + " MB";

            datas.setRAMDatas(usableRAMStr);
        }

        //이건 나중에 CPU 온도나 사용량을 가져오는 데 쓰인다. 이건 남용이에게 물어보는 것도 가능하다.
        public static void getCPUdata(Datas datas)
        {
            //현재 CPU 정보를 계산하는 중이었다면 끝낸다.,
            if(datas.computingCPU)
                return;

            //이제 계산을 시작
            datas.computingCPU = true;

            //http://www.dreamincode.net/forums/topic/278594-finding-temperature-of-cpu/ 참조
            //OpenHardWareMonitor API를 사용하여 CPU 정보를 가져오도록 하였다.

            //필요한 정보들을 가져온다.
            PerformanceCounter cpu = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            PerformanceCounter process_cpu = new PerformanceCounter("Process", "% Processor Time", Process.GetCurrentProcess().ProcessName);
           

            float cpuUsage = 0;
            float myProcessCPU = 0;

            //PerformanceCounter가 0을 반환할 수도 있으니 루프를 돌린다.
            while(true)
            {
                cpuUsage = cpu.NextValue();
                if (cpuUsage != 0) break;

                Thread.Sleep(1000);
            }

            //PerformanceCounter가 0을 반환할 수도 있으니 루프를 돌린다.
            while (true)
            {
                myProcessCPU = process_cpu.NextValue();
                if (myProcessCPU != 0) break;

                Thread.Sleep(1000);
            }

            

            //정보를 string으로 변환
            string CPUUsageStr = cpuUsage.ToString() + "%";
            string myProcessCPUStr = myProcessCPU.ToString() + "%";

            //datas에 저장
            datas.setCPUDatas(CPUUsageStr, myProcessCPUStr);

        }
    }
}
