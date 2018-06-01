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
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Notest
{
    /// <summary>
    /// Логика взаимодействия для CreaterWindow.xaml
    /// </summary>
    public partial class CreaterWindow : Window
    {
        QuestionChange questionChangePanel = new QuestionChange() { HorizontalAlignment = HorizontalAlignment.Right };
        Context db = new Context();
        List<Answer> answerList = new List<Answer>();

        int countQuestion = 0;

        public CreaterWindow()
        {
            #region Удаление сод. бд
            //db.Answers.RemoveRange(db.Answers);
            //db.Questions.RemoveRange(db.Questions);
            //db.Tests.RemoveRange(db.Tests);
            //db.SaveChanges();           

            //MessageBox.Show("Tests: ");
            //foreach(var item in db.Tests)
            //{
            //    MessageBox.Show(item.Header);
            //}
            //MessageBox.Show("Questions: ");
            //foreach (var item in db.Questions)
            //{
            //    MessageBox.Show(item.Id.ToString());
            //}
            //MessageBox.Show("Answerd: ");
            //foreach (var item in db.Answers)
            //{
            //    MessageBox.Show(item.Answer1);
            //}
            #endregion

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

            questionChangePanel.IsEnabled = false;
            TestToolsWindow.Children.Add(questionChangePanel);
            questionChangePanel.questionCosttxb.BorderBrush = new SolidColorBrush(Colors.Gray);
            questionChangePanel.SaveChanges.Click += SaveChanges_Click;
            questionChangePanel.Clear.Click += Clear_Click;
            questionChangePanel.RemoveAnswer.Click += RemoveAnswer_Click;
            questionChangePanel.AnswerDtgrd.ItemsSource = answerList;

            UserLogin.Text = Class.CurrentUser.user != null ? Class.CurrentUser.user.Login : "Unknown";
        }

        #region вспомогательные функции
        //обновить список вопросов
        private void UpdateQuestionList()
        {
            try
            {
                question_ListBox.Items.Clear();

                if (CurrentTest.test.Questions != null)
                {
                    foreach (Question q in CurrentTest.test.Questions)
                    {
                        question_ListBox.Items.Add((question_ListBox.Items.Count + 1).ToString() + ". " + q.Question1);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Update error");
            }
        }

        //очистка рабочей области
        private void CleanWindow()
        {
            answerList.Clear();
            questionChangePanel.questionText.Document.Blocks.Clear();
            questionChangePanel.PictureBox.Source = null;
            questionChangePanel.questionCosttxb.Text = "1";
            questionChangePanel.questionCosttxb.BorderBrush = new SolidColorBrush(Colors.Gray);
            questionChangePanel.AnswerDtgrd.ItemsSource = answerList;
            questionChangePanel.AnswerDtgrd.Items.Refresh();
            question_ListBox.SelectedIndex = -1;
        }

        #endregion

        //создание нового теста
        private void NewTest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NewTest_Dialog newTest_Dialog = new NewTest_Dialog();
                newTest_Dialog.Owner = this;

                if (newTest_Dialog.ShowDialog() == true)
                {
                    //очистка рабочей области
                    question_ListBox.Items.Clear();
                    countQuestion = 0;
                    CleanWindow();

                    questionChangePanel.IsEnabled = true;
                    NameTest.Text = CurrentTest.test.Header;

                    // добавление вовзможность создать/удалить вопрос
                    QuestionTools.Visibility = Visibility;
                    AddQuestionFromDb.Visibility = Visibility;
                    questionChangePanel.IsEnabled = true;

                    CurrentTest.test.Author = UserLogin.Text;
                    PrintTest.IsEnabled = true;
                    SaveTest.IsEnabled = true;
                }
            }
            catch
            {
                MessageBox.Show("It is not possible to create", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //сохранить тест
        private void SaveTest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Validation.IsTestExist(CurrentTest.test.Topic, CurrentTest.test.Header) == false)
                {
                    db.Tests.Add(CurrentTest.test);
                    db.SaveChanges();
                    CurrentTest.test.Id = db.Tests.FirstOrDefault(t => (t.Header == CurrentTest.test.Header) && (t.Topic == CurrentTest.test.Topic)).Id;

                    foreach (Question q in CurrentTest.test.Questions)
                    {
                        q.Test_Id = CurrentTest.test.Id;
                        db.Questions.Add(q);
                        db.SaveChanges();
                        q.Id = db.Questions.FirstOrDefault(dbQ => (dbQ.Question1 == q.Question1) && (dbQ.Test_Id == q.Test_Id)).Id;

                        foreach (Answer a in q.Answers)
                        {
                            a.Question_Id = q.Id;
                            db.Answers.Add(a);
                            db.SaveChanges();
                        }
                    }
                }
                else //если работаем с существующим тестом
                {
                    List<Question> questions = Question.ChangeFromDb(db.Questions.Where(q => q.Test_Id == CurrentTest.test.Id));
                 
                    foreach (Question question in questions)
                    {
                            db.Answers.RemoveRange(db.Answers.Where(a => a.Question_Id == question.Id));
                            db.SaveChanges();
                    }

                    if(questions.Count!=0)
                    {
                        db.Questions.RemoveRange(db.Questions.Where(q => q.Test_Id == CurrentTest.test.Id));
                        db.SaveChanges();
                    }                    

                    foreach (Question q in CurrentTest.test.Questions)
                    {
                        q.Test_Id = CurrentTest.test.Id;
                        db.Questions.Add(q);
                        db.SaveChanges();
                        q.Id = db.Questions.FirstOrDefault(dbQ => (dbQ.Question1 == q.Question1) && (dbQ.Test_Id == q.Test_Id)).Id;

                        foreach (Answer a in q.Answers)
                        {
                            a.Question_Id = q.Id;
                            db.Answers.Add(a);
                            db.SaveChanges();
                        }

                    }
                }
                MessageBox.Show("Test saved", "", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }  
            catch
            {
                MessageBox.Show("It is not possible to save", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //открыть тест
        private void OpenTest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FindTest_Dialog findTest_Dialog = new FindTest_Dialog();
                findTest_Dialog.Owner = this;

                if (findTest_Dialog.ShowDialog() == true)
                {
                    CleanWindow();

                    // добавление вовзможность создать/удалить вопрос
                    QuestionTools.Visibility = Visibility;
                    QuestionTools.Visibility = Visibility;
                    AddQuestionFromDb.Visibility = Visibility;
                    questionChangePanel.IsEnabled = true;
                    SaveTest.IsEnabled = true;
                    PrintTest.IsEnabled = true;

                    //добавление названия теста
                    NameTest.Text = CurrentTest.test.Header;

                    CurrentTest.test.Questions = Question.ChangeFromDb(db.Questions.Where(q => q.Test_Id == CurrentTest.test.Id));
                    foreach (Question q in CurrentTest.test.Questions)
                    {
                        q.Answers = Answer.ChangeFromDb(db.Answers.Where(a => a.Question_Id == q.Id));
                    }

                    UpdateQuestionList();
                    countQuestion = question_ListBox.Items.Count;
                }
            }
            catch
            {
                MessageBox.Show("Open error", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //сохранить тест в ворд
        private void PrintTest_Click(object sender, RoutedEventArgs e)
        {
            if(CurrentTest.test.Questions.Count!=0)
            {
                CurrentTest.Print();
            }
            else
            {
                MessageBox.Show("There is nothing to save", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        //добавить вопрос
        private void AddQuestion_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //добавляем вопрос
                TextRange range = new TextRange(questionChangePanel.questionText.Document.ContentStart, questionChangePanel.questionText.Document.ContentEnd);
                if(range.IsEmpty == false)
                {
                    question_ListBox.Items.Insert(countQuestion, (countQuestion + 1).ToString() + ". " + range.Text);

                    Question question = new Question
                    {
                        Question1 = range.Text
                    };

                    if (questionChangePanel.questionCosttxb.BorderBrush != new SolidColorBrush(Colors.IndianRed))
                    {
                        question.Cost = Convert.ToInt32(questionChangePanel.questionCosttxb.Text);
                    }
                    else
                    {
                        questionChangePanel.questionCosttxb.Text = "1";
                        question.Cost = 1;
                    }

                    if (questionChangePanel.PictureBox.Source != null)
                    {
                        question.Image = questionChangePanel.PictureBox.Source.ToString();
                    }

                    //варианты ответов
                    answerList.AddRange((List<Answer>)questionChangePanel.AnswerDtgrd.ItemsSource);
                    answerList.RemoveRange(answerList.Count / 2, answerList.Count / 2);
                    question.Answers.AddRange(answerList);

                    bool flag = false;
                    foreach (Question q in CurrentTest.test.Questions)
                    {
                        if (question.Question1 == q.Question1)
                        {
                            flag = true;
                        }
                    }

                    if (flag == false)
                    {
                        CurrentTest.test.Questions.Add(question);
                        countQuestion += 1;
                    }
                    else
                    {
                        MessageBox.Show("Question already exists", "", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }

                CleanWindow();
            }
            catch
            {
                MessageBox.Show("Add question error", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        //добавить вопрос из бд
        private void AddQuestionFromDb_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddQuestions addQuestions = new AddQuestions();
                addQuestions.ShowDialog();
                UpdateQuestionList();
            }
            catch
            {
                MessageBox.Show("Add question error", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // удалить вопрос
        private void RemoveQuestion_Click(object sender, RoutedEventArgs e)
        {
            try
            {                
                string selectedQuestion = question_ListBox.SelectedItem.ToString().Remove(0, question_ListBox.SelectedItem.ToString().IndexOf('.') + 2);
                int index = question_ListBox.SelectedIndex;
                if(index!=-1)
                {
                    MessageBoxResult result = MessageBox.Show("Are you shure?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        question_ListBox.SelectedItem = question_ListBox.Items[0];
                        question_ListBox.Items.RemoveAt(index);

                        Question question = new Question();
                        foreach (Question q in CurrentTest.test.Questions)
                        {
                            if (q.Question1 == selectedQuestion)
                            {
                                question = q;
                                break;
                            }
                        }

                        CurrentTest.test.Questions.Remove(question);
                        countQuestion -= 1;
                        UpdateQuestionList();
                    }
                }
                else
                {
                    MessageBox.Show("Question is not selected", "", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch
            {
                MessageBox.Show("Delete question error", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //выбор вопроса в списке      
        private void question_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (question_ListBox.SelectedItem != null)
                {
                    questionChangePanel.PictureBox.Source = null;
                    string selectedQuestion = question_ListBox.SelectedItem.ToString().Remove(0, question_ListBox.SelectedItem.ToString().IndexOf('.') + 2);
                    TextRange range = new TextRange(questionChangePanel.questionText.Document.ContentStart, questionChangePanel.questionText.Document.ContentEnd);
                    range.Text = selectedQuestion;

                    foreach (Question q in CurrentTest.test.Questions)
                    {
                        if (q.Question1 == selectedQuestion)
                        {
                            questionChangePanel.questionCosttxb.Text = q.Cost.ToString();

                            if (q.Image != null)
                            {
                                questionChangePanel.PictureBox.Source = new BitmapImage(new Uri(q.Image));
                            }
                            questionChangePanel.AnswerDtgrd.ItemsSource = q.Answers;
                            questionChangePanel.AnswerDtgrd.Items.Refresh();
                            break;
                        }
                    }
                }

            }
            catch
            {

            }
        }

        //сохранить изменения
        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (question_ListBox.SelectedItem != null)
                {
                    string selectedQuestion = question_ListBox.SelectedItem.ToString();
                    TextRange range = new TextRange(questionChangePanel.questionText.Document.ContentStart, questionChangePanel.questionText.Document.ContentEnd);

                    foreach (Question q in CurrentTest.test.Questions)
                    {
                        if (q.Question1 == selectedQuestion.Remove(0, selectedQuestion.IndexOf('.') + 2))
                        {
                            question_ListBox.Items[question_ListBox.SelectedIndex] = selectedQuestion.Remove(selectedQuestion.IndexOf('.') + 1) + " " + range.Text;
                            q.Question1 = range.Text;
                            if (questionChangePanel.questionCosttxb.BorderBrush != new SolidColorBrush(Colors.IndianRed))
                            {
                                q.Cost = Convert.ToInt32(questionChangePanel.questionCosttxb.Text);
                            }
                            else
                            {
                                questionChangePanel.questionCosttxb.Text = "1";
                                q.Cost = 1;
                            }
                            if (questionChangePanel.PictureBox.Source != null)
                            {
                                q.Image = questionChangePanel.PictureBox.Source.ToString();
                            }
                            else
                            {
                                q.Image = null;
                            }
                            //варианты ответов
                            answerList = (List<Answer>)questionChangePanel.AnswerDtgrd.ItemsSource;
                            if (answerList != null)
                            {
                                q.Answers = answerList;
                            }
                            break;
                        }
                    }
                    MessageBox.Show("Changes saved", "", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch
            {
                MessageBox.Show("Save changes error", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //очистить questionChangePanel
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            CleanWindow();
        }

        //удаление ответа
        private void RemoveAnswer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedAnswer = questionChangePanel.AnswerDtgrd.SelectedItem;

                answerList = (List<Answer>)questionChangePanel.AnswerDtgrd.ItemsSource;
                if ((selectedAnswer as Answer) != null)
                {
                    answerList.Remove((Answer)selectedAnswer);
                    questionChangePanel.AnswerDtgrd.Items.Refresh();
                }
                else
                {
                    questionChangePanel.AnswerDtgrd.CanUserAddRows = false;
                }
            }
            catch
            {
                MessageBox.Show("Сan not be removed", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #region  выход в окно регистрации/входа
        private void GoOut(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow startWindow = new MainWindow();
                MessageBoxResult dialogResult = MessageBox.Show("Are you shure?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (dialogResult == MessageBoxResult.Yes)
                {
                    CurrentTest.test = null;                   
                    Close();
                    startWindow.Show();
                }               
            }
            catch
            {
               
            }
        }
        private void OnMouseOver(object sender, MouseEventArgs e)
        {
            var image = sender as System.Windows.Controls.Image;
            image.Source = BitmapFrame.Create(new Uri(@"pack://application:,,,/ico/opened_door.ico"));
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            var image = sender as System.Windows.Controls.Image;
            image.Source = BitmapFrame.Create(new Uri(@"pack://application:,,,/ico/door.ico"));
        }
        #endregion

        #region кнопки для окна
        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dialogResult = MessageBox.Show("Are you shure?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (dialogResult == MessageBoxResult.Yes)
            {
                Close();  
            }

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

        #endregion

        private void UsersResult_Click(object sender, RoutedEventArgs e)
        {
            TestResults testResults = new TestResults();
            testResults.ShowDialog();
        }

       
    }
}

