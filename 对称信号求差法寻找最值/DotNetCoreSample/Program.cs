using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DotNetCoreSample
{
    class Program
    {
        public static void Main(string[] args)
        {
            // int[] rim = new int[25000];
            List<int> rim = new List<int>();
            List<double> angle = new List<double>();
        
            // 1. 文件读取
            try
            {
                // 创建一个 StreamReader 的实例来读取文件 
                // using 语句也能关闭 StreamReader
                using (StreamReader sr = new StreamReader("E:/Temp/9-9/19.txt"))
                {
                    string line;
                    // 从文件读取并显示行，直到文件的末尾 
                    while ((line = sr.ReadLine()) != null)
                    {                        
                        rim.Add(int.Parse(line.Split(",")[1]));
                        angle.Add(double.Parse(line.Split(",")[0]));
                    }
                }
            }
            catch (Exception e)
            {
                // 向用户显示出错消息
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
            // Console.ReadKey();      

            int[] Rim = rim.ToArray();
            double[] Angle = angle.ToArray();

            // 2. 窗口宽度确定
            int rimL = 0, MaxL = 0;
            int Max = 0;
            int windowSize = 0;
            int _w = 0;

            Max = Rim.Max();
            MaxL = Rim.ToList().IndexOf(Max); 
            rimL = Rim.Length;
            
                // 求门限附近点
            double[] diff = new double[rimL];
            List<int> diffL = new List<int>();
            for(int i = 0; i < rimL; i++)
            {
                diff[i] = Math.Abs(Rim[i] - Max * 0.4);
            }
            
            Array.Sort(diff);
            for(int i = 0; i < rimL; i++)
            {
                // 找到所有门限附近的点
                // Console.WriteLine(diff[i]);
                if( Math.Abs(Rim[i] - Max * 0.4) <= diff[19])
                {
                    diffL.Add(i);
                }
            }
            int[] _L = diffL.ToArray();
            _w = _L[^1] - _L[0]; 
            // Console.WriteLine("--------");
            // Console.WriteLine(_w);
            
            // 过宽过窄调整
            if(_w < rimL / 20)
            {
                _w = (int)(rimL / 20);
            }
            // windowSize = 2000;
            if(MaxL - 3 * _w <= 0)            
            {
                _w = (int)(_w / 3);
            }
            if(MaxL + 3 * _w >= rimL)
            {
                _w = (int)(_w / 3);
            }
            windowSize = _w;
            // Console.WriteLine("--------");
            // Console.WriteLine(MaxL + 4*windowSize);

            // 3. 对称求差变换
            double[] Res = new double[rimL];

            for(int i = MaxL-2*windowSize; i < MaxL+2*windowSize; i++)
            {
                for(int j = 0; j < windowSize; j++)
                {
                    Res[i] += Math.Pow(Rim[i-j] - Rim[i+j+1], 2) / windowSize;
                    // Res[i] += Math.Pow(Rim[i-j] - Rim[i+j+1], 2) / windowSize;
                }
            }
            
            // 4. 寻找中间最低点

            // double _R = Res[MaxL - windowSize .. MaxL + windowSize];
            double[] _R = new double[2 * windowSize];
            int MinL = 0;
            double Min = 0;
            for(int i = MaxL- windowSize; i < MaxL + windowSize; i++)
            {
                _R[i-MaxL+windowSize] = Res[i];
            }
            Min = _R.Min();
            MinL = _R.ToList().IndexOf(Min);
            MinL += MaxL - windowSize;
            Console.WriteLine(Angle[MinL]);
            Console.WriteLine(windowSize);  
            Console.WriteLine(MaxL);              
        }
    }
}
/*
%% c寻找 中间的局部最低点
% index = find(Res > 0);
[MinR, LMinR] = min(Res( LMax-windowSize:LMax+windowSize ));
LMinR = LMinR + LMax - windowSize -1;
R3 = data(LMinR, 1);
plot(R3, MinR, '+','linewidth', 2, 'Color', [153, 230, 0]/255, 'MarkerSize', 10);
text(R3, MinR, num2str(R3) );

% 寻找数组D中最接近 i 的 n个点
function index = MaxPercent(D, i, n)
    d = sort(abs(D-i));         % 最接近i的几个值
    di = find((D >= -d(n)+i) & (D <= d(n)+i));
    index = di(1:n);            % 返回前n个点
end */