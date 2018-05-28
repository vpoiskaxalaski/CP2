using System;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.Text.RegularExpressions;

namespace Notest
{
    public partial class QuestionChange : UserControl
    {
        public QuestionChange()
        {
            InitializeComponent();
        }

        //проверка questionCost
        private void OnQuestionCostChanged(object sender, TextChangedEventArgs e)
        {
            questionCosttxb.ToolTip = "";
            questionCosttxb.BorderBrush = new SolidColorBrush(Colors.Gray);
            string regex = @"[0-9]";

            for (int i = 0; i < questionCosttxb.Text.Length; i++)
            {
                if (Regex.IsMatch(questionCosttxb.Text[i].ToString(), regex) == false)
                {
                    questionCosttxb.ToolTip = "Неккоректные данные";
                    questionCosttxb.BorderBrush = new SolidColorBrush(Colors.IndianRed);
                }
            }
        }


        #region картинка
        //добавление картинки
        private void AddImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog op = new OpenFileDialog
                {
                    InitialDirectory = "E:\\Pictures\\fon",
                    Title = "Select a picture",
                    Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                  "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                  "Portable Network Graphic (*.png)|*.png"
                };
                if (op.ShowDialog() == true)
                {
                    PictureBox.Source = new BitmapImage(new Uri(op.FileName));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ощибка загрузки изображения: " + ex.Message);
            }
        }

        //удаление картинки
        private void DeleteImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (PictureBox.Source.ToString().Length != 0)
                    PictureBox.Source = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }
        #endregion

        //добавление ответа
        private void AddAnswer_Click(object sender, RoutedEventArgs e)
        {
            AnswerDtgrd.CanUserAddRows = true;
        }

    }
}
