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

        // начало поиска по теме: ввод в textbox ByTopic
        private void OnSearchBeginByTheme(object sender, TextChangedEventArgs e)
        {
            ClearByTopic();
            if (ByTopic.Text != "")
            {
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
        }

        // очистка таба поиска по теме
        private void ClearByTopic()
        {
            SelectedHeaders.Items.Clear();
            SelectedTopics.Items.Clear();
            SelectedHeaders.IsEnabled = false;
            SelectedTopics.IsEnabled = false;
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
            ClearByHeader();
            if (ByHeader.Text != "")
            {
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
        }

        // очистить таб поиска по названию
        private void ClearByHeader()
        {
            Headers.Items.Clear();
            testList.Clear();
            Headers.IsEnabled = false;
        }

        // при нажатии кнопки выбора теста
        private void OnChooseTest(object sender, RoutedEventArgs e)
        {
            var headerTest = Header.Content != null ? Header.Content : "\\_(^_^)_/";
            var result = MessageBox.Show($"{(string)Application.Current.Resources["questSelect"]} \"{headerTest}\"?",
                (string)Application.Current.Resources["sure"], MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                if ((string)Header.Content != null)
                {
                    ChooseTest chooseTest = new ChooseTest();
                    chooseTest.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show((string)Application.Current.Resources["noTest"]);
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

        // вызывается при выборе другого таба
        private void OnSearchTypeSelected(object sender, RoutedEventArgs e)
        {
            var selectedTab = sender as TabItem;
            if (selectedTab.Header.ToString().Equals("By topic"))
            {
                tab = Search.Topic;
                ByHeader.Text = "";
                ClearByHeader();
            }
            else
            {
                tab = Search.Header;
                ByTopic.Text = "";
                ClearByTopic();
            }
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
                MessageBox.Show((string)Application.Current.Resources["impo"]);
            }
        }

        // дверь открывается
        private void OnMouseOver(object sender, MouseEventArgs e)
        {
            var image = sender as Image;
            image.Source = BitmapFrame.Create(new Uri(@"pack://application:,,,/ico/opened_door.ico"));
        }

        // дверь закрывается
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
