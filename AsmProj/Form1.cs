using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AsmProj
{
    public partial class Form1 : Form
    {
        [DllImport(@"C:\Users\qetzi\source\repos\AsmProj\x64\Release\Asm.dll")]
        static extern int MyProc1(int[] tab,int begin, int end);
        [DllImport(@"C:\Users\qetzi\source\repos\AsmProj\x64\Release\Asm.dll")]
        static extern int MyProc2(int[] tab, int begin, int end);


        private OpenFileDialog ofile;
        String option = "";
        float diff = 0.0f;
        int numberThreads = Environment.ProcessorCount;
        bool pictureBoxempty = true;

        public Form1()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            option = (string)comboBox1.SelectedItem;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click_1(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            numberThreads = trackBar1.Value;
            label7.Text = trackBar1.Value.ToString();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            trackBar1.Value=Environment.ProcessorCount;
            label7.Text = trackBar1.Value.ToString();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            diff = (float)(trackBar2.Value)/100f;

            label8.Text = trackBar2.Value.ToString()+" %";
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            ofile = new OpenFileDialog();
                ofile.Filter= "Image Files(*.png; *.bmp)|*.png;*.bmp";
            if (ofile.ShowDialog() == DialogResult.OK) {
                String path = ofile.FileName;
                pictureBox1.Image = Image.FromFile(ofile.FileName);
                pictureBoxempty = false;
            }            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (option == "C#" && !pictureBoxempty)
            {
                Stopwatch stopwatch1 = new Stopwatch();
                stopwatch1.Start();
                List<Task> tl = new List<Task>();
                Bitmap bmap = (Bitmap)pictureBox1.Image;
                BitmapData bitmapData = (bmap.LockBits(new Rectangle(0, 0, bmap.Width, bmap.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, bmap.PixelFormat));
                Bitmap result = new Bitmap(bmap.Width, bmap.Height);
                Bitmap Rbtm = new Bitmap(bmap.Width, bmap.Height);
                Bitmap BGbtm = new Bitmap(bmap.Width, bmap.Height);
                int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(bmap.PixelFormat) / 8;
                int heightInPixels = bitmapData.Height;
                int widthInBytes = bitmapData.Width * bytesPerPixel;

                byte[] currentLine = new byte[bmap.Width * bmap.Height * bytesPerPixel];
                Marshal.Copy(bitmapData.Scan0, currentLine, 0, currentLine.Length);
                int[] tabR = Array.ConvertAll(currentLine, c => (int)c);
                int[] tabBG = Array.ConvertAll(currentLine, c => (int)c);
                bmap.UnlockBits(bitmapData);
                float nmb = (float)(result.Width / 100 * diff);
                int dif = (int)Math.Truncate(nmb);
                Parallel.For(0, numberThreads,new ParallelOptions { MaxDegreeOfParallelism = numberThreads }, i =>
                {
                    int start = i * 128;
                    int end = (i + 1) * 128;
                    ImageProcces.SeperateBitmaps(tabR, start, end, bytesPerPixel);
                    ImageProcces.SeperateBitmapsBG(tabBG, start, end, bytesPerPixel); 
                }
               );
                label10.Text = stopwatch1.ElapsedMilliseconds.ToString() + " ms";
                ImageProcces.IntToBitmap(tabR, Rbtm);
                ImageProcces.IntToBitmap(tabBG, BGbtm);
                ImageProcces.ConvertionMT(Rbtm, BGbtm, result, dif);
                stopwatch1.Stop();
                label3.Text = stopwatch1.ElapsedMilliseconds.ToString() + " ms";
                pictureBox2.Image = result;             
            }
            if (option == "Asm" && !pictureBoxempty)
            {
                Stopwatch stopwatch1 = new Stopwatch();
                stopwatch1.Start();
                List<Task> tl = new List<Task>();
                
                Bitmap bmap = (Bitmap)pictureBox1.Image;
                BitmapData bitmapData = (bmap.LockBits(new Rectangle(0, 0, bmap.Width, bmap.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, bmap.PixelFormat));
                Bitmap result = new Bitmap(bmap.Width, bmap.Height);
                Bitmap Rbtm = new Bitmap(bmap.Width, bmap.Height);
                Bitmap BGbtm = new Bitmap(bmap.Width, bmap.Height);
                int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(bmap.PixelFormat) / 8;
                int heightInPixels = bitmapData.Height;
                int widthInBytes = bitmapData.Width * bytesPerPixel;

                byte[] currentLine = new byte[bmap.Width * bmap.Height * bytesPerPixel];
                Marshal.Copy(bitmapData.Scan0, currentLine, 0, currentLine.Length);
                int y = bmap.Height;
                bmap.UnlockBits(bitmapData);
                float nmb = (float)(result.Width / 100 * diff);
                int dif = (int)Math.Truncate(nmb);
                int[] tabR = Array.ConvertAll(currentLine, c => (int)c);
                int[] tabBG = Array.ConvertAll(currentLine, c => (int)c);

                Parallel.For(0, numberThreads, new ParallelOptions { MaxDegreeOfParallelism = numberThreads }, i =>
                {           
                    int start = i * 128;
                    int end = (i + 1) * 128;
                    MyProc1(tabBG, start, end);
                    MyProc2(tabR, start, end);
                }
               );
                
                label10.Text = stopwatch1.ElapsedMilliseconds.ToString() + " ms";
                ImageProcces.IntToBitmap(tabR, Rbtm);
                ImageProcces.IntToBitmap(tabBG, BGbtm);
                ImageProcces.ConvertionMT(Rbtm, BGbtm, result, dif);
                pictureBox2.Image = result;
                stopwatch1.Stop();
                label3.Text = stopwatch1.ElapsedMilliseconds.ToString() + " ms";
            }
        }
        private void button2_Click_1(object sender, EventArgs e)
        {
            SaveFileDialog ifile = new SaveFileDialog();
            ifile.FileName = ".png";

            if (ifile.ShowDialog() == DialogResult.OK)
            {
                pictureBox2.Image.Save(ifile.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }
    }
}