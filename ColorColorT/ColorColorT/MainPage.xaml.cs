using Microsoft.Graphics.Canvas;
using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace ColorColorT
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void TestBtn_Click(object sender, RoutedEventArgs e)
        {
            Uri uriAddress = new Uri("http://image.cn.made-in-china.com/2f0j01ZevaPGkRhqbc/%E5%A4%8D%E5%90%88%E4%BA%9A%E5%85%8B%E5%8A%9B-%E7%AC%AC%E4%B8%80%E4%BB%A3-%E9%98%B3%E6%98%A5%E7%99%BDR8703.jpg");

            CanvasDevice device = new CanvasDevice();
            //实例化资源
            var bimap = await CanvasBitmap.LoadAsync(device, uriAddress);

            //取色
            Color[] colors = bimap.GetPixelColors();

            myTextBlock.Text = colors[3].ToString();

        }

    }

}
