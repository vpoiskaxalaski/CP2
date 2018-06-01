using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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

namespace Notest
{
    /// <summary>
    /// Логика взаимодействия для FindTest_Dialog.xaml
    /// </summary>
    public partial class FindTest_Dialog : Window
    {
        public FindTest_Dialog()
        {
            InitializeComponent();


            foreach (MenuItem item in Languages.Items)
            {
                string name = item.Name;
                item.Height = 25;
                item.Width = 50;
                item.ToolTip = name;
                item.Margin = new Thickness(10, 0, 0, 0);
                item.Background = new ImageBrush
                {
                    ImageSource = BitmapFrame.Create(new Uri(GetLanguageDirectory() + $"\\{name}.png", UriKind.Relative)),
                    Opacity = 0.7
                };
            }

            //заполнение списка тестов
            using (Context db = new Context())
            {
                List<Test> tests = new List<Test>();
                tests.AddRange(db.Tests);
                AllTests.ItemsSource = tests;
            }
        }

        #region локализация
        private void OnLanguageChange(object sender, RoutedEventArgs e)
        {
            MenuItem selectedLang = sender as MenuItem;
            string lang = selectedLang.Name;
            DirectoryInfo directory = new DirectoryInfo(GetLanguageDirectory() + "/" + lang);
            try
            {
                FileInfo[] dictionaries = directory.GetFiles();
                Application.Current.Resources.Clear();
                foreach (FileInfo dictionary in dictionaries)
                {
                    int index = dictionary.FullName.IndexOf($"Languages\\{lang}");
                    string resourcePath = dictionary.FullName.Substring(index);
                    var uri = new Uri(resourcePath, UriKind.Relative);
                    ResourceDictionary resource = Application.LoadComponent(uri) as ResourceDictionary;
                    Application.Current.Resources.MergedDictionaries.Add(resource);
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private string GetLanguageDirectory()
        {
            DirectoryInfo directory = new DirectoryInfo(Directory.GetCurrentDirectory());
            return directory.Parent.Parent.FullName + "\\Languages";
        }
        #endregion


        private void FindTestButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AllTests.SelectedIndex != -1)
                {
                    CurrentTest.test = AllTests.SelectedItem as Test;
                    DialogResult = true;
                }
                else
                {
                    MessageBox.Show("No test selected");
                    DialogResult = false;
                }

            }
            catch 
            {
                DialogResult = false;               
            }
        }

        #region кнопки для окна
        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion
    }
}
