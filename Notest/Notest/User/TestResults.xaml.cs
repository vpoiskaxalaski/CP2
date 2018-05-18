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
    /// Логика взаимодействия для TestResults.xaml
    /// </summary>
    public partial class TestResults : Window
    {
        public TestResults()
        {
            InitializeComponent();
            using (Context db = new Context())
            {
                var completed = from compl in db.CompletedTest select compl;
                List<Class.Completed> completedTests = new List<Class.Completed>();
                foreach (CompletedTest test in completed)
                {
                    var currentTest = db.Tests.Where(t => t.Id == test.TestId).First();
                    Class.Completed compl = new Class.Completed
                    {
                        Id = test.Id,
                        UserLogin = test.UserLogin,
                        TestName = currentTest.Header,
                        TestTheme = currentTest.Topic,
                        Result = test.Result
                    };
                    completedTests.Add(compl);
                }
                ResultGrid.ItemsSource = completedTests;
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
                MessageBox.Show("Невозможно выйти");
            }
        }
    }
}
