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
    /// Логика взаимодействия для FindTest_Dialog.xaml
    /// </summary>
    public partial class FindTest_Dialog : Window
    {
        public FindTest_Dialog()
        {
            InitializeComponent();
            using (Context db =new Context())
            {
                List<Test> tests = new List<Test>();
                tests.AddRange(db.Tests);                
                AllTests.ItemsSource = tests;
            }
        }

        private void FindTestButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AllTests.SelectedIndex!= -1)
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
            catch(Exception ex)
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
