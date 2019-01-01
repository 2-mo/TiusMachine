using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Pickers;


namespace FindMianTri.Models
{
    public class TimeTurn
    {
        public async Task<StorageFile> PickerImage()
        {
            //文件选择器
            FileOpenPicker openPicker = new FileOpenPicker
            {
                //选择视图模式
                ViewMode = PickerViewMode.Thumbnail,
                //openPicker.ViewMode = PickerViewMode.List;

                //初始位置
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };

            //添加文件类型
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");

            StorageFile file = await openPicker.PickSingleFileAsync();
            return file;
        }

        public async Task<string> GetImageProperties(StorageFile imageFile)
        {
            string timestamp = "181231";
            if (imageFile != null)
            {
                ImageProperties props = await imageFile.Properties.GetImagePropertiesAsync();

                DateTimeOffset date = props.DateTaken;

                if (date != null)
                {
                    timestamp = TurnTimeToHex(date);
                }
            }
            return timestamp;
        }


        private string TurnTimeToHex(DateTimeOffset date)
        {
            // 第一位：年份
            int year = int.Parse(date.ToString("yyyy"));
            year = year % 8;


            // 第二位：月份
            int month = int.Parse(date.ToString("MM"));


            // 第三位：日期
            int day = int.Parse(date.ToString("dd"));
            if (day > 15) { year += 8; }
            day = day % 16;


            // 第四位
            int hour = int.Parse(date.ToString("HH"));
            int hourP = int.Parse(date.ToString("hh"));


            // 五六位
            string minute = date.ToString("mm");
            int minute_d = int.Parse(minute.Substring(0, 1));
            int minute_e = int.Parse(minute.Substring(1, 1));

            if (hour > 12) { minute_d += 10; }


            // 转换为十六进制
            string one = year.ToString("X");
            string two = month.ToString("X");
            string three = day.ToString("X");

            string four = hourP.ToString("X");
            string five = minute_d.ToString("X");
            string six = minute_e.ToString("X");

            string exTime = one + two + three + four + five + six;
            return exTime;
        }

        private void DoClip(string str)
        {
            DataPackage dp = new DataPackage();
            dp.SetText(str);
            Clipboard.SetContent(dp);
        }

    }
}
