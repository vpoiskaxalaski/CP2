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
using System.Data.Entity.Validation;
using System.IO;
using System.Drawing;

namespace Notest
{
    /// <summary>
    /// Логика взаимодействия для CreaterWindow.xaml
    /// </summary>
    public partial class CreaterWindow : Window
    {
        QuestionChange questionChangePanel = new QuestionChange();
        Context db = new Context();
        List<Question> questionList = new List<Question>();
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

            questionChangePanel.IsEnabled = false;
            TestToolsWindow.Children.Add(questionChangePanel);
            questionChangePanel.SaveChanges.Click += SaveChanges_Click;
            questionChangePanel.Clear.Click += Clear_Click;
            questionChangePanel.RemoveAnswer.Click += RemoveAnswer_Click;
            questionChangePanel.AnswerDtgrd.ItemsSource = answerList;

            if (Class.CurrentUser.user != null)
            {
                UserLogin.Text = Class.CurrentUser.user.Login;
            }
        }

        //удаление ответа
        private void RemoveAnswer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Answer selectedAnswer = (Answer) questionChangePanel.AnswerDtgrd.SelectedItem;
                if(selectedAnswer!=null)
                {
                    answerList = (List<Answer>)questionChangePanel.AnswerDtgrd.ItemsSource;
                    answerList.Remove(selectedAnswer);
                    questionChangePanel.AnswerDtgrd.Items.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //обновить список вопросов
        private void UpdateQuestionList()
        {
            try
            {
                question_ListBox.Items.Clear();

                if(questionList!=null)
                {
                    foreach (Question q in questionList)
                    {
                        question_ListBox.Items.Add(question_ListBox.Items.Count.ToString() + ". " + q.Question1);
                    }
                }                
            }
           catch
            {
                MessageBox.Show("Update error");
            }
        }

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
                    questionList.Clear();
                    countQuestion = 0;
                    CleanWindow();
                   
                    questionChangePanel.IsEnabled = true;
                    NameTest.Text = CurrentTest.test.Header;                
                    
                    // добавление вовзможность создать/удалить вопрос
                    QuestionTools.Visibility = Visibility;
                    questionChangePanel.IsEnabled = true;

                    CurrentTest.test.Author = UserLogin.Text;
                }                
            }
            catch 
            {
                MessageBox.Show("Create error");
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

                    foreach (Question q in questionList)
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
                else
                {
                    List<Question>  questions = Question.ChangeFromDb( db.Questions.Where(q=> q.Test_Id == CurrentTest.test.Id));

                    foreach(Question question in questions)
                    {
                        db.Answers.RemoveRange(db.Answers.Where(a=>a.Question_Id == question.Id));
                        db.SaveChanges();
                    }

                    db.Questions.RemoveRange(db.Questions.Where(q => q.Test_Id == CurrentTest.test.Id));                    

                    foreach (Question q in questionList)
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
                MessageBox.Show("Test saved");
            }
            catch
            {
                MessageBox.Show("Saved test error");
            }
        }

        //открыть тест
        private void OpenTest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FindTest_Dialog findTest_Dialog = new FindTest_Dialog();
                findTest_Dialog.Owner = this;

                if ( findTest_Dialog.ShowDialog() == true)
                {
                    CleanWindow();

                    // добавление вовзможность создать/удалить вопрос
                    QuestionTools.Visibility = Visibility;
                    questionChangePanel.IsEnabled = true;

                    //добавление названия теста
                    NameTest.Text = CurrentTest.test.Header;

                    questionList = Question.ChangeFromDb(db.Questions.Where(q=> q.Test_Id == CurrentTest.test.Id));
                    foreach(Question q in questionList)
                    {
                        q.Answers = Answer.ChangeFromDb( db.Answers.Where(a => a.Question_Id == q.Id));
                    }

                    UpdateQuestionList();
                    countQuestion = question_ListBox.Items.Count;
                }               
            }
            catch
            {
                MessageBox.Show("Open error");
            }
        }

        //добавить вопрос
        private void AddQuestion_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //добавляем вопрос
                TextRange range = new TextRange(questionChangePanel.questionText.Document.ContentStart, questionChangePanel.questionText.Document.ContentEnd);
                question_ListBox.Items.Insert(countQuestion, countQuestion.ToString() + ". " + range.Text);                

                Question question = new Question
                {
                    Question1 = range.Text
                };
                
                if(questionChangePanel.questionCosttxb.BorderBrush != new SolidColorBrush(Colors.IndianRed))
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

                questionList.Add(question);
                countQuestion += 1;

                CleanWindow();
            }
            catch 
            {
                MessageBox.Show("Add question error");
            }

        }

        //выбор вопроса в списке      
        private void question_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if(question_ListBox.SelectedItem != null)
                {
                    questionChangePanel.PictureBox.Source = null;
                    string selectedQuestion = question_ListBox.SelectedItem.ToString().Remove(0, question_ListBox.SelectedItem.ToString().IndexOf('.')+2);
                    TextRange range = new TextRange(questionChangePanel.questionText.Document.ContentStart, questionChangePanel.questionText.Document.ContentEnd);                    
                    range.Text = selectedQuestion;

                    foreach(Question q in questionList)
                    {
                        if(q.Question1 == selectedQuestion)
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
                MessageBox.Show("Selected item error");
            }
            
        }

        // удалить вопрос
        private void RemoveQuestion_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string selectedQuestion = question_ListBox.SelectedItem.ToString().Remove(0, question_ListBox.SelectedItem.ToString().IndexOf('.')+2);
                int index = question_ListBox.SelectedIndex;
                question_ListBox.SelectedItem = question_ListBox.Items[0];
                question_ListBox.Items.RemoveAt(index);              

                Question question = new Question();
                foreach (Question q in questionList)
                {
                    if(q.Question1 == selectedQuestion)
                    {
                        question = q;
                        break;
                    }
                }

                questionList.Remove(question);
                countQuestion -= 1;                
                UpdateQuestionList();
            }
            catch
            {
                MessageBox.Show("Delete question error");
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
                    
                    foreach (Question q in questionList)
                    {
                        if (q.Question1 == selectedQuestion.Remove(0, selectedQuestion.IndexOf('.') + 2))
                        {
                            question_ListBox.Items[question_ListBox.SelectedIndex] = selectedQuestion.Remove(selectedQuestion.IndexOf('.')+1) +" "+ range.Text;
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
                            //варианты ответов
                            answerList = (List<Answer>)questionChangePanel.AnswerDtgrd.ItemsSource;
                            if(answerList!=null)
                            {
                                //answerList.RemoveRange(answerList.Count / 2, answerList.Count / 2);
                                q.Answers = answerList;
                            }                           
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Save changes error" + ex.Message);
            }
        }

        //очистить questionChangePanel
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            CleanWindow();
        }

        //очистка рабочей области
        private void CleanWindow()
        {
            answerList.Clear();
            questionChangePanel.questionText.Document.Blocks.Clear();
            questionChangePanel.PictureBox.Source = null;
            questionChangePanel.questionCosttxb.Text = "1";
            questionChangePanel.AnswerDtgrd.ItemsSource = answerList;
            questionChangePanel.AnswerDtgrd.Items.Refresh();
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

