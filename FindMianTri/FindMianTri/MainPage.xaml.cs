using System;
using System.Threading.Tasks;
using FindMianTri.Models;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Composition;
using Windows.UI.Xaml.Hosting;
using System.Numerics;
using Windows.UI;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace FindMianTri
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


        private void DiffuseBtn_Click(object sender, RoutedEventArgs e) //应用文案
        {
            

            RanTextBlock.Text = "";
            RidiTextBlock.Text = RidiTextBox.Text;
            MainColorText.Text = "Updated.";

            InitComposition();

        }

        private void DiffuseBtn2_Click(object sender, RoutedEventArgs e) //保存
        {
            FileAbouts fileAbouts = new FileAbouts();
            fileAbouts.SavePanelPic(PicGrid);
            MainColorText.Text = "Saved in your Pictures Library!";
        }


        private void DropArea_DragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Copy;
            e.DragUIOverride.IsCaptionVisible = false;
            e.DragUIOverride.IsContentVisible = true;
            e.DragUIOverride.IsGlyphVisible = false;

            e.Handled = true;
        }

        private async void DropArea_Drop(object sender, DragEventArgs e)
        {
            if (e.DataView.Contains(StandardDataFormats.StorageItems))
            {
                MainColorText.Text = "Loading...";
                var items = await e.DataView.GetStorageItemsAsync();

                //文件过滤，防止他们往里面拖 excel
                // items = items.OfType<StorageFile>().Where(s => s.FileType.Equals(".jpg")).ToList() as IReadOnlyList<IStorageItem>;
                //items = items.OfType<StorageFile>().Where(s => s.FileType.Equals(".png")).ToList() as IReadOnlyList<IStorageItem>;            

                //细化一下文件类型，然后对于非法类型做一下提示

                if (items.Count == 0)
                {
                    MainColorText.Text = "Please drag a JPG file in this panel.";
                }


                if (items.Count > 0)
                {
                    var storageFile = items[0] as StorageFile;

                    //设置图片路径
                    ImageBrush imageBrush = new ImageBrush();
                    BitmapImage bitmap = new BitmapImage();

                    await bitmap.SetSourceAsync(await storageFile.OpenAsync(FileAccessMode.Read));

                    imageBrush.ImageSource = bitmap;
                    PriImagePath.Fill = imageBrush;
                    PriImagePath2.Fill = imageBrush;
                    TempPanelImg.Fill = imageBrush;
                    TempPanelImg.Visibility = Visibility.Visible;

                    MainColorText.Text = "Calculating...";

                    //获取主色调
                    ColorAbouts colorAbouts = new ColorAbouts();
                    FileAbouts fileAbouts = new FileAbouts();
                    ColorMatch colorMatch = new ColorMatch();

                    StorageFile file2 = await fileAbouts.GetPanelPic(TempPanel);

                    string comColor = await colorAbouts.GetPicMainColor(file2, StatisticsGrid);
                    string mainColor = "#" + comColor.Substring(3, 6);
                    TempPanelImg.Visibility = Visibility.Collapsed;

                    //颜色匹配&设置边框
                    int minNub = colorMatch.CompareDist(mainColor);

                    TriditionalColor triditionalColors = new TriditionalColor();
                    TridColor[] tridColors = new TridColor[630];
                    tridColors = triditionalColors.InitTriditionalColors();

                    ColorNameTextblock.Text = tridColors[minNub].Name;
                    SolidColorBrush triColor = colorAbouts.GetSolidColorBrush(tridColors[minNub].Hex);
                    ColorBorder.BorderBrush = triColor;

                    MainColorGrid.Background = triColor;

                    //设置时间戳
                    TimeTurn timestamp = new TimeTurn();
                    string atime = await timestamp.GetImageProperties(storageFile);
                    TimestampTextBlock.Text = atime;

                    //test
                    TempPanelImg.Visibility = Visibility.Visible;

                    WriteableBitmap wb2 = new WriteableBitmap(1000, 600);
                    string mainColorHex2 = "#000000";


                    if (file2 != null)
                    {
                        // Set the source of the WriteableBitmap to the image stream
                        using (IRandomAccessStream fileStream2 = await file2.OpenAsync(FileAccessMode.Read))
                        {
                            try
                            {
                                await wb2.SetSourceAsync(fileStream2);
                                System.IO.File.Delete(file2.Path);
                            }
                            catch (TaskCanceledException)
                            {
                                // The async action to set the WriteableBitmap's source may be canceled if the user clicks the button repeatedly
                            }
                        }

                        mainColorHex2 = ColorMatch.GetMajorColor2(wb2).ToString();
                        string mainColor2 = "#" + mainColorHex2.Substring(3, 6);
                        //DisplayText.Text = mainColor2;

                        TimestampTextBlock.Foreground = colorAbouts.GetSolidColorBrush(colorAbouts.InvertColor2(mainColor2));


                        //设置天数
                        TextHelper textHelper = new TextHelper();
                        DaysTextblock.Text = textHelper.CalculateDays().ToString();

                        //设置节气                  
                        LunarHolDayTextBlock.Text = TextHelper.GetLunarHolDay(DateTime.Now);

                        //设置表情
                        RanTextBlock.Text = textHelper.RandomEmoji();


                        //弥散阴影
                        MainColorText.Text = "Almost Done...";

                        WriteableBitmap wb = new WriteableBitmap(1600, 1600);
                        StorageFile file = await fileAbouts.GetPanelPic(PicPanel);
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

                            //高斯模糊
                            BlurEffect be = new BlurEffect(wb);
                            ShadowImg.Source = await be.ApplyFilter(22);//高斯模糊等级可以自己定义
                            System.IO.File.Delete(file.Path);
                        }

                        MainColorText.Text = "Done!";
                    }
                }
            }


        }

        // j新建视觉层画阴影
        private void InitComposition()
        {
            Compositor compositor = ElementCompositionPreview.GetElementVisual(PicGrid).Compositor;

            //Create LayerVisual
            LayerVisual layerVisual = compositor.CreateLayerVisual();
            layerVisual.Size = new Vector2(900, 900);

            //Create SpriteVisuals to use as LayerVisual child
            SpriteVisual sv1 = compositor.CreateSpriteVisual();
            sv1.Brush = compositor.CreateColorBrush(Windows.UI.Colors.Blue);
            sv1.Size = new Vector2(300, 300);
            sv1.Offset = new Vector3(200, 200, 0);

            SpriteVisual sv2 = compositor.CreateSpriteVisual();
            sv2.Brush = compositor.CreateColorBrush(Colors.Red);
            sv2.Size = new Vector2(300, 300);
            sv2.Offset = new Vector3(400, 400, 0);

            //Add children to the LayerVisual
            layerVisual.Children.InsertAtTop(sv1);
            layerVisual.Children.InsertAtTop(sv2);

            //Create DropShadow
            DropShadow shadow = compositor.CreateDropShadow();
            shadow.Color = Colors.DarkSlateGray;
            shadow.Offset = new Vector3(40, 40, 0);
            shadow.BlurRadius = 9;
            shadow.SourcePolicy = CompositionDropShadowSourcePolicy.InheritFromVisualContent;

            //Associate Shadow with LayerVisual
            layerVisual.Shadow = shadow;

            ElementCompositionPreview.SetElementChildVisual(PicGrid, layerVisual);
        }
    }
}
