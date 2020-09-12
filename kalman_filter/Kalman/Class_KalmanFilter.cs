using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet;

namespace Kalman
{
    class Class_KalmanFilter
    {
        public static int pL = 1000;//静态变量
        static double[] dhat = new double[2];
        static double[] P = new double[2];
        static double[] d1000 = new double[1000];
        static double[] dhatminus = new double[2];
        static double[] Pminus = new double[2];
        static double[] K = new double[2];
        static public double Q = 1e-10;
        static public double R = 1e0;
        static double[] dhatresult = new double[pL];
        public static double KalmanFilter(double[] array, ref double a,ref double b)//传递的是a、b的地址
        {
            dhat[0] = a;
            P[0] = b;
            for (int q = 0; q < 1000; q++)
            {
                d1000[q] = array[q];
                dhatminus[1] = dhat[0];
                Pminus[1] = P[0] + Q;
                K[1] = Pminus[1] / (Pminus[1] + R);
                dhat[1] = dhatminus[1] + K[1] * (d1000[q] - dhatminus[1]);
                P[1] = (1 - K[1]) * Pminus[1];
                dhatresult[q] = dhat[1];
                P[0] = P[1];
                dhat[0] = dhat[1];

            }
            a = dhat[0];
            b = P[0];
            return dhatresult.Average();//返回矩阵的均值，直接用函数Average()
        }
    }
    class Class_Sigma3
    {
        public static int pL = 1000;
        double[] dhat = new double[2];
        double[] P = new double[2];
        double[] d1000 = new double[1000];
        double[] dhatminus = new double[2];
        double[] Pminus = new double[2];
        double[] K = new double[2];
        public double Q = 1e-10;
        public double R = 1e0;
        double[] dhatresult = new double[pL];
        public double[] sigma3Filter(double[] array)
        {
            List<double> result = new List<double>();;
            double mean = array.Average();
            double SD =MathNet.Numerics.Statistics.Statistics.StandardDeviation(array);
            for(int i=0;i<array.Length;i++)
            {
                if(array[i] <( mean+3*SD) && array[i] > (mean - 3 * SD))
                {
                    result.Add(array[i]);
                }
            }
            return result.ToArray();
        }
    }
}
