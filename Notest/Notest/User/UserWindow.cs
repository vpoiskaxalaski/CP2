using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Notest
{
    enum Panel
    {
        First, Second
    }
    partial class UserWindow
    {
        private void OnChanging(object sender, RoutedEventArgs e)
        {
            if (First.Visibility == Visibility.Visible)
            {
                First.Visibility = Visibility.Collapsed;
                Second.Visibility = Visibility.Visible;
            }
            else
            {
                First.Visibility = Visibility.Visible;
                Second.Visibility = Visibility.Collapsed;
            }
        }

        private void Test_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as DataGrid).SelectedItem is Test selectedItem) // сопоставление шаблонов
            {
                var result = MessageBox.Show($"Выбрать тест {selectedItem.Header}", "Ответственный выбор", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    CurrentTest.test = selectedItem;
                    ChooseTest chooseTest = new ChooseTest();
                    chooseTest.Show();
                    Close();
                }
            }
        }
    }
}
