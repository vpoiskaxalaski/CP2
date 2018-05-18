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
        int testID;
        Search tab;

        public UserWindow()
        {
            InitializeComponent();

            UserLogin.Text = Class.CurrentUser.user != null ? Class.CurrentUser.user.Login : "Неизвестный";
        }

        #region поиск по теме 
        private void OnSearchBeginByTheme(object sender, TextChangedEventArgs e)
        {
            try
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
           catch
            {
                MessageBox.Show("Search error");
            }
        }

   
        private void OnTopicSelected(object sender, SelectionChangedEventArgs e)
        {
            try
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
            catch
            {
                MessageBox.Show("Search error");
            }
        }
        #endregion

        #region поиск по названию
        private void OnHeaderSelected(object sender, SelectionChangedEventArgs e)
        {
            try
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
                    testID = test.Id;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }

        private void OnSearchBeginByHeader(object sender, TextChangedEventArgs e)
        {
            try
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
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        //тест выбран
        private void OnChooseTest(object sender, RoutedEventArgs e)
        {

            try
            {
                var result = MessageBox.Show($"Выбрать тест {Header.Content}?", "Уверены?", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    if ((string)Header.Content != "")
                    {
                        using (StreamWriter writer = new StreamWriter("choosen.txt", false))
                        {
                            writer.WriteLine(testID);
                        }
                        Testing testing = new Testing();
                        testing.Show();
                        this.Close();
                    }
                    else
                    {
                        GetStatus("Вы не выбрали тест!", 1);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GetStatus(string message, int level) // 0 - всё хорошо, 1 - есть недочёты, 2 - критическая ошибка
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
            try
            {
                var selectedTab = sender as TabItem;
                if (selectedTab.Header.ToString().Equals("Search by topic"))
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
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
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


        #region выход в окно регистрации/входа
        private void GoOut(object sender, RoutedEventArgs e)
        {
            try
            {
                Class.CurrentUser.user = null;
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
        #endregion
    }
}
