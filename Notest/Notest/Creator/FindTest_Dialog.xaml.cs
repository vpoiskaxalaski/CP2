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
        }

        private void FindTestButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TestTheme.Text.Length != 0 && TestName.Text.Length != 0)
                {
                    using (Context db = new Context())
                    {
                        var tests = db.Tests;
                        foreach (Test t in tests)
                        {
                            if (TestTheme.Text == t.Topic && TestName.Text == t.Header)
                            {
                                CurrentTest.test = new Test();
                                CurrentTest.test = t;
                                DialogResult = true;
                            }
                        }

                        if (CurrentTest.test == null)
                        {
                            DialogResult = false;
                            MessageBox.Show("Тест не найден");
                        }
                    }

                }
                else
                {
                    
                    MessageBox.Show("Введите данные!");
                }
                
            }
            catch(Exception ex)
            {
                DialogResult = false;
                MessageBox.Show(ex.Message);
            }
        }
    }
}
