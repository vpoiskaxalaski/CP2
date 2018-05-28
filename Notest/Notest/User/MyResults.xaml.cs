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
    /// Логика взаимодействия для MyResults.xaml
    /// </summary>
    public partial class MyResults : Window
    {
        public MyResults()
        {
            InitializeComponent();
            using (Context db = new Context())
            {
                var completed = from compl in db.CompletedTest where compl.UserLogin == Class.CurrentUser.user.Login select compl;
                List<Class.Completed> completedTests = new List<Class.Completed>();
                foreach (CompletedTest test in completed)
                {
                    var currentTest = db.Tests.Where(t => t.Id == test.TestId).First();
                    Class.Completed compl = new Class.Completed
                    {
                        Id = 0,
                        UserLogin = "",
                        TestName = currentTest.Header,
                        TestTheme = currentTest.Topic,
                        Result = test.Result
                    };
                    completedTests.Add(compl);
                }
                ResultGrid.ItemsSource = completedTests;
            }
        }
    }
}
