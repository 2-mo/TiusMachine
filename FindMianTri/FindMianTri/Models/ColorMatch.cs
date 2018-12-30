using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

namespace FindMianTri.Models
{
    class ColorMatch
    {
        public void SetLineLine(StackPanel stackPanel, int[] majorSatCounts) //调用是个问题
        {
            for (int i = 0; i < majorSatCounts.Length; i++)
            {
                var line1 = new Line
                {
                    Stroke = new SolidColorBrush(Windows.UI.Colors.Red),
                    X2 = majorSatCounts[i]
                };
                stackPanel.Children.Add(line1);
            }
        }

        public static Color GetMajorColor(WriteableBitmap bitmap, StackPanel StatisticsGrid)
        {
            ImageBlur imageBlur = new ImageBlur(bitmap);

            //色相数组
            int[] majorHues = new int[361];
            for (int i = 0; i < majorHues.Length; i++)
            {
                majorHues[i] = 0;
            }
            int[] majorSatCounts = new int[361];
            for (int i = 0; i < majorSatCounts.Length; i++)
            {
                majorSatCounts[i] = 0;
            }

            double lum_sum = 0;
            double imageCounts = 0;
            int darkLumCounts = 0;

            //计算主色调
            for (int h = 0; h < bitmap.PixelHeight; h++)
            {
                for (int w = 0; w < bitmap.PixelWidth; w++)
                {
                    int hue = imageBlur.getPixelHue(w, h);
                    int sat = imageBlur.getPixelSat(w, h);
                    int lum = imageBlur.getPixelLig(w, h);

                    imageCounts++;
                    lum_sum += lum;


                    if (lum > 10)
                    {
                        majorHues[hue] += sat;
                        majorSatCounts[hue]++;
                    }
                    else
                    {
                        darkLumCounts++;
                    }
                
                }

            }


            //比较得到最多的色调
            int hueMaxCounts = 0;
            int hueMax = 0;
         
            for (int i = 0; i < majorHues.Length; i++)
            {
                if (majorHues[i] > hueMaxCounts)
                {
                    hueMaxCounts = majorHues[i];
                    hueMax = i;
                }
            }



            int majorHue = hueMax;
            int majorSat = majorHues[hueMax] / majorSatCounts[hueMax];

            double majorLum = 0;
            majorLum = lum_sum / 600000;

           // double abcdef = 1 / 3;
            // int balanceLum = Convert.ToInt32(Math.Pow((2500*majorLum) - 125000, abcdef) +50);

            double aaaaaa = 1 / 2;
            int balanceLum = Convert.ToInt32(Math.Pow(100 * majorLum, aaaaaa));

           int balanceLum2 = Convert.ToInt32(majorLum);

            ColorAbouts colorAbouts = new ColorAbouts();
            ColorAbouts.RgbColor rgbColorMain = colorAbouts.HslToRgb(majorHue, majorSat, balanceLum2);

            Color myColor = Color.FromArgb(255,
                Convert.ToByte(rgbColorMain.red),
                Convert.ToByte(rgbColorMain.green),
                Convert.ToByte(rgbColorMain.blue));


            StatisticsGrid.Children.Clear();

            // 绘图部分
            for (int i = 0; i < majorSatCounts.Length; i++)
            {
                ColorAbouts colorAbouts2 = new ColorAbouts();

                int r = colorAbouts2.HslToRgb(i, 100, 100).red;
                int g = colorAbouts2.HslToRgb(i, 100, 100).green;
                int b = colorAbouts2.HslToRgb(i, 100, 100).blue;
                
                SolidColorBrush solidColorBrush = new SolidColorBrush(Color.FromArgb(255, Convert.ToByte(r), Convert.ToByte(g), Convert.ToByte(b)));

                var line1 = new Line
                {
                    Stroke = solidColorBrush,
                    X2 = Math.Pow(majorHues[i], 0.45)  //这里有个值太小然后闪退的问题，复现：丢一张纯色图进去
                };
                StatisticsGrid.Children.Add(line1);
            }

            //绘制背景
            ColorAbouts colorAbouts3 = new ColorAbouts();

            int r3 = colorAbouts3.HslToRgb(0, 0, balanceLum2).red;
            int g3 = colorAbouts3.HslToRgb(0, 0, balanceLum2).green;
            int b3 = colorAbouts3.HslToRgb(0, 0, balanceLum2).blue;
            SolidColorBrush solidColorBrush2 = new SolidColorBrush(Color.FromArgb(255, Convert.ToByte(r3), Convert.ToByte(g3), Convert.ToByte(b3)));

            StatisticsGrid.Background = solidColorBrush2;

            return myColor;
        }

        //计算部分区域颜色
        public static Color GetMajorColor2(WriteableBitmap bitmap)
        {
            ImageBlur imageBlur = new ImageBlur(bitmap);

            //色相数组
            int[] majorHues = new int[361];
            for (int i = 0; i < majorHues.Length; i++)
            {
                majorHues[i] = 0;
            }

            int sat_sum = 0;
            int lig_sum = 0;
            int hue_sum = 0;
            int counts = 0;

            //计算主色调
            for (int h = 0; h < bitmap.PixelHeight; h++)
            {
                for (int w = 0; w < bitmap.PixelWidth; w++)
                {
                    if (w>880&h<40)
                    {
                        int hue = imageBlur.getPixelHue(w, h);
                        int sat = imageBlur.getPixelSat(w, h);
                        int lig = imageBlur.getPixelLig(w, h);

                        hue_sum += hue;
                        sat_sum += sat;
                        lig_sum += lig;
                        counts++;
                    }
                }

            }

            double finLig1 = Math.Sqrt(lig_sum / (counts)) * 10;
            int finSat2 = Convert.ToInt32(Math.Sqrt(sat_sum / (counts)) * 10);
            int finLig2 = Convert.ToInt32(Math.Sqrt(lig_sum / (counts)) * 10);

            int finLig = lig_sum / (counts);
            int finSat = sat_sum / (counts);
            int finHue = hue_sum / (counts);

            ColorAbouts colorAbouts = new ColorAbouts();

            ColorAbouts.RgbColor rgbColorMain = colorAbouts.HslToRgb(finHue, finSat2, finLig2);

            Color myColor = Color.FromArgb(255,
                Convert.ToByte(rgbColorMain.red),
                Convert.ToByte(rgbColorMain.green),
                Convert.ToByte(rgbColorMain.blue));

            return myColor;
        }


        //颜色匹配部分
        public int CompareDist(string mainColor)
        {
            TriditionalColor triditionalColors = new TriditionalColor();
            TridColor[] tridColors = new TridColor[630];
            tridColors = triditionalColors.InitTriditionalColors();

            double[] dists = new double[630];
            double minDist = 196608;
            int minNub = 0;

            ColorAbouts colorAbouts = new ColorAbouts();
            for (int i = 0; i < 629; i++)
            {
                dists[i] = colorAbouts.CalculateDist(mainColor, tridColors[i].Hex);

                if (dists[i] <= minDist)
                {
                    minDist = dists[i];
                    minNub = i;
                }
            }

            return minNub;
        }

        
    }
}
