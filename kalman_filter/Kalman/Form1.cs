using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace Kalman
{
    public partial class Form1 : Form
    {
        public string[] sXYZ;

        public static int pL = 1000;
        //public int[] sz = new int[2] {2,1};
        public double Q = 1e-10;
        public double R = 1e0;
        public static int row, col, rows;
        double[] mean1s = new double[25];
        double[] mean1skal = new double[25];
        double[] dhat = new double[2];
        double[] dhatresult = new double[pL];
        double[] P = new double[2];
        double[] dhatminus = new double[2];
        double[] Pminus = new double[2];
        double[] K = new double[2];
        double[] d1000 = new double[1000];
        List<double> array = new List<double>();
        public double[] arrayint;
        double[] array1 = new double[5000];
        public static double d1000sum;
        public static double dhatresultsum;
        public double p1, p2,a,b = 0;
        public int linenum = 0;
        //double[] yNew = new double[n];
        List<double> xArraynew = new List<double>();
        double a1 = 0, b1 = 0.1;

        private void button5_Click(object sender, EventArgs e)
        {
            a1 = array[0];
            for (int i = 0; i < 5; i++)
            {

                for (int m = 0; m < 1000; m++)
                {
                    d1000[m] = array[m + 1000 * i];
                }
                // a1 = d1000.Average();

                Class_Sigma3 m_Filter = new Class_Sigma3();
                double[] result1 = m_Filter.sigma3Filter(d1000);
                double[] result2 = m_Filter.sigma3Filter(result1.ToArray());
                this.textBox5.Text += result2.Average()/*.ToString("f5")*/ + "\t\n"; 
                //this.textBox5.Text += "a1:" + a1 + "," + "b1:" + b1+"\t\n";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            double[] arrayint = array.ToArray();

            a1 = arrayint[0];
            //row = array.GetLength(0);
            row = array.Count();
            rows = Convert.ToInt32(row / pL);
            for (int i = 0; i < rows; i++)
            {
                
                for (int m = 0; m < 1000; m++)
                {
                    d1000[m] = arrayint[m+1000*i];
                }
               // a1 = d1000.Average();
                
                //Class_KalmanFilter m_Filter = new Class_KalmanFilter();
                this.textBox4.AppendText(Class_KalmanFilter.KalmanFilter(d1000, ref a1, ref b1).ToString("0.000"));
                this.textBox4.AppendText("\t\n");
                //this.textBox5.Text += "a1:" + a1 + "," + "b1:" + b1+"\t\n";
            }
        }

        public Form1()
        {
            InitializeComponent();

        }

        private void button2_Click(object sender, EventArgs e)//kalman滤波均值
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

            string line;
            string[] s;
            string str;
            OpenFileDialog dialog = new OpenFileDialog();
            if(dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
           // FileStream fs = new FileStream("data.txt", FileMode.Open, FileAccess.Read);    //将data.txt放入Debug中
            StreamReader reader = new StreamReader(dialog.FileName, Encoding.Default);
            //for (int i = 0; i < 6000; i++)
            //{
            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();
                s = line.Split(',');
                str = s[0];               //将字符存入数组str中 
                array.Add(double.Parse(str));   //去掉空格 
                //for (int j = 0; j < 2; j++)
                //{
                //    str = s[j];               //将字符存入数组str中 
                //    array[linenum, j] = double.Parse(str);    //将字符数组转化为double型                   
                //    //array1[linenum] = array[linenum, 0];
                //}
                //tbox_rec.Text += array[linenum, 0] + array[linenum, 1];    //验证一下是否成功 
                linenum++;
            }
            reader.Close();
        }



        private void button1_Click(object sender, EventArgs e)//普通求取1s1000个点的平均值
        {
            //row = array.GetLength(0);
            row = array.Count();
            rows = Convert.ToInt32(row / pL);
            dhat[0] = array[0];          
            P[0] = 0.1;
            a = dhat[0];
            b = P[0];
            for (int j = 1; j <= 25; j++)
            {
                d1000sum = 0;
                dhatresultsum = 0;
                for (int m = 0; m < d1000.Length; m++)
                {
                    d1000[m] = array[((j - 1) * 1000 + m)];
                    d1000sum += d1000[m];
                }
                mean1s[j - 1] = d1000sum / 1000;
                tbox_rec.Text += mean1s[j - 1] + "\t\n";
                KalmanFilter(d1000, a, b, out p1, out p2);
                a = p1;
                b = p2;
               // textBox4.Text += "a:"+a+","+"b:"+b + "\t\n";

                for (int n = 0; n < dhatresult.Length; n++)
                {
                    dhatresultsum += dhatresult[n];
                }
                mean1skal[j - 1] = dhatresultsum / 1000;
                textBox4.Text += mean1skal[j - 1] + "\t\n";
            }

        }
        public void KalmanFilter(double[] array, double a, double b, out double p1, out double p2)
        {
            dhat[0] = a;
            P[0] = b;
            for (int q = 0; q < 1000; q++)
            {
                d1000[q] = array[ q];
                dhatminus[1] = dhat[0];
                Pminus[1] = P[0] + Q;
                K[1] = Pminus[1] / (Pminus[1] + R);
                dhat[1] = dhatminus[1] + K[1] * (d1000[q] - dhatminus[1]);
                P[1] = (1 - K[1]) * Pminus[1];
                dhatresult[q] = dhat[1];
                P[0] = P[1];
                dhat[0] = dhat[1];

            }
            p1 = dhat[0];
            p2 = P[0];
        }


    }
}



      

