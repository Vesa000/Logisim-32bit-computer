using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace ImageToLogisim
{
    class Program
    {
        public static int screenWidth = 32;
        public static int screenHeight = 28;
        public static float Colourcutoff = 0.5f;

        struct PosStruct
        {
            public string x;
            public string y;
        }

        static void Main(string[] args)
        {
            string currentDir = Environment.CurrentDirectory;
            string inputFolder = currentDir + "\\Input";
            string[] imagePaths = Directory.GetFiles(inputFolder);
            int imageOffset = 5500;
            int currentRomAddress = 0;

            string outPath = currentDir + "\\Output\\Image.txt";
            StreamWriter sw = new StreamWriter(outPath);
            sw.WriteLine("v2.0 raw");

            foreach (string imagePath in imagePaths)
            {
                if (imagePath.Contains(".png"))
                {
                    #region png
                    Image image = System.Drawing.Image.FromFile(imagePath);
                    Bitmap bmp = new Bitmap(image, new Size(screenWidth, screenHeight));

                    List<PosStruct> cyanList = new List<PosStruct>();
                    List<PosStruct> magentaList = new List<PosStruct>();
                    List<PosStruct> yellowList = new List<PosStruct>();

                    for (int y = 0; y < screenHeight; y++)
                    {
                        for (int x = 0; x < screenWidth; x++)
                        {
                            Color color = bmp.GetPixel(x, y);
                            float black = new float[] { 1 - color.R, 1 - color.G, 1 - color.B }.Min();
                            float Cyan = (1 - color.R - black) / (1 - black);
                            float magenta = (1 - color.G - black) / (1 - black);
                            float yellow = (1 - color.B - black) / (1 - black);

                            if (Cyan > Colourcutoff)
                            {
                                int width = 1 << x;
                                int height = 1 << (screenHeight - y - 1);
                                PosStruct p = new PosStruct()
                                {
                                    x = width.ToString("x"),
                                    y = height.ToString("x")
                                };
                                cyanList.Add(p);
                            }
                            if (magenta > Colourcutoff)
                            {
                                int width = 1 << x;
                                int height = 1 << (screenHeight - y + 1);
                                PosStruct p = new PosStruct()
                                {
                                    x = width.ToString("x"),
                                    y = height.ToString("x")
                                };
                                magentaList.Add(p);
                            }
                            if (yellow > Colourcutoff)
                            {
                                int width = 1 << x;
                                int height = 1 << (screenHeight - y + 3);
                                PosStruct p = new PosStruct()
                                {
                                    x = width.ToString("x"),
                                    y = height.ToString("x")
                                };
                                yellowList.Add(p);
                            }
                        }
                    }



                    bool cyanFinished = true;
                    bool magentaFinished = true;
                    bool yellowFinished = false;

                    while (!cyanFinished | !magentaFinished | !yellowFinished)
                    {

                        if (cyanList.Count > 0)
                        {
                            PosStruct pos = cyanList.First();
                            sw.Write(" " + pos.x);
                            sw.Write(" " + pos.y);
                            cyanList.Remove(pos);
                        }
                        else
                        {
                            cyanFinished = true;
                            sw.Write(" 0");
                            sw.Write(" 0");
                        }

                        if (magentaList.Count > 0)
                        {
                            PosStruct pos = magentaList.First();
                            sw.Write(" " + pos.x);
                            sw.Write(" " + pos.y);
                            magentaList.Remove(pos);
                        }
                        else
                        {
                            magentaFinished = true;
                            sw.Write(" 0");
                            sw.Write(" 0");
                        }

                        if (yellowList.Count > 0)
                        {
                            PosStruct pos = yellowList.First();
                            sw.Write(" " + pos.x);
                            sw.Write(" " + pos.y);
                            yellowList.Remove(pos);
                        }
                        else
                        {
                            yellowFinished = true;
                            sw.Write(" 0");
                            sw.Write(" 0");
                        }
                        sw.WriteLine();
                        currentRomAddress += 6;
                    }
                    sw.WriteLine((imageOffset - currentRomAddress) + "*0");
                    currentRomAddress = 0;
                    Console.WriteLine("Image converted");
#endregion
                }
                else if (imagePath.Contains(".gif"))
                {
                    #region gif
                    string gifoutPath = currentDir + "\\Output\\gif.txt";
                    StreamWriter swgif = new StreamWriter(gifoutPath);
                    swgif.WriteLine("v2.0 raw");

                    Image gifImg = Image.FromFile(imagePath);
                    FrameDimension dimension = new FrameDimension(gifImg.FrameDimensionsList[0]);
                    int framecount = gifImg.GetFrameCount(dimension);

                    for (int i = 0; i < framecount; i++)
                    {
                        gifImg.SelectActiveFrame(dimension, i);
                        Bitmap bmp = new Bitmap(gifImg, new Size(screenWidth, screenHeight-2));
                        currentRomAddress = 0;

                        for (int y = 0; y < screenHeight-2; y++)
                        {
                            for (int x = 0; x < screenWidth; x++)
                            {
                                Color color = bmp.GetPixel(x, y);
                                int coloravg = (color.R + color.G + color.B)/3;

                                if (coloravg > 200)
                                {
                                    int width = 1 << x;
                                    int height = 1 << (screenHeight - y + 5);
                                    swgif.Write(width.ToString("x") + " ");
                                    swgif.WriteLine(height.ToString("x"));
                                    currentRomAddress += 2;
                                }
                            }
                        }
                        swgif.WriteLine((4096 - currentRomAddress) + "*0");
                    }
                    swgif.Close();
                    Console.WriteLine("Gif converted");
                    #endregion
                }
                else
                {
                    Console.WriteLine("File type was not .supported: " + imagePath);
                }
            }
            sw.Close();
            Console.ReadKey();
        }
    }
}
