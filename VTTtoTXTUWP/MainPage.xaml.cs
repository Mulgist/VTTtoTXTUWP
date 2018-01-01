using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using VTTtoTXTUWP.Views;

// 빈 페이지 항목 템플릿에 대한 설명은 https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x412에 나와 있습니다.

namespace VTTtoTXTUWP
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            MainSplitView.IsPaneOpen = false;
            App.titleStack.Push("홈");
            Title.Text = App.titleStack.Peek();
            (MainSplitView.Content as Frame).Navigate(typeof(HomeView));

        }

        private void HambergerButton_Click(object sender, RoutedEventArgs e)
        {
            MainSplitView.IsPaneOpen = !MainSplitView.IsPaneOpen;
        }

        private void IconsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void BottomIconsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        
    }
}
