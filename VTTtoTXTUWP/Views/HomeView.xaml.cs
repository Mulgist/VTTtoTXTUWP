using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

// 빈 페이지 항목 템플릿에 대한 설명은 https://go.microsoft.com/fwlink/?LinkId=234238에 나와 있습니다.

namespace VTTtoTXTUWP.Views
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class HomeView : Page
    {
        StorageFile file;
        String fileName = "";

        public HomeView()
        {
            InitializeComponent();
        }

        private async void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            await FilePickAsync();
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            await FileSaveAsync();
        }

        private void ConvertButton_Click(object sender, RoutedEventArgs e)
        {
            TxtText.Text = VttToTxt(VttText.Text);
        }

        private async Task FilePickAsync()
        {
            var open = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            open.FileTypeFilter.Add(".vtt");
            open.FileTypeFilter.Add("*");
            

            file = await open.PickSingleFileAsync();

            if (file != null)
            {
                // 파일 로드됨
                FilePathText.Text = file.Path;
                fileName = file.Name;
            }

            VttText.Text = await FileIO.ReadTextAsync(file);
        }

        private async Task FileSaveAsync()
        {
            var save = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            save.FileTypeChoices.Add("텍스트 파일", new List<String> { ".txt" });
            save.FileTypeChoices.Add("모든 파일", new List<String> { "." });
            save.DefaultFileExtension = ".txt";

            //save.SuggestedSaveFile = saveFile;
            save.SuggestedFileName = fileName;
            
            var saveFile = await save.PickSaveFileAsync();
            var lineList = textRead(TxtText.Text);

            await FileIO.WriteLinesAsync(saveFile, lineList);
        }

        private List<String> textRead(string text)
        {
            List<String> list = new List<string> { };
            string line;
            StringReader reader = new StringReader(text);
            while (true)
            {
                line = reader.ReadLine();
                if (line != null)
                {
                    list.Add(line);
                }
                else
                {
                    break;
                }
            }
            return list;
        }

        private string VttToTxt(string original)
        {
            char[] charArray;
            string txtContent = "";
            string line;
            StringReader reader = new StringReader(original);
            while (true)
            {
                line = reader.ReadLine();
                if (line != null)
                {
                    if (line == "")
                    {
                        continue;
                    }
                    else if (line.Contains(" --> "))
                    {
                        continue;
                    }
                    else
                    {
                        charArray = line.ToCharArray();
                        if (charArray[charArray.Length - 1] == '.')
                        {
                            txtContent += line + " ";
                        }
                        else
                        {
                            txtContent += line + " ";
                        }
                    }
                }
                else
                {
                    break;
                }
            }

            return txtContent;
        }
    }

}
