using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace FindMianTri.Models
{
    class ColorAbouts
    {
        public string InvertColor(string priColor)
        {
            string newColor = "#000000";
            int[] priColorRGB = new int[5];
            priColorRGB[0] = Convert.ToInt32(priColor.Substring(1, 2), 16);
            priColorRGB[1] = Convert.ToInt32(priColor.Substring(3, 2), 16);
            priColorRGB[2] = Convert.ToInt32(priColor.Substring(5, 2), 16);
            int resultR = 255 - priColorRGB[0];//反红
            int resultG = 255 - priColorRGB[1];//反绿
            int resultB = 255 - priColorRGB[2];//反蓝

            newColor = "#" + resultR.ToString("x2") + resultB.ToString("x2") + resultB.ToString("x2");
            return newColor;
        }

        public string InvertColor2(string priColor)
        {
            string newColor = "#000000";
            int[] priColorRGB = new int[5];
            priColorRGB[0] = Convert.ToInt32(priColor.Substring(1, 2), 16);
            priColorRGB[1] = Convert.ToInt32(priColor.Substring(3, 2), 16);
            priColorRGB[2] = Convert.ToInt32(priColor.Substring(5, 2), 16);

            int resultR = priColorRGB[0] < 128 ? 255 : 0;
            int resultG = priColorRGB[1] < 128 ? 255 : 0;
            int resultB = priColorRGB[2] < 128 ? 255 : 0;

            newColor = "#" + resultR.ToString("x2") + resultB.ToString("x2") + resultB.ToString("x2");
            return newColor;




        }

        public async Task<string> GetPicMainColor(StorageFile storageFile, StackPanel StatisticsGrid)
        {
            WriteableBitmap wb = new WriteableBitmap(1600, 1600);
            string mainColorHex = "#000000";

            StorageFile file = storageFile;
            if (file != null)
            {
                // Set the source of the WriteableBitmap to the image stream
                using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read))
                {
                    try
                    {
                        await wb.SetSourceAsync(fileStream);
                    }
                    catch (TaskCanceledException)
                    {
                        // The async action to set the WriteableBitmap's source may be canceled if the user clicks the button repeatedly
                    }
                }

                mainColorHex = ColorMatch.GetMajorColor(wb, StatisticsGrid).ToString();  //这个颜色带不透明度的。。

            }
            return mainColorHex;

        }



        public double CalculateDist(string mainColor, string compareColor)
        {
            int[] mainColorRgb = ColorDisRGB(mainColor);
            int[] compareColorRgb = ColorDisRGB(compareColor);

            double dist = Math.Pow((mainColorRgb[0] - compareColorRgb[0]), 2)
                + Math.Pow((mainColorRgb[1] - compareColorRgb[1]), 2)
                + Math.Pow((mainColorRgb[2] - compareColorRgb[2]), 2);

            return dist;
        }

        public int[] ColorDisRGB(string priColor)
        {
            int[] priColorRGB = new int[5];
            priColorRGB[0] = Convert.ToInt32(priColor.Substring(1, 2), 16);
            priColorRGB[1] = Convert.ToInt32(priColor.Substring(3, 2), 16);
            priColorRGB[2] = Convert.ToInt32(priColor.Substring(5, 2), 16);

            return priColorRGB;
        }

        public SolidColorBrush GetSolidColorBrush(string hex)
        {
            hex = hex.Replace("#", string.Empty);
            byte a = (byte)(Convert.ToUInt32("FF", 16));
            byte r = (byte)(Convert.ToUInt32(hex.Substring(0, 2), 16));
            byte g = (byte)(Convert.ToUInt32(hex.Substring(2, 2), 16));
            byte b = (byte)(Convert.ToUInt32(hex.Substring(4, 2), 16));

            SolidColorBrush myBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(a, r, g, b));
            return myBrush;
        }


        //This method converts the values to RGB
        public RgbColor HslToRgb(int Hue, int Saturation, int Lightness)
        {
            double num4 = 0.0;
            double num5 = 0.0;
            double num6 = 0.0;
            double num = ((double)Hue) % 360.0;
            double num2 = ((double)Saturation) / 100.0;
            double num3 = ((double)Lightness) / 100.0;
            if (num2 == 0.0)
            {
                num4 = num3;
                num5 = num3;
                num6 = num3;
            }
            else
            {
                double d = num / 60.0;
                int num11 = (int)Math.Floor(d);
                double num10 = d - num11;
                double num7 = num3 * (1.0 - num2);
                double num8 = num3 * (1.0 - (num2 * num10));
                double num9 = num3 * (1.0 - (num2 * (1.0 - num10)));
                switch (num11)
                {
                    case 0:
                        num4 = num3;
                        num5 = num9;
                        num6 = num7;
                        break;
                    case 1:
                        num4 = num8;
                        num5 = num3;
                        num6 = num7;
                        break;
                    case 2:
                        num4 = num7;
                        num5 = num3;
                        num6 = num9;
                        break;
                    case 3:
                        num4 = num7;
                        num5 = num8;
                        num6 = num3;
                        break;
                    case 4:
                        num4 = num9;
                        num5 = num7;
                        num6 = num3;
                        break;
                    case 5:
                        num4 = num3;
                        num5 = num7;
                        num6 = num8;
                        break;
                }
            }
            return new RgbColor((int)(num4 * 255.0), (int)(num5 * 255.0), (int)(num6 * 255.0));
        }
        //The structure that will hold the RGB Values
        public struct RgbColor
        {
            public RgbColor(int r, int g, int b)
            {
                red = r;
                green = g;
                blue = b;
            }
            public int red;
            public int green;
            public int blue;
        }

        public int[] RgbTranToHsl(int R, int G, int B)
        {
            int hVALUE, sVALUE, lVALUE;
            //   RGB空间到HSL空间的转换  
            double Delta, CMax, CMin;
            double Red, Green, Blue, Hue, Sat, Lum;
            Red = (double)R / 255;
            Green = (double)G / 255;
            Blue = (double)B / 255;
            CMax = Math.Max(Red, Math.Max(Green, Blue));
            CMin = Math.Min(Red, Math.Min(Green, Blue));
            //计算hsb
            //Lum = (CMax + CMin) / 2;
            if (CMax == CMin)
            {
                Sat = 0;
                Hue = 0;
            }
            else
            {
                //计算hsb
                //if (Lum < 0.5)
                //    Sat = (CMax - CMin) / (CMax + CMin);
                //else
                //    Sat = (CMax - CMin) / (2 - CMax - CMin);
                //计算hsv（彩色空间）
                if (CMax == 0)
                {
                    Sat = 0;
                }
                else
                {
                    Sat = 1 - CMin / CMax;
                }
                Delta = CMax - CMin;
                if (Red == CMax)
                {
                    Hue = (Green - Blue) / Delta;
                }
                else if (Green == CMax)
                {
                    Hue = 2 + (Blue - Red) / Delta;
                }
                else
                {
                    Hue = 4.0 + (Red - Green) / Delta;
                }
                Hue = Hue / 6;
                if (Hue < 0)
                    Hue = Hue + 1;
            }
            hVALUE = (int)Math.Round(Hue * 360);
            sVALUE = (int)Math.Round(Sat * 100);
            lVALUE = (int)Math.Round(CMax * 100);
            int[] HSL = new int[3] { hVALUE, sVALUE, lVALUE };
            return HSL;
            //lvalue = (int)Math.Round(Lum * 100);
        }
    }
}
