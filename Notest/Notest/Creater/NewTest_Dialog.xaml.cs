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
    /// Логика взаимодействия для NewTest_Dialog.xaml
    /// </summary>
    public partial class NewTest_Dialog : Window
    {
        public NewTest_Dialog()
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

        private void NewTestButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TestTheme.Text.Length != 0 && TestName.Text.Length != 0)
                {
                    if (Validation.IsTestExist(TestTheme.Text, TestName.Text) == false)
                    {
                        CurrentTest.test = new Test
                        {
                            Topic = TestTheme.Text,
                            Header = TestName.Text
                        };
                        DialogResult = true;
                    }
                    else
                    {
                        MessageBox.Show("Test already exists","", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    DialogResult = false;
                    MessageBox.Show("Enter the data!", "", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                DialogResult = false;
                MessageBox.Show(ex.Message);
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
