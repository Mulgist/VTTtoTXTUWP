using System;
using System.Collections.Generic;
using System.IO;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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

            // Object를 건들려면 InitializeComponent()를 먼저 수행한 뒤 진행해야함
            if (App.localSettings.Values["Option"] != null)
                switch (App.localSettings.Values["Option"])
                {
                    case 1:
                        Option1.IsChecked = true;
                        break;
                    case 2:
                        Option2.IsChecked = true;
                        break;
                    case 3:
                        Option3.IsChecked = true;
                        break;
                    case 4:
                        Option4.IsChecked = true;
                        break;
                }
            else
            {
                App.localSettings.Values["Option"] = 1;
                Option1.IsChecked = true;
            }
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
            if ((bool)Option1.IsChecked)
            {
                TxtText.Text = DontRemoveLineBreaks(VttText.Text);
                App.localSettings.Values["Option"] = 1;
            }
            else if ((bool)Option2.IsChecked)
            {
                TxtText.Text = RemoveExceptPureText(VttText.Text);
                App.localSettings.Values["Option"] = 2;
            }
            else if ((bool)Option3.IsChecked)
            {
                TxtText.Text = RemoveExceptPeriod(VttText.Text);
                App.localSettings.Values["Option"] = 3;
            }
            else if ((bool)Option4.IsChecked)
            {
                TxtText.Text = RemoveAllLineBreaks(VttText.Text);
                App.localSettings.Values["Option"] = 4;
            } 
        }

        private async Task FilePickAsync()
        {
            var open = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            open.FileTypeFilter.Add(".vtt");
            // open.FileTypeFilter.Add("*");

            file = await open.PickSingleFileAsync();

            if (file != null)
            {
                // 파일 로드됨
                FilePathText.Text = file.Path;
                fileName = file.Name.Substring(0, file.Name.LastIndexOf('.'));
                VttText.Text = await FileIO.ReadTextAsync(file);
            }
        }

        private async Task FileSaveAsync()
        {
            var save = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            save.FileTypeChoices.Add("텍스트 파일", new List<String> { ".txt" });
            save.FileTypeChoices.Add("모든 파일", new List<String> { "." });
            save.SuggestedFileName = fileName;
            save.DefaultFileExtension = ".txt";

            var saveFile = await save.PickSaveFileAsync();
            if (saveFile != null)
            {
                // 파일 저장한 다음 텍스트 넣기
                var lineList = textRead(TxtText.Text);
                await FileIO.WriteLinesAsync(saveFile, lineList);
            }
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

        // 옵션 1에 대한 함수
        private string DontRemoveLineBreaks(string original)
        {
            string txtContent = "";
            string line;
            StringReader reader = new StringReader(original);
            while (true)
            {
                line = reader.ReadLine();
                if (line != null)
                    if (line.Contains(" --> ") || line == "WEBVTT")
                        continue;
                    else
                        txtContent += line + '\n';
                else
                    break;
            }
            return txtContent;
        }

        // 옵션 2에 대한 함수
        private string RemoveExceptPureText(string original)
        {
            string txtContent = "";
            string line;
            StringReader reader = new StringReader(original);
            while (true)
            {
                line = reader.ReadLine();
                if (line != null)
                    if (line == "" || line.Contains(" --> ") || line == "WEBVTT")
                        continue;
                    else
                        txtContent += line + '\n';
                else
                    break;
            }
            return txtContent;
        }

        // 옵션 3에 대한 함수
        private string RemoveExceptPeriod(string original)
        {
            char[] charArray;
            string txtContent = "";
            string line;
            StringReader reader = new StringReader(original);
            while (true)
            {
                line = reader.ReadLine();
                if (line != null)
                    if (line == "" || line.Contains(" --> ") || line == "WEBVTT")
                        continue;
                    else
                    {
                        charArray = line.ToCharArray();
                        if (charArray[charArray.Length - 1] == '.')
                            txtContent += line + '\n';
                        else
                            txtContent += line + ' ';
                    }
                else
                    break;
            }
            return txtContent;
        }

        // 옵션 4에 대한 함수
        private string RemoveAllLineBreaks(string original)
        {
            string txtContent = "";
            string line;
            StringReader reader = new StringReader(original);
            while (true)
            {
                line = reader.ReadLine();
                if (line != null)
                    if (line == "" || line.Contains(" --> ") || line == "WEBVTT")
                        continue;
                    else
                        txtContent += line + ' ';
                else
                    break;
            }
            return txtContent;
        }
    }
}
