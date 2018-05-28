using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using System.Threading;
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
    /// <summary>
    /// Логика взаимодействия для Testing.xaml
    /// </summary>
    public partial class ChooseTest : Window
    {
        public ChooseTest()
        {
            InitializeComponent();
            if (File.Exists("choosen.txt"))
            {
                string topic = "";
                string header = "";
                using (Context db = new Context())
                {
                    var test = db.Tests.Where(t => t.Id == CurrentTest.test.Id).First();
                    FillTest(db); // заполнение теста
                    topic = test.Topic;
                    header = test.Header;
                }
                Topic.Content += topic;
                Header.Content += header;
            }
            else
            {
                MessageBox.Show("Тест не выбран!");
                UserWindow user = new UserWindow();
                user.Show();
                Close();
            }
        }

        private void OnStartTest(object sender, RoutedEventArgs e)
        {
            if (Class.CurrentUser.user == null)
            {
                User user = new User
                {
                    Login = "ExperimentalUser",
                    Password = Validation.GetHashString("12345678"),
                    UserType = "Пользователь"
                };
                if (!Validation.NoRepeatLogin(user.Login))
                {
                    using (Context db = new Context())
                    {
                        db.Users.Add(user);
                        db.SaveChanges();
                    }
                }
                Class.CurrentUser.user = user;
                MessageBox.Show("Вы зашли под логином " + user.Login);
            }
        }

        private void OnFinishTest(object sender, RoutedEventArgs e)
        {
            try
            {
                var childrens = SelectedTest.Children;
                int result = 0;
                int maxResult = 0;
                // перебор вопросов
                foreach (UIElement child in childrens)
                {
                    // нашли вопрос
                    if (child is Expander)
                    {
                        Expander hider = child as Expander;
                        WrapPanel container = hider.Content as WrapPanel;
                        WrapPanel answers = new WrapPanel();
                        // поиск блока с ответами
                        foreach (UIElement part in container.Children)
                        {
                            if (part is WrapPanel)
                            {
                                answers = part as WrapPanel;
                                break;
                            }
                        }
                        int counter = 1;
                        string isRights = "";
                        // перебор ответов 
                        foreach (UIElement element in answers.Children)
                        {
                            if (element is CheckBox)
                            {
                                CheckBox answer = element as CheckBox;
                                if (answer.IsChecked == true)
                                {
                                    isRights += counter;
                                }
                            }
                            if (element is Label)
                            {
                                Label meta = element as Label;
                                string[] weightAndRight = ((string)meta.Content).Split(':');
                                int weight = Convert.ToInt32(weightAndRight[0]);
                                maxResult += weight;
                                string rightAnswers = weightAndRight[1];
                                int countRight = 0;
                                foreach (char right in rightAnswers.ToCharArray())
                                {
                                    if (isRights.Contains(right))
                                        countRight++;
                                }
                                result += Rate(countRight, rightAnswers.Length, weight);
                            }
                            counter++;
                        }
                    }
                }
                MessageBox.Show("Поздравляем, ваш результат - " + result + "\n Максимальный результат - " + maxResult + "\n");
                using (Context db = new Context())
                {
                    CompletedTest completedTest = new CompletedTest
                    {
                        Result = result,
                        TestId = CurrentTest.test.Id,
                        UserLogin = Class.CurrentUser.user.Login
                    };
                    db.CompletedTest.Add(completedTest);
                    db.SaveChanges();
                }
                FinishHim();
            }
            catch
            {
                MessageBox.Show("Неожиданная ошибка. Позовите программиста");
                Close();
            }
        }

        private int Rate(int current, int total, int weight) // current - набранное количество правильных ответов | total - количество правильных ответов | weight - вес вопроса
        {
            if (total == 0) // если правильных ответов нет вообще
            {
                return current == 0 ? weight : 0; // если пользователь ничего не ответил, то на нет и суда нет
            }
            double resultMark = (current / (double)total) * weight; //total
            return (int)Math.Round(resultMark);
        }
        
        private void FillTest(Context db)
        {
            int counter = 1;
            var questions = from question in db.Questions where question.Test_Id == CurrentTest.test.Id select question;
            foreach (Question question in questions)
            {
                Expander expander = new Expander
                {
                    Header = new TextBlock
                    {
                        TextWrapping = TextWrapping.Wrap,
                        Text = question.Question1
                    }
                };
                expander.Expanded += OnThisExpanded;
                WrapPanel wrapPanel = new WrapPanel(); // контейнер для вопроса
                WrapPanel answersPanel = new WrapPanel
                {
                    Orientation = Orientation.Vertical,
                    MinHeight = 70.0
                };
                // если в вопросе присутствует картинка
                if (question.Image != null)
                {
                    Image image = new Image
                    {
                        Source = new BitmapImage(new Uri(question.Image)),
                        Width = 100,
                        Margin = new Thickness(3,0,5,15)
                    };
                    wrapPanel.Children.Add(image);
                }
                // добавление ответов (CheckBox)
                int number = 1;
                string rights = "";
                var answers = from answer in db.Answers where answer.Question_Id == question.Id select answer;
                foreach (var answer in answers)
                {
                    CheckBox variant = new CheckBox
                    {
                        Content = new TextBlock
                        {
                            TextWrapping = TextWrapping.Wrap,
                            Text = answer.Answer1
                        }
                    };
                    answersPanel.Children.Add(variant);
                    if (answer.IsRight)
                    {
                        rights += number;
                    }
                    number++;
                }
                Label weight = new Label
                {
                    Content = question.Cost + ":" + rights,
                    Name = "WeightAndRights",
                    Visibility = Visibility.Collapsed
                };
                answersPanel.Children.Add(weight);
                wrapPanel.Children.Add(answersPanel);
                counter++;
                expander.Content = wrapPanel;
                SelectedTest.Children.Add(expander);
                rights = "";
            }
            Button finishButton = new Button
            {
                Content = "Завершить тест"
            };
            finishButton.Click += OnFinishTest;
            SelectedTest.Children.Add(finishButton);
        }

        // чтобы открывался только один Expander
        private void OnThisExpanded(object sender, RoutedEventArgs e)
        {
            var thisExpander = (Expander)sender; // получаем открываемый expander
            var stackParent = (StackPanel)thisExpander.Parent; // получаем контейнер с вопросами
            foreach (UIElement child in stackParent.Children) // проходимся по всем элементам
            {
                if (child is Expander exChild) 
                {
                    if (exChild != thisExpander) // если данный элемент expander, но не текущий, то закрываем его 
                    {
                        exChild.IsExpanded = false;
                    }
                }
            }
        }

        private void FinishHim()
        {
            UserWindow userWindow = new UserWindow();
            userWindow.Show();
            Close();
        }
    }
}
