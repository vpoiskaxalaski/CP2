using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;


namespace Notest
{
    enum Search
    { Topic, Header }
    public partial class UserWindow : Window
    {
        List<Test> testList = new List<Test>();
        Search tab;

        public UserWindow()
        {
            InitializeComponent();
            UserLogin.Text = Class.CurrentUser.user != null ? Class.CurrentUser.user.Login : "Неизвестный";
            TestGrid.Items.Clear();
            using (Context db = new Context())
            {
                TestGrid.ItemsSource = db.Tests.ToList();
            }
        }

        private void OnSearchBeginByTheme(object sender, TextChangedEventArgs e)
        {
            SelectedHeaders.Items.Clear();
            SelectedTopics.Items.Clear();
            using (Context db = new Context())
            {
                var topics = from test in db.Tests where test.Topic.StartsWith(ByTopic.Text) select test.Topic;
                if (topics.Count() > 0)
                {
                    testList.Clear();
                    SelectedTopics.IsEnabled = true;
                    foreach (string topic in topics)
                    {
                        SelectedTopics.Items.Add(topic);
                    }
                    SelectedTopics.SelectedItem = SelectedTopics.Items[0];
                }
            }
        }

        private void OnTopicSelected(object sender, SelectionChangedEventArgs e)
        {
            var selectedTopic = SelectedTopics.SelectedItem;
            if (selectedTopic != null)
            {
                string topic = selectedTopic.ToString();
                using (Context db = new Context())
                {
                    var tests = from test in db.Tests where test.Topic == topic select test;
                    if (tests.Count() > 0)
                    {
                        SelectedHeaders.IsEnabled = true;
                        foreach (Test test in tests)
                        {
                            testList.Add(test);
                            SelectedHeaders.Items.Add(test.Header);
                        }
                        SelectedHeaders.SelectedItem = SelectedHeaders.Items[0];
                    }
                }
            }
        }

        private void OnHeaderSelected(object sender, SelectionChangedEventArgs e)
        {
            int index;
            if (tab.ToString() == "Topic")
            {
                index = SelectedHeaders.SelectedIndex;
            }
            else
            {
                index = Headers.SelectedIndex;
            }
            if (index >= 0)
            {
                Test test = testList.ElementAt(index);
                Header.Content = test.Header;
                Topic.Content = test.Topic;
                Author.Content = test.Author;
                CurrentTest.test = test;
            }
        }

        private void OnSearchBeginByHeader(object sender, TextChangedEventArgs e)
        {
            Headers.Items.Clear();
            testList.Clear();
            using (Context db = new Context())
            {
                var tests = from test in db.Tests where test.Header.StartsWith(ByHeader.Text) select test;
                if (tests.Count() > 0)
                {
                    Headers.IsEnabled = true;
                    foreach (Test test in tests)
                    {
                        Headers.Items.Add(test.Header);
                        testList.Add(test);
                    }
                    Headers.SelectedItem = Headers.Items[0];
                }
            }
        }

        private void OnChooseTest(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show($"Выбрать тест {Header.Content}?", "Уверены?", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                if ((string)Header.Content != "")
                {
                    ChooseTest chooseTest = new ChooseTest();
                    chooseTest.Show();
                    this.Close();
                }
                else
                {
                    GetStatus("Вы не выбрали тест!", 2);
                    e.Handled = true;
                }
            }
            else
            {
                Author.Content = null;
                Header.Content = null;
                Topic.Content = null;
                testList.Clear();
                for (int i = Tabs.Items.Count - 1; i > -1; i--)
                {
                    var tabIndex = Tabs.Items[i] as TabItem;
                    tabIndex.IsSelected = true;
                }
            }
        }

        private void GetStatus(string message, int level) // 0 - всё хорошо, 1 - есть недочёты, 2 - критическая ошибка
        {
            Status.Content = message;
            Status.FontStyle = FontStyles.Italic;
            Status.FontWeight = FontWeights.Bold;
            switch (level)
            {
                case 0:
                    Status.Background = new SolidColorBrush(Color.FromArgb(0xaa, 0x00, 0xff, 0x00));
                    break;
                case 1:
                    Status.Background = new SolidColorBrush(Color.FromArgb(0xaa, 0xff, 0xff, 0x80));
                    break;
                case 2:
                    Status.Background = new SolidColorBrush(Color.FromArgb(0xaa, 0xff, 0x00, 0x00));
                    break;
                default:
                    Status.Background = new SolidColorBrush(Color.FromArgb(0xaa, 0xba, 0xac, 0xc7));  // очень светлый пурпурно-синий
                    break;
            }
        }

        private void OnLoadedWindow(object sender, RoutedEventArgs e)
        {

        }
        private void OnCloseWindow(object sender, EventArgs e)
        {

        }

        private void OnTabChanged(object sender, SelectionChangedEventArgs e)
        {
            TabControl tabs = sender as TabControl;
            tab = (Search)tabs.SelectedIndex;
        }

        private void OnSearchTypeSelected(object sender, RoutedEventArgs e)
        {
            var selectedTab = sender as TabItem;
            if (selectedTab.Header.ToString().Equals("Поиск по теме"))
            {
                tab = Search.Topic;
                DisposeHeader();
            }
            else
            {
                tab = Search.Header;
                DisposeTopic();
            }
        }

        private void DisposeTopic()
        {
            ByTopic.Text = "";
            SelectedHeaders.Items.Clear();
            SelectedHeaders.IsEnabled = false;
            SelectedTopics.Items.Clear();
            SelectedTopics.IsEnabled = false;
        }

        private void DisposeHeader()
        {
            ByHeader.Text = "";
            Headers.Items.Clear();
            Headers.IsEnabled = false;
        }
        
        

         //выход в окно регистрации/входа
        private void GoOut(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow startWindow = new MainWindow();
                Close();
                startWindow.Show();
            }
            catch
            {
                MessageBox.Show("Невозможно выйти");
            }
        }

        private void OnMouseOver(object sender, MouseEventArgs e)
        {
            var image = sender as Image;
            image.Source = BitmapFrame.Create(new Uri(@"pack://application:,,,/ico/opened_door.ico"));
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            var image = sender as Image;
            image.Source = BitmapFrame.Create(new Uri(@"pack://application:,,,/ico/door.ico"));
        }

        private void OnWatchResults(object sender, RoutedEventArgs e)
        {
            MyResults results = new MyResults();
            results.ShowDialog();
        }

        #region кнопки для окна
        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void HideWindow_Click(object sender, RoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        private void Fullscreen_Click(object sender, RoutedEventArgs e)
        {
            SystemCommands.MaximizeWindow(this);
            Fullscreen.Visibility = Visibility.Hidden;
            FullscreenExit.Visibility = Visibility.Visible;
        }

        private void FullscreenExit_Click(object sender, RoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(this);
            FullscreenExit.Visibility = Visibility.Hidden;
            Fullscreen.Visibility = Visibility.Visible;
        }
        #endregion
    }
}
