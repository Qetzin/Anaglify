using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;

namespace AsmProj
{
    class ImageProcces
    {
        public ImageProcces()
        {
        }
        /// <summary>
        /// Metoda zczytująca wartości kolorów poszczególnych pikseli, która zapisuje jedynie wartości koloru czerwonego do tablicy int
        /// </summary>
        /// <param name="tabR">tablica z wartościami kolorów pikseli</param>
        /// <param name="start">indeks początkowy</param>
        /// <param name="end">indeks końcowy</param>
        /// <param name="bytesPerPixel">ilość bajtów w jednym pikselu</param>
        public static void SeperateBitmaps(int[] tabR, int start, int end, int bytesPerPixel)
        {

            for (int i = start; i < end; i += bytesPerPixel)
            {

                int R = tabR[i + 2];

                tabR[i] = 0;
                tabR[i + 1] = 0;
                tabR[i + 2] = R;
            }
        }
        /// <summary>
        /// Metoda zczytująca wartości kolorów poszczególnych pikseli, która zapisuje jedynie wartości koloru niebieskiego i zielonego do tablicy int
        /// </summary>
        /// <param name="tabBG">tablica z wartościami kolorów pikseli</param>
        /// <param name="start">indeks początkowy</param>
        /// <param name="end">indeks końcowy</param>
        /// <param name="bytesPerPixel">ilość bajtów w jednym pikselu</param>
        public static void SeperateBitmapsBG(int[] tabBG, int start, int end, int bytesPerPixel)
        {

            for (int i = start; i < end; i += bytesPerPixel)
            {
                int B = tabBG[i];
                int G = tabBG[i + 1];

                tabBG[i] = B;
                tabBG[i + 1] = G;
                tabBG[i + 2] = 0;
            }
        }
        /// <summary>
        /// Metoda konwertująca wartości tablicy z int na byte do bitmapy
        /// </summary>
        /// <param name="tab">tablica int do przekonwertowania</param>
        /// <param name="result">bitmapa na którą zapisujemy wartości</param>
        public static void IntToBitmap(int[] tab, Bitmap result)
        {
            BitmapData resultd = result.LockBits(new Rectangle(0, 0, result.Width, result.Height), ImageLockMode.ReadWrite, result.PixelFormat);
            byte[] resulttab = Array.ConvertAll(tab, c => (byte)c);
            Marshal.Copy(resulttab, 0, resultd.Scan0, resulttab.Length);
            result.UnlockBits(resultd);
        }
        /// <summary>
        /// Metoda łącząca bitmape czerwoną z bitmapą niebiesko-zieloną z odpowiednim przezunięciem
        /// </summary>
        /// <param name="RedBitmap">bitmapa z wartościami koloru czerwoneg</param>
        /// <param name="BlueGreenBitmap">bitmapa z wartościami koloru niebiesko-zieloneg</param>
        /// <param name="result">bitmapa w której zapisywany jest wynik</param>
        /// <param name="dif">wartość przesunięcia "odległości" między bitmapami</param>
        public static void ConvertionMT(Bitmap RedBitmap, Bitmap BlueGreenBitmap, Bitmap result, int dif)
        {
            for (int i = 0; i < result.Width; i++)
            {
                for (int j = 0; j < result.Height; j++)
                {
                    int rr = 0;
                    int gBG = 0;
                    int bBG = 0;
                    if ((i - dif) >= 0)
                    {
                        Color cr = RedBitmap.GetPixel(i - dif, j);
                        rr = cr.R;
                    }
                    if ((i + dif < BlueGreenBitmap.Width))
                    {
                        Color cBG = BlueGreenBitmap.GetPixel(i + dif, j);
                        gBG = cBG.G;
                        bBG = cBG.B;
                    }
                    result.SetPixel(i, j, Color.FromArgb((rr), (gBG), (bBG)));
                }
            }
        }
    }
}
