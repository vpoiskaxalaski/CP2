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
using System.Data.Linq;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Data;

namespace Notest
{
    public partial class AddQuestions : Window
    {
        List<Question> questionList = new List<Question>();
        List<Question> selectedQuestions = new List<Question>();
        public AddQuestions()
        {
            InitializeComponent();
            #region добавление вопросов по данной теме
            try
            {               
                using (Context db = new Context())
                {
                     var result = db.Questions.SqlQuery("SELECT * FROM Questions join Tests on Questions.Test_Id = Tests.Id where Tests.Topic = @topic except select* from Questions join Tests on Questions.Test_Id = Tests.Id where Questions.Test_Id = @id",
                         new SqlParameter("@topic", CurrentTest.test.Topic), new SqlParameter("@id", CurrentTest.test.Id));
                    foreach(var item in result)
                    {
                        questionList.Add(item);
                    }
                }
                QuestionList.ItemsSource = questionList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            #endregion
        }
             
        private void Select_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (Context db = new Context())
                {
                    foreach (Question q in selectedQuestions)
                    {
                       q.Answers = Answer.ChangeFromDb(db.Answers.Where(a => a.Question_Id == q.Id));                       
                    }                    
                }

               for(int i=0; i<selectedQuestions.Count; i++)
                {
                    bool isExcist = false;
                    for (int j = 0; j < CurrentTest.test.Questions.Count; j++)
                    {
                        if(selectedQuestions[i].Question1 == CurrentTest.test.Questions[j].Question1)
                        {
                            isExcist = true;
                        }                        
                    }
                    if(isExcist == false)
                    {
                        CurrentTest.test.Questions.Add(selectedQuestions[i]);
                    }
                }
                DialogResult = true;
            }            
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                object tag = (sender as FrameworkElement).Tag;
                int index = QuestionList.Items.IndexOf(tag);
                var item = QuestionList.Items[index];
                if ((sender as System.Windows.Controls.Primitives.ToggleButton).IsChecked == true)
                {
                    selectedQuestions.Add(item as Question);
                }
                else
                {
                    selectedQuestions.Remove(item as Question);
                }
            }
            catch(Exception ex)
            {
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
