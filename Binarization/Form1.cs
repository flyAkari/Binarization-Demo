using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Binarization
{
    public partial class Form1 : Form
    {
        string SourceFile;
        public Form1()
        {
            InitializeComponent();
        }

        public static Bitmap ToGray(Bitmap bmp)
        {
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    //获取该点的像素的RGB的颜色
                    Color color = bmp.GetPixel(i, j);
                    //利用公式计算灰度值
                    int gray = (int)(color.R * 0.3 + color.G * 0.59 + color.B * 0.11);
                    Color newColor = Color.FromArgb(gray, gray, gray);
                    bmp.SetPixel(i, j, newColor);
                }
            }
            return bmp;
        }

        public static Bitmap ConvertTo1Bpp1(Bitmap bmp)
        {
            int average = 0;
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    Color color = bmp.GetPixel(i, j);
                    average += color.B;
                }
            }
            average = (int)average / (bmp.Width * bmp.Height);
            int[,] testmatrix = new int[bmp.Height, bmp.Width];
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    //获取该点的像素的RGB的颜色
                    Color color = bmp.GetPixel(i, j);
                    int value = 255 - color.B;
                    //Color newColor = value > average ? Color.FromArgb(0, 0, 0) : Color.FromArgb(255, 255, 255);
                    Color newColor;
                    if(value > average)
                    {
                        newColor = Color.FromArgb(0, 0, 0); testmatrix[j, i] = 0;
                    }
                    else 
                    {
                        newColor = Color.FromArgb(255, 255, 255); testmatrix[j, i] = 1;
                    }
                    bmp.SetPixel(i, j, newColor);
                }
            }  
            return bmp;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                SourceFile = openFileDialog1.FileName;
                txbPath.Text = SourceFile;
                pictureBox1.BackgroundImage = new Bitmap(SourceFile);
                Bitmap bitmapgray = ToGray(new Bitmap(SourceFile));
                pictureBox2.BackgroundImage = ToGray(new Bitmap(SourceFile));
                Bitmap bitmap2 = ConvertTo1Bpp1(bitmapgray);
                pictureBox3.BackgroundImage = bitmap2;
            }
        }
    }
}
