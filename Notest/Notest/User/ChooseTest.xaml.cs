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
            if (CurrentTest.test != null)
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
                //MessageBox.Show(this.Resources["noselectTest"].ToString());
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
                MessageBox.Show((string)Application.Current.Resources["cameBy"] + " " + user.Login);
            }
        }

        // завершение теста: подсчёт результатов
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
                            if (element is TextBox)
                            {
                                var textBox = element as TextBox;
                                isRights += textBox.Text;
                            }
                            if (element is Label)
                            {
                                Label meta = element as Label;
                                string[] weightAndRight = ((string)meta.Content).Split(':');
                                int weight = Convert.ToInt32(weightAndRight[0]);
                                maxResult += weight;
                                string rightAnswers = weightAndRight[1];
                                int countRight = 0;
                                bool isWritten = false; // рукописный ответ
                                foreach (char right in rightAnswers.ToCharArray())
                                {
                                    if (!isWritten)
                                    {
                                        if (right == 't') // если не цифра - ответ рукописный
                                        {
                                            isWritten = true;
                                        }
                                        if (isRights.Contains(right))
                                            countRight++;
                                    }
                                    else
                                    {
                                        countRight = CheckWritten(isRights, rightAnswers.Substring(1)); // передаём rightAnswers без t
                                        break;
                                    }
                                }
                                result += Rate(countRight, rightAnswers.Length, weight);
                            }
                            counter++;
                        }
                    }
                }
                MessageBox.Show((string)Application.Current.Resources["congratulation"] + " - " + result +
                    "\n" + (string)Application.Current.Resources["max"] + " - " + maxResult + "\n");
                using (Context db = new Context())
                {
                    CompletedTest completedTest = new CompletedTest
                    {
                        Result = result,
                        TestId = CurrentTest.test.Id,
                        UserLogin = Class.CurrentUser.user.Login,
                        Date = DateTime.Now.ToString()
                    };
                    db.CompletedTest.Add(completedTest);
                    db.SaveChanges();
                }
                FinishHim();
            }
            catch
            {
                MessageBox.Show((string)Application.Current.Resources["error"]);
                Close();
            }
        }

        // проверка письменного ответа
        private int CheckWritten(string writtenAnswer, string rightsAnswer)
        {
            return writtenAnswer.Equals(rightsAnswer) ? writtenAnswer.Length + 1 : 0; // + 1 потому, чтобы учитывать всю длину ответа (нужно для Rate)
        }

        // подсчёт промежуточного результата
        private int Rate(int current, int total, int weight) // current - набранное количество правильных ответов | total - количество правильных ответов | weight - вес вопроса
        {
            if (total == 0) // если правильных ответов нет вообще
            {
                return current == 0 ? weight : 0; // если пользователь ничего не ответил, то на нет и суда нет
            }
            double resultMark = (current / (double)total) * weight; //total
            return (int)Math.Round(resultMark);
        }

        // заполнение теста
        private void FillTest(Context db)
        {
            int counter = 1;
            var questions = from question in db.Questions where question.Test_Id == CurrentTest.test.Id select question;
            var randQuestions = GetShakedQuestions(questions.ToList());
            foreach (Question question in randQuestions)
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
                        Margin = new Thickness(3, 0, 5, 15)
                    };
                    wrapPanel.Children.Add(image);
                }
                // добавление ответов (CheckBox)
                int number = 1;
                string rights = "";
                var answers = from answer in db.Answers where answer.Question_Id == question.Id select answer;
                if (answers.Count() == 1)
                {
                    TextBox textBox = new TextBox
                    {
                        TextWrapping = TextWrapping.Wrap,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Width = this.Width,
                        TextAlignment = TextAlignment.Center
                    };
                    rights += "t" + (answers.First().IsRight ? answers.First().Answer1 : ""); // единственный ответ - это скорее ошибка создателя теста
                    counter++;

                    answersPanel.Children.Add(textBox);
                }
                else
                {
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

            Button finishButton = new Button();
            finishButton.SetResourceReference(Button.ContentProperty, "finish");
            finishButton.Click += OnFinishTest;
            SelectedTest.Children.Add(finishButton);
        }

        // получение списка случайно расположенных вопросов
        private List<Question> GetShakedQuestions(List<Question> questions)
        {
            List<Question> shakedQuestions = new List<Question>();
            List<int> indexList = new List<int>();
            for (int i = 0; i < questions.Count; i++)
            {
                indexList.Add(i);
            }
            int randIndex = 0;
            Random randomize = new Random();
            for (int i = 0; i < questions.Count; i++)
            {
                randIndex = indexList.Count > 1 ? randomize.Next(indexList.Count) : indexList.First();
                shakedQuestions.Add(questions[randIndex]);
                indexList.Remove(randIndex);
            }
            return shakedQuestions;
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

        // завершение теста: выход из окна
        private void FinishHim()
        {
            UserWindow userWindow = new UserWindow();
            userWindow.Show();
            Close();
        }

        #region кнопки для окна
        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void HideWindow_Click(object sender, RoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        private void Fullscreen_Click(object sender, RoutedEventArgs e)
        {
            SystemCommands.MaximizeWindow(this);
            Fullscreen.Visibility = Visibility.Hidden;
            FullscreenExit.Visibility = Visibility.Visible;
        }

        private void FullscreenExit_Click(object sender, RoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(this);
            FullscreenExit.Visibility = Visibility.Hidden;
            Fullscreen.Visibility = Visibility.Visible;
        }
        #endregion
    }
}
