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
        }

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
